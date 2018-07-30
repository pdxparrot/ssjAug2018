using System;

using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    public sealed class ServerAddPlayerEventArgs : EventArgs
    {
        public NetworkConnection NetworkConnection { get; set; }

        public short PlayerControllerId { get; set; }

        public ServerAddPlayerEventArgs(NetworkConnection conn, short playerControllerId)
        {
            NetworkConnection = conn;
            PlayerControllerId = playerControllerId;
        }
    }
}
