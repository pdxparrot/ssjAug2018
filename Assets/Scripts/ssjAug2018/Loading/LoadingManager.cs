using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ssjAug2018.Actors;
using pdxpartyparrot.ssjAug2018.GameState;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.UI;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
#region Manager Prefabs
        [SerializeField]
        private GameStateManager _gameStateManagerPrefab;

        [SerializeField]
        private UIManager _uiManagerPrefab;

        [SerializeField]
        private GameManager _gameManagerPrefab;

        [SerializeField]
        private PlayerManager _playerManagerPrefab;

        [SerializeField]
        private ItemManager _itemManagerPrefab;

        [SerializeField]
        private MailboxManager _mailboxManagerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameStateManager.CreateFromPrefab(_gameStateManagerPrefab, ManagersContainer);
            UIManager.CreateFromPrefab(_uiManagerPrefab, ManagersContainer);
            GameManager.CreateFromPrefab(_gameManagerPrefab, ManagersContainer);
            PlayerManager.CreateFromPrefab(_playerManagerPrefab, ManagersContainer);
            SpawnManager.Create(ManagersContainer);
            ItemManager.CreateFromPrefab(_itemManagerPrefab, ManagersContainer);
            HighScoreManager.Create(ManagersContainer);
            MailboxManager.CreateFromPrefab(_mailboxManagerPrefab, ManagersContainer);
            DroneManager.Create(ManagersContainer);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GameStateManager.Instance.TransitionToInitialState();
        }
    }
}
