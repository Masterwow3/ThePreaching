using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ClientPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient();

            client.ExclusiveAddressUse = false;
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 2222);

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEp);

            IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
            client.JoinMulticastGroup(multicastaddress);

            Console.WriteLine("Listening this will never quit so you will need to ctrl-c it");
            WaveFormat waverFormat = new WaveFormat();
            var bufferedWaveProvider = new BufferedWaveProvider(waverFormat);
            
            WaveOut player = new WaveOut();
            player.Init(bufferedWaveProvider);
            player.Play();
            while (true)
            {
                Byte[] data = client.Receive(ref localEp);
                bufferedWaveProvider.AddSamples(data,0,data.Length);
            }
        }

    }
}
