using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public abstract class SubGameState : MonoBehaviour, GameStateManager.IGameState
    {
        public string Name => name;

        public virtual void OnEnter()
        {
            Debug.Log($"Enter SubState: {Name}");
        }

        public virtual void OnUpdate(float dt)
        {
        }

        public virtual void OnExit()
        {
            Debug.Log($"Exit SubState: {Name}");
        }
    }
}
