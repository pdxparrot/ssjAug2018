using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Profiling;

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

            public bool IsKinematic;

            public Action OnComplete;
        }

#region Movement
        [Header("Movement")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastMoveAxes;

        public Vector3 LastMoveAxes
        {
            get { return _lastMoveAxes; }

            set { _lastMoveAxes = value; }
        }

        [SerializeField]
        [ReadOnly]
        private bool _isMoving;

        public bool IsMoving => _isMoving;
#endregion

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
        private Actor _owner;

        public Actor Owner => _owner;

        public virtual bool CanMove => !IsAnimating;

        public Rigidbody Rigidbody { get; private set; }

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
            _isMoving = LastMoveAxes.sqrMagnitude > float.Epsilon;

            float dt = Time.deltaTime;

            UpdateAnimations(dt);

            AnimationMove(LastMoveAxes, dt);
        }

        protected virtual void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            PhysicsMove(LastMoveAxes, dt);
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

        public virtual void MoveTo(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position}");
            Rigidbody.position = position;
        }

        // NOTE: axes are (x, y, 0)
        public virtual void AnimationMove(Vector3 axes, float dt)
        {
        }

        // NOTE: axes are (x, y, 0)
        public virtual void PhysicsMove(Vector3 axes, float dt)
        {
        }

#region Manual Animations
        public void StartAnimation(Vector3 targetPosition, Quaternion targetRotation, float timeSeconds, Action onComplete=null)
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

            _animationState.IsKinematic = Rigidbody.isKinematic;
            Rigidbody.isKinematic = true;

            _animationState.OnComplete = onComplete;
        }

        private void UpdateAnimations(float dt)
        {
            if(!IsAnimating || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            Profiler.BeginSample("ActorController.UpdateAnimations");
            try {
                if(_animationState.IsFinished) {
                    Debug.Log("Manual animation complete!");

                    _animationState.IsAnimating = false;

                    Rigidbody.position = _animationState.EndPosition;
                    Rigidbody.rotation = _animationState.EndRotation;
                    Rigidbody.isKinematic = _animationState.IsKinematic;

                    _animationState.OnComplete?.Invoke();
                    _animationState.OnComplete = null;

                    return;
                }

                _animationState.AnimationSecondsRemaining -= dt;
                if(_animationState.AnimationSecondsRemaining < 0.0f) {
                    _animationState.AnimationSecondsRemaining = 0.0f;
                }

                Rigidbody.position = Vector3.Slerp(_animationState.StartPosition, _animationState.EndPosition, _animationState.PercentComplete);
                Rigidbody.rotation = Quaternion.Slerp(_animationState.StartRotation, _animationState.EndRotation, _animationState.PercentComplete);
            } finally {
                Profiler.EndSample();
            }
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
