using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DeLauncherForm
{
    [Serializable]
    public class BannedIpsList
    {
        public List<string> BannedAdresses = new List<string>();
        public BannedIpsList()
        {

        }
    }
    public static class BannedIpshandler
    {
        public static List<IPAddress> GetBannedIpsList()
        {
            var result = new List<IPAddress>();
            var fileName = String.Empty;
            var repo = EntryPoint.BanLink;
            var url = $"https://api.github.com/repos/{repo}/contents";
            foreach (var parsedData in DownloadsHandler.GetRepoContent(url))
            {
                fileName = parsedData["name"].ToString();

                var downloadUrl = parsedData["download_url"].ToString();

                using (var client = new DownloadsHandler(downloadUrl, fileName))
                {
                    client.StartDownload().GetAwaiter().GetResult();
                }
            }

            BannedIpsList blist;
            try
            {
                var formatter = new XmlSerializer(typeof(BannedIpsList));

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    blist = (BannedIpsList)formatter.Deserialize(fs);
                }

                foreach (var t in blist.BannedAdresses)                
                    result.Add(IPAddress.Parse(t));
                if (File.Exists(fileName))
                File.Delete(fileName);
            }
            catch
            {

            }
            return result;
        }
    }
}
