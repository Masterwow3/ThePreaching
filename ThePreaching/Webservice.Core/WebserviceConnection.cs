using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Entitie.Requests.Base;
using Entitie.Requests.Multicast;
using Sockets.Plugin;

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
            

            UdpSocketMulticastClient receiver = new UdpSocketMulticastClient();
            receiver.TTL = 5;
            
                receiver.MessageReceived += (sender, args) =>
                {
                    var data = Encoding.Unicode.GetString(args.ByteData, 0, args.ByteData.Length);
                    if (data.StartsWith(MulticastCommunication.ImTheServer))
                    {
                        webServiceName = MulticastCommunication.ServerNameFromImTheServerMessage(data);
                        receiver.DisconnectAsync();
                    }
                };
            try
            {
                await receiver.JoinMulticastGroupAsync(MulticastCommunication.FindServerMulticastAddress, 2222);
            }
            catch (Exception ex)
            {

            }
            var whoIsServerBuffer = Encoding.Unicode.GetBytes(MulticastCommunication.WhoIsServer);
            await Task.Run(() =>
            {
                while (String.IsNullOrWhiteSpace(webServiceName))
                {
                    receiver.SendMulticastAsync(whoIsServerBuffer);
                    Task.Delay(1000);
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