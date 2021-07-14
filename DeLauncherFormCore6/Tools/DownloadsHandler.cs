using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


namespace DeLauncherForm
{
    public class DownloadsHandler : IDisposable
    {
        private readonly string _downloadUrl;
        private readonly string _destinationFilePath;
        private readonly CancellationToken? _cancellationToken;
        private bool extractionRequers = false;
        private int tempPrefixLengh = EntryPoint.TempFilesPrefix.Length;

        private HttpClient _httpClient;
        public event Action<long?, long, double?, string> ProgressChanged;

        public DownloadsHandler(string downloadUrl, string destinationFilePath, CancellationToken? cancellationToken = null)
        {            
            _downloadUrl = downloadUrl;
            _destinationFilePath = destinationFilePath;
            _cancellationToken = cancellationToken;
            if (Path.GetExtension(_destinationFilePath).Replace(".", "") == "zip")
                extractionRequers = true;
            _destinationFilePath = _destinationFilePath + EntryPoint.TempFilesPrefix;            
        }

        public async Task StartDownload()
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };

            using (var response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                await DownloadFileFromHttpResponseMessage(response);
        }

        private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength;

            using (var contentStream = await response.Content.ReadAsStreamAsync())
                await ProcessContentStream(totalBytes, contentStream);
        }

        private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
        {
            var totalBytesRead = 0L;
            var readCount = 0L;
            var buffer = new byte[8192];
            var isMoreToRead = true;

            using (var fileStream = new FileStream(_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                do
                {
                    int bytesRead;
                    if (_cancellationToken.HasValue)
                    {
                        bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, _cancellationToken.Value);
                    }
                    else
                    {
                        bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    }

                    if (bytesRead == 0)
                    {
                        isMoreToRead = false;
                        continue;
                    }

                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                    totalBytesRead += bytesRead;
                    readCount += 1;

                    if (readCount % 10 == 0)
                        TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                }
                while (isMoreToRead);

            }

            TriggerProgressChanged(totalDownloadSize, totalBytesRead);

            if (extractionRequers)
                ExtractArchieve(_destinationFilePath);
            else
            {
                if (File.Exists(_destinationFilePath.Substring(0, _destinationFilePath.Length - tempPrefixLengh)))
                    File.Delete(_destinationFilePath.Substring(0, _destinationFilePath.Length - tempPrefixLengh));
                File.Move(_destinationFilePath, _destinationFilePath.Substring(0, _destinationFilePath.Length - tempPrefixLengh));
            }
        }

        private void ExtractArchieve(string _destinationFilePath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(_destinationFilePath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.Length > 4 && entry.FullName.Substring(0, 4) == "!!!!" &&
                        (Path.GetExtension(entry.FullName).Replace(".", "") == "big" || Path.GetExtension(entry.FullName).Replace(".", "") == "gib"))
                        entry.ExtractToFile(entry.FullName, true);
                    else
                        if (Path.GetExtension(entry.FullName).Replace(".", "") == "big" || Path.GetExtension(entry.FullName).Replace(".", "") == "gib")
                        entry.ExtractToFile("!!!!" + entry.FullName, true);
                    else
                        if (!entry.FullName.Contains("/") && !entry.FullName.Contains("\\"))
                        entry.ExtractToFile(entry.FullName, true);
                }
            }
            File.Delete(_destinationFilePath);
        }
        private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
        {
            if (ProgressChanged == null)
                return;

            double? progressPercentage = null;
            if (totalDownloadSize.HasValue)
                progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

            if (_destinationFilePath.Substring(_destinationFilePath.Length - tempPrefixLengh, tempPrefixLengh) == EntryPoint.TempFilesPrefix)
                ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage, _destinationFilePath.Substring(0, _destinationFilePath.Length - tempPrefixLengh));
            else
                ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage, _destinationFilePath);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public static IEnumerable<Dictionary<string, object>> GetRepoContent(string url)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("DeLauncherForm", typeof(EntryPoint).Assembly.GetName().Version.ToString()));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Tls13;

            dynamic contents = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(httpClient.GetStringAsync(url).GetAwaiter().GetResult());

            foreach (var content in contents)
            {
                yield return (Dictionary<string, object>)content;
            }
        }

        public static IEnumerable<Dictionary<string, object>> GetRepoContent(Patch patch)
        {
            var repo = patch.Repository;
            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            foreach (var result in GetRepoContent(contentsUrl))
                yield return result;
        }
    }        
}
