using System;

using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class NetworkConnect : SubGameState
    {
        [SerializeField]
        private pdxpartyparrot.Game.State.GameState _gameStatePrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            Debug.Log("TODO: show connecting screen");

            NetworkManager.Instance.ClientConnectEvent += ClientConnectEventHandler;
// TODO: handle connection errors
            GameManager.Instance.NetworkClient = NetworkManager.Instance.StartLANHost();
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

// TODO: possible UI updates here
        }

        public override void OnExit()
        {
            if(NetworkManager.HasInstance) {
                NetworkManager.Instance.ClientConnectEvent += ClientConnectEventHandler;
            }

            base.OnExit();
        }

#region Event Handlers
        private void ClientConnectEventHandler(object sender, EventArgs args)
        {
            Debug.Log("TODO: hide connecting screen");

            GameStateManager.Instance.TransitionState(_gameStatePrefab);
        }
#endregion
    }
}
