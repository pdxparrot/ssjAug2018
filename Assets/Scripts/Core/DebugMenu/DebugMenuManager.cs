#define USE_LOG_MESSAGE_BUFFER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.DebugMenu
{
    // https://blogs.unity3d.com/2015/12/22/going-deep-with-imgui-and-editor-customization/
    public sealed class DebugMenuManager : SingletonBehavior<DebugMenuManager>
    {
#if !USE_LOG_MESSAGE_BUFFER
        private struct LogMessage
        {
            public string message;
            public LogType type;
        };
#endif

        [SerializeField]
        private Key _enableKey = Key.Backquote;

        [SerializeField]
        private bool _enabled;

        private DebugWindow _window;

        [SerializeField]
        [ReadOnly]
        private Vector2 _windowScrollPos;

        private readonly List<DebugMenuNode> _nodes = new List<DebugMenuNode>();

        [CanBeNull]
        private DebugMenuNode _currentNode;

        [SerializeField]
        [Range(0, 1000)]
        private int _fpsAccumulatorSize = 100;

        private float _lastFrameTime;
        private float _maxFrameTime;
        private float _minFrameTime;

        private readonly Queue<float> _fpsAccumulator = new Queue<float>();

        private float AverageFPS => _fpsAccumulator.Count < 1 ? 0 : _fpsAccumulator.Average();

#if USE_LOG_MESSAGE_BUFFER
        private readonly StringBuilder _logMessageBuffer = new StringBuilder();
#else
        private readonly Queue<LogMessage> _logMessages = new Queue<LogMessage>();
#endif

        [SerializeField]
        private int _maxLogMessages = 1000;

#region Unity Lifecycle
        private void Awake()
        {
            ResetFrameStats();

            _window = new DebugWindow(new Rect(10, 10, 800, 600), RenderWindowContents)
            {
                Title = () => {
                    string title = "Debug Menu";

                    if(null != _currentNode) {
                        if(null != _currentNode.Parent) {
                            title = $"{_currentNode.Parent.Title()}";
                        }
                        title += $" => {_currentNode.Title()}";
                    }

                    return title;
                }
            };

            Application.logMessageReceived += OnLogMessageReceived;

            InitLogMessageDebugNode();
        }

        protected override void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessageReceived;

            base.OnDestroy();
        }

        private void Update()
        {
            if(Keyboard.current[_enableKey].wasJustPressed) {
                _enabled = !_enabled;
            }

            Profiler.BeginSample("DebugMenuManager.Update");
            try {
                if(_enabled) {
                    _window.Update();
                }
            } finally {
                Profiler.EndSample();
            }

            UpdateFrameStats(Time.unscaledDeltaTime);
        }

        private void OnGUI()
        {
            if(!_enabled) {
                return;
            }

            Profiler.BeginSample("DebugMenuManager.OnGUI");
            try {
                _window.Render();
            } finally {
                Profiler.EndSample();
            }
        }
#endregion

        public DebugMenuNode AddNode(Func<string> title)
        {
            DebugMenuNode node = new DebugMenuNode(title);
            _nodes.Add(node);
            return node;
        }

        public void RemoveNode(DebugMenuNode node)
        {
            if(null == node) {
                return;
            }

            _nodes.Remove(node);

            if(_currentNode == node) {
                SetCurrentNode(null);
            }
        }

        public void SetCurrentNode(DebugMenuNode node)
        {
            _currentNode = node;
            _windowScrollPos = Vector2.zero;
        }

        public void ResetFrameStats()
        {
            _lastFrameTime = 0;
            _minFrameTime = float.MaxValue;
            _maxFrameTime = float.MinValue;

            _fpsAccumulator.Clear();
        }

        private void UpdateFrameStats(float dt)
        {
            _lastFrameTime = dt;

            if(_lastFrameTime < _minFrameTime) {
                _minFrameTime = _lastFrameTime;
            }

            if(_lastFrameTime > _maxFrameTime) {
                _maxFrameTime = _lastFrameTime;
            }

            _fpsAccumulator.Enqueue(1.0f / _lastFrameTime);
            if(_fpsAccumulator.Count > _fpsAccumulatorSize) {
                _fpsAccumulator.Dequeue();
            }
        }

        private void RenderWindowContents()
        {
            if(null == _currentNode) {
                GUILayout.Label($"Frame Time: {(int)(_lastFrameTime * 1000.0f)} ms");
                GUILayout.Label($"Min Frame Time: {(int)(_minFrameTime * 1000.0f)} ms");
                GUILayout.Label($"Max Frame Time: {(int)(_maxFrameTime * 1000.0f)} ms");
                GUILayout.Label($"Average FPS: {(int)AverageFPS}");

                _windowScrollPos = GUILayout.BeginScrollView(_windowScrollPos);
                    foreach(DebugMenuNode node in _nodes) {
                        node.RenderNode();
                    }
                GUILayout.EndScrollView();

                if(GUILayout.Button("Quit", GUIUtils.GetLayoutButtonSize("Quit"))) {
                    Application.Quit();
                }
            } else {
                _windowScrollPos = GUILayout.BeginScrollView(_windowScrollPos);
                    _currentNode.RenderContents();
                GUILayout.EndScrollView();

                if(GUILayout.Button("Back", GUIUtils.GetLayoutButtonSize("Back"))) {
                    SetCurrentNode(_currentNode.Parent);
                }
            }
        }

        private void InitLogMessageDebugNode()
        {
            DebugMenuNode debugMenuNode = AddNode(() => "Logs");
            debugMenuNode.RenderContentsAction = () => {
#if USE_LOG_MESSAGE_BUFFER
                GUIStyle style = GUI.skin.textArea;
                style.richText = true;
                GUILayout.TextArea(_logMessageBuffer.ToString(), style);
#else
                foreach(LogMessage message in _logMessages) {
                    switch(message.type)
                    {
                    case LogType.Assert:
                        GUI.color = Color.green;
                        break;
                    case LogType.Warning:
                        GUI.color = Color.yellow;
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                        GUI.color = Color.red;
                        break;
                    default:
                        GUI.color = Color.white;
                        break;
                    }
                    GUILayout.Label($"[{message.type}]: {message.message}");
                }
#endif
            };
        }

#region Event Handlers
        private void OnLogMessageReceived(string logString, string stackTrace, LogType type)
        {
#if USE_LOG_MESSAGE_BUFFER
            Color color;
            switch(type)
            {
            case LogType.Assert:
                color = Color.green;
                break;
            case LogType.Warning:
                color = Color.yellow;
                break;
            case LogType.Error:
            case LogType.Exception:
                color = Color.red;
                break;
            default:
                color = Color.white;
                break;
            }
            _logMessageBuffer.AppendLine($"<color={color}>[{type}]: {logString}</color>");
#else
            _logMessages.Enqueue(new LogMessage{ message = logString, type = type });
            if(_logMessages.Count > _maxLogMessages) {
                _logMessages.Dequeue();
            }
#endif
        }
#endregion
    }
}
