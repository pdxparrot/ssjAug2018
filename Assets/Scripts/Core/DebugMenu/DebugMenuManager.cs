using System;
using System.Collections.Generic;

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

        private float _lastFPS;

#region Unity Lifecycle
        private void Awake()
        {
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

            _lastFPS = 1.0f / Time.unscaledDeltaTime;
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

        private void RenderWindowContents()
        {
            if(null == _currentNode) {
                GUILayout.Label($"FPS: {(int)_lastFPS}");

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
