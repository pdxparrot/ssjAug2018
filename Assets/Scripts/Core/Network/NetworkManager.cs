using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    [RequireComponent(typeof(NetworkManagerHUD))]
    public sealed class NetworkManager : UnityEngine.Networking.NetworkManager
    {
        [SerializeField]
        private int _maxNetworkPlayers = 16;

        public int MaxNetworkPlayers => _maxNetworkPlayers;

        public void Initialize(bool enableNetwork)
        {
            if(!enableNetwork) {
                StartLANHost();
            }

            GetComponent<NetworkManagerHUD>().enabled = enableNetwork;
        }

        private void StartLANHost()
        {
            Debug.Log($"Starting LAN host on {networkAddress}:{networkPort}...");

            StartServer();
            StartClient();
        }
    }
}
