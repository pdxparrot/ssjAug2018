using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    public abstract class NetworkActorManager : NetworkSingletonBehavior
    {
        private readonly HashSet<IActor> _actors = new HashSet<IActor>();

        public void Register<T>(T actor) where T: IActor
        {
            _actors.Add(actor);
        }

        public void Unregister<T>(T actor) where T: IActor
        {
            _actors.Remove(actor);
        }
    }
}
