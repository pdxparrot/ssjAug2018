using pdxpartyparrot.Core.Actors;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    [RequireComponent(typeof(Actor))]
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    public abstract class NetworkActor : NetworkBehaviour
    {
        public NetworkIdentity NetworkIdentity { get; private set; }

        public NetworkTransform NetworkTransform { get; private set; }

        protected Actor Actor { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            NetworkIdentity = GetComponent<NetworkIdentity>();
            NetworkTransform = GetComponent<NetworkTransform>();
            Actor = GetComponent<Actor>();
        }
#endregion
    }
}
