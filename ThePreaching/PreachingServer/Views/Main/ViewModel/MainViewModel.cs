using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PreachingServer.Annotations;
using PreachingServer.Server;
using PreachingServer.Server.WebService;
using PreachingServer.Views.Main.View;
using ThePreaching.Base;
using System.Speech.Recognition;

namespace PreachingServer.Views.Main.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Settings _settings;
        private int _port;
        private string _currentPreacher;
        private string _serverName;
        private bool _setServerNameEnabled;

        public MainViewModel()
        {
            SetServerNameEnabled = true;
            SetPreacherCommand = new DelegateCommand(SetPreacher);
            OpenSettingsCommand = new DelegateCommand(OpenSettings);
        }



        #region Properties
        #region Commands
        
        public ICommand SetPreacherCommand { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }
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
        #region Commands

        private void OpenSettings()
        {
            SettingsView view = new SettingsView();
            view.DataContext = new SettingsViewModel();
            view.ShowDialog();
        }
        #endregion
        private async Task RefreshUserList()
        {
            
        }
        private void SetPreacher()
        {
            AppSettings.CurrentPreacher = CurrentPreacher;
        }

        private void startTest()
        {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            Grammar dictationGrammar = new DictationGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            try
            {
                recognizer.SetInputToDefaultAudioDevice();
                RecognitionResult result = recognizer.Recognize();
                string text = result.Text;
            }
            catch (InvalidOperationException exception)
            {
                var error = String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message);
            }
            finally
            {
                recognizer.UnloadAllGrammars();
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