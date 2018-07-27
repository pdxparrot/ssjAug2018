using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.UI;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
#region Manager Prefabs
        [SerializeField]
        private GameManager _gameManagerPrefab;

        [SerializeField]
        private UIManager _uiManagerPrefab;

        [SerializeField]
        private ItemManager _itemManagerPrefab;

        [SerializeField]
        private MailboxManager _mailboxManagerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameManager.CreateFromPrefab(_gameManagerPrefab, ManagersContainer);
            UIManager.CreateFromPrefab(_uiManagerPrefab, ManagersContainer);
            SpawnManager.Create(ManagersContainer);
            ItemManager.CreateFromPrefab(_itemManagerPrefab, ManagersContainer);
            MailboxManager.CreateFromPrefab(_mailboxManagerPrefab, ManagersContainer);
        }
    }
}
