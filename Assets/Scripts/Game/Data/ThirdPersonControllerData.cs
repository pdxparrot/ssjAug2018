using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="ThirdPersonControllerData", menuName="ssjAug2018/Data/ThirdPersonController Data")]
    [Serializable]
    public sealed class ThirdPersonControllerData : ScriptableObject
    {
#region Physics
        [SerializeField]
        [Range(0, 50)]
        private float _moveSpeed = 1.0f;

        public float MoveSpeed => _moveSpeed;

        [SerializeField]
        [Range(0, 50)]
        private float _jumpHeight = 1.0f;

        public float JumpHeight => _jumpHeight;

        [SerializeField]
        [Range(0, 10)]
        private float _fallSpeedAdjustment = 0.0f;

        public float FallSpeedAdjustment => _fallSpeedAdjustment;

        [SerializeField]
        [Range(0, 100)]
        private float _terminalVelocity = 50.0f;

        public float TerminalVelocity => _terminalVelocity;

        [SerializeField]
        private float _groundedCheckEpsilon = 0.1f;

        public float GroundedCheckEpsilon => _groundedCheckEpsilon;
#endregion

        [SerializeField]
        private bool _allowAirControl;

        public bool AllowAirControl => _allowAirControl;
    }
}
