using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Items
{
    public sealed class ItemManager : SingletonBehavior<ItemManager>
    {
        private const string MailItemPool = "mail";
        private const string SnowballItemPool = "snowballs";

        [SerializeField]
        private ItemData _itemData;

        public ItemData ItemData => _itemData;

#region Unity Lifecycle
        protected override void OnDestroy()
        {
            if(ObjectPoolManager.HasInstance) {
                ObjectPoolManager.Instance.DestroyPool(MailItemPool);
            }
        }
#endregion

        public void PopulateItemPools()
        {
            ObjectPoolManager.Instance.InitializePool(MailItemPool, ItemData.MailPrefab.GetComponent<PooledObject>(), PlayerManager.Instance.PlayerData.MaxLetters);
        }

        public void FreeItemPools()
        {
            ObjectPoolManager.Instance.DestroyPool(MailItemPool);
        }

        [CanBeNull]
        public Mail GetMail()
        {
            return ObjectPoolManager.Instance.GetPooledObject<Mail>(MailItemPool);
        }
    }
}
