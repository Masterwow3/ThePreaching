using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Entitie.Requests.Base;
using Entitie.Requests.Multicast;

namespace ThePreaching.Base
{
    public class WebserviceConnection
    {
        public WebserviceConnection()
        {
            Initializer();
        }

        #region Methods

        private async Task Initializer()
        {
            UserGuid = Guid.NewGuid().ToString();
        }
        public async Task FindWebService()
        {
            string webServiceName = "";
            MulticastConnection multicastConnection = new MulticastConnection(IPAddress.Parse(MulticastCommunication.FindServerMulticastAddress));
            var endpoint = multicastConnection.RemoteEndPoint;

            var whoIsServerBuffer = Encoding.Unicode.GetBytes(MulticastCommunication.WhoIsServer);
            Task.Run(() =>
            {
                while (String.IsNullOrWhiteSpace(webServiceName))
                {
                    multicastConnection.UdpClient.SendAsync(whoIsServerBuffer, whoIsServerBuffer.Length, endpoint);
                    System.Threading.Thread.Sleep(1000);
                }
                
            });

            UdpClient client = new UdpClient();

            client.ExclusiveAddressUse = false;
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 2222);

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEp);

            IPAddress multicastaddress = IPAddress.Parse(MulticastCommunication.FindServerMulticastAddress);
            client.JoinMulticastGroup(multicastaddress);
            await Task.Run(() =>
            {
                while (true)
                {
                    Byte[] data = client.Receive(ref localEp);
                    string strData = Encoding.Unicode.GetString(data);
                    if (strData.StartsWith(MulticastCommunication.ImTheServer))
                    {
                        webServiceName = MulticastCommunication.ServerNameFromImTheServerMessage(strData);
                        break;
                    }
                }
            });
            WebServiceUrl = $"http://{webServiceName}/ThePreaching";
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(WebServiceUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<object> PostWebservice(object postObject, string webserviceMethod)
        {
            var type = postObject.GetType();
            var request = new Request<object>() { Data = postObject, UserGuid = UserGuid };
            string postBody = JsonSerializer(request);
            HttpResponseMessage response =
                await HttpClient.PostAsync(webserviceMethod, new StringContent(postBody, Encoding.UTF8, "application/json"));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(postObject.GetType());
            var content = await response.Content.ReadAsStreamAsync();
            return jsonSerializer.ReadObject(content);
        }
        private string JsonSerializer(object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentException("objectToSerialize must not be null");
            }
            MemoryStream ms = null;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
            ms = new MemoryStream();
            serializer.WriteObject(ms, objectToSerialize);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }
        #endregion
        #region Properties

        public string WebServiceUrl { get; private set; }
        private HttpClient HttpClient { get; set; }
        public string UserGuid { get; private set; }
        #endregion
    }
}