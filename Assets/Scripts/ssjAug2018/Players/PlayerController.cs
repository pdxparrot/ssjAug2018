using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ssjAug2018.Data;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerController : ThirdPersonController
    {
#region Grab Check
        [Header("Grab Check")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _grabCheckStart;

        [SerializeField]
        [ReadOnly]
        private Vector3 _grabCheckEnd;

        [SerializeField]
        [ReadOnly]
        private float _grabCheckRadius;

        [SerializeField]
        [ReadOnly]
        private bool _canGrab;

        public bool CanGrab => _canGrab && !_isClimbing && !_isSwinging;
#endregion

        [SerializeField]
        [ReadOnly]
        private bool _isClimbing;

        public bool IsClimbing => _isClimbing;

        [SerializeField]
        [ReadOnly]
        private bool _isSwinging;

        public bool IsSwinging => _isSwinging;

        private PlayerData _playerData;

#region Unity Lifecycle
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            CheckCanGrab();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_grabCheckStart, _grabCheckRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_grabCheckEnd, _grabCheckRadius);
        }
#endregion

        public void Initialize(Player player, PlayerData playerData, ThirdPersonControllerData controllerData)
        {
            base.Initialize(player, controllerData);

            _playerData = playerData;
        }

        public void Grab()
        {
            if(!CanGrab) {
                return;
            }

            _isClimbing = true;
            Rigidbody.isKinematic = true;
        }

        public void Drop()
        {
            if(!IsClimbing) {
                return;
            }

            _isClimbing = false;
            Rigidbody.isKinematic = false;
        }

        public override void Turn(Vector3 axes, float dt)
        {
            if(!IsClimbing && !IsSwinging) {
                base.Turn(axes, dt);
                return;
            }

// TODO
        }

        public override void Move(Vector3 axes, float dt)
        {
            if(!IsClimbing && !IsSwinging) {
                base.Move(axes, dt);
                return;
            }

// TODO
        }

        public override void Jump()
        {
            if(!IsClimbing && !IsSwinging) {
                base.Jump();
                return;
            }

// TODO
        }

        private void CheckCanGrab()
        {
            Vector3 center = Owner.Collider.bounds.center;
            Vector3 extents = Owner.Collider.bounds.extents;
            Vector3 max = Owner.Collider.bounds.max;

            _grabCheckRadius = extents.z - 0.1f;
            _grabCheckStart = new Vector3(center.x, max.y - 0.1f - _grabCheckRadius, max.z -_grabCheckRadius - 0.1f);
            _grabCheckEnd   = new Vector3(center.x, max.y - 0.1f - _grabCheckRadius, max.z -_grabCheckRadius + _playerData.ClimbCheckEpsilon);

            _canGrab = Physics.CheckCapsule(_grabCheckStart, _grabCheckEnd, _grabCheckRadius, CollisionCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);
        }
    }
}
