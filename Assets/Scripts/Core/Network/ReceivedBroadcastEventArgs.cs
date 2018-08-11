using System;
using System.Net;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.Core.Network
{
    public sealed class ReceivedBroadcastEventArgs : EventArgs
    {
        public string FromAddress { get; }

        public string Data { get; }

        private readonly IPEndPoint _endPoint;

        public IPEndPoint EndPoint => _endPoint;

        public ReceivedBroadcastEventArgs(string fromAddress, string data, string endPoint)
        {
            FromAddress = fromAddress;
            Data = data;

            IPEndPointExtensions.TryParseWithPort(endPoint, out _endPoint);
        }
    }
}
