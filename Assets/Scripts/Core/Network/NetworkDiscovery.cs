using System;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    public sealed class NetworkDiscovery : UnityEngine.Networking.NetworkDiscovery
    {
#region Events
        public event EventHandler<ReceivedBroadcastEventArgs> ReceivedBroadcastEvent;
#endregion

        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            Debug.Log($"[NetworkDiscovery]: Broadcast from {fromAddress}: {data}");

            ReceivedBroadcastEvent?.Invoke(this, new ReceivedBroadcastEventArgs(fromAddress, data));
        }
    }
}
