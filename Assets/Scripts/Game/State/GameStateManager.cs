using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public abstract class GameStateManager<T> : SingletonBehavior<T> where T: GameStateManager<T>
    {
        [SerializeField]
        private GameState _initialGameStatePrefab;

        [SerializeField]
        [ReadOnly]
        private IGameState _currentGameState;

        [CanBeNull]
        public IGameState CurrentState => _currentGameState;

        private readonly Stack<IGameState> _stateStack = new Stack<IGameState>();

#region Unity Lifecycle
        protected virtual void Awake()
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
            float dt = Time.deltaTime;

            _currentGameState?.OnUpdate(dt);
        }
#endregion

        public void TransitionToInitialState(Action<GameState> initializeState=null)
        {
            Debug.Log("Transition to initial state");
            TransitionState(_initialGameStatePrefab, initializeState);
        }

        public void TransitionState<TV>(TV gameStatePrefab, Action<TV> initializeState=null) where TV: GameState
        {
            ShowLoadingScreen(true);

            ExitCurrentState(() => {
                // TODO: this should enable the state from the set rather than allocating
                TV gameState = Instantiate(gameStatePrefab, transform);
                initializeState?.Invoke(gameState);

                UpdateLoadingScreen(0.5f, "Loading scene...");
                gameState.LoadScene(() => {
                    _currentGameState = gameState;
                    _currentGameState.OnEnter();

                    ShowLoadingScreen(false);
                });

            });
        }

        private void ExitCurrentState(Action callback)
        {
            if(null == _currentGameState) {
                callback?.Invoke();
                return;
            }

            while(_stateStack.Count > 0 && !(_currentGameState is GameState)) {
                PopSubState();
            }

            GameState gameState = (GameState)_currentGameState;
            _currentGameState = null;

            gameState.UnloadScene(() => {
                if(null != gameState) {
                    gameState.OnExit();

                    // TODO: disable the state, don't destroy it
                    Destroy(gameState.gameObject);
                }

                callback?.Invoke();
            });
        }

        public void PushSubState<TV>(TV gameStatePrefab, Action<TV> initializeState=null) where TV: SubGameState
        {
            _currentGameState?.OnPause();

            // enqueue the current state if we have one
            if(null != _currentGameState) {
                _stateStack.Push(_currentGameState);
            }

            // new state is now the current state
            // TODO: this should enable the state from the set rather than allocating
            TV gameState = Instantiate(gameStatePrefab, transform);
            initializeState?.Invoke(gameState);

            _currentGameState = gameState;
            _currentGameState.OnEnter();
        }

        public void PopSubState()
        {
            SubGameState previousState = (SubGameState)_currentGameState;
            _currentGameState = null;

            if(null != previousState) {
                previousState.OnExit();
                Destroy(previousState.gameObject);
            }

            _currentGameState = _stateStack.Count > 0 ? _stateStack.Pop() : null;
            _currentGameState?.OnResume();
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.GameStateManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Current Game State: {CurrentState?.Name}");

                string text = "Reset";
                if(GUILayout.Button(text, GUIUtils.GetLayoutButtonSize(text))) {
                    TransitionToInitialState();
                }
            };
        }

        protected abstract void ShowLoadingScreen(bool show);

        protected abstract void UpdateLoadingScreen(float percent, string text);
    }
}
