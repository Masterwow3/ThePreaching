using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ThePreaching.Base;

namespace ThePreaching.Views.Splash.ViewModel
{
    public class SplashViewModel : INotifyPropertyChanged
    {
        private string _loadingText;

        public SplashViewModel()
        {
            Application.WebserviceConnection = new WebserviceConnection();
        }

        public async Task Initializing()
        {
            LoadingText = "Suche The Preaching Server";
            await Application.WebserviceConnection.FindWebService();
            LoadingText = "Abgeschlossen";
        }

        public string LoadingText
        {
            get { return _loadingText; }
            set
            {
                _loadingText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}