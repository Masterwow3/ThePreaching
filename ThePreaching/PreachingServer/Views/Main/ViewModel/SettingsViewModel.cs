using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Entitie.Requests.Result;
using PreachingServer.Annotations;
using PreachingServer.Server;
using ThePreaching.Base;

namespace PreachingServer.Views.Main.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<KeyValuePair<string, string>> _allowedLanguages;
        private int _port;
        private string _serverName;
        #endregion

        public SettingsViewModel()
        {
            InitializeCommands();
            ServerName = AppSettings.Settings.ServerName;
            Port = AppSettings.Settings.Port;
            AllowedLanguages = new ObservableCollection<KeyValuePair<string, string>>(AppSettings.Settings.AllowedLanguages.Select(x=>new KeyValuePair<string,string>(x.Key,x.Value)));
        }

        #region Methods
        #region Commands
        private void InitializeCommands()
        {
            AddLanguageCommand = new DelegateCommand<string>(AddLanguage);
            SaveSettingsCommand = new DelegateCommand(SaveSettings);
            RestartWebApiCommand = new DelegateCommand(RestartWebApi);
        }
        private void AddLanguage(string languageName)
        {
            if (AllowedLanguages.Any(x => x.Key.Trim().ToLower() == languageName.Trim().ToLower()))
                MessageBox.Show("Die Sprache wurde bereits hinzugefügt", "Fehler");
            else
            {
                string multicastAddress = MulticastAddress.RandomMulticastIp();
                while (AllowedLanguages.All(x => x.Value != multicastAddress))
                {
                    multicastAddress = MulticastAddress.RandomMulticastIp();
                }

                AllowedLanguages.Add(new KeyValuePair<string, string>(languageName, multicastAddress));
            }
        }
        private void SaveSettings()
        {
            AppSettings.Settings.AllowedLanguages = AllowedLanguages.ToDictionary(x => x.Key, f => f.Value);
            AppSettings.Settings.Port = Port;
            AppSettings.Settings.ServerName = ServerName;
            var result = AppSettings.Settings.SaveSettings();
            if (result.State != ResultState.Success)
                MessageBox.Show(result.Message, "Achtung");
        }

        private void RestartWebApi()
        {
            WebApi.Stop();
            WebApi.Start();
        }
        #endregion


        #endregion
        #region Properties
        #region Commands

        public ICommand AddLanguageCommand { get; private set; }
        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand RestartWebApiCommand { get; private set; }

        #endregion
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

        public ObservableCollection<KeyValuePair<string, string>> AllowedLanguages
        {
            get { return _allowedLanguages; }
            set
            {
                _allowedLanguages = value;
                OnPropertyChanged();
            }
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