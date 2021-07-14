using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeLauncherForm
{    
    public static class NewsHandler
    {
        public static string LauncherNewsRU = String.Empty;
        public static string HPNewsRU = String.Empty;
        public static string BPNewsRU = String.Empty;
        public static string LauncherNewsEng = String.Empty;
        public static string HPNewsEng = String.Empty;
        public static string BPNewsEng = String.Empty;

        public static string GetNewsRu()
        {
            return String.Format("                        {0}        {1}        {2}                             ", LauncherNewsRU, BPNewsRU, HPNewsRU);
        }

        public static string GetNewsEng()
        {
            return String.Format("                        {0}        {1}        {2}                             ", LauncherNewsEng, BPNewsEng, HPNewsEng);
        }

        public static void SetLauncherNews(string newsString)
        {
            var parsedNews = newsString.Substring(0, newsString.Length - EntryPoint.NewsFilesExt.Length - 1);

            var news = parsedNews.Split('$');
            LauncherNewsRU = news[0];
            LauncherNewsEng = news[1];
        }

        public static void SetHPNews(string newsString)
        {
            var parsedNews = newsString.Substring(0, newsString.Length - EntryPoint.NewsFilesExt.Length - 1);

            var news = parsedNews.Split('$');
            HPNewsRU = news[0];
            HPNewsEng = news[1];
        }

        public static void SetBPNews(string newsString)
        {
            var parsedNews = newsString.Substring(0, newsString.Length - EntryPoint.NewsFilesExt.Length - 1);

            var news = parsedNews.Split('$');
            BPNewsRU = news[0];
            BPNewsEng = news[1];
        }
    }
}
