using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    public abstract class NetworkActor : NetworkBehaviour, IActor
    {
#region IActor
        [SerializeField]
        [ReadOnly]
        private int _id = -1;

        public int Id => _id;

        public GameObject GameObject => gameObject;

        [SerializeField]
        private Collider _collider;

        public Collider Collider => _collider;

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

        protected NetworkIdentity NetworkIdentity { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            NetworkIdentity = GetComponent<NetworkIdentity>();
        }
#endregion

        public virtual void Initialize(int id)
        {
            _id = id;

            _controller.Initialize(this);
        }

        public abstract void OnSpawn();
    }
}
