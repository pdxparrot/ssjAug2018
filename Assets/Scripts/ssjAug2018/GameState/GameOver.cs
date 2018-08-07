using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class GameOver : pdxpartyparrot.Game.State.SubGameState
    {
        [SerializeField]
        private float _completeWaitTimeSeconds = 5.0f;

        [SerializeField]
        [ReadOnly]
        private Timer _completeTimer;

        public void Initialize()
        {
            foreach(IActor actor in PlayerManager.Instance.Actors) {
                Player player = actor as Player;
                HighScoreManager.Instance.AddHighScore($"{actor.Id}", null == player ? 0 : player.Score);
            }
        }

        public override void OnEnter()
        {
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowGameOverText();
            }

            _completeTimer.Start(_completeWaitTimeSeconds, () => {
                GameStateManager.Instance.TransitionToInitialState();
            });
        }

        public override void OnUpdate(float dt)
        {
            _completeTimer.Update(dt);
        }
    }
}
