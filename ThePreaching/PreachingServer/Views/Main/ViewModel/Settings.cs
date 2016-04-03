using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PreachingServer.Annotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Entitie.Requests.Result;

namespace PreachingServer.Views.Main.ViewModel
{
    [Serializable]
    public class Settings
    {
        public Settings()
        {
            ServerName = GetLocalIPAddress();
            Port = 8777;
            AllowedLanguages = new Dictionary<string, string>();
        }

        #region Properties
        public string ServerName { get; set; }
        public int Port { get; set; }
        public Dictionary<string,string> AllowedLanguages { get; set; }
        #endregion

        #region Methods

        public IResult SaveSettings()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = appData + Properties.Settings.Default.AppDataPath;
            CreateFolders(path);

            var fullSettingsPath = path + Properties.Settings.Default.SettingFileName;
            FileStream stream;
            stream = new FileStream(fullSettingsPath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Close();

            return new MethodResult();
        }

        private void CreateFolders(string path)
        {
            string[] folders = path.Split('\\');

            var currentPath = "";
            foreach (var folder in folders)
            {
                currentPath += folder + "\\";
                if (!Directory.Exists(currentPath))
                    Directory.CreateDirectory(currentPath);
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string GetLocalIPAddress()
        {
            //var host = Dns.GetHostEntry(Dns.GetHostName());
            Dictionary<string,int> ipRanking = new Dictionary<string, int>();

            // Get a list of all network interfaces (usually one per network card, dialup, and VPN connection) 
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface network in networkInterfaces.Where(x=>x.OperationalStatus != OperationalStatus.Down && x.NetworkInterfaceType != NetworkInterfaceType.Loopback))
            {
                // Read the IP configuration for each network 
                IPInterfaceProperties properties = network.GetIPProperties();

                // Each network interface may have multiple IP addresses 
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    // We're only interested in IPv4 addresses for now 
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    // Ignore loopback addresses (e.g., 127.0.0.1) 
                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    int rank = 0;
                    rank += properties.DhcpServerAddresses.Count;
                    try
                    {
                        ipRanking.Add(address.Address.ToString(), rank);
                    }
                    catch (ArgumentException ex)
                    {
                        ipRanking[address.Address.ToString()] += 1;
                    }
                }
            }
            if(ipRanking.Count == 0)
                return System.Security.Principal.WindowsIdentity.GetCurrent()?.Name.ToString();
            return ipRanking.OrderByDescending(x=>x.Value).First().Key;
        }
        public IResult LoadSettings()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = appData + Properties.Settings.Default.AppDataPath;
            var fullSettingsPath = path + Properties.Settings.Default.SettingFileName;

            if (!File.Exists(fullSettingsPath))
                return new MethodResult(ResultState.Error,"File not found");
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(fullSettingsPath, FileMode.Open);
                var settings = (Settings) formatter.Deserialize(stream);
                stream.Close();
                this.Port = settings.Port;
                this.ServerName = settings.ServerName;

                return new MethodResult();
            }
            catch (Exception ex)
            {
                return new MethodResult(ResultState.Error,ex.Message,ex);
            }
        }
    }
}