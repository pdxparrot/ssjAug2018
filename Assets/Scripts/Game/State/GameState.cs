using System;

using pdxpartyparrot.Core.Scenes;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public abstract class GameState : MonoBehaviour, GameStateManager.IGameState
    {
        public string Name => name;

        [SerializeField]
        private string _sceneName;

        public string SceneName => _sceneName;

        public bool HasScene => !string.IsNullOrWhiteSpace(SceneName);

        public void LoadScene(Action callback)
        {
            if(!HasScene) {
                callback?.Invoke();
                return;
            }

            SceneManager.Instance.LoadScene(SceneName, callback, true);
        }

        public void UnloadScene(Action callback)
        {
            if(!HasScene) {
                callback?.Invoke();
                return;
            }

            if(SceneManager.HasInstance) {
                SceneManager.Instance.UnloadScene(SceneName, callback);
            }
        }

        public virtual void OnEnter()
        {
            Debug.Log($"Enter State: {Name}");
        }

        public virtual void OnUpdate(float dt)
        {
        }

        public virtual void OnExit()
        {
            Debug.Log($"Exit State: {Name}");
        }
    }
}
