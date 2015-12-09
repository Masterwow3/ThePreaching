using System;
using System.ServiceModel.Web;
using PreachingServer.Server.WebService.Rest;

namespace PreachingServer.Server.WebService
{
    public class WebService
    {
        private const string _baseUrl = "/ThePreaching";
        private string _serverUrl;
        private RestService _restService;
        private WebServiceHost _serviceHost;
        public WebService(string serverName, int serverPort)
        {
            _serverUrl = $"http://{serverName}:{serverPort}{_baseUrl}";
            _restService = new RestService();
            _serviceHost = new WebServiceHost(_restService, new Uri(_serverUrl));
        }

        public void StartWebService()
        {
            _serviceHost.Open();
        }
    }
}