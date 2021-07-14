using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.ComponentModel;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Net.Http;

namespace DeLauncherForm
{

    //code taken and refactored from ContraLauncher(https://github.com/ContraMod/Launcher), thanks to launcher author - tet.
    static class GentoolsUpdater
    {
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileVersionInfoSize(string lptstrFilename, out int lpdwHandle);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandle, int dwLen, byte[] lpData);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen);
        public static async Task<bool> CheckAndUpdateGentools(LaunchOptions options, Windows.ApplyingOptions window)
        {
            if (options.Gentool == GentoolsMode.Current)
                return true;

            if (options.Gentool == GentoolsMode.Disable)
            {
                if (File.Exists("d3d8.dll"))
                    File.Delete("d3d8.dll");
                return true;
            }

            var connection = await ConnectionChecker.CheckConnection("http://www.gentool.net/");            

            if (connection == ConnectionChecker.ConnectionStatus.Connected)
            {
                var latestVersion = GetGentoolLatestVersion();
                if (GetCurrentGentoolVersion() < ParseGentoolVersionToInt(latestVersion))
                {
                    window.Show();
                    await DownloadGentool(GetGentoolDownloadName(latestVersion));
                }
                return true;
            }

            return false;
        }

        private static string GetGentoolLatestVersion()
        {
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("https://www.gentool.net/").GetAwaiter().GetResult();
            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var strings = responseBody.Split("\n");

            var versionNumber = "";

            foreach (var line in strings)
            {
                if (line.Contains("var gentool_ver"))
                {
                    versionNumber = new string(line.ToCharArray().Where(n => n >= '0' && n <= '9' || n == '.').ToArray());
                    break;
                }
            }
            if (!String.IsNullOrEmpty(versionNumber))
                return versionNumber;
            else
                throw new ArgumentException("Cannot find gentool_ver on https://www.gentool.net/");
        }

        private static int ParseGentoolVersionToInt(string version)
        {
            var versionDigits = version.Split('.');

            var stringVersion = "";

            foreach (var element in versionDigits)
                stringVersion += element;

            return int.Parse(stringVersion);
        }


        private static string GetGentoolDownloadName(string version)
        {
            return String.Format("http://www.gentool.net/download/GenTool_v{0}.zip", version);
        }

        public static int GetCurrentGentoolVersion()
        {
            try
            {
                var size = GetFileVersionInfoSize("d3d8.dll", out _);
                if (size == 0) { throw new Win32Exception(); };
                var bytes = new byte[size];
                bool success = GetFileVersionInfo("d3d8.dll", 0, size, bytes);
                if (!success) { throw new Win32Exception(); }

                // 040904E4 US English + CP_USASCII
                VerQueryValue(bytes, @"\StringFileInfo\040904E4\ProductVersion", out IntPtr ptr, out _);
                return int.Parse(Marshal.PtrToStringUni(ptr));
            }
            catch 
            {
                return -1;
            }
        }

        public static async Task DownloadGentool(string url)
        {
            using var client = new DownloadsHandler(url, "gentools.zip");
            await client.StartDownload();
        }
    }
}
