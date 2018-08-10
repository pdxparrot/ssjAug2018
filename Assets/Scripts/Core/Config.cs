using System;
using System.IO;
using System.Net;

using UnityEngine;

namespace pdxpartyparrot.Core
{
    [Serializable]
    public class Config
    {
        [Serializable]
        public struct NetworkConfig
        {
            [Serializable]
            public struct DiscoveryConfig
            {
                [SerializeField]
                private bool enable;

                public bool Enable => enable;

                [SerializeField]
                private int port;

                public int Port => port <= 0 ? 4777 : port;
            }

            [Serializable]
            public struct ServerConfig
            {
                [SerializeField]
                private string networkAddress;

                // TODO: this should only be true if it's an ip
                public bool BindIp()
                {
                    IPAddress address;
                    return IPAddress.TryParse(networkAddress, out address);
                }

                public string NetworkAddress => networkAddress;

                [SerializeField]
                private int port;

                public int Port => port <= 0 ? 7777 : port;

                [SerializeField]
                private int maxConnections;

                public int MaxConnections => maxConnections < 0 ? 0 : maxConnections;
            }

            [SerializeField]
            private DiscoveryConfig discovery;

            public DiscoveryConfig Discovery => discovery;

            [SerializeField]
            private ServerConfig server;

            public ServerConfig Server => server;
        }

        [SerializeField]
        private NetworkConfig network;

        public NetworkConfig Network => network;

        public void Load(string path, string fileName)
        {
            string configPath = Path.Combine(path, fileName);
            if(!File.Exists(configPath)) {
                return;
            }

            Debug.Log($"Loading config from {configPath}...");

            string configJson = File.ReadAllText(configPath);
            JsonUtility.FromJsonOverwrite(configJson, this);
        }
    }
}
