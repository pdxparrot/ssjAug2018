using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public sealed class GameStateManager : SingletonBehavior<GameStateManager>
    {
        public interface IGameState
        {
            string Name { get; }

            void OnEnter();

            void OnUpdate(float dt);

            void OnExit();
        }

        [SerializeField]
        private GameState _initialGameStatePrefab;

        [SerializeField]
        [ReadOnly]
        private IGameState _currentGameState;

        public IGameState CurrentState => _currentGameState;

        private readonly Queue<IGameState> _stateQueue = new Queue<IGameState>();

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
            float dt = Time.deltaTime;

            _currentGameState?.OnUpdate(dt);
        }
#endregion

        public void TransitionToInitialState(Action<GameState> initializeState=null)
        {
            Debug.Log("Transition to initial state");
            TransitionState(_initialGameStatePrefab, initializeState);
        }

        public void TransitionState(GameState gameStatePrefab, Action<GameState> initializeState=null)
        {
            ShowLoadingScreen(true);

            ExitCurrentState(() => {
                // TODO: this should enable the state from the set rather than allocating
                GameState gameState = Instantiate(gameStatePrefab, transform);
                initializeState?.Invoke(gameState);

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

            while(_stateQueue.Count > 0) {
                PopSubState();
            }

            GameState gameState = (GameState)_currentGameState;
            _currentGameState = null;

            gameState.UnloadScene(() => {
                gameState?.OnExit();

                // TODO: disable the state, don't destroy it
                Destroy(gameState?.gameObject);

                callback?.Invoke();
            });
        }

        public void PushSubState(SubGameState gameStatePrefab, Action<SubGameState> initializeState=null)
        {
            // enqueue the current state if we have one
            if(null != _currentGameState) {
                _stateQueue.Enqueue(_currentGameState);
            }

            // new state is now the current state
            // TODO: this should enable the state from the set rather than allocating
            SubGameState gameState = Instantiate(gameStatePrefab, transform);
            initializeState?.Invoke(gameState);

            _currentGameState = gameState;
            _currentGameState.OnEnter();
        }

        public void PopSubState()
        {
            SubGameState previousState = (SubGameState)_currentGameState;
            _currentGameState = null;

            previousState?.OnExit();
            Destroy(previousState?.gameObject);

            _currentGameState = _stateQueue.Count > 0 ? _stateQueue.Dequeue() : null;
        }

        private void ShowLoadingScreen(bool show)
        {
Debug.Log($"TODO: show loading screen: {show}");
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "GameStateManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Current Game State: {CurrentState.Name}");
            };
        }
    }
}
