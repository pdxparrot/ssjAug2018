using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    public abstract class NetworkActorManager : NetworkSingletonBehavior
    {
        protected List<IActor> Actors { get; } = new List<IActor>();

        public void Register(IActor actor)
        {
            Actors.Add(actor);
        }

        public void Unregister(IActor actor)
        {
            Actors.Remove(actor);
        }
    }
}
