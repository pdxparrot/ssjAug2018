using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="ThirdPersonControllerData", menuName="pdxpartyparrot/Game/Data/ThirdPersonController Data")]
    [Serializable]
    public sealed class ThirdPersonControllerData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _collisionCheckLayerMask;

        public LayerMask CollisionCheckLayerMask => _collisionCheckLayerMask;

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _moveXAxisParam = "InputX";

        public string MoveXAxisParam => _moveXAxisParam;

        [SerializeField]
        private string _moveZAxisParam = "InputZ";

        public string MoveZAxisParam => _moveZAxisParam;

        [SerializeField]
        private string _groundedParam = "Landed";

        public string GroundedParam => _groundedParam;

        [SerializeField]
        private string _fallingParam = "Falling";

        public string FallingParam => _fallingParam;

        [SerializeField]
        private string _jumpParam = "Jump";

        public string JumpParam => _jumpParam;

        [SerializeField]
        private string _doubleJumpParam = "DoubleJump";

        public string DoubleJumpParam => _doubleJumpParam;
#endregion

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [Range(0, 50)]
        [Tooltip("Move speed in m/s")]
        private float _moveSpeed = 1.0f;

        public float MoveSpeed => _moveSpeed;

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("The controller-based threshold that we consider the character to be running")]
        private float _runThreshold = 1.0f;

        public float RunThreshold => _runThreshold;

        public float RunThresholdSquared => RunThreshold * RunThreshold;

        [SerializeField]
        [Range(0, 500)]
        [Tooltip("Add this many m/s to the player's fall speed, to make movement feel better without changing actual gravity")]
        private float _fallSpeedAdjustment = 0.0f;

        public float FallSpeedAdjustment => _fallSpeedAdjustment;

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("The characters terminal velocity in m/s")]
        private float _terminalVelocity = 50.0f;

        public float TerminalVelocity => _terminalVelocity;

        [SerializeField]
        [Tooltip("Max distance from the ground that the character is considered grounded")]
        private float _groundedCheckEpsilon = 0.1f;

        public float GroundedCheckEpsilon => _groundedCheckEpsilon;
#endregion

        [Space(10)]

#region Jumping
        [Header("Jumping")]

        [SerializeField]
        private bool _enableJumping = true;

        public bool EnableJumping => _enableJumping;

        [SerializeField]
        [Range(0, 50)]
        [Tooltip("How high does the character jump")]
        private float _jumpHeight = 1.0f;

        public float JumpHeight => _jumpHeight;

        [SerializeField]
        [Range(0, 50)]
        [Tooltip("How high does the character jump when double jumping")]
        private float _doubleJumpHeight = 1.0f;

        public float DoubleJumpHeight => _doubleJumpHeight;

        [SerializeField]
        [Tooltip("How many times is the player able to double jump (-1 is infinite)")]
        private int _doubleJumpCount;

        public int DoubleJumpCount => _doubleJumpCount;
#endregion

        [Space(10)]

        [SerializeField]
        [Tooltip("Allow movement while not grounded")]
        private bool _allowAirControl;

        public bool AllowAirControl => _allowAirControl;
    }
}
