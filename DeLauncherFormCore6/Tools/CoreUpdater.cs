using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using DeLauncherFormCore6;

namespace DeLauncherForm
{
    static class CoreUpdater
    {
        const string repo = "p0ls3r/DeLauncherUpdaterCore";

        public static int GetLatestVersionNumber()
        {
            var result = -1;

            foreach (var parsedData in DownloadsHandler.GetRepoContent($"https://api.github.com/repos/{repo}/contents"))
            {
                var fileName = parsedData["name"].ToString();

                if (parsedData["type"].ToString() == "file" && (Path.GetExtension(fileName).Replace(".", "") == "zip" || Path.GetExtension(fileName).Replace(".", "") == "exe"))
                    result = LocalFilesWorker.GetVersionNumberFromName(fileName);

                if (parsedData["type"].ToString() == "file" && Path.GetExtension(fileName).Replace(".", "") == "info")
                    NewsHandler.SetLauncherNews(fileName);
            }

            if (result == -1)
                throw new ApplicationException("unable to get updaterVersion!");
            else
                return result;
        }

        public static async Task DownloadUpdate()
        {
            foreach (var parsedData in DownloadsHandler.GetRepoContent($"https://api.github.com/repos/{repo}/contents"))
            {
                if (parsedData["type"].ToString() == "file")
                {
                    var downloadUrl = parsedData["download_url"].ToString();
                    var fileName = parsedData["name"].ToString();

                    if (Path.GetExtension(fileName).Replace(".", "") == "zip" || Path.GetExtension(fileName).Replace(".", "") == "exe")
                        using (var client = new DownloadsHandler(downloadUrl, fileName))
                        {
                            await client.StartDownload();
                        }
                }
            }            
        }
    }
}
