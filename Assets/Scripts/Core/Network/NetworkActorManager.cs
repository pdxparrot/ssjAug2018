using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    public abstract class NetworkActorManager : NetworkSingletonBehavior
    {
        public static NetworkActorManager Instance { get; private set; }

        public static bool HasInstance => null != Instance;

        private readonly HashSet<IActor> _actors = new HashSet<IActor>();

#region Unity Lifecycle
        protected virtual void Awake()
        {
            if(HasInstance) {
                Debug.LogError($"[NetworkSingleton] Instance already created: {Instance.gameObject.name}");
                return;
            }

            Instance = this;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
#endregion

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
