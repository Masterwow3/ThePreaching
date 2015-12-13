using System.Net;
using System.Net.Sockets;

namespace ThePreaching.Base
{
    public class MulticastConnection
    {
        //IP Range Multicast: 239.0.0.1 - 239.255.255.255
        public MulticastConnection(IPAddress multicastAddress)
        {
            MulticastAddress = multicastAddress;
            UdpClient = new UdpClient();
            UdpClient.JoinMulticastGroup(multicastAddress);
            RemoteEndPoint = new IPEndPoint(multicastAddress, 2222);
            LocalEndPoint = new IPEndPoint(IPAddress.Any, 2222);
        }

        #region Properties

        public IPAddress MulticastAddress { get; private set; }
        public UdpClient UdpClient { get;private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        public IPEndPoint LocalEndPoint { get; private set; }
        #endregion
    }
}
