using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ThePreachingServer
{
    public class Main : IDisposable
    {
        private MulticastConnection multicastConnection;
        public Main()
        {
            SendServerIp();
            multicastConnection = new MulticastConnection(IPAddress.Parse("224.0.0.117")); // Default
        }
        public async Task SendServerIp()
        {
            string hostName ="TPH:"+ Dns.GetHostName();
            var buffer = Encoding.Unicode.GetBytes(hostName);
            while (true)
            {
                multicastConnection.UdpClient.Send(buffer, buffer.Length, multicastConnection.RemoteEndPoint);
                System.Threading.Thread.Sleep(2000);
            }
        }


        public void Dispose()
        {
            
        }
    }
}