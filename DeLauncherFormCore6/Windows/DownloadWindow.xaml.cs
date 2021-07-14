using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeLauncherForm
{
    /// <summary>
    /// Логика взаимодействия для DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        private CancellationTokenSource _tokenSource;
        private FormConfiguration configuration;
        private int currentDownloadPercent;
        public DownloadWindow(FormConfiguration conf)
        {
            InitializeComponent();

            configuration = conf;
            this.Closing += CloseWindowClick;

            if (conf.Lang == DeLauncherForm.Language.Rus)
            {
                CancelDownload.Content = "Отменить загрузку";
                Info.Source = new BitmapImage(new Uri("/Windows/Resources/info_rus.png", UriKind.Relative));
            }
        }
        public async Task StartDownload(CancellationToken token, CancellationTokenSource tokenSource)
        {
            _tokenSource = tokenSource;
            try
            {
                await GameLauncher.PrepareWithUpdate(configuration, token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        public void UpdateInformation(long totalSize, long downloaded, int percent, string filename)
        {
            if (configuration.Lang == DeLauncherForm.Language.Rus)
            {
                if (filename == "unpacking")
                    DownloadInfo.Text = "Распаковка архива...";
                else
                    DownloadInfo.Text = "Загрузка: " + filename;
            }
            else
            {
                if (filename == "unpacking")
                    DownloadInfo.Text = "Archive extraction...";
                else
                    DownloadInfo.Text = "Loading file: " + filename;
            }
            ProgressBar.Value = percent;
            currentDownloadPercent = percent;
            ProgressLine.Text = String.Format("{0}MB / {1}MB", (downloaded / 1048576).ToString(), (totalSize / 1048576).ToString());
        }

        private void CancelDownload_Click(object sender, RoutedEventArgs e)
        {
            if (currentDownloadPercent != 100)
                _tokenSource.Cancel();
        }

        private void CloseWindowClick(object sender, CancelEventArgs e)
        {
            if (currentDownloadPercent != 100)
                _tokenSource.Cancel();
        }
    }
}
