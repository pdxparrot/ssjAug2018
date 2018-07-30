using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.DebugMenu
{
// TODO: collect logs in a log window here also

    public sealed class DebugMenuManager : SingletonBehavior<DebugMenuManager>
    {
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

        private float AverageFPS => _fpsAccumulator.Average();

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
            _nodes.Remove(node);
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

                if(GUILayout.Button("Quit", GUILayout.Width(100), GUILayout.Height(25))) {
                    Application.Quit();
                }
            } else {
                _windowScrollPos = GUILayout.BeginScrollView(_windowScrollPos);
                    _currentNode.RenderContents();
                GUILayout.EndScrollView();

                if(GUILayout.Button("Back", GUILayout.Width(100), GUILayout.Height(25))) {
                    SetCurrentNode(_currentNode.Parent);
                }
            }
        }
    }
}
