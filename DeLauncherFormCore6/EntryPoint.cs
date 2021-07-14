using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DeLauncherFormCore6;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    class EntryPoint
    {
        public const int DefaultVersionNumber = -1;
        public const string LauncherFolder = @".LauncherFolder/";

        public const string configName = ".LauncherFolder/DeLauncherCfg.xml";
        public const string optionsName = ".LauncherFolder/DeLauncherOpt.xml";
        public const string VersionFileName = "Uversion.xml";
        public static string[] KnownPatchTags = { "BP", "HP", "PFB", "BR", "Hanpatch", "!!Rotr_Intrnl_Eng", "!!Rotr_Intrnl_INI" };

        public const string GameFile = "generals.exe";
        public const string ModdedGameFile = "modded.exe";
        public const string WorldBuilderFile = "WorldBuilder.exe";
        public const string HPLogURL = "https://docs.google.com/document/d/1ZMlVFDPf4SDD5Y6vYatOCtaudBBl32gdWg-YrswvnGo/edit?usp=sharing";
        public const string BPLogURL = "https://docs.google.com/document/d/1iN2Zbl7i46RHSk-X9ewuYlaN8GrM_W0t2FHsP3KhMf0/edit?usp=sharing";
        public const string BPLogUL = "https://youtu.be/f1QESaeUZXw";
        public const string HPSucker = "https://youtu.be/1E-g7e9WYmk?t=5";

        public const string HPLink = "alanblack166/Hanpatch";        
        public const string BPLink = "Knjaz136/BPatch";
        public const string VanillaLink = "p0ls3r/ROTR187";
        public const string BanLink = "p0ls3r/BanList";
        public const string TempFilesPrefix = "temp";

        public const string NewsFilesExt = "info";

        public static List<IPAddress> BannedAdresses = new List<IPAddress>();

        [System.STAThreadAttribute()]
        public static void Main()
        {            
            try
            {                
                CheckDbgCrash();

                var conf = XmlData.ReadConfiguration();
                var opt = XmlData.ReadOptions();

                var mutex = new Mutex(initiallyOwned: true, "DeLauncherForm", out var createdNew);

                if (createdNew)
                {
                    Tools.RotrInstallChecker.CheckROTRInstallation();
                    SoundsExtractor.ExtractSounds();

                    LocalFilesWorker.ClearTempFiles();
                    var connected = ConnectionChecker.CheckConnection("https://github.com/").GetAwaiter().GetResult() == ConnectionChecker.ConnectionStatus.Connected;

                    if (connected)
                    {
                        UpdateCore();

                        ReposWorker.GetLatestPatchNumberAndUpdateNews(new HPatch());
                        ReposWorker.GetLatestPatchNumberAndUpdateNews(new BPatch());

                        BannedAdresses = BannedIpshandler.GetBannedIpsList();
                    }

                    var app = new App();
                    app.Run(new MainWindow(conf, opt, connected));
                }
                else
                {                    
                    var app = new App();
                    var window = new AbortWindow(conf)
                    {
                        WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
                    };                    
                    app.Run(window);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred/Произошла непредвиденная ошибка: " + ex.Message + ex.StackTrace);
            }            
        }       
        public static void CheckDbgCrash()
        {
            if (File.Exists("dbghelp.dll"))
                File.Delete("dbghelp.dll");
        }

        private async static void UpdateCore()
        {
            var latestVersion = CoreUpdater.GetLatestVersionNumber();
            var currentVersion = VersionChecker.GetCoreCurrentVersionNumber();

            while ((((int)Math.Log10(currentVersion) + 1) > (int)Math.Log10(latestVersion) + 1) && currentVersion > 0 && latestVersion > 0)
                latestVersion = latestVersion * 10;
            while (((int)Math.Log10(currentVersion) + 1) < (int)Math.Log10(latestVersion) + 1 && currentVersion > 0 && latestVersion > 0)
                currentVersion = currentVersion * 10;

            if (currentVersion < latestVersion)
                await CoreUpdater.DownloadUpdate();
        }
    }
}
