﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ThePreachingServer
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

        }

        #region Properties

        public IPAddress MulticastAddress { get; private set; }
        public UdpClient UdpClient { get;private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        #endregion
    }
}
