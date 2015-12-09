using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using NAudio.Wave;
using ThePreachingServer;
using MulticastConnection = ThePreaching.Base.MulticastConnection;

namespace ThePreaching.Capturing
{
    public class RecordingController
    {
        private WaveIn recorder;
        private BufferedWaveProvider bufferedWaveProvider;
        private SavingWaveProvider savingWaveProvider;
        private readonly MulticastConnection multicastConnection;

        public RecordingController()
        {
            multicastConnection = new MulticastConnection(IPAddress.Parse("239.0.0.222"));
        }


        public void StartRecording(string filePath)
        {
            // set up the recorder
            recorder = new WaveIn();
            recorder.WaveFormat = new WaveFormat();
            recorder.DataAvailable += RecorderOnDataAvailable;

            // set up our signal chain
            bufferedWaveProvider = new BufferedWaveProvider(recorder.WaveFormat);
            savingWaveProvider = new SavingWaveProvider(bufferedWaveProvider, filePath);
            
            recorder.StartRecording();
        }
        public void StopRecording()
        {
            // stop recording
            recorder.StopRecording();
            // finalise the WAV file
            savingWaveProvider.Dispose();

        }
        #region Events
        private void RecorderOnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            bufferedWaveProvider.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);
            multicastConnection.UdpClient.Send(waveInEventArgs.Buffer, waveInEventArgs.BytesRecorded, multicastConnection.RemoteEndPoint);
        }
        #endregion
    }
}