using pdxpartyparrot.Core.Util;

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
            Debug.Log("Game over!");
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
