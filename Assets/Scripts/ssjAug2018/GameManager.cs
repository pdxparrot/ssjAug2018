using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.UI;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018
{
    [RequireComponent(typeof(NetworkIdentity))]
    public sealed class GameManager : NetworkSingletonBehavior
    {
#region NetworkSingleton
        public static GameManager Instance { get; private set; }

        public static bool HasInstance => null != Instance;
#endregion

        [SerializeField]
        private GameData _gameData;

        public GameData GameData => _gameData;

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private long _gameOverTime;

        public long GameOverTime => _gameOverTime;

        public bool IsGameOver => _gameOverTime > 0 && TimeManager.Instance.CurrentUnixMs >= GameOverTime;

        public int RemainingMs => IsGameOver ? 0 : (int)(_gameOverTime - TimeManager.Instance.CurrentUnixMs);

        public int RemainingMinutesPart => RemainingMs / 1000 / 60;

        public int RemainingSecondsPart => (RemainingMs / 1000) % 60;

#region Unity Lifecycle
        private void Awake()
        {
            if(HasInstance) {
                Debug.LogError($"[NetworkSingleton] Instance already created: {Instance.gameObject.name}");
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
#endregion

        [Server]
        public void StartGame()
        {
            MailboxManager.Instance.Initialize();
            _gameOverTime = TimeManager.Instance.CurrentUnixMs + GameData.GameTimeMs;
        }

        [Server]
        public void ScoreHit(Player player)
        {
            Score(player);

            RpcHit();
        }

        [Server]
        public void Score(Player player)
        {
            player.IncreaseScore(ItemManager.Instance.ItemData.MailScoreAmount);
            _gameOverTime += GameData.ScoreGameTimeMs;

            RpcGameTimeUpdated(GameData.ScoreGameTimeSeconds);
        }

        [ClientRpc]
        private void RpcGameTimeUpdated(int amount)
        {
            UIManager.Instance.PlayerUI.PlayerHUD.ShowTimeAdded(amount);
        }

        [ClientRpc]
        private void RpcHit()
        {
            UIManager.Instance.PlayerUI.PlayerHUD.ShowHitMarker();
        }
    }
}
