using System;

using UnityEngine;

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

            int idx = data.IndexOf(":", StringComparison.Ordinal);
            if(idx < 0 || idx >= data.Length - 1) {
                return;
            }

            ReceivedBroadcastEvent?.Invoke(this, new ReceivedBroadcastEventArgs(fromAddress, data, data.Substring(idx + 1)));
        }
    }
}
