using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace DeLauncherForm
{
    public static class OptionsSetter
    {
        public static async Task CheckAndApplyOptions(LaunchOptions options, Windows.ApplyingOptions window)
        {
            if (options.FixFile && !(File.Exists("d3d8to9.dll") || File.Exists("d3d8x.dll")))
            {
                window.Show();
                await LoadContent("p0ls3r/d3d8");
            }            

            if (options.FixFile && (options.Gentool == GentoolsMode.AutoUpdate || File.Exists("d3d8.dll")))
            {
                if (File.Exists("d3d8to9.dll"))
                    File.Move("d3d8to9.dll", "d3d8x.dll");
            }

            if (options.FixFile && options.Gentool == GentoolsMode.Disable )
            {
                if (File.Exists("d3d8x.dll"))
                    File.Move("d3d8x.dll", "d3d8to9.dll");

                if (File.Exists("d3d8to9.dll"))
                    File.Move("d3d8to9.dll", "d3d8.dll");
            }

            if (!options.FixFile)
            {
                if (File.Exists("d3d8to9.dll"))
                    File.Delete("d3d8to9.dll");
                if (File.Exists("d3d8x.dll"))
                    File.Delete("d3d8x.dll");
                if (File.Exists("d3d8.dll") && options.Gentool == GentoolsMode.Disable)
                    File.Delete("d3d8.dll");
            }

            if (options.DebugFile && File.Exists("dbghelp.dll"))
                File.Delete("dbghelp.dll");

            if (options.ModdedExe && !File.Exists("modded.exe"))
            {
                window.Show();
                await LoadContent("p0ls3r/moddedExe");                
            }
        }

        public static async Task LoadContent(string repos)
        {
            var url = $"https://api.github.com/repos/{repos}/contents";

            foreach (var parsedData in DownloadsHandler.GetRepoContent(url))
            {
                if (parsedData["type"].ToString() == "file")
                {
                    var downloadUrl = parsedData["download_url"].ToString();
                    var fileName = parsedData["name"].ToString();

                    using (var client = new DownloadsHandler(downloadUrl, fileName))
                    {
                        await client.StartDownload();
                    }
                }
            }
        }
    }
}
