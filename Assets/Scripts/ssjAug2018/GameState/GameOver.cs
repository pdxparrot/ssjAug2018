using pdxpartyparrot.Core.Util;
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
        private long _completeTime;

        public override void OnEnter()
        {
            UIManager.Instance.PlayerUI.PlayerHUD.ShowGameOverText();

            _completeTime = TimeManager.Instance.CurrentUnixMs + (int)(_completeWaitTimeSeconds * 1000.0f);
        }

        public override void OnUpdate(float dt)
        {
            if(TimeManager.Instance.CurrentUnixMs >= _completeTime) {
                GameStateManager.Instance.TransitionToInitialState();
            }
        }
    }
}
