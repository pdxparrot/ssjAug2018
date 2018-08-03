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

        [Serializable]
        private struct AnimationState
        {
            public bool IsAnimating;

            public float AnimationSeconds;
            public float AnimationSecondsRemaining;

            public float PercentComplete => 1.0f - (AnimationSecondsRemaining / AnimationSeconds);

            public bool IsFinished => AnimationSecondsRemaining <= 0.0f;

            public Vector3 StartPosition;
            public Vector3 EndPosition;

            public Quaternion StartRotation;
            public Quaternion EndRotation;

            public bool WasKinematic;
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

#region Manual Animation
        [Header("Manual Animation")]

        [SerializeField]
        [ReadOnly]
        private AnimationState _animationState;

        public bool IsAnimating => _animationState.IsAnimating;
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

        protected virtual void Update()
        {
            float dt = Time.deltaTime;

            UpdateAnimations(dt);
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

#region Manual Animations
        protected void StartAnimation(Vector3 targetPosition, Quaternion targetRotation, float timeSeconds)
        {
            if(IsAnimating) {
                return;
            }

            Debug.Log($"Starting manual animation from {Rigidbody.position}:{Rigidbody.rotation} to {targetPosition}:{targetRotation} over {timeSeconds} seconds");

            _animationState.IsAnimating = true;

            _animationState.StartPosition = Rigidbody.position;
            _animationState.EndPosition = targetPosition;

            _animationState.StartRotation = Rigidbody.rotation;
            _animationState.EndRotation = targetRotation;

            _animationState.AnimationSeconds = timeSeconds;
            _animationState.AnimationSecondsRemaining = timeSeconds;

            _animationState.WasKinematic = Rigidbody.isKinematic;
            Rigidbody.isKinematic = true;
        }

        private void UpdateAnimations(float dt)
        {
            if(!IsAnimating) {
                return;
            }

            if(_animationState.IsFinished) {
                Debug.Log("Manual animation complete!");

                _animationState.IsAnimating = false;

                Rigidbody.position = _animationState.EndPosition;
                Rigidbody.rotation = _animationState.EndRotation;
                Rigidbody.isKinematic = _animationState.WasKinematic;
                return;
            }

            _animationState.AnimationSecondsRemaining -= dt;
            if(_animationState.AnimationSecondsRemaining < 0.0f) {
                _animationState.AnimationSecondsRemaining = 0.0f;
            }

            Rigidbody.position = Vector3.Slerp(_animationState.StartPosition, _animationState.EndPosition, _animationState.PercentComplete);
            Rigidbody.rotation = Quaternion.Slerp(_animationState.StartRotation, _animationState.EndRotation, _animationState.PercentComplete);
        }
#endregion

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
