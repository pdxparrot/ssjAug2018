using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public abstract class SubGameState : MonoBehaviour, IGameState
    {
        public string Name => name;

        public virtual void OnEnter()
        {
            Debug.Log($"Enter SubState: {Name}");
        }

        public virtual void OnExit()
        {
            Debug.Log($"Exit SubState: {Name}");
        }

        public virtual void OnResume()
        {
            Debug.Log($"Resume SubState: {Name}");
        }

        public virtual void OnPause()
        {
            Debug.Log($"Pause SubState: {Name}");
        }

        public virtual void OnUpdate(float dt)
        {
        }
    }
}
