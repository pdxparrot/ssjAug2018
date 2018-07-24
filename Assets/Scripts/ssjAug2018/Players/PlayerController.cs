using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
/*
 TODO: raycast from the "head" to see if we need to climb up

 "attach" to the grabbable when we grab it so that we have a consistent distance
 */

    public sealed class PlayerController : ThirdPersonController
    {
#region Grab Check
        [Header("Grab Check")]

        [SerializeField]
        private Transform _leftGrabCheckTransform;

        [SerializeField]
        private Transform _rightGrabCheckTransform;

        [SerializeField]
        private float _grabCheckRadius = 1;

        [SerializeField]
        [ReadOnly]
        private bool _canGrabLeft;

        [SerializeField]
        [ReadOnly]
        private bool _canGrabRight;

        public bool CanGrab => (_canGrabLeft || _canGrabRight) && !_isClimbing && !_isSwinging;

        private IGrabbable _leftGrabbedObject;

        private IGrabbable _rightGrabbedObject;
#endregion

        [SerializeField]
        [ReadOnly]
        private bool _isClimbing;

        public bool IsClimbing => _isClimbing;

        [SerializeField]
        [ReadOnly]
        private bool _isSwinging;

        public bool IsSwinging => _isSwinging;

        public bool IsGrabbing => IsClimbing || IsSwinging;

        public Player Player => (Player)Owner;

        private PlayerData _playerData;

#region Unity Lifecycle
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            UpdateCanGrab();

            CheckShouldRotateOrClimb();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = _canGrabLeft ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(LeftGrabCheckCenter(), _grabCheckRadius);

            Gizmos.color = _canGrabRight ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(RightGrabCheckCenter(), _grabCheckRadius);
        }
#endregion

        public void Initialize(Player player, PlayerData playerData, ThirdPersonControllerData controllerData)
        {
            base.Initialize(player, controllerData);

            _playerData = playerData;
        }

#region Actions
        public void Grab()
        {
            if(!CanGrab) {
                return;
            }

            EnableClimbing(true);
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
// TODO
        }

        public override void Turn(Vector3 axes, float dt)
        {
            if(!IsGrabbing) {
                base.Turn(axes, dt);
                return;
            }

            // dont't turn
        }

        public override void Move(Vector3 axes, float dt)
        {
            if(!IsGrabbing) {
                base.Move(axes, dt);
                return;
            }

            Vector3 velocity = transform.localRotation * (axes * _playerData.ClimbSpeed);
            if(IsGrounded && velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }

            Rigidbody.MovePosition(transform.position + velocity * dt);
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
        }

        private void EnableClimbing(bool enable)
        {
            //Debug.Log($"Enable climbing: {enable}");

            _isClimbing = enable;
            Rigidbody.isKinematic = enable;
        }

        private void EnableSwinging(bool enable)
        {
            //Debug.Log($"Enable swinging: {enable}");

            _isSwinging = enable;
            Rigidbody.isKinematic = enable;
        }

#region Grab Check
        private Vector3 LeftGrabCheckCenter()
        {
            Vector3 center = _leftGrabCheckTransform != null ? _leftGrabCheckTransform.position : transform.position;
            return new Vector3(center.x, center.y + _grabCheckRadius - 0.1f, center.z);
        }

        private Vector3 RightGrabCheckCenter()
        {
            Vector3 center = _rightGrabCheckTransform != null ? _rightGrabCheckTransform.position : transform.position;
            return new Vector3(center.x, center.y + _grabCheckRadius - 0.1f, center.z);
        }

        private void UpdateCanGrab()
        {
            // NOTE: right can overwrite whatever left would have grabbed
            // that might be an issue at some point depending on how things work out design-wise
            UpdateCanGrabLeft();
            UpdateCanGrabRight();
        }

        private void UpdateCanGrabLeft()
        {
            _canGrabLeft = false;

            var hits = Physics.OverlapSphere(LeftGrabCheckCenter(), _grabCheckRadius, CollisionCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);
            foreach(Collider hit in hits) {
                IGrabbable grabbable = hit.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _canGrabLeft = true;
                    _leftGrabbedObject = grabbable;
                    break;
                }
            }
        }

        private void UpdateCanGrabRight()
        {
            _canGrabRight = false;

            var hits = Physics.OverlapSphere(RightGrabCheckCenter(), _grabCheckRadius, CollisionCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);
            foreach(Collider hit in hits) {
                IGrabbable grabbable = hit.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _canGrabRight = true;
                    _rightGrabbedObject = grabbable;
                    break;
                }
            }
        }
#endregion

#region Auto-Rotate/Climb
        private void CheckShouldRotateOrClimb()
        {
            if(!IsGrabbing) {
                return;
            }

            float radius = Player.CapsuleCollider.radius;
            float armheight = _rightGrabCheckTransform.localPosition.y - _grabCheckRadius;

// TODO: smooth/animate these things
            if(!_canGrabLeft && _canGrabRight) {
                if(CheckRotateLeft()) {
                    transform.Rotate(Vector3.up, 90.0f);
                    transform.position += transform.localRotation * new Vector3(-radius - 0.1f, 0.0f, -radius);
                }
            } else if(_canGrabLeft && !_canGrabRight) {
                if(CheckRotateRight()) {
                    transform.Rotate(Vector3.up, -90.0f);
                    transform.position += transform.localRotation * new Vector3(radius + 0.1f, 0.0f, -radius);
                }
            } else if(!_canGrabLeft && !_canGrabRight) {
                if(CheckClimbUp()) {
                    transform.position += transform.localRotation * new Vector3(0.0f, armheight, radius);
                } else {
                    //Debug.Log("fell off");
                    EnableGrabbing(false);
                }
            }
        }

// TODO: these could probably pass back the predicted position/rotation to so we don't have to re-do the math in CheckShouldRotateOrClimb()

        private bool CheckRotateLeft()
        {
            // TODO: 0.5 necessary?
            float radius = Player.CapsuleCollider.radius * 0.5f;

            Vector3 movement = transform.localRotation * new Vector3(radius, 0.0f, radius);
            Vector3 position = LeftGrabCheckCenter() + movement;

            return Physics.CheckSphere(position, _grabCheckRadius, CollisionCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);
        }

        private bool CheckRotateRight()
        {
            // TODO: 0.5 necessary?
            float radius = Player.CapsuleCollider.radius * 0.5f;

            Vector3 movement = transform.localRotation * new Vector3(-radius, 0.0f, radius);
            Vector3 position = RightGrabCheckCenter() + movement;

            return Physics.CheckSphere(position, _grabCheckRadius, CollisionCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);
        }

        private bool CheckClimbUp()
        {
            float radius = Player.CapsuleCollider.radius;
            float armheight = _rightGrabCheckTransform.localPosition.y - _grabCheckRadius;

            Vector3 groundCheckCenter = GetGroundCheckCenter();
            Vector3 movement = transform.localRotation * new Vector3(0.0f, armheight, radius);

            return CheckIsGrounded(groundCheckCenter + movement);
        }
#endregion
    }
}
