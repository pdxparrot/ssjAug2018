using System;
using System.Collections;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.UI;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;

namespace pdxpartyparrot.ssjAug2018.Players
{
    [RequireComponent(typeof(JumpControllerComponent))]
    [RequireComponent(typeof(DoubleJumpControllerComponent))]
    [RequireComponent(typeof(HoverControllerComponent))]
    public sealed class PlayerController : CharacterActorController
    {
        private enum MovementState
        {
            Platforming,
            Climbing
        }

        [SerializeField]
        private PlayerControllerData _playerControllerData;

#region Movement State
        [Header("Movement State")]

        [SerializeField]
        [ReadOnly]
        private MovementState _movementState = MovementState.Platforming;

        public bool IsClimbing => MovementState.Climbing == _movementState;

        public bool IsGrabbing => IsClimbing;

        [SerializeField]
        [ReadOnly]
        private float _fallStartHeight = float.MinValue;
#endregion

        [Space(10)]

#region Hands
        [Header("Hands")]

        [SerializeField]
        [FormerlySerializedAs("_leftGrabCheckTransform")]
        private Transform _leftHandTransform;

        private RaycastHit? _leftHandHitResult;

        private bool CanGrabLeft => null != _leftHandHitResult;

        [SerializeField]
        [FormerlySerializedAs("_rightGrabCheckTransform")]
        private Transform _rightHandTransform;

        private RaycastHit? _rightHandHitResult;

        private bool CanGrabRight => null != _rightHandHitResult;
#endregion

        [Space(10)]

#region Head
        [Header("Head")]

        [SerializeField]
        private Transform _headTransform;

        private RaycastHit? _headHitResult;
#endregion

        [Space(10)]

#region Chest
        [Header("Chest")]

        [SerializeField]
        private Transform _chestTransform;

        private RaycastHit? _chestHitResult;
#endregion

        private bool CanClimbUp => IsClimbing && (null == _headHitResult && null != _chestHitResult);

        [Space(10)]

#region Long Jump
        [Header("Long Jump")]

        [SerializeField]
        [ReadOnly]
        private long _longJumpTriggerTime;

        private bool CanLongJump => _longJumpTriggerTime > 0 && TimeManager.Instance.CurrentUnixMs >= _longJumpTriggerTime;
#endregion

        [Space(10)]

#region Throwing
        [Header("Throwing")]

        [SerializeField]
        [ReadOnly]
        private bool _canThrowMail;

        [SerializeField]
        private long _autoThrowMailTriggerTime;

        private bool ShouldAutoThrowMail => _autoThrowMailTriggerTime > 0 && TimeManager.Instance.CurrentUnixMs >= _autoThrowMailTriggerTime;

        [SerializeField]
        [ReadOnly]
        private bool _canThrowSnowball;
#endregion

        [Space(10)]

#region Aiming
        [Header("Aiming")]

        [SerializeField]
        [ReadOnly]
        private bool _isAiming;

        public bool IsAiming => _isAiming;
#endregion

        [Space(10)]

        [SerializeField]
        [Tooltip("Debug break when grabbing fails")]
        private bool _breakOnFall;

        public override bool CanMove => base.CanMove && !IsAnimating && !Player.IsStunned && !Player.IsDead;

        public Player Player => (Player)Owner;

#region Components
        [CanBeNull]
        private DoubleJumpControllerComponent _doubleJumpComponent;

        [CanBeNull]
        public HoverControllerComponent HoverComponent { get; private set; }
#endregion

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitDebugMenu();

            Debug.Assert(Math.Abs(_leftHandTransform.position.y - _rightHandTransform.position.y) < float.Epsilon, "Player hands are at different heights!");
            Debug.Assert(_headTransform.position.y > _leftHandTransform.position.y, "Player head should be above player hands!");
            Debug.Assert(_chestTransform.position.y < _leftHandTransform.position.y, "Player chest should be below player hands!");

            _doubleJumpComponent = GetControllerComponent<DoubleJumpControllerComponent>();
            HoverComponent = GetControllerComponent<HoverControllerComponent>();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();

            base.OnDestroy();
        }

        protected override void Update()
        {
            base.Update();

            float dt = Time.deltaTime;

            UpdateLongJumping(dt);
            UpdateThrowing(dt);
        }

        protected override void FixedUpdate()
        {
            bool wasFalling = IsFalling;

            base.FixedUpdate();

            if(!wasFalling && IsFalling) {
                _fallStartHeight = Rigidbody.position.y;
            } else if(wasFalling && !IsFalling && _playerControllerData.EnableFallStun) {
                float distance = _fallStartHeight - Rigidbody.position.y;
                if(distance >= _playerControllerData.FallStunDistance) {
                    Stun();
                }
                _fallStartHeight = float.MinValue;
            }

            Rigidbody.angularVelocity = Vector3.zero;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

// TODO: encapsulate the math here so we a) don't duplicate it in the raycast methods and b) guarantee we always match the math done in the raycast methods

            // left hand
            Gizmos.color = null != _leftHandHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + transform.forward * _playerControllerData.ArmRayLength);
            if(IsClimbing) {
                //if(!CanGrabLeft && CanGrabRight) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(_playerControllerData.WrapAroundAngle, transform.up) * transform.forward) * _playerControllerData.ArmRayLength * 2.0f);
                //}

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(-90.0f, transform.up) * transform.forward) * _playerControllerData.ArmRayLength * 0.5f);
            }

            // right hand
            Gizmos.color = null != _rightHandHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + transform.forward * _playerControllerData.ArmRayLength);
            if(IsClimbing) {
                //if(CanGrabLeft && !CanGrabRight) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(-_playerControllerData.WrapAroundAngle, transform.up) * transform.forward) * _playerControllerData.ArmRayLength * 2.0f);
                //}

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(90.0f, transform.up) * transform.forward) * _playerControllerData.ArmRayLength * 0.5f);
            }

            if(IsClimbing) {
                // head
                Gizmos.color = null != _headHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_headTransform.position, _headTransform.position + (Quaternion.AngleAxis(-_playerControllerData.HeadRayAngle, transform.right) * transform.forward) * _playerControllerData.HeadRayLength);

                if(!CanGrabLeft && !CanGrabRight && CanClimbUp) {
                    Gizmos.color = Color.white;
                    Vector3 start = _headTransform.position + (Quaternion.AngleAxis(-_playerControllerData.HeadRayAngle, transform.right) * transform.forward) * _playerControllerData.HeadRayLength;
                    Vector3 end = start + Player.CapsuleCollider.height * -Vector3.up;
                    Gizmos.DrawLine(start, end);
                }

                // chest
                Gizmos.color = null != _chestHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_chestTransform.position, _chestTransform.position + transform.forward * _playerControllerData.ChestRayLength);
            }

            // feet
/*
            if(!IsGrabbing && IsGrounded) {
                Gizmos.color = null != _footHitResults[0] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(0.0f, transform.up) * Quaternion.AngleAxis(_playerControllerData.FootRayAngle, transform.right) * transform.forward) * _playerControllerData.FootRayLength);
                Gizmos.color = null != _footHitResults[1] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(90.0f, transform.up) * Quaternion.AngleAxis(_playerControllerData.FootRayAngle, transform.right) * transform.forward) * _playerControllerData.FootRayLength);
                Gizmos.color = null != _footHitResults[2] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(180.0f, transform.up) * Quaternion.AngleAxis(_playerControllerData.FootRayAngle, transform.right) * transform.forward) * _playerControllerData.FootRayLength);
                Gizmos.color = null != _footHitResults[3] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(270.0f, transform.up) * Quaternion.AngleAxis(_playerControllerData.FootRayAngle, transform.right) * transform.forward) * _playerControllerData.FootRayLength);
            }
*/
        }
#endregion

        public void Initialize(Player player)
        {
            base.Initialize(player);

            StartCoroutine(RaycastRoutine());
        }

        public override void AnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove || IsGrabbing) {
                return;
            }

            base.AnimationMove(axes, dt);
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(IsGrabbing) {
                Vector3 velocity = Rigidbody.rotation * (axes * _playerControllerData.ClimbSpeed);
                if(IsGrounded && velocity.y < 0.0f) {
                    velocity.y = 0.0f;
                }
                Rigidbody.MovePosition(Rigidbody.position + velocity * dt);
            } else {
                base.PhysicsMove(axes, dt);
            }
        }

#region Actions
        public void Grab()
        {
            if(!CanMove) {
                return;
            }

            if(!_playerControllerData.EnableGrabbing || IsGrabbing || (!CanGrabLeft && !CanGrabRight)) {
                return;
            }

            EnableClimbing(true);

            if(null != _leftHandHitResult) {
                AttachToSurface(_leftHandHitResult.Value);
            } else if(null != _rightHandHitResult) {
                AttachToSurface(_rightHandHitResult.Value);
            }
        }

        public void Drop()
        {
            if(!CanMove) {
                return;
            }

            if(!_playerControllerData.EnableGrabbing) {
                return;
            }

            if(IsGrabbing) {
                DisableGrabbing();
            } else {
                CheckDropDown();
            }
        }

// TODO: enable/disable aim

        public void StartAim()
        {
            if(!CanMove) {
                return;
            }

            _isAiming = true;

            Debug.Log("TODO: zoom and aim!");

            UIManager.Instance.PlayerUI.PlayerHUD.ShowAimer(true);
        }

        public void Aim()
        {
            UIManager.Instance.PlayerUI.PlayerHUD.ShowAimer(false);

            _isAiming = false;
        }

// TODO: enable/disable throwing mail

        public void StartThrowMail()
        {
            if(!CanMove || !Player.CanThrowMail) {
                return;
            }

            _canThrowMail = true;

            _autoThrowMailTriggerTime = TimeManager.Instance.CurrentUnixMs + _playerControllerData.AutoThrowMs;

            Player.Animator.SetBool(_playerControllerData.ThrowingMailParam, true);
        }

        public void ThrowMail()
        {
            if(!CanMove) {
                return;
            }

            if(_canThrowMail) {
                DoThrowMail();
            }

            Player.Animator.SetBool(_playerControllerData.ThrowingMailParam, false);

            _canThrowMail = true;
        }

        private void DoThrowMail()
        {
            _autoThrowMailTriggerTime = 0;

            if(null == Player.Viewer) {
                Debug.LogWarning("Non-local player doing a throw!");
                return;
            }

            Player.CmdThrowMail(_rightHandTransform.position, /*!IsAiming ? Player.transform.forward :*/ Player.Viewer.transform.forward, _playerControllerData.ThrowSpeed);

            Player.Animator.SetTrigger(_playerControllerData.ThrowMailParam);
        }

// TODO: enable/disable throwing snowballs

        public void StartThrowSnowball()
        {
            if(!CanMove) {
                return;
            }

            _canThrowSnowball = true;

            //Player.Animator.SetBool(_playerControllerData.ThrowingSnowballParam, true);
        }

        public void ThrowSnowball()
        {
            if(!CanMove) {
                return;
            }

            if(_canThrowSnowball) {
                DoThrowSnowball();
            }

            //Player.Animator.SetBool(_playerControllerData.ThrowingSnowballParam, false);

            _canThrowSnowball = true;
        }

        private void DoThrowSnowball()
        {
            if(null == Player.Viewer) {
                Debug.LogWarning("Non-local player doing a throw!");
                return;
            }

            Player.CmdThrowSnowball(_leftHandTransform.position, /*!IsAiming ? Player.transform.forward :*/ Player.Viewer.transform.forward, _playerControllerData.ThrowSpeed);

            //Player.Animator.SetTrigger(_playerControllerData.ThrowSnowballParam);
        }

        public void LongJumpStart()
        {
            if(!CanMove) {
                return;
            }

            _longJumpTriggerTime = 0;

            if(_playerControllerData.EnableLongJump && (IsGrounded || IsGrabbing)) {
                _longJumpTriggerTime = TimeManager.Instance.CurrentUnixMs + _playerControllerData.LongJumpHoldMs;
            }
        }

        public void LongJump()
        {
            if(!CanMove) {
                return;
            }

            _longJumpTriggerTime = 0;
        }

        public override void ActionPerformed(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            if(action is JumpControllerComponent.JumpAction) {
                DisableGrabbing();
            }

            base.ActionPerformed(action);
        }
#endregion

        public void Stun()
        {
            if(IsAnimating) {
                return;
            }

            DisableGrabbing();
            if(null != HoverComponent) {
                HoverComponent.DisableHovering();
            }

            Player.Stun(_playerControllerData.FallStunTimeSeconds);
        }

        private void DisableGrabbing()
        {
            EnableClimbing(false);

            // fix our orientation, just in case
            Vector3 rotation = transform.eulerAngles;
            rotation.x = rotation.z = 0.0f;
            transform.eulerAngles = rotation;
        }

        private void EnableClimbing(bool enable)
        {
            _movementState = enable ? MovementState.Climbing : MovementState.Platforming;
            Rigidbody.isKinematic = enable;

            Player.Animator.SetBool(_playerControllerData.ClimbingParam, enable);

            if(enable && null != _doubleJumpComponent) {
                _doubleJumpComponent.Reset();
            }
        }

        private void UpdateLongJumping(float dt)
        {
            if(CanLongJump) {
                DisableGrabbing();
                DefaultJump(_playerControllerData.LongJumpHeight, _playerControllerData.LongJumpParam);
            }
        }

        private void UpdateThrowing(float dt)
        {
            if(ShouldAutoThrowMail) {
                DoThrowMail();
                _canThrowMail = false;
            }
        }

        private IEnumerator RaycastRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(RaycastRoutineRate);
            while(true) {
                UpdateRaycasts();

                HandleRaycasts();

                yield return wait;
            }
        }

        private void UpdateRaycasts()
        {
            if(IsAnimating) {
                return;
            }

            Profiler.BeginSample("PlayerController.UpdateRaycasts");
            try {
                UpdateHandRaycasts();
                UpdateHeadRaycasts();
                UpdateChestRaycasts();
            } finally {
                Profiler.EndSample();
            }
        }

#region Hand Raycasts
        private void UpdateHandRaycasts()
        {
            UpdateLeftHandRaycasts();
            UpdateRightHandRaycasts();
        }

        private void UpdateLeftHandRaycasts()
        {
            _leftHandHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_leftHandTransform.position, transform.forward, out hit, _playerControllerData.ArmRayLength, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _leftHandHitResult = hit;
                }
            }
        }

        private void UpdateRightHandRaycasts()
        {
            _rightHandHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_rightHandTransform.position, transform.forward, out hit, _playerControllerData.ArmRayLength, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _rightHandHitResult = hit;
                }
            }
        }
#endregion

#region Head Raycasts
        private void UpdateHeadRaycasts()
        {
            if(!IsClimbing) {
                return;
            }

            _headHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_headTransform.position, Quaternion.AngleAxis(-_playerControllerData.HeadRayAngle, transform.right) * transform.forward, out hit, _playerControllerData.HeadRayLength, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _headHitResult = hit;
                }
            }
        }
#endregion

#region Chest Raycasts
        private void UpdateChestRaycasts()
        {
            if(!IsClimbing) {
                return;
            }

            _chestHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_chestTransform.position, transform.forward, out hit, _playerControllerData.ChestRayLength, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _chestHitResult = hit;
                }
            }
        }
#endregion

/*
#region Foot Raycasts
        private void UpdateFootRaycasts()
        {
            if(IsGrabbing || !IsGrounded) {
                return;
            }

            UpdateFootRaycast(0, 0.0f);
            UpdateFootRaycast(1, 90.0f);
            UpdateFootRaycast(2, 180.0f);
            UpdateFootRaycast(3, 270.0f);
        }

        private void UpdateFootRaycast(int idx, float angle)
        {
            _footHitResults[idx] = null;

            RaycastHit hit;
            if(Physics.Raycast(_footTransform.position, Quaternion.AngleAxis(angle, transform.up) * Quaternion.AngleAxis(_playerControllerData.FootRayAngle, transform.right) * transform.forward, out hit, _playerControllerData.FootRayLength, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                _footHitResults[idx] = hit;
            }
        }
#endregion
*/

        private void HandleRaycasts()
        {
            Profiler.BeginSample("PlayerController.HandleRaycasts");
            try {
                if(IsClimbing) {
                    HandleClimbingRaycasts();
                }
            } finally {
                Profiler.EndSample();
            }
        }

        private void HandleClimbingRaycasts()
        {
            if(!CanGrabLeft && CanGrabRight) {
                CheckWrapLeft();
            } else if(CanGrabLeft && !CanGrabRight) {
                CheckWrapRight();
            } else if(!CanGrabLeft && !CanGrabRight) {
                if(CanClimbUp) {
                    CheckClimbUp();
                } else {
                    Debug.LogWarning("Unexpectedly fell off!");
                    DisableGrabbing();

                    if(_breakOnFall) {
                        Debug.Break();
                    }
                }
            }

            // check for non-wrap rotations
            if(!CheckRotateLeft()) {
                CheckRotateRight();
            }
        }

#region Auto-Rotate/Climb
// TODO: encapsulate the common code from these

        private bool CheckWrapLeft()
        {
            if(null == _rightHandHitResult || Driver.LastMoveAxes.x >= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_leftHandTransform.position, Quaternion.AngleAxis(_playerControllerData.WrapAroundAngle, transform.up) * transform.forward, out hit, _playerControllerData.ArmRayLength * 2.0f, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _rightHandHitResult.Value.normal) {
                return false;
            }

            _leftHandHitResult = hit;

            Vector3 offset = (Player.CapsuleCollider.radius * 2.0f) * transform.forward;
            StartAnimation(Rigidbody.position + GetSurfaceAttachmentPosition(hit, Player.CapsuleCollider.radius * hit.normal) + offset, Quaternion.LookRotation(-hit.normal), _playerControllerData.WrapTimeSeconds);

            return true;
        }

        private bool CheckRotateLeft()
        {
            if(null == _rightHandHitResult || Driver.LastMoveAxes.x >= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_leftHandTransform.position, Quaternion.AngleAxis(-90.0f, transform.up) * transform.forward, out hit, _playerControllerData.ArmRayLength * 0.5f, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _rightHandHitResult.Value.normal) {
                return false;
            }

            _leftHandHitResult = hit;

            Vector3 offset = (Player.CapsuleCollider.radius * 2.0f) * transform.forward;
            StartAnimation(Rigidbody.position + GetSurfaceAttachmentPosition(hit, Player.CapsuleCollider.radius * hit.normal) - offset, Quaternion.LookRotation(-hit.normal), _playerControllerData.WrapTimeSeconds);

            return true;
        }

        private bool CheckWrapRight()
        {
            if(null == _leftHandHitResult || Driver.LastMoveAxes.x <= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_rightHandTransform.position, Quaternion.AngleAxis(-_playerControllerData.WrapAroundAngle, transform.up) * transform.forward, out hit, _playerControllerData.ArmRayLength * 2.0f, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _leftHandHitResult.Value.normal) {
                return false;
            }

            _rightHandHitResult = hit;

            Vector3 offset = (Player.CapsuleCollider.radius * 2.0f) * transform.forward;
            StartAnimation(Rigidbody.position + GetSurfaceAttachmentPosition(hit, Player.CapsuleCollider.radius * hit.normal) + offset, Quaternion.LookRotation(-hit.normal), _playerControllerData.WrapTimeSeconds);

            return true;

        }

        private bool CheckRotateRight()
        {
            if(null == _leftHandHitResult || Driver.LastMoveAxes.x <= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_rightHandTransform.position, Quaternion.AngleAxis(90.0f, transform.up) * transform.forward, out hit, _playerControllerData.ArmRayLength * 0.5f, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _leftHandHitResult.Value.normal) {
                return false;
            }

            _rightHandHitResult = hit;

            Vector3 offset = (Player.CapsuleCollider.radius * 2.0f) * transform.forward;
            StartAnimation(Rigidbody.position + GetSurfaceAttachmentPosition(hit, Player.CapsuleCollider.radius * hit.normal) - offset, Quaternion.LookRotation(-hit.normal), _playerControllerData.WrapTimeSeconds);

            return true;

        }

        private bool CheckClimbUp()
        {
            if(Driver.LastMoveAxes.y <= 0.0f) {
                return false;
            }

            // cast a ray from the end of our rotated head check straight down to see if we can stand here
            Vector3 start = _headTransform.position + (Quaternion.AngleAxis(-_playerControllerData.HeadRayAngle, transform.right) * transform.forward) * _playerControllerData.HeadRayLength;
            float length = Player.CapsuleCollider.height;

            RaycastHit hit;
            if(!Physics.Raycast(start, -Vector3.up, out hit, length, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            DisableGrabbing();

            Vector3 offset = Player.CapsuleCollider.radius * 2.0f * transform.forward;
            StartAnimation(Rigidbody.position + GetSurfaceAttachmentPosition(hit, offset), Rigidbody.rotation, _playerControllerData.ClimbUpTimeSeconds);

            return true;
        }

        private bool CheckDropDown()
        {
            if(Driver.LastMoveAxes.y >= 0.0f) {
                return false;
            }

Debug.Log("TODO: check drop down");
            return false;
        }
#endregion

        private Vector3 GetSurfaceAttachmentPosition(RaycastHit hit, Vector3 offset)
        {
            // keep a set distance away from the surface
            Vector3 targetPoint = hit.point + (hit.normal * _playerControllerData.AttachDistance);
            Vector3 a = targetPoint - Rigidbody.position;
            Vector3 p = Vector3.Project(a, hit.normal);

            return p + offset;
        }

        private void AttachToSurface(RaycastHit hit)
        {
            Debug.Log($"Attach to surface {hit.transform.name}");

            transform.forward = -hit.normal;
            Rigidbody.position += GetSurfaceAttachmentPosition(hit, Player.CapsuleCollider.radius * hit.normal);
        }

        private void DropDown()
        {
Debug.Log("TODO: drop down");
        }

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Player {Player.name} Controller");
            _debugMenuNode.RenderContentsAction = () => {
                _breakOnFall = GUILayout.Toggle(_breakOnFall, "Break on fall");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
