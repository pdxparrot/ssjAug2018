using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(ActorController))]
    public abstract class LocalActor : MonoBehaviour, IActor
    {
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

        public virtual void Initialize(int id)
        {
            _id = id;

            _controller.Initialize(this);
        }

        public abstract void OnSpawn();
    }
}
