using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    static class ReposWorker
    {    
        public static event Action<long, long, int, string> DownloadStatusChanged;
        public static int? HPVersion;
        public static int? BPVersion;

        private static Stopwatch sw = new Stopwatch();        
        static void DownloadProgressChanged(long? totalDownloadSize, long totalBytesRead, double? progressPercentage, string filename)
        {
            if (progressPercentage.HasValue)
            {
                var percentage = Convert.ToInt32(progressPercentage.Value);

                if (percentage == 100)
                    filename = "unpacking";

                if (totalDownloadSize.HasValue)
                    DownloadStatusChanged(totalDownloadSize.Value, totalBytesRead, percentage, filename);
                else
                    DownloadStatusChanged(0, totalBytesRead, percentage, filename);
            }
        }

        //Получить последнюю номер последней версии патча из репозитория, заполнить новости по патчам. Методы совмещены чтобы уменьшить нагрузку на API.
        public static int GetLatestPatchNumberAndUpdateNews(Patch patch)
        {
            //особый кейс ваниальный ротр
            if (patch is Vanilla)
                return new Vanilla().PatchVersion;

            if (patch is HPatch && !HPVersion.HasValue)
                HPVersion = GetLatestPatchVersionAndNews(patch);
                
            if (patch is HPatch)
                return HPVersion.Value;

            if (patch is BPatch && !BPVersion.HasValue)
                BPVersion = GetLatestPatchVersionAndNews(patch);

            if (patch is BPatch)
                return BPVersion.Value;

            return -1;
        }

        private static int GetLatestPatchVersionAndNews(Patch patch)
        {
            int versionNumber = -1;

            foreach (var parsedData in DownloadsHandler.GetRepoContent(patch))
            {
                var fileName = parsedData["name"].ToString();
                if (Path.GetExtension(fileName).Replace(".", "") == "big" || Path.GetExtension(fileName).Replace(".", "") == "gib" || Path.GetExtension(fileName).Replace(".", "") == "zip")
                    versionNumber = LocalFilesWorker.GetVersionNumberFromName(fileName);

                if (parsedData["type"].ToString() == "file" && Path.GetExtension(fileName).Replace(".", "") == "info")
                {
                    if (patch is HPatch)
                        NewsHandler.SetHPNews(fileName);
                    if (patch is BPatch)
                        NewsHandler.SetBPNews(fileName);
                }
            }

            return versionNumber;
        }

        public static async Task LoadActualPatch(Patch patch, CancellationToken token)
        {
            if (patch is None)
                return;            

            foreach (var parsedData in DownloadsHandler.GetRepoContent(patch))
            {
                var fileName = parsedData["name"].ToString();
                if (parsedData["type"].ToString() == "file" && (Path.GetExtension(fileName).Replace(".", "") == "exe" || Path.GetExtension(fileName).Replace(".", "") == "zip"))
                {
                    var downloadUrl = parsedData["download_url"].ToString();

                    using (var client = new DownloadsHandler(downloadUrl, fileName, token))
                    {                        
                        client.ProgressChanged += DownloadProgressChanged;
                        await client.StartDownload();
                    }
                    sw.Reset();
                }
            }
        }
    }
}
