using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerControllerData", menuName="pdxpartyparrot/ssjAug2018/Data/PlayerControllerData Data")]
    [Serializable]
    public sealed class PlayerControllerData : ScriptableObject
    {
#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _throwingMailParam = "ReadyMail";

        public string ThrowingMailParam => _throwingMailParam;

        [SerializeField]
        private string _throwMailParam = "ThrowMail";

        public string ThrowMailParam => _throwMailParam;

        [SerializeField]
        private string _throwingSnowballParam = "ReadySnowball";

        public string ThrowingSnowballParam => _throwingSnowballParam;

        [SerializeField]
        private string _throwSnowballParam = "ThrowSnowball";

        public string ThrowSnowballParam => _throwSnowballParam;
#endregion

        [Space(10)]

#region Throwing
        [Header("Throwing")]

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("Player will auto throw after this many seconds")]
        private float _autoThrowSeconds = 1.0f;

        public float AutoThrowSeconds => _autoThrowSeconds;

        [SerializeField]
        private float _throwSpeed = 5.0f;

        public float ThrowSpeed => _throwSpeed;

        [SerializeField]
        private float _throwConvergeDistance = 100.0f;

        public float ThrowConvergeDistance => _throwConvergeDistance;
#endregion

        [Space(10)]

#region Falling
        [Header("Falling")]

        [SerializeField]
        [Tooltip("Enable to stun the player on large falls")]
        private bool _enableFallStun;

        public bool EnableFallStun => _enableFallStun;

        [SerializeField]
        [Range(1, 100)]
        [Tooltip("How far can the player fall without getting stunned")]
        private float _fallStunDistance = 5.0f;

        public float FallStunDistance => _fallStunDistance;

        [SerializeField]
        private float _fallStunTimeSeconds;

        public float FallStunTimeSeconds => _fallStunTimeSeconds;
#endregion
    }
}
