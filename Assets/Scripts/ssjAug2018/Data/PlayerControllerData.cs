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

#region Physics
        [SerializeField]
        [Range(0, 50)]
        private float _climbSpeed = 1.0f;

        public float ClimbSpeed => _climbSpeed;
#endregion

#region Raycasts
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
    }
}
