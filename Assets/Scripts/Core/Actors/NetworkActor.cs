using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    public abstract class NetworkActor : NetworkBehaviour, IActor
    {
        [SerializeField]
        [ReadOnly]
        private int _id = -1;

        public int Id => _id;

        [SerializeField]
        private ActorController _controller;

        public ActorController Controller => _controller;

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
    }
}
