using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PreachingServer.Annotations;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Entitie.Requests.Result;

namespace PreachingServer.Views.Main.ViewModel
{
    [Serializable]
    public class Settings : INotifyPropertyChanged
    {
        #region Fields
        private string _serverName;
        private int _port;
        private string _currentPreacher;
        #endregion

        #region Properties
        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                OnPropertyChanged();
            }
        }
        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }
        public string CurrentPreacher
        {
            get { return _currentPreacher; }
            set
            {
                _currentPreacher = value;
                OnPropertyChanged();
            }
        }
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