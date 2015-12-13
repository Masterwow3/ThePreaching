using System;
using System.Net;

namespace Entitie.Requests.Multicast
{
    public static class MulticastCommunication
    {
        public const string FindServerMulticastAddress = "239.0.0.1";
         public const string WhoIsServer = "WhoIsServer";
         public const string ImTheServer = "S:";

        public static string ServerNameFromImTheServerMessage(string message)
        {
            if (!message.StartsWith(ImTheServer))
                return "";
            return message.Split(new string[] {ImTheServer}, StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
}