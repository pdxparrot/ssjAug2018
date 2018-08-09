using System;

using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    public sealed class ServerAddPlayerEventArgs : EventArgs
    {
        public NetworkConnection NetworkConnection { get; }

        public short PlayerControllerId { get; }

        public ServerAddPlayerEventArgs(NetworkConnection conn, short playerControllerId)
        {
            NetworkConnection = conn;
            PlayerControllerId = playerControllerId;
        }
    }
}
