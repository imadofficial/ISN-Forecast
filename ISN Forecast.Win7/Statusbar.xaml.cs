using ISN_Forecast.Win7.FirstSetup;
using ISN_Forecast.Win7.WeatherComponents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    public partial class Statusbar : Page
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public static Statusbar Instance;

        public Statusbar()
        {
            InitializeComponent();
            string Settings = File.ReadAllText("Assets/Settings.json");
            var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            if (SettingsJSON[0]["TimeFormat"] == "24" || SettingsJSON[0]["TimeFormat"] == "12")
            {
                Timer.Tick += new EventHandler(Timer_Click);
                Timer.Interval = new TimeSpan(0, 0, 1);
                Timer.Start();
            }
            else
            {
                Timer.Tick += new EventHandler(DefaultTimer);
                Timer.Interval = new TimeSpan(0, 0, 1);
                Timer.Start();
            }
            Instance = this;
        }

        private void DefaultTimer(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToString("HH:mm");
            string date = DateTime.UtcNow.ToString("dd/MM/yyyy");
            Date.Text = date;
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            string Settings = File.ReadAllText("Assets/Settings.json");
            var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            try
            {
                if (SettingsJSON[0]["TimeFormat"] == "24")
                {
                    Time.Text = DateTime.Now.ToString("HH:mm");
                    string date = DateTime.UtcNow.ToString("MM/dd/yyyy");
                    Date.Text = date;
                }
                if (SettingsJSON[0]["TimeFormat"] == "12")
                {
                    Time.Text = DateTime.Now.ToString("hh:mm tt");
                    string date = DateTime.UtcNow.ToString("MM/dd/yyyy");
                    Date.Text = date;
                }
            }
            catch (Exception)
            {
                DateTime d;
                d = DateTime.Now;
                    
                Time.Text = DateTime.Now.ToString("hh:mm tt");

                string date = DateTime.UtcNow.ToString("MM/dd/yyyy");
                Date.Text = date;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            Status.Text = Lang["Search"]["Search"];

            BlurEffect myBlurEffect = new BlurEffect(); //Adding the blur effect
            myBlurEffect.Radius = 24;
            NewWeather.Instance.Effect = myBlurEffect;
            NewWeather.Instance.ScrollPerms.IsEnabled = false;

            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, 60, 0, 0);
            MainWindow.Instance.ButtonedScreen.Content = new Search();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            BlurEffect myBlurEffect = new BlurEffect(); //Adding the blur effect
            myBlurEffect.Radius = 24;
            NewWeather.Instance.Effect = myBlurEffect;
            NewWeather.Instance.ScrollPerms.IsEnabled = false;
            Status.Text = Lang["Settings"]["Settings"];

            Settings.IsEnabled = false; Settings.Opacity = 0;
            Globe.IsEnabled = false;    Globe.Opacity = 0;
            Search.IsEnabled = false;   Search.Opacity = 0;

            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, 60, 0, 0);
            MainWindow.Instance.ButtonedScreen.Content = new Settings();
        }

        private void Globe_Click(object sender, RoutedEventArgs e)
        {
            Status.Text = "Globe";
        }

        private void Cities_Click(object sender, RoutedEventArgs e) //Opens the SidePanel with all the cities you have saved
        {
            
            var ScreenWidth = MainWindow.Instance.Screen.ActualWidth;
            if (ScreenWidth < 1280)
            {
                    MessageBox.Show("Your Screenres is too low, idot.");
            }

            if(ScreenWidth > 1781)
            {
                MainWindow.Instance.MainContents.Width = 1180;
                NewWeather.Instance.SevenDay.Margin = new Thickness(50, -250, 0, 0);
                NewWeather.Instance.AQIBox.Margin = new Thickness(0, 10, 210, 0);
                MainWindow.Instance.MainContents.HorizontalAlignment = HorizontalAlignment.Right;
                MainWindow.Instance.Sidepanel.Content = new Sidepanel();
                MainWindow.Instance.Sidepanel.Margin = new Thickness(40, 70, 0, 0);
            }


            if (ScreenWidth >= 1280 && ScreenWidth <= 1780)
            {
                Cities.Margin = new Thickness(-60, 6, 10, 0);
                Cities.Opacity = 0;
                AppBehavior.Hamburger = "Forkie";
                MainWindow.Instance.MainContents.Width = 900;
                MainWindow.Instance.MainContents.HorizontalAlignment = HorizontalAlignment.Right;
                NewWeather.Instance.SevenDay.Margin = new Thickness(0, -120, 0, 0);
                NewWeather.Instance.AQIBox.Margin = new Thickness(0, 10, 0, 0);

                MainWindow.Instance.Sidepanel.Content = new Sidepanel();
                MainWindow.Instance.Sidepanel.Margin = new Thickness(40, 70, 0, 0);
             }
        }

        private async void Music_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect myBlurEffect = new BlurEffect(); //Adding the blur effect
            myBlurEffect.Radius = 28;
            //Setup.Instance.Effect = myBlurEffect;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseInOut;

            DoubleAnimation HeightAnimation = new DoubleAnimation()
            {
                To = 80,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            DoubleAnimation WidthAnimation = new DoubleAnimation()
            {
                To = 250,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation MusicTitle = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };
            var converter = new System.Windows.Media.BrushConverter();

            TextBlock TimePassed = new TextBlock();
            TimePassed.Text = "0:58";
            TimePassed.VerticalAlignment = VerticalAlignment.Top;
            TimePassed.HorizontalAlignment = HorizontalAlignment.Left;
            TimePassed.Margin = new Thickness(20, 5, 0, 0);
            TimePassed.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
            TimePassed.FontSize = 14;
            TimePassed.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

            TextBlock TotalMusicTime = new TextBlock();
            TotalMusicTime.Text = "2:58";
            TotalMusicTime.VerticalAlignment = VerticalAlignment.Top;
            TotalMusicTime.HorizontalAlignment = HorizontalAlignment.Right;
            TotalMusicTime.Margin = new Thickness(0, -16, 20, 0);
            TotalMusicTime.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
            TotalMusicTime.FontSize = 14;
            TotalMusicTime.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

            //Statusbar.Instance.MusicBox.BeginAnimation(Border.HeightProperty, HeightAnimation);
            //Statusbar.Instance.MusicBox.BeginAnimation(Border.WidthProperty, WidthAnimation);
            //await Task.Delay(200);
            //MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, 0, 0, 0);
            //MainWindow.Instance.ButtonedScreen.Content = new MusicUX();
            //MusicControls.Children.Add(TimePassed);
            //MusicControls.Children.Add(TotalMusicTime);

            
        }
    }
}
