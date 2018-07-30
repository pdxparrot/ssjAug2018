using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.ssjAug2018.Data;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Items
{
    public sealed class ItemManager : SingletonBehavior<ItemManager>
    {
        private const string MailItemPool = "mail";

        [SerializeField]
        private ItemData _itemData;

        public ItemData ItemData => _itemData;

#region Unity Lifecycle
        private void Awake()
        {
            Core.Network.NetworkManager.Instance.ServerStartEvent += ServerStartEventHandler;
            Core.Network.NetworkManager.Instance.ServerStopEvent += ServerStopEventHandler;
        }

        protected override void OnDestroy()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerStopEvent -= ServerStopEventHandler;
                Core.Network.NetworkManager.Instance.ServerStartEvent -= ServerStartEventHandler;
            }

            if(ObjectPoolManager.HasInstance) {
                ObjectPoolManager.Instance.DestroyPool(MailItemPool);
            }
        }
#endregion

        [CanBeNull]
        public Mail GetMail()
        {
            return ObjectPoolManager.Instance.GetPooledObject<Mail>(MailItemPool);
        }

#region Event Handlers
        private void ServerStartEventHandler(object sender, EventArgs args)
        {
            ObjectPoolManager.Instance.InitializeNetworkPool(MailItemPool, ItemData.MailPrefab.GetComponent<PooledObject>(), 5);
        }

        private void ServerStopEventHandler(object sender, EventArgs args)
        {
            ObjectPoolManager.Instance.DestroyPool(MailItemPool);
        }
#endregion
    }
}
