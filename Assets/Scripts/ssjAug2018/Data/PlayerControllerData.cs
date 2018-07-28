using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerControllerData", menuName="ssjAug2018/Data/PlayerControllerData Data")]
    [Serializable]
    public sealed class PlayerControllerData : ScriptableObject
    {
        [SerializeField]
        private float _attachDistance = 0.1f;

        public float AttachDistance => _attachDistance;

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

#region Jumping
        [Header("Jumping")]

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

#region Hover
        [Header("Hover")]

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("How long to hold jump before hovering starts")]
        private float _hoverHoldSeconds = 1.0f;

        public int HoverHoldMs => (int)(_hoverHoldSeconds * 1000.0f);

        [SerializeField]
        [Range(0, 60)]
        [Tooltip("Max time hover can last")]
        private float _hoverTimeSeconds = 10.0f;

        public int HoverTimeMs => (int)(_hoverTimeSeconds * 1000.0f);

        [SerializeField]
        [Range(0, 60)]
        private float _hoverCooldownSeconds = 1.0f;

        public int HoverCooldownMs => (int)(_hoverCooldownSeconds * 1000.0f);

        [SerializeField]
        [Range(0, 60)]
        [Tooltip("Seconds of charge to recover every second after cooldown")]
        private float _hoverRechargeRate = 1.0f;

        public float HoverRechargeRate => _hoverRechargeRate;

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("The acceleration caused by hovering")]
        private float _hoverAcceleration = 20.0f;

        public float HoverAcceleration => _hoverAcceleration;
#endregion
    }
}
