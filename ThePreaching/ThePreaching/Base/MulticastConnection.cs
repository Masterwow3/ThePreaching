﻿using System.Net;
using System.Net.Sockets;

namespace ThePreaching.Base
{
    public class MulticastConnection
    {
        //IP Range Multicast: 224.0.0.116 - 224.0.0.250
        public MulticastConnection(IPAddress multicastAddress)
        {
            MulticastAddress = multicastAddress;
            UdpClient = new UdpClient();
            UdpClient.JoinMulticastGroup(multicastAddress);
            RemoteEndPoint = new IPEndPoint(multicastAddress, 2222);

        }

        #region Properties

        public IPAddress MulticastAddress { get; private set; }
        public UdpClient UdpClient { get;private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        #endregion
    }
}
