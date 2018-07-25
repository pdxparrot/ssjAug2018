using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ActorController : MonoBehaviour
    {
        [Serializable]
        protected struct PauseState
        {
            private Vector3 _velocity;

            public Vector3 Velocity => _velocity;

            public void Save(Rigidbody rigidbody)
            {
                _velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;
            }

            public void Restore(Rigidbody rigidbody)
            {
                rigidbody.velocity = _velocity;
            }
        }

        [SerializeField]
        private ActorDriver _driver;

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastAngularVelocity;
#endregion

        public ActorDriver Driver => _driver;

        public Rigidbody Rigidbody { get; private set; }

        protected IActor Owner { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            _lastVelocity = Rigidbody.velocity;
            _lastAngularVelocity = Rigidbody.angularVelocity;
        }
#endregion

        public virtual void Initialize(IActor owner)
        {
            Owner = owner;

            _driver.Initialize(owner, this);
        }

        public virtual void MoveTo(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position}");
            Rigidbody.position = position;
        }

        public virtual void AnimationMove(Vector3 axes, float dt)
        {
        }

        public virtual void PhysicsMove(Vector3 axes, float dt)
        {
        }
    }
}
