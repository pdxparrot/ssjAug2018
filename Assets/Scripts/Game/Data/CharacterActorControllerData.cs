using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="CharacterActorControllerData", menuName="pdxpartyparrot/Game/Data/CharacterActorController Data")]
    [Serializable]
    public sealed class CharacterActorControllerData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _collisionCheckLayerMask;

        public LayerMask CollisionCheckLayerMask => _collisionCheckLayerMask;

        [Space(10)]

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

        [SerializeField]
        private string _hoverParam = "Hover";

        public string HoverParam => _hoverParam;
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
        [Range(0, 50)]
        [Tooltip("How high does the character jump")]
        private float _jumpHeight = 1.0f;

        public float JumpHeight => _jumpHeight;
#endregion

        [Space(10)]

#region Double Jump
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

#region Hover
        [Header("Hover")]

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("How long to hold jump before hovering starts")]
        private float _hoverHoldSeconds = 1.0f;

        public float HoverHoldSeconds => _hoverHoldSeconds;

        [SerializeField]
        [Range(0, 60)]
        [Tooltip("Max time hover can last")]
        private float _hoverTimeSeconds = 10.0f;

        public float HoverTimeSeconds => _hoverTimeSeconds;

        [SerializeField]
        [Range(0, 60)]
        private float _hoverCooldownSeconds = 1.0f;

        public float HoverCooldownSeconds => _hoverCooldownSeconds;

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

        [SerializeField]
        private bool _hoverWhenGrounded;

        public bool HoverWhenGrounded => _hoverWhenGrounded;
#endregion

        [Space(10)]

        [SerializeField]
        [Tooltip("Allow movement while not grounded")]
        private bool _allowAirControl;

        public bool AllowAirControl => _allowAirControl;
    }
}
