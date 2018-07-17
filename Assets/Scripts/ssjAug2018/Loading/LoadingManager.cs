using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
#region Manager Prefabs
        [SerializeField]
        private PlayerManager _playerManagerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            PlayerManager.CreateFromPrefab(_playerManagerPrefab, ManagersContainer);
        }
    }
}
