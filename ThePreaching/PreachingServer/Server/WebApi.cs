using System;
using PreachingServer.Views.Main.ViewModel;

namespace PreachingServer.Server
{
    public static class WebApi
    {
        public static WebService.WebService WebService { get; private set; }
        public static ServerDisclosure Disclosure { get; private set; }

        public static async void Start()
        {
            if (String.IsNullOrWhiteSpace(AppSettings.Settings.ServerName))
                return;
            if (AppSettings.Settings.Port < 1023 || AppSettings.Settings.Port > 65535)
                return;
            Disclosure = new ServerDisclosure($"{AppSettings.Settings.ServerName}:{AppSettings.Settings.Port}");
            Disclosure.StartDisclosure();
            WebService = new WebService.WebService(AppSettings.Settings.ServerName, AppSettings.Settings.Port);
            WebService.StartWebService();
        }
        public static async void Stop()
         {
            Disclosure?.StopDisclosure();
            WebService?.StopWebService();
        }
    }
}