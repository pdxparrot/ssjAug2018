using System;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public sealed class GameStateManager : SingletonBehavior<GameStateManager>
    {
        [SerializeField]
        private GameState _initialGameStatePrefab;

        [SerializeField]
        [ReadOnly]
        private GameState _currentGameState;

        public GameState CurrentState => _currentGameState;

#region Unity Lifecycle
        private void Awake()
        {
// TODO: allocate and disable *all* game states

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            ExitCurrentState(null);

// TODO: destroy all game states

            base.OnDestroy();
        }

        private void Update()
        {
            _currentGameState?.OnUpdate(Time.deltaTime);
        }
#endregion

        public void TransitionToInitialState(Action<GameState> initializeState=null)
        {
            TransitionState(_initialGameStatePrefab, initializeState);
        }

        public void TransitionState(GameState gameStatePrefab, Action<GameState> initializeState=null)
        {
            ShowLoadingScreen(true);

            ExitCurrentState(() => {
                // TODO: this should enable the state from the set rather than allocating
                _currentGameState = Instantiate(gameStatePrefab, transform);
                initializeState?.Invoke(_currentGameState);

                _currentGameState.LoadScene(() => {
                    _currentGameState.OnEnter();

                    ShowLoadingScreen(false);

                    Debug.Log($"State: {_currentGameState.Name}");
                });

            });
        }

        private void ExitCurrentState(Action callback)
        {
            if(null == _currentGameState) {
                callback?.Invoke();
                return;
            }

            _currentGameState.UnloadScene(() => {
                _currentGameState?.OnExit();

                // TODO: disable the state, don't destroy it
                Destroy(_currentGameState?.gameObject);
                _currentGameState = null;

                callback?.Invoke();
            });
        }

        private void ShowLoadingScreen(bool show)
        {
Debug.Log($"TODO: show loading screen: {show}");
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "GameStateManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Current Game State: {CurrentState.name}");
            };
        }
    }
}
