using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    // https://bitbucket.org/Unity-Technologies/networking
    [RequireComponent(typeof(NetworkManagerHUD))]
    public sealed class NetworkManager : UnityEngine.Networking.NetworkManager
    {
        public static NetworkManager Instance => (NetworkManager)singleton;

        [SerializeField]
        private int _maxNetworkPlayers = 16;

        public int MaxNetworkPlayers => _maxNetworkPlayers;

        public NetworkManagerHUD HUD => GetComponent<NetworkManagerHUD>();

        // TODO: whenever this becomes a thing...
/*
        protected override void Awake()
        {
            base.Awake();

            HUD.showGUI = false;
        }
*/

        private void Start()
        {
            HUD.showGUI = false;
        }

        public void StartLANHost()
        {
            Debug.Log($"Starting LAN host on {networkAddress}:{networkPort}...");

            StartHost();
        }

        public void Stop()
        {
            // TODO: don't assume this
            StopHost();
        }
    }
}
