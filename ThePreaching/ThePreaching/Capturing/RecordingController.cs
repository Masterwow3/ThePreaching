using System.IO;
using System.Runtime.InteropServices;

namespace ThePreaching.Capturing
{
    public class RecordingController
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public void StartRecording()
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
        }

        public void StopRecording(string savePath, string filename)
        {
           var path = Path.Combine(savePath, filename);
            path += ".wav";
            mciSendString($"save recsound {path}", "", 0, 0);
            mciSendString("close recsound", "", 0, 0);
        }
    }
}