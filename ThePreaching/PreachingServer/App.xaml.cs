using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PreachingServer.Server;
using PreachingServer.Views.Main.ViewModel;

namespace PreachingServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Exit += (sender, args) => WebApi.Stop();
            AppSettings.Settings = new Settings();
            AppSettings.Settings.LoadSettings();
            WebApi.Start();
        }
    }
}
