using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.GameState;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018
{
    public sealed class GameManager : SingletonBehavior<GameManager>
    {
        [SerializeField]
        [ReadOnly]
        private Timer _gameTimer;

        public bool IsGameOver { get; private set; }

        public int RemainingMinutesPart => ((int)_gameTimer.SecondsRemaining) / 60;

        public int RemainingSecondsPart => ((int)_gameTimer.SecondsRemaining) % 60;

#region Unity Lifecycle
        private void Update()
        {
            if(!NetworkServer.active) {
                return;
            }

            float dt = Time.deltaTime;

            _gameTimer.Update(dt);
        }
#endregion

        //[Server]
        public void StartGame()
        {
            Assert.IsTrue(NetworkServer.active);

            MailboxManager.Instance.Initialize();

            _gameTimer.Start(GameStateManager.Instance.GameData.GameTimeSeconds, () => {
                IsGameOver = true;
            });
        }

        //[Server]
        public void ScoreHit(Player player)
        {
            Assert.IsTrue(NetworkServer.active);

            Score(player);

            player.NetworkPlayer.RpcHit();
        }

        //[Server]
        public void Score(Player player)
        {
            Assert.IsTrue(NetworkServer.active);

            player.NetworkPlayer.IncreaseScore(ItemManager.Instance.ItemData.MailScoreAmount);
            _gameTimer.AddTime(GameStateManager.Instance.GameData.ScoreGameTimeSeconds);

            player.NetworkPlayer.RpcGameTimeUpdated(GameStateManager.Instance.GameData.ScoreGameTimeSeconds);
        }
    }
}
