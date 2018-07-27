using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core
{
    public sealed class PartyParrotManager : SingletonBehavior<PartyParrotManager>
    {
#region Events
        public event EventHandler<EventArgs> PauseEvent;
#endregion

#region VR Config
        [SerializeField]
        private bool _enableVR;

        public bool EnableVR => _enableVR;

        [SerializeField]
        private bool _enableGoogleVR;

        public bool EnableGoogleVR => _enableGoogleVR;
#endregion

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        private PhysicMaterial _frictionlesssMaterial;

        public PhysicMaterial FrictionlessMaterial => _frictionlesssMaterial;
#endregion

        [Space(10)]

#region Game State
        [Header("Game State")]

        [SerializeField]
        [ReadOnly]
        private bool _isPaused;

        public bool IsPaused => _isPaused;
#endregion

#region Unity Lifecycle
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;

            Debug.Log($"Gravity: {Physics.gravity}");
        }
#endregion

#region Player Config

#region Bool
        public bool GetBool(string key)
        {
            return GetInt(key) != 0;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            return GetInt(key, defaultValue ? 1 : 0) != 0;
        }

        public void SetBool(string key, bool value)
        {
            SetInt(key, value ? 1 : 0);
        }
#endregion

#region Int
        public int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
#endregion

#region Float
        public float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
#endregion

#region String
        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public string GetString(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
#endregion

#region Generic Objects
        public T Get<T>(string key) where T: class
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        }

        public T Get<T>(string key, T defaultValue) where T: class
        {
            return PlayerPrefs.HasKey(key) ? JsonUtility.FromJson<T>(PlayerPrefs.GetString(key)) : defaultValue;
        }

        public void Set<T>(string key, T value) where T: class
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
        }
#endregion

#endregion

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            Debug.Log($"Pause: {_isPaused}");

            PauseEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
