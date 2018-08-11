using System.Net;

namespace pdxpartyparrot.Core.Util
{
    public static class IPEndPointExtensions
    {
        public static bool TryParseWithPort(string endPointStr, out IPEndPoint endpoint)
        {
            endpoint = null;

            string[] tokens = endPointStr.Split(':');
            if(tokens.Length < 2) {
                return false;
            }

            IPAddress ipAddress;
            if(tokens.Length > 2) {
                // IPv6
                if(!IPAddress.TryParse(string.Join(":", tokens, 0, tokens.Length - 1), out ipAddress)) {
                    return false;
                }
            } else {
                // IPv4
                if(!IPAddress.TryParse(tokens[0], out ipAddress)) {
                    return false;
                }
            }

            int port;
            if(!int.TryParse(tokens[tokens.Length - 1], out port)) {
                return false;
            }

            endpoint = new IPEndPoint(ipAddress, port);
            return true;
        }
    }
}
