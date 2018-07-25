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

        protected ActorController Controller { get; private set; }

        protected IActor Owner { get; private set; }

        protected virtual bool CanDrive => !PartyParrotManager.Instance.IsPaused;

        private bool _moveStopped;

#region Unity Lifecycle
        private void Update()
        {
            if(!CanDrive) {
                return;
            }

            float dt = Time.deltaTime;

            Controller.RotateModel(LastMoveAxes, dt);
        }

        private void FixedUpdate()
        {
            if(!CanDrive) {
                return;
            }

            float dt = Time.fixedDeltaTime;

            Vector3 axes = LastMoveAxes;
            if(axes.sqrMagnitude < float.Epsilon) {
                if(_moveStopped) {
                    return;
                }
                _moveStopped = true;
            } else {
                _moveStopped = false;
            }

            Controller.Turn(axes, dt);
            Controller.Move(axes, dt);
        }
#endregion

        public virtual void Initialize(IActor owner, ActorController controller)
        {
            Owner = owner;
            Controller = controller;
        }
    }
}
