using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using ISN_Forecast.Win7.Diagnostics;
using ISN_Forecast.Win7.FirstSetup;
using ISN_Forecast.Win7.API;
using Newtonsoft.Json;

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public class data
        {
            public String OOBESetup { get; set; }
            public String Language { get; set; }
            public String Appearance { get; set; }
            public String TimeFormat { get; set; }
            public String DateFormat { get; set; }
            public String Country { get; set; }
            public String AllowAlerts { get; set; }
            public String AllowAutoUpdate { get; set; }
            public String Unit { get; set; }

        }

        public Settings()
        {
            InitializeComponent();
            Lang();

            CheckColorscheme();
            CheckAutoUpdates();
            CheckUnit();
            CheckRegion();
            CheckTime();
            CheckSpeed();
        }

        public void Lang()
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

            Statusbar.Instance.Status.Text = Lang["Settings"]["Settings"];
            Status.Text = Lang["Settings"]["Behavior"];
            UpdatesTitle.Text = Lang["Settings"]["AutoUpdates"];
            UpdatesDescription.Text = Lang["Settings"]["AutoUpdatesDesc"];
            UnitTitle.Text = Lang["Settings"]["Unit"];
            UnitDescription.Text = Lang["Settings"]["UnitDesc"];
            GPSTitle.Text = Lang["Settings"]["GPS"];
            GPSDescription.Text = Lang["Settings"]["GPSDesc"];

            RegionAndGeography.Text = Lang["Settings"]["General"];
            RegionTitle.Text = Lang["Settings"]["Region"];
            RegionDescription.Text = Lang["Settings"]["RegionDesc"];
            TimeTitle.Text = Lang["Settings"]["TimeFormat"];
            TimeDescription.Text = Lang["Settings"]["TimeFormatDesc"];
            SpeedTitle.Text = Lang["Settings"]["Speed"];
            SpeedDescription.Text = Lang["Settings"]["SpeedDesc"];

            Appearance.Text = Lang["Settings"]["Appearance"];
            AccentColorTitle.Text = Lang["Settings"]["Color"];
            AccentColorDescription.Text = Lang["Settings"]["ColorDesc"];

            Additional.Text = Lang["Settings"]["AdditionalServices"];
            EASTitle.Text = Lang["Settings"]["EmergencyAlerts"];
            EASDescription.Text = Lang["Settings"]["EmergencyAlertsDesc"];
            EEWTitle.Text = Lang["Settings"]["EEW"];
            EEWDescription.Text = Lang["Settings"]["EEWDesc"];

            Troubleshoot.Text = Lang["Settings"]["DiagCategory"];
            ConnectTitle.Text = Lang["Settings"]["Diag"];
            ConnectDescription.Text = Lang["Settings"]["DiagDesc"];
            ResetTitle.Text = Lang["Settings"]["Reset"];
            ResetDescription.Text = Lang["Settings"]["ResetDesc"];

            WiiMODE.Text = Lang["Settings"]["Misc"];
            WiiTitle.Text = Lang["Settings"]["Wii"];
            WiiDescription.Text = Lang["Settings"]["WiiDesc"];
        }

        private void CheckSpeed()
        {
            var Settings = File.ReadAllText("Assets/Settings.json");
            var JSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            SpeedSelection.Items.Insert(0, "km/h");
            SpeedSelection.Items.Insert(1, "mph");

            if (JSON[0]["Speed"] == "kmh")
            {
                SpeedSelection.SelectedIndex = 0;
            }
            if (JSON[0]["Speed"] == "mph")
            {
                SpeedSelection.SelectedIndex = 1;
            }
        }

        private void CheckTime()
        {
            var Settings = File.ReadAllText("Assets/Settings.json");
            var JSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            TimeSelection.Items.Insert(0, DateTime.Now.ToString("hh:mm tt")); //12-Hour Time
            TimeSelection.Items.Insert(1, DateTime.Now.ToString("HH:mm")); //24-Hour Time

            if (JSON[0]["TimeFormat"] == "24")
            {
                TimeSelection.SelectedIndex = 1;
            }
            if (JSON[0]["TimeFormat"] == "12")
            {
                TimeSelection.SelectedIndex = 0;
            }
        }

        private void CheckRegion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);

                RegionSelection.Items.Insert(0, Lang["Countries"]["BE"]);
                RegionSelection.Items.Insert(1, Lang["Countries"]["ES"]);
                RegionSelection.Items.Insert(2, Lang["Countries"]["FR"]);
                RegionSelection.Items.Insert(3, Lang["Countries"]["UK"]);
                RegionSelection.Items.Insert(4, Lang["Countries"]["NL"]);

                var Settings = File.ReadAllText("Assets/Settings.json");
                var JSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

                String CountryCode = JSON[0]["Country"];
                var Results = Lang["Countries"][CountryCode];

               RegionSelection.SelectedItem = Results;
            }
        }
        private void CheckUnit()
        {
            var Settings = File.ReadAllText("Assets/Settings.json");
            var JSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            UnitSelection.Items.Insert(0, "°C");
            UnitSelection.Items.Insert(1, "°F");

            if (JSON[0]["Unit"] == "temp_f")
            {
                UnitSelection.SelectedItem = "°F";
            }
            if (JSON[0]["Unit"] == "temp_c")
            {
                UnitSelection.SelectedItem = "°C";
            }
        }

        private void CheckAutoUpdates()
        {
            var Settings = File.ReadAllText("Assets/Settings.json");
            var JSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);
            if(JSON[0]["AllowAutoUpdate"] == "True")
            {
                UpdatesBox.IsChecked = true;
            }
        }
        private void CheckColorscheme()
        {
            ColorPicker.Items.Insert(0, "Colorful");
            ColorPicker.Items.Insert(1, "Dark");
            ColorPicker.Items.Insert(2, "Bright");

            if (Configs.Look == "Gradient")
            {
                ColorPicker.SelectedIndex = 0;
            }
            if (Configs.Look == "Dark")
            {
                ColorPicker.SelectedIndex = 1;
            }
            if (Configs.Look == "Bright")
            {
                ColorPicker.SelectedIndex = 2;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

            DyamicLoading.Load();
            DyamicLoading.FutureForecasting();
            var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.FullText);
            var JSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("Assets/Settings.json"));

            Statusbar.Instance.Status.Text = "Current Weather";
            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, -10240, 0, 0);
            NewWeather.Instance.Effect = null;
            NewWeather.Instance.ScrollPerms.IsEnabled = true;

            Statusbar.Instance.Settings.IsEnabled = true; Statusbar.Instance.Settings.Opacity = 1;
            Statusbar.Instance.Globe.IsEnabled = true; Statusbar.Instance.Globe.Opacity = 1;
            Statusbar.Instance.Search.IsEnabled = true; Statusbar.Instance.Search.Opacity = 1;
        }

        private async void ConnectTest(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#000000");
            MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#000000");
            await Task.Delay(500);
            MainWindow.Instance.MainContents.Opacity = 0;
            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0,-10240,0,0);
            await Task.Delay(1000);
            Statusbar.Instance.Status.Text = "Diagnostics Mode";
            MainWindow.Instance.MainContents.Margin = new Thickness(0, 60, 0, 0);
            await Task.Delay(2000);
            MainWindow.Instance.MainContents.Content = new Setup();
            Setup.Instance.MainContent.Content = new Splash();
            Setup.Instance.Status.Text = "Please select an action";
            Setup.Instance.Progress.Opacity = 0;
            MainWindow.Instance.MainContents.Opacity = 1;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            GetSettings();

            List<data> _data = new List<data>();
            _data.Add(new data()
            {
                OOBESetup = "False",
                Language = Configs.Language, //ehhhhh
                Appearance = Configs.Look, //ok
                TimeFormat = Configs.TimeFormat, //ok
                DateFormat = Configs.DateFormat, //still needs work
                Country = Configs.Country, //ok
                AllowAlerts = Configs.AllowAlerts, //definitely still needs some work
                AllowAutoUpdate = Configs.AllowAutoUpdate, //ok
                Unit = Configs.Unit //ok
            });

            string json = JsonConvert.SerializeObject(_data.ToArray());

            //write string to file
            System.IO.File.WriteAllText(@"Assets/Settings.json", json);
            SavedText.Opacity = 1;
        }

        private void GetSettings()
        {
            Configs.AllowAutoUpdate = UpdatesBox.IsChecked.ToString();
            Configs.Language = "en_US";
            Configs.DateFormat = "DD/MM/YYYY";
            GetCountry();
            GetUnit();
            GetAppearance();
            GetTime();
        }
        private void GetTime()
        {
            if (TimeSelection.SelectedIndex == 0)
            {
                Configs.TimeFormat = "12";
            }
            if (TimeSelection.SelectedIndex == 1)
            {
                Configs.TimeFormat = "24";
            }
        }

        private void GetAppearance()
        {
            if (ColorPicker.SelectedIndex == 0)
            {
                Configs.Look = "Gradient";
            }
            if (ColorPicker.SelectedIndex == 1)
            {
                Configs.Look = "BlackAndWhite";
            }
        }

        private void GetUnit()
        {
            if(UnitSelection.SelectedIndex == 0)
            {
                Configs.Unit = "temp_c";
            }
            if (UnitSelection.SelectedIndex == 1)
            {
                Configs.Unit = "temp_f";
            }
        }

        private void GetCountry()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);

                Configs.Country = Lang["Countries"][RegionSelection.Text];
            }
        }
    }
}
