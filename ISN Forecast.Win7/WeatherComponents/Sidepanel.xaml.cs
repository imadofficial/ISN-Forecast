using ISN_Forecast.Win7.FirstSetup;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ISN_Forecast.Win7.WeatherComponents
{
    /// <summary>
    /// Interaction logic for Sidepanel.xaml
    /// </summary>
    public partial class Sidepanel : Page
    {
        public static Sidepanel Instance;
        public Sidepanel()
        {
            InitializeComponent();
            Instance = this;

            string Settings = File.ReadAllText("Assets/Settings.json");
            var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            Configs.Language = SettingsJSON[0]["Language"];

            //Setting all the global variables
            Configs.Unit = SettingsJSON[0]["Unit"];

            var converter = new System.Windows.Media.BrushConverter();

            if (Configs.Look == "Gradient")
            {

            }

            if (Configs.Look == "Dark")
            {
                PanelBG.Background = (Brush)converter.ConvertFromString("#303030");
                Default.Background = (Brush)converter.ConvertFromString("#B3B3B3");
            }

            var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.FullText);
            DefaultCity.Text = WeatherData["location"]["name"];
            DefaultCountry.Text = WeatherData["location"]["country"];
            DefaultTemps.Text = WeatherData["current"][Configs.Unit] + "°";
            DefaultCondition.Text = WeatherData["current"]["condition"]["text"];
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var ScreenWidth = MainWindow.Instance.Screen.ActualWidth;
            Statusbar.Instance.Cities.Margin = new Thickness(0, 6, 10, 0);
            Statusbar.Instance.Cities.Opacity = 1;

            MainWindow.Instance.MainContents.Width = Double.NaN;
            MainWindow.Instance.MainContents.HorizontalAlignment = HorizontalAlignment.Center;

            MainWindow.Instance.Sidepanel.Content = null;
            MainWindow.Instance.Sidepanel.Margin = new Thickness(-25565, 70, 0, 0);

            if (ScreenWidth > 1480)
            {
            }
            if (ScreenWidth >= 1180 && ScreenWidth <= 1480)
            {
                MainWindow.Instance.MainContents.Width = 1180;
                NewWeather.Instance.SevenDay.Margin = new Thickness(50, -250, 0, 0);
                NewWeather.Instance.AQIBox.Margin = new Thickness(0, 10, 210, 0);
            }
            if (ScreenWidth <= 1179)
            {
                MainWindow.Instance.MainContents.Width = 900;
                NewWeather.Instance.SevenDay.Margin = new Thickness(0, -120, 0, 0);
                //NewWeather.Instance.UVBox.Margin = new Thickness(0, 10, 0, 0);
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
