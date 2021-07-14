using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DeLauncherForm
{
    static class XmlData
    {
        #region Опции запуска
        static public LaunchOptions ReadOptions()
        {
            LaunchOptions options;
            try
            {                
                if (!Directory.Exists(EntryPoint.LauncherFolder))
                    CreateLauncherFolder();

                if (!File.Exists(EntryPoint.optionsName))
                    CreateNewOptionsFile();

                var formatter = new XmlSerializer(typeof(LaunchOptions));                

                using (FileStream fs = new FileStream(EntryPoint.optionsName, FileMode.Open))
                {
                    options = (LaunchOptions)formatter.Deserialize(fs);
                }
            }
            catch
            {
                if (File.Exists(EntryPoint.optionsName))
                    File.Delete(EntryPoint.optionsName);
                options = CreateNewOptions();
            }

            return options;
        }

        static private LaunchOptions CreateNewOptions()
        {
            return new LaunchOptions
            {
                ModdedExe = true,
                FixFile = false,
                Gentool = GentoolsMode.Current,
                DeleteOldVersions = true
            };
        }

        static private void CreateNewOptionsFile()
        {
            var opt = CreateNewOptions();

            var formatter = new XmlSerializer(typeof(LaunchOptions));

            using (var fs = new FileStream(EntryPoint.optionsName, FileMode.Create))
            {
                formatter.Serialize(fs, opt);
            }
        }

        static public void SaveOptions(LaunchOptions opt)
        {
            try
            {
                if (File.Exists(EntryPoint.optionsName))
                    File.Delete(EntryPoint.optionsName);

                var formatter = new XmlSerializer(typeof(LaunchOptions));

                using (var fs = new FileStream(EntryPoint.optionsName, FileMode.Create))
                {
                    formatter.Serialize(fs, opt);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region конфигурация лаунчера
        static public FormConfiguration ReadConfiguration()
        {
            FormConfiguration configuration;
            try
            {
                var path = EntryPoint.LauncherFolder;
                if (!Directory.Exists(path))
                    CreateLauncherFolder();

                if (!File.Exists(EntryPoint.configName))
                    CreateNewConfigurationFile();

                var formatter = new XmlSerializer(typeof(FormConfiguration));


                using (FileStream fs = new FileStream(EntryPoint.configName, FileMode.Open))
                {
                    configuration = (FormConfiguration)formatter.Deserialize(fs);
                }
            }
            catch
            {
                if (File.Exists(EntryPoint.configName))
                    File.Delete(EntryPoint.configName);
                configuration = CreateNewConfiguration();
            }

            return configuration;
        }

        static public void SaveConfiguration(FormConfiguration conf)
        {
            try
            {
                if (File.Exists(EntryPoint.configName))
                File.Delete(EntryPoint.configName);

                var formatter = new XmlSerializer(typeof(FormConfiguration));

                using (var fs = new FileStream(EntryPoint.configName, FileMode.Create))
                {
                    formatter.Serialize(fs, conf);
                }
            }
            catch
            {

            }
        }

        static private FormConfiguration CreateNewConfiguration()
        {
            return new FormConfiguration
            {
                Patch = new Vanilla(),
                Lang = Language.Eng
            };
        }
        static private void CreateNewConfigurationFile()
        {
            var conf = CreateNewConfiguration();

            var formatter = new XmlSerializer(typeof(FormConfiguration));

            using (var fs = new FileStream(EntryPoint.configName, FileMode.Create))
            {
                formatter.Serialize(fs, conf);
            }
        }

        #endregion
        private static void CreateLauncherFolder()
        {
            var path = EntryPoint.LauncherFolder;

            var folder = Directory.CreateDirectory(path);
            folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }
    }
}
