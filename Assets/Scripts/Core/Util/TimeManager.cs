using System;

using pdxpartyparrot.Core.DebugMenu;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public sealed class TimeManager : SingletonBehavior<TimeManager>
    {
        public readonly DateTime Epoch = new DateTime(1970, 1, 1);

        public static long SecondsToMilliseconds(float seconds)
        {
            return (long)(seconds * 1000.0f);
        }

        [SerializeField]
        private float _offsetSeconds = 0;

        public double CurrentUnixSeconds => DateTime.UtcNow.Subtract(Epoch).TotalSeconds + _offsetSeconds;

        public long CurrentUnixMs => (long)DateTime.UtcNow.Subtract(Epoch).TotalMilliseconds + SecondsToMilliseconds(_offsetSeconds);

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.TimeManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Current Unix Seconds: {CurrentUnixSeconds}");
                GUILayout.Label($"Current Unix Milliseconds: {CurrentUnixMs}");
            };
        }
    }
}
