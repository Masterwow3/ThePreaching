using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PreachingServer.Annotations;
using PreachingServer.Server;
using PreachingServer.Server.WebService;
using ThePreaching.Base;

namespace PreachingServer.Views.Main.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ServerDisclosure _disclosure;
        private Settings _settings;
        private int _port;
        private string _currentPreacher;
        private string _serverName;
        private WebService _webService;
        private bool _setServerNameEnabled;

        public MainViewModel()
        {
            SetServerNameEnabled = true;
            SetServernameCommand = new DelegateCommand(StartServer);
            SetPreacherCommand = new DelegateCommand(SetPreacher);
            Settings = new Settings();
        }



        #region Properties
        #region Commands

        public ICommand SetServernameCommand { get; private set; }
        public ICommand SetPreacherCommand { get; private set; }
        #endregion

        public bool SetServerNameEnabled
        {
            get { return _setServerNameEnabled; }
            set
            {
                _setServerNameEnabled = value;
                OnPropertyChanged();
            }
        }

        public Settings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }

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

        private void SetPreacher(object param)
        {
            Settings.CurrentPreacher = CurrentPreacher;
        }
        private async void StartServer(object param)
        {
            if (String.IsNullOrWhiteSpace(ServerName))
                return;
            if (Port < 1023 || Port > 65535)
                return;
            Settings.Port = this.Port;
            Settings.ServerName = this.ServerName;
            _disclosure = new ServerDisclosure(Settings.ServerName);
            _disclosure.StartDisclosure();
            _webService = new WebService(Settings.ServerName,Settings.Port);
            _webService.StartWebService();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}