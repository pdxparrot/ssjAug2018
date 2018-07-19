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
        [SerializeField]
        private Controls _controls;

        public Controls Controls => _controls;

        [SerializeField]
        private EventSystem _eventSystemPrefab;

        public EventSystem EventSystem { get; private set; }

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

// TODO: acquire/release gamepads

        private List<Gamepad> GetGamepads()
        {
            return (from device in InputSystem.devices where device is Gamepad select (Gamepad)device).ToList();
        }

#region Event Handlers
        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
            case InputDeviceChange.Added:
                Debug.Log("Input device added");
                break;
            case InputDeviceChange.Removed:
                Debug.Log("Input device removed");
                break;
            case InputDeviceChange.Enabled:
                Debug.Log("Input device enabled");
                break;
            case InputDeviceChange.Disabled:
                Debug.Log("Input device disabled");
                break;
            default:
                // not sure if we need to handle the rest of these
                break;
            }
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "InputManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Joysticks", GUI.skin.box);
                    var gamepads = GetGamepads();
                    foreach(Gamepad gamepad in gamepads) {
                        GUILayout.Label(gamepad.name);
                    }
                GUILayout.EndVertical();
            };
        }
    }
}

