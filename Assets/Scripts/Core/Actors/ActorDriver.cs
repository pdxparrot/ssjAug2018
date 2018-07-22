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

#region Unity Lifecycle
        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.deltaTime;

            Controller.RotateModel(LastMoveAxes, dt);
        }

        private void FixedUpdate()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.fixedDeltaTime;

            //Controller.Turn(LastMoveAxes, dt);
            Controller.Move(LastMoveAxes, dt);
        }
#endregion

        public void Initialize(IActor owner, ActorController controller)
        {
            Owner = owner;
            Controller = controller;
        }
    }
}
