using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorDriver : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private Vector3 _lastMoveAxes;

        public Vector3 LastMoveAxes
        {
            get { return _lastMoveAxes; }

            protected set { _lastMoveAxes = value; }
        }

        [SerializeField]
        [ReadOnly]
        private bool _isMoving;

        public bool IsMoving => _isMoving;

        protected ActorController Controller { get; private set; }

        protected IActor Owner { get; private set; }

        protected virtual bool CanDrive => !PartyParrotManager.Instance.IsPaused;

#region Unity Lifecycle
        protected virtual void Update()
        {
            if(!CanDrive) {
                return;
            }

            _isMoving = LastMoveAxes.sqrMagnitude > float.Epsilon;

            float dt = Time.deltaTime;

            Controller.AnimationMove(LastMoveAxes, dt);
        }

        protected virtual void FixedUpdate()
        {
            if(!CanDrive) {
                return;
            }

            float dt = Time.fixedDeltaTime;

            Controller.PhysicsMove(LastMoveAxes, dt);
        }
#endregion

        public virtual void Initialize(IActor owner, ActorController controller)
        {
            Owner = owner;
            Controller = controller;
        }
    }
}
