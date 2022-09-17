using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ISN_Forecast.Win7.API;

namespace ISN_Forecast.Win7.FirstSetup
{
    /// <summary>
    /// Interaction logic for LocationServices.xaml
    /// </summary>
    public partial class LocationServices : Page
    {
        private GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
        public static class Status
        {
            public static String GPS;
            public static String IP;
        }

        public LocationServices()
        {
            Statusbar.Instance.GPSUsage.Opacity = 1;
            InitializeComponent();
            Init();
        }

        public async void Init()
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

            //Setup.Instance.Status.Text = Lang["Setup"]["AppearanceTitle"];
            Title.Text = Lang["Setup"]["LocationCheck"];
            Description.Text = Lang["Setup"]["LocationDescription"];
            Title.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontBold"].ToString());
            Description.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
            GPSStatus.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontBold"].ToString());
            GPSDesc.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
            IPDesc.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
            IPStatus.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontBold"].ToString());
            Description.Text = Lang["Setup"]["LocationDescription"];
            GPSDesc.Text = Lang["Setup"]["GPSFailDesc"];
            GPSStatus.Text = "GPS - " + Lang["Setup"]["Fail"];
            ContinueGPS.Text = Lang["Setup"]["ContinueGPS"];
            ContinueIP.Text = Lang["Setup"]["ContinueIP"];

            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            GeoCoordinate coord = watcher.Position.Location;

            if (Configs.Look == "Colorful")
            {
                var converter = new System.Windows.Media.BrushConverter();
                PanelBG.Background = (Brush)converter.ConvertFromString("#0051FF");
            }

            if (Configs.Look == "Dark")
            {
                var converter = new System.Windows.Media.BrushConverter();
                PanelBG.Background = (Brush)converter.ConvertFromString("#595959");
            }


            if (Configs.Language == "kr" || Configs.Language == "cn_SP")
            {
                GPSCensor.Height = 120;
                IPLocation.Height = 120;
            }

            await Task.Delay(5000);

            GPS();
        }

        public void GPS()
        {
            var converter = new System.Windows.Media.BrushConverter();

            GeoCoordinate coord = watcher.Position.Location;
            if (coord.IsUnknown != true)
            {
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

                GPSStatus.Text = "GPS - " + Lang["Setup"]["Pass"];
                GPSDesc.Text = Lang["Setup"]["Longitude"] + ": \"" + coord.Latitude + "\" \n" + Lang["Setup"]["Latitude"] + ": \"" + coord.Longitude + "\"";
                GPSCensor.Background = (Brush)converter.ConvertFromString("#0A2E00");
                Status.GPS = "Pass";
            }
            else
            {
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

                GPSStatus.Text = "GPS - " + Lang["Setup"]["Fail"];
                GPSCensor.Background = (Brush)converter.ConvertFromString("#2E0000");
                ProcessRing.IsActive = false;
                Status.GPS = "Failed";
            }

            IP();
        }

        public void IP()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringAsync(new Uri("http://ip-api.com/json"));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ProcessLocation);
        }

        private void ProcessLocation(object sender, DownloadStringCompletedEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

            try
            {
                var IP = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result);
                IPStatus.Text = "IP - " + Lang["Setup"]["Pass"];
                IPDesc.Text = Lang["Setup"]["City"] + ": \"" + IP["city"] + "\" \n" + Lang["Setup"]["Country"] + ": \"" + IP["country"] + "\"";

                LocationStatus.Opacity = 1;
                Statusbar.Instance.GPSUsage.Opacity = 0;
                ProcessRing.IsActive = false;
                Status.IP = "Pass";
                Processing();
            }
            catch (Exception)
            {
                LocationStatus.Opacity = 1;
                IPLocation.Background = (Brush)converter.ConvertFromString("#2E0000");
                Status.IP = "Fail";
                Processing();
            }
            
        }

        public void Processing()
        {
            if (Status.GPS == "Pass")
            {
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
                CheckStatus.Text = Lang["Setup"]["Pass"];

                LocationCheckImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/Universal/Win11/Check.png"));
                UseGPS.IsEnabled = true;
                UseGPS.Opacity = 1;
            }

            if (Status.IP == "Pass")
            {
                LocationCheckImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/Universal/Win11/Check.png"));
                UseIP.IsEnabled = true;
                UseIP.Opacity = 1;
            }
            if (Status.IP == "Fail")
            {
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
                CheckStatus.Text = Lang["Setup"]["Fail"];
                IPStatus.Text = "IP - " + Lang["Setup"]["Fail"];
                IPDesc.Text = Lang["Setup"]["IPFail_NoInternet"];

                LocationCheckImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/Universal/Win11/Stop.png"));
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Setup.Instance.Step3.Opacity = 1;
            Setup.Instance.Step4.Opacity = 0.5;


            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation Current = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 2000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step2 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step3 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step4 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step5 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -2000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
            Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
            Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
            Setup.Instance.Status3.Opacity = 1;

            Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
            Setup.Instance.Status4.Opacity = 0.5;

            Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);

            Setup.Instance.MainContent.Content = new Appearance();
        }
        private void IP_Click(object sender, RoutedEventArgs e)
        {
            Configs.GPS = "False";

            var appWindow = new GPSWarning();

            appWindow.Show();
        }

        private void GPS_Click(object sender, RoutedEventArgs e)
        {
            Setup.Instance.Step4.Opacity = 0.5;
            Setup.Instance.Step5.Opacity = 1;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation Current = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 4000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step2 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 3000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step3 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 2000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step4 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step5 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
            Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
            Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
            Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
            Setup.Instance.Status4.Opacity = 0.5;

            Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);
            Setup.Instance.Status5.Opacity = 1;

            Configs.GPS = "True";

            Setup.Instance.MainContent.Content = new SmallerSettings();
        }
    }
}
