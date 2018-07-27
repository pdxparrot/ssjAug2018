using System;
using System.Collections.Generic;
using System.Linq;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Core.Input
{
    // https://github.com/Unity-Technologies/InputSystem/wiki/How-Do-I...
    // https://github.com/Unity-Technologies/InputSystem/wiki/OldVsNewInputSystem

    public sealed class InputManager : SingletonBehavior<InputManager>
    {
        private static int _lastListenerId;

        private static int NextListenerId => ++_lastListenerId;

        private class GamepadListener
        {
            public int id;

            public Action<Gamepad> acquireCallback;

            public Action<Gamepad> disconnectCallback;
        }

#region Controls
        [SerializeField]
        private Controls _controls;

        public Controls Controls => _controls;
#endregion

        [SerializeField]
        private EventSystem _eventSystemPrefab;

        public EventSystem EventSystem { get; private set; }

#region Gamepads
        private readonly List<Gamepad> _unacquiredGamepads = new List<Gamepad>();

        private readonly Dictionary<Gamepad, GamepadListener> _acquiredGamepads = new Dictionary<Gamepad, GamepadListener>();

        private readonly List<GamepadListener> _gamepadListeners = new List<GamepadListener>();
#endregion

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();

            if(PartyParrotManager.Instance.EnableGoogleVR) {
                // TODO
            } else {
                Debug.Log("Creating EventSystem (no VR)...");
                EventSystem = Instantiate(_eventSystemPrefab, transform);
            }

            InitGamepads();

            InputSystem.onDeviceChange += OnDeviceChange;
        }

        protected override void OnDestroy()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;

            Destroy(EventSystem.gameObject);
            EventSystem = null;

            base.OnDestroy();
        }
#endregion

#region Gamepads
        public int AcquireGamepad(Action<Gamepad> acquireCallback, Action<Gamepad> disconnectCallback)
        {
            GamepadListener listener = new GamepadListener
            {
                id = NextListenerId,
                acquireCallback = acquireCallback,
                disconnectCallback = disconnectCallback
            };

            if(_unacquiredGamepads.Count < 1) {
                _gamepadListeners.Add(listener);
                return listener.id;
            }

            Gamepad gamepad = _unacquiredGamepads.RemoveFront();
            listener.acquireCallback.Invoke(gamepad);
            _acquiredGamepads[gamepad] = listener;

            return listener.id;
        }

        public void ReleaseGamepad(int listenerId)
        {
            if(listenerId < 1) {
                return;
            }

            _gamepadListeners.RemoveAll(x => x.id == listenerId);

            List<Gamepad> remove = new List<Gamepad>();
            foreach(var kvp in _acquiredGamepads) {
                if(kvp.Value.id == listenerId) {
                    remove.Add(kvp.Key);
                }
            }

            foreach(Gamepad gamepad in remove) {
                _acquiredGamepads.Remove(gamepad);
                _unacquiredGamepads.Add(gamepad);
            }
        }

        private void InitGamepads()
        {
            var gamepads = from device in InputSystem.devices where device is Gamepad select (Gamepad)device;
            foreach(Gamepad gamepad in gamepads) {
                _unacquiredGamepads.Add(gamepad);
            }
            Debug.Log($"Found {_unacquiredGamepads.Count} gamepads");
        }

        private void AddGamepad(Gamepad gamepad)
        {
            Debug.Log("Gamepad added");

            if(!NotifyAddGamepad(gamepad)) {
                _unacquiredGamepads.Add(gamepad);
            }
        }

        private void RemoveGamepad(Gamepad gamepad)
        {
            Debug.Log("Gamepad removed");

            if(_unacquiredGamepads.Remove(gamepad)) {
                return;
            }

            NotifyRemoveGamepad(gamepad);
        }

        private bool NotifyAddGamepad(Gamepad gamepad)
        {
            if(_gamepadListeners.Count < 1) {
                return false;
            }

            GamepadListener listener = _gamepadListeners.RemoveFront();
            listener.acquireCallback.Invoke(gamepad);
            _acquiredGamepads[gamepad] = listener;
            return true;
        }

        private void NotifyRemoveGamepad(Gamepad gamepad)
        {
            GamepadListener listener = _acquiredGamepads.GetOrDefault(gamepad);
            listener?.disconnectCallback.Invoke(gamepad);
            _acquiredGamepads.Remove(gamepad);
            _gamepadListeners.Add(listener);
        }
#endregion

#region Event Handlers
        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if(device is Gamepad) {
                OnGamepadChange((Gamepad)device, change);
                return;
            }
        }

        private void OnGamepadChange(Gamepad gamepad, InputDeviceChange change)
        {
            switch (change)
            {
            case InputDeviceChange.Added:
                AddGamepad(gamepad);
                break;
            case InputDeviceChange.Enabled:
                Debug.LogWarning("Unhandled gamepad enabled");
                break;
            case InputDeviceChange.Removed:
                RemoveGamepad(gamepad);
                break;
            case InputDeviceChange.Disabled:
                Debug.LogWarning("Unhandled gamepad disabled");
                break;
            case InputDeviceChange.StateChanged:
                break;
            default:
                Debug.LogWarning($"Unhandled gamepad change: {change}");
                break;
            }
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "InputManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Gamepads", GUI.skin.box);
                    GUILayout.Label($"Queued listeners: {_gamepadListeners.Count}");

                    GUILayout.BeginVertical("Unacquired:", GUI.skin.box);
                        foreach(Gamepad gamepad in _unacquiredGamepads) {
                            GUILayout.Label(gamepad.name);
                        }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("Acquired:", GUI.skin.box);
                        foreach(var kvp in _acquiredGamepads) {
                            GUILayout.Label($"{kvp.Key.name}:{kvp.Value.id}");
                        }
                    GUILayout.EndVertical();
                GUILayout.EndVertical();
            };
        }
    }
}

