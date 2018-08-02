using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    public abstract class NetworkActorManager : NetworkSingletonBehavior
    {
        private readonly List<IActor> _actors = new List<IActor>();

        public IReadOnlyCollection<IActor> Actors => _actors;

        public void Register(IActor actor)
        {
            _actors.Add(actor);
        }

        public void Unregister(IActor actor)
        {
            _actors.Remove(actor);
        }
    }
}
