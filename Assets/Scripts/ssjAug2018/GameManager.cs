using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.GameState;
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
        [ReadOnly]
        //[SyncVar]
        private Timer _gameTimer;

        public bool IsGameOver { get; private set; }

        public int RemainingMinutesPart => ((int)_gameTimer.SecondsRemaining) / 60;

        public int RemainingSecondsPart => ((int)_gameTimer.SecondsRemaining) % 60;

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

        private void Update()
        {
            float dt = Time.deltaTime;

            _gameTimer.Update(dt);
        }
#endregion

        [Server]
        public void StartGame()
        {
            MailboxManager.Instance.Initialize();

            _gameTimer.Start(GameStateManager.Instance.GameData.GameTimeSeconds, () => {
                IsGameOver = true;
            });
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
            _gameTimer.AddTime(GameStateManager.Instance.GameData.ScoreGameTimeSeconds);

            RpcGameTimeUpdated(GameStateManager.Instance.GameData.ScoreGameTimeSeconds);
        }

        [ClientRpc]
        private void RpcGameTimeUpdated(int amount)
        {
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowTimeAdded(amount);
            }
        }

        [ClientRpc]
        private void RpcHit()
        {
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowHitMarker();
            }
        }
    }
}
