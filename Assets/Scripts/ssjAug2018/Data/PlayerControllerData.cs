using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerControllerData", menuName="pdxpartyparrot/ssjAug2018/Data/PlayerControllerData Data")]
    [Serializable]
    public sealed class PlayerControllerData : ScriptableObject
    {
        [SerializeField]
        private float _attachDistance = 0.1f;

        public float AttachDistance => _attachDistance;

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _climbingParam = "Climbing";

        public string ClimbingParam => _climbingParam;

        [SerializeField]
        private string _longJumpParam = "LongJump";

        public string LongJumpParam => _longJumpParam;

        [SerializeField]
        private string _throwingMailParam = "ReadyMail";

        public string ThrowingMailParam => _throwingMailParam;

        [SerializeField]
        private string _throwMailParam = "ThrowMail";

        public string ThrowMailParam => _throwMailParam;

        [SerializeField]
        private float _wrapTimeSeconds = 1.0f;

        public float WrapTimeSeconds => _wrapTimeSeconds;

        [SerializeField]
        private float _climbUpTimeSeconds = 1.0f;

        public float ClimbUpTimeSeconds => _climbUpTimeSeconds;
#endregion

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [Range(0, 50)]
        private float _climbSpeed = 1.0f;

        public float ClimbSpeed => _climbSpeed;
#endregion

        [Space(10)]

#region Raycasts
        [Header("Raycasts")]

        [SerializeField]
        [Range(0, 10)]
        private float _armRayLength = 1.0f;

        public float ArmRayLength => _armRayLength;

        [SerializeField]
        [Range(0, 90)]
        private float _wrapAroundAngle = 45.0f;

        public float WrapAroundAngle => _wrapAroundAngle;

        [SerializeField]
        [Range(0, 10)]
        private float _headRayLength = 1.0f;

        public float HeadRayLength => _headRayLength;

        [SerializeField]
        [Range(0, 90)]
        private float _headRayAngle = 45.0f;

        public float HeadRayAngle => _headRayAngle;

        [SerializeField]
        [Range(0, 10)]
        private float _chestRayLength = 1.0f;

        public float ChestRayLength => _chestRayLength;

        [SerializeField]
        [Range(0, 10)]
        private float _footRayLength = 1.0f;

        public float FootRayLength => _footRayLength;

        [SerializeField]
        [Range(0, 90)]
        private float _footRayAngle = 45.0f;

        public float FootRayAngle => _footRayAngle;
#endregion

        [Space(10)]

#region Grabbing
        [SerializeField]
        private bool _enableGrabbing = true;

        public bool EnableGrabbing => _enableGrabbing;
#endregion

        [Space(10)]

#region Long Jumping
        [Header("Long Jumping")]

        [SerializeField]
        private bool _enableLongJump = true;

        public bool EnableLongJump => _enableLongJump;

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("How long to hold jump before allowing a long jump")]
        private float _longJumpHoldSeconds = 1.0f;

        public int LongJumpHoldMs => (int)(_longJumpHoldSeconds * 1000.0f);

        [SerializeField]
        [Range(0, 50)]
        [Tooltip("How high does the character jump when long jumping")]
        private float _longJumpHeight = 5.0f;

        public float LongJumpHeight => _longJumpHeight;
#endregion

        [Space(10)]

#region Throwing
        [Header("Throwing")]

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("Player will auto throw after this many seconds")]
        private float _autoThrowSeconds = 1.0f;

        public int AutoThrowMs => (int)(_autoThrowSeconds * 1000.0f);

        [SerializeField]
        private float _throwSpeed = 5.0f;

        public float ThrowSpeed => _throwSpeed;
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
