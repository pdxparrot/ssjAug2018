using System.Collections;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;
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
            Climbing,
            Swinging
        }

#region Movement State
        [Header("Movement State")]

        [SerializeField]
        [ReadOnly]
        private MovementState _movementState = MovementState.Platforming;

        public bool IsClimbing => MovementState.Climbing == _movementState;

        public bool IsSwinging => MovementState.Swinging == _movementState;

        public bool IsGrabbing => MovementState.Platforming != _movementState;
#endregion

        [Space(10)]

#region Hands
        [Header("Hands")]

        [SerializeField]
        [FormerlySerializedAs("_leftGrabCheckTransform")]
        private Transform _leftHandTransform;

        private RaycastHit?[] _leftHandHitResults = new RaycastHit?[2];     // forward check, rotate around check

        public bool CanGrabLeft => null != _leftHandHitResults[0];

        [SerializeField]
        [FormerlySerializedAs("_rightGrabCheckTransform")]
        private Transform _rightHandTransform;

        private RaycastHit?[] _rightHandHitResults = new RaycastHit?[2];    // forward check, rotate around check

        public bool CanGrabRight => null != _rightHandHitResults[0];

        [SerializeField]
        private float _armLength = 1.0f;

        [SerializeField]
        private float _wrapAroundAngle = 45.0f;
#endregion

        [Space(10)]

#region Head
        [Header("Head")]

        [SerializeField]
        private Transform _headTransform;

        [SerializeField]
        private float _headCheckLength = 1.0f;

        [SerializeField]
        private float _headCheckAngle = 45.0f;

        private RaycastHit?[] _headHitResults = new RaycastHit?[2];

        public bool CanClimbUp => IsClimbing && (null == _headHitResults[0] && null == _headHitResults[1]);
#endregion

        [Space(10)]

#region Feet
        [Header("Feet")]

        [SerializeField]
        private Transform _footTransform;

        [SerializeField]
        private float _footCheckLength = 1.0f;

        [SerializeField]
        private float _footCheckAngle = 45.0f;

        private RaycastHit?[] _footHitResults = new RaycastHit?[4];     // foward, player right, rear, player left

        public bool CanDropDown => !IsClimbing && (null == _footHitResults[0] || null == _footHitResults[1] || null == _footHitResults[2] || null == _footHitResults[3]);
#endregion

        public Player Player => (Player)Owner;

        private PlayerData _playerData;

        private IGrabbable _attachedTo;

#region Unity Lifecycle
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = null != _leftHandHitResults[0] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + transform.forward * _armLength);
            Gizmos.color = null != _leftHandHitResults[1] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(_wrapAroundAngle, transform.up) * transform.forward) * _armLength);

            Gizmos.color = null != _rightHandHitResults[0] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + transform.forward * _armLength);
            Gizmos.color = null != _rightHandHitResults[1] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(-_wrapAroundAngle, transform.up) * transform.forward) * _armLength);

            Gizmos.color = null != _headHitResults[0] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_headTransform.position, _headTransform.position + (Quaternion.AngleAxis(0.0f, transform.right) * transform.forward) * _headCheckLength);
            Gizmos.color = null != _headHitResults[1] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_headTransform.position, _headTransform.position + (Quaternion.AngleAxis(-_headCheckAngle, transform.right) * transform.forward) * _headCheckLength);

            Gizmos.color = null != _footHitResults[0] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(0.0f, transform.up) * Quaternion.AngleAxis(_footCheckAngle, transform.right) * transform.forward) * _footCheckLength);
            Gizmos.color = null != _footHitResults[1] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(90.0f, transform.up) * Quaternion.AngleAxis(_footCheckAngle, transform.right) * transform.forward) * _footCheckLength);
            Gizmos.color = null != _footHitResults[2] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(180.0f, transform.up) * Quaternion.AngleAxis(_footCheckAngle, transform.right) * transform.forward) * _footCheckLength);
            Gizmos.color = null != _footHitResults[3] ? Color.red : Color.yellow;
            Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(270.0f, transform.up) * Quaternion.AngleAxis(_footCheckAngle, transform.right) * transform.forward) * _footCheckLength);
        }
#endregion

        public void Initialize(Player player, PlayerData playerData, ThirdPersonControllerData controllerData)
        {
            base.Initialize(player, controllerData);

            _playerData = playerData;

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

            Vector3 velocity = transform.localRotation * (axes * _playerData.ClimbSpeed);
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

            if(null != _leftHandHitResults[0]) {
                AttachToSurface(_leftHandHitResults[0].Value);
            } else if(null != _rightHandHitResults[0]) {
                AttachToSurface(_rightHandHitResults[0].Value);
            }
        }

        public void Drop()
        {
            if(!IsGrabbing) {
                return;
            }

            EnableGrabbing(false);
        }

        public void Throw()
        {
            Debug.Log("TODO: throw!");
        }

        public override void Jump(bool force=false)
        {
            bool wasGrabbing = IsGrabbing;

            EnableGrabbing(false);

            base.Jump(wasGrabbing);
        }
#endregion

        private void EnableGrabbing(bool enable)
        {
            EnableClimbing(enable);
            EnableSwinging(enable);

            if(!enable) {
                _attachedTo = null;
            }
        }

        private void EnableClimbing(bool enable)
        {
            _movementState = enable ? MovementState.Climbing : MovementState.Platforming;
            Rigidbody.isKinematic = enable;
        }

        private void EnableSwinging(bool enable)
        {
            _movementState = enable ? MovementState.Swinging : MovementState.Platforming;
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
                UpdateFootRaycasts();
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
            UpdateLeftHandRaycast(0, 0.0f);
            UpdateLeftHandRaycast(1, _wrapAroundAngle);
        }

        private void UpdateLeftHandRaycast(int idx, float angle)
        {
            _leftHandHitResults[idx] = null;

            RaycastHit hit;
            if(Physics.Raycast(_leftHandTransform.position, Quaternion.AngleAxis(angle, transform.up) * transform.forward, out hit, _armLength, CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _leftHandHitResults[idx] = hit;
                }
            }
        }

        private void UpdateRightHandRaycasts()
        {
            UpdateRightHandRaycast(0, 0.0f);
            UpdateRightHandRaycast(1, -_wrapAroundAngle);
        }

        private void UpdateRightHandRaycast(int idx, float angle)
        {
            _rightHandHitResults[idx] = null;

            RaycastHit hit;
            if(Physics.Raycast(_rightHandTransform.position, Quaternion.AngleAxis(angle, transform.up) * transform.forward, out hit, _armLength, CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _rightHandHitResults[idx] = hit;
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

            UpdateHeadRaycast(0, -_headCheckAngle);
            UpdateHeadRaycast(1, 0.0f);
        }

        private void UpdateHeadRaycast(int idx, float angle)
        {
            _headHitResults[idx] = null;

            RaycastHit hit;
            if(Physics.Raycast(_headTransform.position, Quaternion.AngleAxis(angle, transform.right) * transform.forward, out hit, _headCheckLength, CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _headHitResults[idx] = hit;
                }
            }
        }
#endregion

#region Foot Raycasts
        private void UpdateFootRaycasts()
        {
            if(IsGrabbing) {
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
            if(Physics.Raycast(_footTransform.position, Quaternion.AngleAxis(angle, transform.up) * Quaternion.AngleAxis(_footCheckAngle, transform.right) * transform.forward, out hit, _footCheckLength, CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                _footHitResults[idx] = hit;
            }
        }
#endregion

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
                /*if(CheckClimbUp()) {
                    transform.position += transform.localRotation * new Vector3(0.0f, armheight, radius);
                } else {*/
                    Debug.Log("fell off");
                    EnableGrabbing(false);
                //}
            }
        }

#region Auto-Rotate/Climb
// TODO: smooth/animate these things

        private void CheckRotateLeft()
        {
            if(_leftHandHitResults[1] == null || null == _rightHandHitResults[0]) {
                return;
            }

            if(_leftHandHitResults[1].Value.normal != _rightHandHitResults[0].Value.normal) {
Debug.Log("rotate left");
                AttachToSurface(_leftHandHitResults[1].Value);

                Rigidbody.position = Rigidbody.position - (transform.right * Player.CapsuleCollider.radius * 2.0f);
            }
        }

        private void CheckRotateRight()
        {
            if(_rightHandHitResults[1] == null || null == _leftHandHitResults[0]) {
                return;
            }

            if(_rightHandHitResults[1].Value.normal != _leftHandHitResults[0].Value.normal) {
Debug.Log("rotate right");
                AttachToSurface(_rightHandHitResults[1].Value);

                Rigidbody.position = Rigidbody.position + (transform.right * Player.CapsuleCollider.radius * 2.0f);
            }
        }

/*
        private bool CheckClimbUp()
        {
            float radius = Player.CapsuleCollider.radius;
            float armheight = _rightHandTransform.localPosition.y - _armLength;

            Vector3 groundCheckCenter = GetGroundCheckCenter();
            Vector3 movement = transform.localRotation * new Vector3(0.0f, armheight, radius);

            return CheckIsGrounded(groundCheckCenter + movement);
        }
*/
#endregion

        private void AttachToSurface(RaycastHit hit)
        {
            _attachedTo = hit.collider.GetComponent<IGrabbable>();

            // align to the surface
            transform.forward = -hit.normal;

            Vector3 x = hit.point + hit.normal * 0.1f;
            Vector3 a = x - Rigidbody.position;
            Vector3 p = Vector3.Project(a, hit.normal);
            Rigidbody.position -= p - (Player.CapsuleCollider.radius * transform.forward);
        }
    }
}
