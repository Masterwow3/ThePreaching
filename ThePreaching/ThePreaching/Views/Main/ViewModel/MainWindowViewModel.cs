using System.Windows.Input;
using Sockets.Plugin;
using ThePreaching.Base;
using ThePreaching.Capturing;

namespace ThePreaching.Views.Main.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            StartRecordingCommand = new DelegateCommand(StartRecording);
            RecController = new RecordingController();
        }

        #region Properties
        #region Commands

        public ICommand StartRecordingCommand { get;private set; }
        #endregion

        public RecordingController RecController { get; set; }
        #endregion
        #region Methods

        private async void StartRecording(object param)
        {
            var start = (bool) param;
            if (!start) {
                StopRecording(null);
                return;
            }
            RecController.StartRecording(@"C:\test01.wav");
            //UdpSocketMulticastClient receiver = new UdpSocketMulticastClient();
            //receiver.JoinMulticastGroupAsync()

        }

        private void StopRecording(object param)
        {
            RecController.StopRecording();
        }
        #endregion

    }
}