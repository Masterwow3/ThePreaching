using System.Windows.Input;
using ThePreaching.Base;
using ThePreaching.Capturing;

namespace ThePreaching.Views.Main.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            StartRecordingCommand = new DelegateCommand(StartRecording);
            StopRecordingCommand = new DelegateCommand(StopRecording);
            RecController = new RecordingController();
        }

        #region Properties
        #region Commands

        public ICommand StartRecordingCommand { get;private set; }
        public ICommand StopRecordingCommand { get;private set; }
        #endregion

        public RecordingController RecController { get; set; }
        #endregion
        #region Methods

        private void StartRecording(object param)
        {
            RecController.StartRecording();
        }

        private void StopRecording(object param)
        {
            RecController.StopRecording(@"C:\","test01");
        }
        #endregion

    }
}