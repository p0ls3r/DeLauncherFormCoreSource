using DeLauncherFormCore6.Tools;
using NAudio.Wave;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Lng = DeLauncherForm.Language;

namespace DeLauncherForm
{
    public partial class MainWindow : Window
    {
        private FormConfiguration configuration;
        private LaunchOptions options;
        private ImageHandler repos = new ImageHandler();

        public const float Volume1 = 0.2f;

        private bool noInternet = false;
        private int theCode = 0;

        public MainWindow(FormConfiguration cfg, LaunchOptions opt, bool connected)
        {            
            configuration = cfg;
            options = opt;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            
            LocalFilesWorker.ConvertBigsToGibs();

            InitializeComponent();
            ApplyConfig();

            NoInternet.Opacity = 0;

            if (!connected)
                NoInternetMode();

            SetButtonsBindings();
        }

        private void StopNewsTicker()
        {
            NewsTicker.BeginAnimation(TextBox.PaddingProperty, null);
        }

        private void InitializeNewsTicker(string news)
        {
            StopNewsTicker();
            NewsTicker.Text = news;
            string Copy = " " + NewsTicker.Text;
            var speedValue = NewsTicker.Text.Length / 5;

            var TextGraphicalWidth = new FormattedText(Copy, System.Globalization.CultureInfo.CurrentCulture,
                                           FlowDirection.LeftToRight, new Typeface(NewsTicker.FontFamily.Source), NewsTicker.FontSize, Brushes.Black,
                                           VisualTreeHelper.GetDpi(NewsTicker).PixelsPerDip).WidthIncludingTrailingWhitespace;

            double TextLenghtGraphicalWidth = 0;
            while (TextLenghtGraphicalWidth < NewsTicker.ActualWidth)
            {
                NewsTicker.Text += Copy;
                TextLenghtGraphicalWidth = new FormattedText(Copy, System.Globalization.CultureInfo.CurrentCulture,
                                           FlowDirection.LeftToRight, new Typeface(NewsTicker.FontFamily.Source), NewsTicker.FontSize, Brushes.Black,
                                           VisualTreeHelper.GetDpi(NewsTicker).PixelsPerDip).WidthIncludingTrailingWhitespace;
            }
            NewsTicker.Text += " " + NewsTicker.Text;
            ThicknessAnimation ThickAnimation = new ThicknessAnimation();
            ThickAnimation.From = new Thickness(0, 0, 0, 0);
            ThickAnimation.To = new Thickness(-TextGraphicalWidth, 0, 0, 0);
            ThickAnimation.RepeatBehavior = RepeatBehavior.Forever;
            ThickAnimation.Duration = new Duration(TimeSpan.FromSeconds(speedValue));
            NewsTicker.BeginAnimation(TextBox.PaddingProperty, ThickAnimation);            
        }

        #region SoundsHandlers

        public void GetSound1()
        {
            if (options.Sounds)
            {
                var waveOut = new WaveOutEvent();
                var reader = new MediaFoundationReader(EntryPoint.LauncherFolder + "press1_new.wav");
                waveOut.Init(reader);
                waveOut.Volume = Volume1;

                waveOut.Play();
            }
        }

        public void GetSound2()
        {
            if (options.Sounds)
            {
                var waveOut = new WaveOutEvent();
                var reader = new MediaFoundationReader(EntryPoint.LauncherFolder + "press2.wav");
                waveOut.Init(reader);
                waveOut.Volume = Volume1;

                waveOut.Play();
            }
        }

        public void GetSound3()
        {
            if (options.Sounds)
            {
                var waveOut = new WaveOutEvent();
                var reader = new MediaFoundationReader(EntryPoint.LauncherFolder + "press3.wav");
                waveOut.Init(reader);
                waveOut.Volume = Volume1;

                waveOut.Play();
            }
        }

        private void GetSound4()
        {
            var waveOut = new WaveOutEvent();
            var reader = new MediaFoundationReader(EntryPoint.LauncherFolder + "press4.wav");
            waveOut.Init(reader);
            waveOut.Volume = Volume1;

            waveOut.Play();
        }

        #endregion
        public void ShowWindow(FormConfiguration cfg, LaunchOptions opt)
        {
            this.Show();
            configuration = cfg;
            options = opt;
            GetSound1();
        }

        public void ShowWindow()
        {
            this.Show();
        }

        private void NoInternetMode()
        {
            gifImage.Opacity = 0;
            NoInternet.Opacity = 100;
            SetVanillaShtora();

            AdvancedOptions.Visibility = Visibility.Collapsed;
            noInternet = true;
            //ManualFileSelect.Visibility = Visibility.Collapsed;
            ManualFileSelect.IsHitTestVisible = false;            

            ManualFileMode();
        }

        private void ManualFileMode()
        {
            if (theCode == 3)
            {
                theCode = 4;
            }
            else
                theCode = 0;

            StopNewsTicker();
            NewsTicker.Visibility = Visibility.Hidden;
            NewsTickerBorder.Visibility = Visibility.Hidden;

            configuration.Patch = new Vanilla();
            configuration.ManualFile = true;
            BackGrondImage2.Visibility = Visibility.Visible;            


            string prevFile = null;
            if (configuration.PreviousActivatedFiles.Count > 0)
               prevFile = configuration.PreviousActivatedFiles[0];

            FilesList.Items.Clear();

            foreach (var file in LocalFilesWorker.GetPatchFileNames())
                FilesList.Items.Add(file);

            var fileFounded = false;

            if (prevFile != null)
                foreach (var file in FilesList.Items)
                {
                    if (file.ToString() == prevFile)
                    {
                        FilesList.SelectedItem = file;
                        configuration.PreviousActivatedFiles.Clear();
                        configuration.PreviousActivatedFiles.Add(file.ToString());
                        fileFounded = true;
                        ClearShtora();
                    }
                }

            if (theCode == 4)
                FilesList.Items.Add("КОД56-24-81АЛЬФА");

            if (!fileFounded)
                SetVanillaShtora();

            BP.Visibility = Visibility.Collapsed;
            HP.Visibility = Visibility.Collapsed;
            FilesList.Visibility = Visibility.Visible;

            ShtoraManual.Visibility = Visibility.Visible;

            HPChanglog.Visibility = Visibility.Collapsed;
            BPChanglog.Visibility = Visibility.Collapsed;
        }

        private void AutoUpdateMode()
        {
            NewsTicker.Visibility = Visibility.Visible;
            NewsTickerBorder.Visibility = Visibility.Visible;
            if (configuration.Lang == Lng.Eng)
            {
                InitializeNewsTicker(NewsHandler.GetNewsEng());
            }
            else
            {
                InitializeNewsTicker(NewsHandler.GetNewsRu());
            }

            BackGrondImage2.Visibility = Visibility.Hidden;
            BP.Visibility = Visibility.Visible;
            HP.Visibility = Visibility.Visible;

            FilesList.Visibility = Visibility.Collapsed;

            HPChanglog.Visibility = Visibility.Visible;
            BPChanglog.Visibility = Visibility.Visible;            
        }

        #region WindowPreparation
        private void SetButtonsBindings()
        {
            launch.PreviewMouseLeftButtonDown += LaunchStart;
            launch.PreviewMouseLeftButtonUp += LaunchEnd;
            Windowed.PreviewMouseLeftButtonDown += WindowedStart;
            Windowed.PreviewMouseLeftButtonUp += WindowedEnd;
            QuickStart.PreviewMouseLeftButtonDown += QuickStartStart;
            QuickStart.PreviewMouseLeftButtonUp += QuickStartEnd;
            WorldBuilder.PreviewMouseLeftButtonDown += LaunchWorldBuilderStart;
            WorldBuilder.PreviewMouseLeftButtonUp += LaunchWorldBuilderEnd;
            Exit.PreviewMouseLeftButtonDown += GoExitStart;
            Exit.PreviewMouseLeftButtonUp += GoExitEnd;
            BP.PreviewMouseLeftButtonDown += BPSetStart;
            BP.PreviewMouseLeftButtonUp += BPSetEnd;
            HP.PreviewMouseLeftButtonDown += HPSetStart;
            HP.PreviewMouseLeftButtonUp += HPSetEnd;
            Vanilla.PreviewMouseLeftButtonDown += VanillaSetStart;
            Vanilla.PreviewMouseLeftButtonUp += VanillaSetEnd;
            AdvancedOptions.PreviewMouseLeftButtonDown += AdvancedOptionsWindowStart;
            AdvancedOptions.PreviewMouseLeftButtonUp += AdvancedOptionsWindowEnd;
            FilesList.SelectionChanged += FilesListSelectionChanged;

            HPChanglog.PreviewMouseLeftButtonDown += OpenHPChangeLogStart;
            HPChanglog.PreviewMouseLeftButtonUp += OpenHPChangeLogEnd;
            BPChanglog.PreviewMouseLeftButtonDown += OpenBPChangeLogStart;
            BPChanglog.PreviewMouseLeftButtonUp += OpenBPChangeLogEnd;

            ManualFileSelect.PreviewMouseLeftButtonDown += ManualFileSelectStart;
            ManualFileSelect.PreviewMouseLeftButtonUp += ManualFileSelectEnd;

            this.MouseDown += Window_MouseDown;
            this.Closing += SaveConfigAndOptions;

            Rus.Click += RusSet;
            Eng.Click += EngSet;
        }        

        private void ApplyConfig()
        {
            QuickStartIndicatorStatusChange(configuration.QuickStart);
            WindowedIndicatorStatusChange(configuration.Windowed);

            gifImage.GifSource = "/Windows/Resources/monitor.gif";
            gifImage.AutoStart = true;

            if (configuration.Patch is HPatch)
                SetHPShtora();
            if (configuration.Patch is BPatch)
                SetBPShtora();
            if (configuration.Patch is Vanilla)
                SetVanillaShtora();
            if (configuration.Patch is None)
            {
                SetVanillaShtora();
                configuration.Patch = new None();
            }

            if (configuration.Lang == DeLauncherForm.Language.Eng)
                SetEngLang();


            if (configuration.Lang == DeLauncherForm.Language.Rus)
                SetRusLang();            


            if (!configuration.ManualFile)
                FilesList.Visibility = Visibility.Hidden;
            else
                ManualFileMode();


            VersionInfo.Content = typeof(EntryPoint).Assembly.GetName().Version.ToString();
        }

        private void SetRusLang()
        {
            var r = Lng.Rus;

            if (!configuration.ManualFile)
            {
                InitializeNewsTicker(NewsHandler.GetNewsRu());
            }

            launchSource.Source = repos.GetImage(false, r, "launch");
            QuickStartSource.Source = repos.GetImage(false, r, "quickstart");
            WindowedSource.Source = repos.GetImage(false, r, "windowed");
            ExitSource.Source = repos.GetImage(false, r, "exit");
            ManualFileSelectSource.Source = repos.GetImage(false, r, "manual");

            BPSource.Source = repos.GetImage(false, r, "BP");
            HPSource.Source = repos.GetImage(false, r, "HP");
            VanillaSource.Source = repos.GetImage(false, r, "vanilla");

            HPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            BPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            WorldbuilderSource.Source = repos.GetImage(false, Lng.Eng, "worldbuilder");

            OptionsSource.Source = repos.GetImage(false, r, "options");

            InfoAll.Source = new BitmapImage(new Uri("/Windows/Resources/Main/info_r.png", UriKind.Relative));

            NoInternet.Source = new BitmapImage(new Uri("/Windows/Resources/Main/nointernet_r.png", UriKind.Relative));

            RusImage.Visibility = Visibility.Visible;
            EngImage.Visibility = Visibility.Hidden;
        }

        private void SetEngLang()
        {
            var e = Lng.Eng;

            if (!configuration.ManualFile)
            {
                InitializeNewsTicker(NewsHandler.GetNewsEng());
            }

            launchSource.Source = repos.GetImage(false, e, "launch");
            QuickStartSource.Source = repos.GetImage(false, e, "quickstart");
            WindowedSource.Source = repos.GetImage(false, e, "windowed");
            ExitSource.Source = repos.GetImage(false, e, "exit");

            ManualFileSelectSource.Source = repos.GetImage(false, e, "manual");
            HPChangelogSource.Source = repos.GetImage(false, e, "changelog");
            BPChangelogSource.Source = repos.GetImage(false, e, "changelog");
            WorldbuilderSource.Source = repos.GetImage(false, e, "worldbuilder");

            BPSource.Source = repos.GetImage(false, e, "BP");
            HPSource.Source = repos.GetImage(false, e, "HP");
            VanillaSource.Source = repos.GetImage(false, e, "vanilla");

            OptionsSource.Source = repos.GetImage(false, e, "options");

            RusImage.Visibility = Visibility.Hidden;
            EngImage.Visibility = Visibility.Visible;

            InfoAll.Source = new BitmapImage(new Uri("/Windows/Resources/Main/info_e.png", UriKind.Relative));

            NoInternet.Source = new BitmapImage(new Uri("/Windows/Resources/Main/nointernet_e.png", UriKind.Relative));
        }

        #endregion

        private void AdvancedOptionsWindowStart(object sender, EventArgs e)
        {
            OptionsSource.Source = repos.GetImage(true, configuration.Lang, "options");
        }

        private void AdvancedOptionsWindowEnd(object sender, EventArgs e)
        {
            theCode = 0;
            OptionsSource.Source = repos.GetImage(false, configuration.Lang, "options");
            this.Hide();
            Windows.Options optionsWindow = new Windows.Options(configuration, options, repos)
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
            };
            
            optionsWindow.PlaySound2 += GetSound2;
            optionsWindow.PlaySound3 += GetSound3;
            optionsWindow.ApplyOptions += ShowWindow;
            optionsWindow.CloseWindow += ShowWindow;
            optionsWindow.Show();
            GetSound1();
        }        

        private void SaveConfigAndOptions(object sender, EventArgs e)
        {
            configuration.PreviousActivatedFiles.Clear();
            if (FilesList.SelectedItem != null)
              configuration.PreviousActivatedFiles.Add(FilesList.SelectedItem.ToString());

            XmlData.SaveConfiguration(configuration);
            XmlData.SaveOptions(options);
        }

        private async Task CheckAndApplyOptions()
        {
            this.Hide();
            DeLauncherForm.Windows.ApplyingOptions applyingOptions = new DeLauncherForm.Windows.ApplyingOptions(configuration);
            applyingOptions.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            var succes = await GentoolsUpdater.CheckAndUpdateGentools(options, applyingOptions);

            if (!succes)
            {
                DeLauncherForm.Windows.GentoolUpdateFailed gentoolFailedWindow = new Windows.GentoolUpdateFailed(configuration);
                gentoolFailedWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                gentoolFailedWindow.ShowDialog();
            }

            await OptionsSetter.CheckAndApplyOptions(options, applyingOptions);

            applyingOptions.Close();
        }        

        #region LaunchLogic
        private async void Launch(bool worldBuilderLaunch)
        {
            if (configuration.ManualFile && FilesList.SelectedItem != null && FilesList.SelectedItem.ToString() == "КОД56-24-81АЛЬФА")
            {
                GetSound4();
                URLHandler.OpenURL(EntryPoint.BPLogUL);
                return;
            }
            theCode = 0;

            if (!noInternet && !worldBuilderLaunch)
                await CheckAndApplyOptions();

            SaveConfigAndOptions(this, null);

            //Кейс нет интернета и/или мануал мод
            if (configuration.ManualFile)
            {
                var fileName = string.Empty;
                if (FilesList.SelectedItem != null)
                    fileName = FilesList.SelectedItem.ToString();

                this.Hide();

                await GameLauncher.LaunchGameInManualModeByFile(fileName, worldBuilderLaunch, configuration, options);

                this.Close();
                return;
            }
            //Кейс автообновление
            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            this.Hide();

            await GameLauncher.CheckUpdatesAndLaunchGame(worldBuilderLaunch, configuration, options, token, cancelTokenSource);

            if (token.IsCancellationRequested)
            {
                this.Show();
                return;
            }
            else
                this.Close();
        }
        #endregion

        #region WindowHandlers
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void LaunchWorldBuilderStart(object sender, RoutedEventArgs e)
        {
            WorldbuilderSource.Source = repos.GetImage(true, Lng.Eng, "worldbuilder");
        }
        private void LaunchWorldBuilderEnd(object sender, RoutedEventArgs e)
        {
            WorldbuilderSource.Source = repos.GetImage(false, Lng.Eng, "worldbuilder");
            //запускаем ворлд билдер
            GetSound1();
            Launch(true);
        }
        private void LaunchStart(object sender, RoutedEventArgs e)
        {
            launchSource.Source = repos.GetImage(true, configuration.Lang, "launch");
        }
        private void LaunchEnd(object sender, RoutedEventArgs e)
        {
            launchSource.Source = repos.GetImage(false, configuration.Lang, "launch");
            GetSound1();
            //запускаем игру
            Launch(false);
        }
        private void SetHPShtora()
        {
            ClearShtora();
            ShtoraHP.Visibility = Visibility.Visible;
        }

        private void SetBPShtora()
        {
            ClearShtora();
            ShtoraBP.Visibility = Visibility.Visible;
        }

        private void SetVanillaShtora()
        {
            ClearShtora();
            ShtoraVanilla.Visibility = Visibility.Visible;
            if (configuration.ManualFile)
                ShtoraManual.Visibility = Visibility.Visible;
        }

        private void ClearShtora()
        {
            ShtoraBP.Visibility = Visibility.Hidden;
            ShtoraHP.Visibility = Visibility.Hidden;
            ShtoraVanilla.Visibility = Visibility.Hidden;
            ShtoraManual.Visibility = Visibility.Hidden;
        }

        private void QuickStartStart(object sender, RoutedEventArgs e)
        {            
            QuickStartSource.Source = repos.GetImage(true, configuration.Lang, "quickstart");
        }

        private void QuickStartEnd(object sender, RoutedEventArgs e)
        {
            QuickStartSource.Source = repos.GetImage(false, configuration.Lang, "quickstart");
            configuration.QuickStart = !configuration.QuickStart;
            QuickStartIndicatorStatusChange(configuration.QuickStart);
            GetSound2();
            theCode = 0;
        }

        private void ManualFileSelectStart(object sender, RoutedEventArgs e)
        {
            ManualFileSelectSource.Source = repos.GetImage(true, configuration.Lang, "manual");
        }

        private void ManualFileSelectEnd(object sender, RoutedEventArgs e)
        {
            configuration.ManualFile = !configuration.ManualFile;
            ManualFileSelectSource.Source = repos.GetImage(false, configuration.Lang, "manual");

            if (!configuration.ManualFile)
            {                
                AutoUpdateMode();
                configuration.Patch = new Vanilla();
                SetVanillaShtora();
            }
            else
                ManualFileMode();

            GetSound1();
        }

        private void FilesListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilesList.SelectedItem != null)
            {
                configuration.PreviousActivatedFiles.Clear();
                configuration.PreviousActivatedFiles.Add(FilesList.SelectedItem.ToString());
            }
            ClearShtora();
            if (configuration.ManualFile)
                ShtoraManual.Visibility = Visibility.Visible;
        }

        private void WindowedStart(object sender, RoutedEventArgs e)
        {            
            WindowedSource.Source = repos.GetImage(true, configuration.Lang, "windowed");
        }

        private void WindowedEnd(object sender, RoutedEventArgs e)
        {
            WindowedSource.Source = repos.GetImage(false, configuration.Lang, "windowed");
            configuration.Windowed = !configuration.Windowed;
            WindowedIndicatorStatusChange(configuration.Windowed);
            GetSound2();
            theCode = 0;
        }

        private void QuickStartIndicatorStatusChange(bool status)
        {
            if (status)
                QSIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_on.png", UriKind.Relative));
            else
                QSIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_off.png", UriKind.Relative));
        }

        private void WindowedIndicatorStatusChange(bool status)
        {
            if (status)
                WindowIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_on.png", UriKind.Relative));
            else
                WindowIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_off.png", UriKind.Relative));
        }

        private void BPSetStart(object sender, RoutedEventArgs e)
        {            
            BPSource.Source = repos.GetImage(true, configuration.Lang, "BP");
            theCode = 1;
        }

        private void HPSetStart(object sender, RoutedEventArgs e)
        {            
            HPSource.Source = repos.GetImage(true, configuration.Lang, "HP");
            if (theCode == 1)
                theCode = 2;
            else
                theCode = 0;
        }

        private void VanillaSetStart(object sender, RoutedEventArgs e)
        {            
            VanillaSource.Source = repos.GetImage(true, configuration.Lang, "vanilla");
            theCode = 0;
        }

        private void BPSetEnd(object sender, RoutedEventArgs e)
        {
            BPSource.Source = repos.GetImage(false, configuration.Lang, "BP");
            configuration.Patch = new BPatch();
            SetBPShtora();
            GetSound3();
        }

        private void HPSetEnd(object sender, RoutedEventArgs e)
        {
            HPSource.Source = repos.GetImage(false, configuration.Lang, "HP");
            configuration.Patch = new HPatch();
            SetHPShtora();
            GetSound3();
        }

        private void VanillaSetEnd(object sender, RoutedEventArgs e)
        {
            VanillaSource.Source = repos.GetImage(false, configuration.Lang, "vanilla");
            configuration.Patch = new Vanilla();
            if (configuration.ManualFile)
            {
                FilesList.SelectedItem = null;
                configuration.PreviousActivatedFiles.Clear();                
            }

            SetVanillaShtora();
            GetSound3();
        }

        private void OpenHPChangeLogStart(object sender, RoutedEventArgs e)
        {            
            HPChangelogSource.Source = repos.GetImage(true, Lng.Eng, "changelog");
            theCode = 0;
        }

        private void OpenBPChangeLogStart(object sender, RoutedEventArgs e)
        {            
            BPChangelogSource.Source = repos.GetImage(true, Lng.Eng, "changelog");
            theCode = 0;
        }

        private void OpenHPChangeLogEnd(object sender, RoutedEventArgs e)
        {
            HPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            URLHandler.OpenURL(EntryPoint.HPLogURL);
            GetSound2();
        }

        private void OpenBPChangeLogEnd(object sender, RoutedEventArgs e)
        {
            BPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            URLHandler.OpenURL(EntryPoint.BPLogURL);
            GetSound2();
        }

        private void RusSet(object sender, RoutedEventArgs e)
        {           
            configuration.Lang = DeLauncherForm.Language.Rus;
            SetRusLang();
            if (theCode == 2)
                theCode = 3;
            else
                theCode = 0;
        }

        private void EngSet(object sender, RoutedEventArgs e)
        {
            configuration.Lang = DeLauncherForm.Language.Eng;
            SetEngLang();
            theCode = 0;
        }

        private void GoExitStart(object sender, RoutedEventArgs e)
        {
            ExitSource.Source = repos.GetImage(true, configuration.Lang, "exit");
        }

        private void GoExitEnd(object sender, RoutedEventArgs e)
        {
            ExitSource.Source = repos.GetImage(false, configuration.Lang, "exit");
            this.Close();
        }

        #endregion
    }
}
