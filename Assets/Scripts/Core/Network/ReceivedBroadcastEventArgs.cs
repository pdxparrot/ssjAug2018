using System;

namespace pdxpartyparrot.Core.Network
{
    public sealed class ReceivedBroadcastEventArgs : EventArgs
    {
        public string FromAddress { get; }

        public string Data { get; }

        public ReceivedBroadcastEventArgs(string fromAddress, string data)
        {
            FromAddress = fromAddress;
            Data = data;
        }
    }
}
