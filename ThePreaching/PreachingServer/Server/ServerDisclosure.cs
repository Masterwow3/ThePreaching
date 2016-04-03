using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Entitie.Requests.Multicast;

namespace PreachingServer.Server
{
    public class ServerDisclosure
    {
        private UdpClient _client;
        private IPEndPoint _localEp;
        private IPEndPoint _remoteep;
        private string _serverName;
        private bool _announcerIsRunning ;
        private CancellationTokenSource _cancellationTokenSource ;

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
            IPAddress multicastaddress = IPAddress.Parse(MulticastCommunication.FindServerMulticastAddress);
            _remoteep = new IPEndPoint(multicastaddress, 2222);
            _client.JoinMulticastGroup(multicastaddress);
        }

        public void StartDisclosure()
        {
            if(_announcerIsRunning)
                return;
            _cancellationTokenSource = new CancellationTokenSource();
            ServerAnnouncer(_cancellationTokenSource);
        }

        public void StopDisclosure()
        {
            _cancellationTokenSource.Cancel();
        }
        private async Task ServerAnnouncer(CancellationTokenSource source)
        {
            
            CancellationToken ct = source.Token;
            await Task.Run(() =>
            {
                _announcerIsRunning = true;
                var buffer = Encoding.Unicode.GetBytes(MulticastCommunication.ImTheServer + _serverName);
                while (true)
                {
                    if(ct.IsCancellationRequested)
                        break;
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