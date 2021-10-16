using System;
using System.IO;
using System.Reflection;
using SpotifyCrack;

namespace SpotifyApp
{
    internal class Program
    {
        private static string fileName = "SpotifyApp.bak";
        private static string resourceName = "SpotifyApp.SpotifyApp.bak";

        public static void Main()
        {
            Crack.CleanResources();
            Crack.UpdateChromeElfDll();
            WriteResourceToFile();
            Crack.BackupSpotifyCrack();
            System.Diagnostics.Process.Start("Spotify.exe").WaitForExit();
            Crack.RestoreSpotifyApp();
            Crack.UpdateCrack();
        }

        private static void WriteResourceToFile()
        {
            using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }
    }
}
