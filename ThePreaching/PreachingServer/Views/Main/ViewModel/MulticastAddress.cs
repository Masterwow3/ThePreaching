using System;

namespace PreachingServer.Views.Main.ViewModel
{
    public static class MulticastAddress
    {
        public static string RandomMulticastIp()
        {
            string baseString = "224.0.0.";
            Random rn = new Random();
            baseString += rn.Next(118, 224).ToString();
            return baseString;
        }
    }
}