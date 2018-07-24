using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Items
{
    public sealed class ItemManager : SingletonBehavior<ItemManager>
    {
        private const string MailItemPool = "mail";
        private const string SnowballItemPool = "snowballs";

#region Items
        [Header("Items")]

        [SerializeField]
        private Mail _mailPrefab;
#endregion

#region Unity Lifecycle
        private void Awake()
        {
            Core.Network.NetworkManager.Instance.ClientConnectEvent += ClientConnectEventHandler;
            Core.Network.NetworkManager.Instance.ClientDisconnectEvent += ClientDisconnectEventHandler;
        }

        protected override void OnDestroy()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ClientConnectEvent -= ClientConnectEventHandler;
                Core.Network.NetworkManager.Instance.ClientDisconnectEvent -= ClientDisconnectEventHandler;
            }

            if(ObjectPoolManager.HasInstance) {
                ObjectPoolManager.Instance.DestroyPool(MailItemPool);
            }
        }
#endregion

#region Event Handlers
        private void ClientConnectEventHandler(object sender, EventArgs args)
        {
            ObjectPoolManager.Instance.InitializePool(MailItemPool, _mailPrefab.GetComponent<PooledObject>(), 5);
        }

        private void ClientDisconnectEventHandler(object sender, EventArgs args)
        {
            ObjectPoolManager.Instance.DestroyPool(MailItemPool);
        }
#endregion
    }
}
