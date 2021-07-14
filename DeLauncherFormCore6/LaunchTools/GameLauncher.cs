using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DeLauncherForm
{
    static class GameLauncher
    {
        public static async Task LaunchGameInManualModeByFile(string fileName, bool worldBuilderLaunch, FormConfiguration conf, LaunchOptions options)
        {
            //кейс мануал мод, но файл не выбран(запуск ванильного ротра)
            if (string.IsNullOrEmpty(fileName))
            {
                conf.Patch = new Vanilla();

                await Task.Run(() => ActivatePatchFiles(conf.Patch));
                await Task.Run(() => GameLauncher.Launch(conf, options, worldBuilderLaunch));
            }
            //кейс мануал мод, файл выбран
            else
            {
                if (fileName.Contains("HP"))
                    conf.Patch = new HPatch();

                if (fileName.Contains("BP"))
                    conf.Patch = new BPatch();

                LocalFilesWorker.ActivateFileByName(fileName);

                await Task.Run(() => GameLauncher.Launch(conf, options, worldBuilderLaunch));
            }
        }

        public static async Task CheckUpdatesAndLaunchGame(bool worldBuilderLaunch, FormConfiguration conf, LaunchOptions options, CancellationToken token, CancellationTokenSource tokenSource)
        {
            //Кейс если файлов патча нет или версия устаревшая, обновить файл патча
            if (!LocalFilesWorker.CheckPatchFileExist(conf.Patch) || (LocalFilesWorker.GetCurrentVersionNumber(conf.Patch) < ReposWorker.GetLatestPatchNumberAndUpdateNews(conf.Patch)))
            {
                await DownloadUpdatedPatch(conf, token, tokenSource);
                if (token.IsCancellationRequested)
                    return;
                //Дополнительно после скачивания конвертировать big в gib для корректной проверки актуальности последней версии
                LocalFilesWorker.ConvertBigsToGibs();
            }

            await Task.Run(() => ActivatePatchFiles(conf.Patch));
            await Task.Run(() => GameLauncher.Launch(conf, options, worldBuilderLaunch));
        }

        private static async Task DownloadUpdatedPatch(FormConfiguration conf, CancellationToken token, CancellationTokenSource tokenSource)
        {
            DownloadWindow downloadWindow = new DownloadWindow(conf);
            downloadWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            ReposWorker.DownloadStatusChanged += downloadWindow.UpdateInformation;

            downloadWindow.Show();
            await downloadWindow.StartDownload(token, tokenSource);

            downloadWindow.Close();

            if (token.IsCancellationRequested)
            {
                LocalFilesWorker.ClearTempFiles();
                return;
            }
        }

        private static void ActivatePatchFiles(Patch patch)
        {
            foreach (var file in LocalFilesWorker.GetLatestPatchFileNames(patch))
                LocalFilesWorker.ActivateFileByName(file);
        }

        public static void Launch(FormConfiguration conf, LaunchOptions options, bool worldBuilderLaunch)
        {
            LocalFilesWorker.SetGameFiles(conf);
            if (options.DeleteOldVersions)
                LocalFilesWorker.DeleteAllOldPatchFiles(conf.Patch);

            if (conf.Patch is BPatch && IPChecker.IsCurrentUserBanned())
            {
                LocalFilesWorker.DeleteAllPatchFiles(conf.Patch);
                MessageBox.Show(" Sorry, but infantry is countered by antiInfantry, DeLauncher counters BP Bruce now. \r Nonsense: Russian sanctions against an American, lol) \r If you are not Bruce, sorry, send a private message to DeL, i'll fix that");
                return;
            }

            var id = StartExe(conf, options, worldBuilderLaunch);

            var mon = new Monitor(id);
            mon.StartMonitoring();

            while (!mon.IsArrived)
            {
                Thread.Sleep(5000);
                if (conf.Patch is BPatch && IPChecker.IsCurrentUserBanned())
                {
                    var process = Process.GetProcessById(id);
                    process.Kill();
                    process.WaitForExit();
                    break;
                }
            }

            LocalFilesWorker.SetGameFilesBack();
        }

        public static async Task PrepareWithUpdate(FormConfiguration conf, CancellationToken token)
        {
            await ReposWorker.LoadActualPatch(conf.Patch, token);            
        }


        private static int StartExe(FormConfiguration conf, LaunchOptions options, bool worldBuilderLaunch)
        {
            Process process;

            if (worldBuilderLaunch)
            {
                process = Process.Start(EntryPoint.WorldBuilderFile);
                return process.Id;
            }

            var parameters = "";

            if (conf.Windowed)
                parameters += "-win ";
            if (conf.QuickStart)
                parameters += "-quickstart ";
            if (conf.particleEdit)
                parameters += "-particleEdit ";
            if (conf.scriptDebug)
                parameters += "-scriptDebug";


            if (!options.ModdedExe)
                process = Process.Start(EntryPoint.GameFile, parameters);
            else
                process = Process.Start(EntryPoint.ModdedGameFile, parameters);


            return process.Id;
        }
    }
}
