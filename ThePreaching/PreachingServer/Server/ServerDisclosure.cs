using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PreachingServer.Server
{
    public class ServerDisclosure
    {
        private UdpClient _client;
        private IPEndPoint _localEp;
        private IPEndPoint _remoteep;
        private string _serverName;
        private bool _announcerIsRunning ;
        public ServerDisclosure(string serverName)
        {
            _client = new UdpClient();
            _announcerIsRunning = false;
            _serverName = serverName;
            _client.ExclusiveAddressUse = false;
            _localEp = new IPEndPoint(IPAddress.Any, 2222);
            _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _client.ExclusiveAddressUse = false;

            _client.Client.Bind(_localEp);
            IPAddress multicastaddress = IPAddress.Parse("239.0.0.1");
            _remoteep = new IPEndPoint(multicastaddress, 2222);
            _client.JoinMulticastGroup(multicastaddress);
        }

        public void StartDisclosure()
        {
            ServerAnnouncer();
        }
        private async Task ServerAnnouncer()
        {
            await Task.Run(() =>
            {
                _announcerIsRunning = true;
                var buffer = Encoding.Unicode.GetBytes(MulticastCommunication.ImTheServer + _serverName);
                while (true)
                {
                    Byte[] data = _client.Receive(ref _localEp);
                    string strData = Encoding.Unicode.GetString(data);
                    if (strData == MulticastCommunication.WhoIsServer)
                    {
                        _client.Send(buffer, buffer.Length, _remoteep);
                    }
                }
            });
            _announcerIsRunning = false;
        }


    }
}