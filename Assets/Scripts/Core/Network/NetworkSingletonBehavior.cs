using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Util
{
    // this is just a marker for now, we can't really do much because generics aren't allowed :(
    public class NetworkSingletonBehavior : NetworkBehaviour
    {
/*
        public static MyNetworkSingleton Instance { get; private set; }

        public static bool HasInstance => null != Instance;

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
*/
    }
}
