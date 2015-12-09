using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PreachingServer.Annotations;
using PreachingServer.Base.Result;

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
            
            return new MethodResult();
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
            
            return new MethodResult();
        }
    }
}