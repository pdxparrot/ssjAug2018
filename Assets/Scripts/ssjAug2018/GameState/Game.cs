using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Game : BaseGame
    {
        [SerializeField]
        private GameOver _gameOverState;

        public override void OnUpdate(float dt)
        {
            if(GameManager.Instance.IsGameOver) {
                GameStateManager.Instance.PushSubState(_gameOverState);
            }
        }
    }
}
