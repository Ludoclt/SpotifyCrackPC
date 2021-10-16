using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Linq;
using Mono.Cecil;

namespace SpotifyCrack
{
    public class Crack
    {
        private static string crackName = "Spotify.exe";
        private static string zipName = "chrome_elf.zip";

        private static string assemblyPath = "SpotifyTmp.bak";
        private static string resourcePath = "SpotifyApp.bak";
        private static string resourceName = "SpotifyApp.SpotifyApp.bak";

        public static void BackupSpotifyCrack()
        {
            File.Move(crackName, assemblyPath);
            File.Move(resourcePath, crackName);
        }

        public static void RestoreSpotifyApp()
        {
            File.Move(crackName, resourcePath);
        }

        public static void UpdateCrack()
        {
            UpdateChromeElfDll();
            ReplaceAssemblyResource();
        }

        public static void UpdateChromeElfDll()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://github.com/mrpond/BlockTheSpot/releases/latest/download/chrome_elf.zip"), zipName);
            }

            File.Delete("chrome_elf.dll");
            File.Delete("config.ini");

            ZipFile.ExtractToDirectory(zipName, Directory.GetCurrentDirectory());
        }

        private static void ReplaceAssemblyResource()
        {
            AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(assemblyPath);

            Mono.Collections.Generic.Collection<Resource> resources = assemblyDef.MainModule.Resources;

            Resource selectedResource = resources.FirstOrDefault(x => x.Name == resourceName);

            if (selectedResource != null)
            {
                using (FileStream filestream = File.OpenRead(resourcePath))
                {
                    EmbeddedResource newResource = new EmbeddedResource(resourceName, selectedResource.Attributes, filestream);
                    resources.Remove(selectedResource);
                    resources.Add(newResource);

                    assemblyDef.Write(crackName);
                }
            }
        }

        public static void CleanResources()
        {
            File.Delete(assemblyPath);
            File.Delete(resourcePath);
        }
    }
}
