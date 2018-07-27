using System;
using System.Collections;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerController : ThirdPersonController
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

        public bool IsGrabbing => MovementState.Platforming != _movementState;
#endregion

        [Space(10)]

#region Hands
        [Header("Hands")]

        [SerializeField]
        [FormerlySerializedAs("_leftGrabCheckTransform")]
        private Transform _leftHandTransform;

        private RaycastHit? _leftHandHitResult;

        public bool CanGrabLeft => null != _leftHandHitResult;

        [SerializeField]
        [FormerlySerializedAs("_rightGrabCheckTransform")]
        private Transform _rightHandTransform;

        private RaycastHit? _rightHandHitResult;

        public bool CanGrabRight => null != _rightHandHitResult;
#endregion

        [Space(10)]

#region Head
        [Header("Head")]

        [SerializeField]
        private Transform _headTransform;

        private RaycastHit? _headHitResult;
#endregion

        [Space(10)]

#region Head
        [Header("Chest")]

        [SerializeField]
        private Transform _chestTransform;

        private RaycastHit? _chestHitResult;
#endregion

        public bool CanClimbUp => IsClimbing && (null == _headHitResult && null != _chestHitResult);

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private bool _hasDoubleJumped;

        private bool CanDoubleJump => !IsGrounded && !_hasDoubleJumped;


        public Player Player => (Player)Owner;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Debug.Assert(Math.Abs(_leftHandTransform.position.y - _rightHandTransform.position.y) < float.Epsilon, "Player hands are at different heights!");
            Debug.Assert(_headTransform.position.y > _leftHandTransform.position.y, "Player head should be above player hands!");
            Debug.Assert(_chestTransform.position.y < _leftHandTransform.position.y, "Player chest should be below player hands!");
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

// TODO: encapsulate the math here so we a) don't duplicate it in the raycast methods and b) guarantee we always match the math done in the raycast methods

            // left hand
            Gizmos.color = null != _leftHandHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + transform.forward * _playerControllerData.ArmRayLength);
            if(IsClimbing && !CanGrabLeft && CanGrabRight) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(_playerControllerData.WrapAroundAngle, transform.up) * transform.forward) * _playerControllerData.ArmRayLength * 2.0f);
            }

            // right hand
            Gizmos.color = null != _rightHandHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + transform.forward * _playerControllerData.ArmRayLength);
            if(IsClimbing && CanGrabLeft && !CanGrabRight) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(-_playerControllerData.WrapAroundAngle, transform.up) * transform.forward) * _playerControllerData.ArmRayLength * 2.0f);
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
            if(IsGrabbing) {
                return;
            }

            base.AnimationMove(axes, dt);
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!IsGrabbing) {
                base.PhysicsMove(axes, dt);
                return;
            }

            Vector3 velocity = transform.localRotation * (axes * _playerControllerData.ClimbSpeed);
            if(IsGrounded && velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }
            Rigidbody.MovePosition(Rigidbody.position + velocity * dt);
        }

#region Actions
        public void Grab()
        {
            if(IsGrabbing || (!CanGrabLeft && !CanGrabRight)) {
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
            if(IsGrabbing) {
                EnableGrabbing(false);
            } else {
                CheckDropDown();
            }
        }

        public void Throw()
        {
            Debug.Log("TODO: throw!");
        }

        public override void Jump(bool force=false)
        {
            bool wasGrabbing = IsGrabbing;

            EnableGrabbing(false);

            bool doubleJump = !wasGrabbing && !IsGrounded;
            if(doubleJump) {
                _hasDoubleJumped = true;
            }

            base.Jump(wasGrabbing || doubleJump);
        }
#endregion

        private void EnableGrabbing(bool enable)
        {
            EnableClimbing(enable);
        }

        private void EnableClimbing(bool enable)
        {
            _movementState = enable ? MovementState.Climbing : MovementState.Platforming;
            Rigidbody.isKinematic = enable;
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
                CheckRotateLeft();
            } else if(CanGrabLeft && !CanGrabRight) {
                CheckRotateRight();
            } else if(!CanGrabLeft && !CanGrabRight) {
                if(CanClimbUp) {
                    CheckClimbUp();
                } else {
                    Debug.LogWarning("Unexpectedly fell off!");
                    EnableGrabbing(false);
                    Debug.Break();
                }
            }
        }

#region Auto-Rotate/Climb
        private bool CheckRotateLeft()
        {
            if(null == _rightHandHitResult) {
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

            AttachToSurface(hit);
            _leftHandHitResult = hit;

            Vector3 offset = (Player.CapsuleCollider.radius * 2.0f) * -transform.right;
            Rigidbody.position += offset;

            return true;
        }

        private bool CheckRotateRight()
        {
            if(null == _leftHandHitResult) {
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

            AttachToSurface(hit);
            _rightHandHitResult = hit;

            Vector3 offset = (Player.CapsuleCollider.radius * 2.0f) * transform.right;
            Rigidbody.position += offset;

            return true;

        }

        private bool CheckClimbUp()
        {
            // cast a ray from the end of our rotated head check straight down to see if we can stand here
            Vector3 start = _headTransform.position + (Quaternion.AngleAxis(-_playerControllerData.HeadRayAngle, transform.right) * transform.forward) * _playerControllerData.HeadRayLength;
            float length = Player.CapsuleCollider.height;

            RaycastHit hit;
            if(!Physics.Raycast(start, -Vector3.up, out hit, length, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            ClimbUp(hit);

            return true;
        }

        private bool CheckDropDown()
        {
Debug.Log("TODO: check drop down");
            return false;
        }
#endregion

// TODO: smooth/animate these things

        private void AttachToSurface(RaycastHit hit)
        {
            // align to the surface
            transform.forward = -hit.normal;

            // keep a set distance away from the surface
            Vector3 targetPoint = hit.point + (hit.normal * _playerControllerData.AttachDistance);
            Vector3 a = targetPoint - Rigidbody.position;
            Vector3 p = Vector3.Project(a, hit.normal);
            Vector3 offset = Player.CapsuleCollider.radius * hit.normal;
            Rigidbody.position += p + offset;
        }

        private void ClimbUp(RaycastHit hit)
        {
            Vector3 targetPoint = hit.point + (hit.normal * _playerControllerData.AttachDistance);
            Vector3 a = targetPoint - Rigidbody.position;
            Vector3 p = Vector3.Project(a, hit.normal);
            Vector3 offset = (Player.CapsuleCollider.radius * 2.0f) * transform.forward;
            Rigidbody.position += p + offset;

            EnableGrabbing(false);
        }

        private void DropDown()
        {
Debug.Log("TODO: drop down");
        }
    }
}
