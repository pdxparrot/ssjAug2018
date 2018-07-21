using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class LocalActor : MonoBehaviour, IActor
    {
#region IActor
        [SerializeField]
        [ReadOnly]
        private int _id = -1;

        public int Id => _id;

        public GameObject GameObject => gameObject;

        [SerializeField]
        private GameObject _model;

        public GameObject Model => _model;

        [SerializeField]
        private ActorController _controller;

        public ActorController Controller => _controller;

        [SerializeField]
        [ReadOnly]
        private bool _canMove = true;

        public bool CanMove => _canMove;
#endregion

        public virtual void Initialize(int id)
        {
            _id = id;

            _controller.Initialize(this);
        }

        public abstract void OnSpawn();
    }
}
