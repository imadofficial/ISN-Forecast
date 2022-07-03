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
            Date.Text = DateTime.UtcNow.ToString("MM/dd/yyyy");
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
                }
                if (SettingsJSON[0]["TimeFormat"] == "12")
                {
                    Time.Text = DateTime.Now.ToString("hh:mm tt");
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
            Status.Text = "Search";

            BlurEffect myBlurEffect = new BlurEffect(); //Adding the blur effect
            myBlurEffect.Radius = 24;
            NewWeather.Instance.Effect = myBlurEffect;
            NewWeather.Instance.ScrollPerms.IsEnabled = false;

            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, 60, 0, 0);
            MainWindow.Instance.ButtonedScreen.Content = new Search();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect myBlurEffect = new BlurEffect(); //Adding the blur effect
            myBlurEffect.Radius = 24;
            NewWeather.Instance.Effect = myBlurEffect;
            NewWeather.Instance.ScrollPerms.IsEnabled = false;
            Status.Text = "Settings";

            Settings.IsEnabled = false; Settings.Opacity = 0;
            Refresh.IsEnabled = false;  Refresh.Opacity = 0;
            Globe.IsEnabled = false;    Globe.Opacity = 0.01;
            Search.IsEnabled = false;   Search.Opacity = 0;

            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, 60, 0, 0);
            MainWindow.Instance.ButtonedScreen.Content = new Settings();
        }

        private void Globe_Click(object sender, RoutedEventArgs e)
        {
            Status.Text = "Globe";
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainContents.Content = new NewWeather();
        }
    }
}
