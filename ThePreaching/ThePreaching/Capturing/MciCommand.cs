namespace ThePreaching.Capturing
{
    public static class MciCommand
    {
         public const string OpenRecording = "open new Type waveaudio Alias recsound";
         public const string StartRecording = "record recsound";
         public const string StopRecording = "save recsound";
         public const string CloseRecording = "close recsound";
    }
}