using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ActorController : MonoBehaviour
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;

            public Vector3 Velocity;

            public void Save(Rigidbody rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;
            }

            public void Restore(Rigidbody rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
            }
        }

        [SerializeField]
        private ActorDriver _driver;

        public ActorDriver Driver => _driver;

        [Space(10)]

#region Physics
        [Header("Physics")]

#pragma warning disable 0414
        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastAngularVelocity;
#pragma warning restore 0649
#endregion

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private bool _canMove = true;

        public virtual bool CanMove { get { return _canMove; } protected set { _canMove = value; } }

        public Rigidbody Rigidbody { get; private set; }

        protected IActor Owner { get; private set; }

        [SerializeField]
        [ReadOnly]
        private InternalPauseState _pauseState;

        protected InternalPauseState PauseState => _pauseState;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        protected virtual void LateUpdate()
        {
            _lastVelocity = Rigidbody.velocity;
            _lastAngularVelocity = Rigidbody.angularVelocity;
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
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

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(PartyParrotManager.Instance.IsPaused) {
                _pauseState.Save(Rigidbody);
            } else {
                _pauseState.Restore(Rigidbody);
            }
        }
#endregion
    }
}
