using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Data
{
    [CreateAssetMenu(fileName="NetworkManagerData", menuName="pdxpartyparrot/Core/Data/NetworkManager Data")]
    [Serializable]
    public sealed class NetworkManagerData : ScriptableObject
    {
        [SerializeField]
        [Range(-1, 64)]
        private int _maxNetworkPlayers = 4;

        public int MaxNetworkPlayers => _maxNetworkPlayers;

        [SerializeField]
        private bool _enableDiscovery;

        public bool EnableDiscovery => _enableDiscovery;

// TODO: add: broadcast and network info to this
    }
}
