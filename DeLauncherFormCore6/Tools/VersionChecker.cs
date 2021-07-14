using System.Diagnostics;
using System.IO;
using System.Xml;

namespace DeLauncherForm
{
    class VersionChecker
    {
        public static int GetCoreCurrentVersionNumber()
        {
            if (File.Exists("DeLauncher.exe"))
            {
                var coreVersionInfo = FileVersionInfo.GetVersionInfo("DeLauncher.exe");
                var versionString = coreVersionInfo.FileVersion;

                return LocalFilesWorker.GetVersionNumberFromName(versionString);
            }
            else
                return -1;
        }
    }
}
