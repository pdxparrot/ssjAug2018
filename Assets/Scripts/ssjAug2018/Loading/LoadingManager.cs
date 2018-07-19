using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
#region Manager Prefabs
        [SerializeField]
        private UIManager _uiManagerPrefab;

        [SerializeField]
        private PlayerManager _playerManagerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            UIManager.CreateFromPrefab(_uiManagerPrefab, ManagersContainer);
            PlayerManager.CreateFromPrefab(_playerManagerPrefab, ManagersContainer);
            SpawnManager.Create(ManagersContainer);
        }
    }
}
