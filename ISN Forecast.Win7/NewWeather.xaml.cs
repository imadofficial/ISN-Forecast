using ISN_Forecast.Win7.FirstSetup;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for NewWeather.xaml
    /// </summary>
    public partial class NewWeather : Page
    {
        public static NewWeather Instance;
        public static class GlobalStrings
        {
            public static String IP;
            public static String WeatherData;
            public static String City;
            public static String Country;

        }

        public NewWeather()
        {
            InitializeComponent();
            Instance = this;
            Init();
            GetIP();
        }



        public void Init()
        {
            try
            {
                string Settings = File.ReadAllText("Assets/Settings.json");
                var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

                Configs.Language = SettingsJSON[0]["Language"];

                //Setting all the global variables
                Configs.Unit = SettingsJSON[0]["Unit"];

                if (SettingsJSON[0]["Appearance"].ToString() == "Gradient")
                {
                    MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#0D52AA");
                    MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#7CC0E8");
                    Configs.Look = "Gradient";
                }

                if (SettingsJSON[0]["Appearance"].ToString() == "BlackAndWhite")
                {
                    var converter = new System.Windows.Media.BrushConverter();
                    var BackgroundColor = (Brush)converter.ConvertFromString(BlackWhite.BoxColorBlack);

                    TwoDayForecast.Background = BackgroundColor;
                    Configs.Look = "White";
                }

                var asm = Assembly.GetExecutingAssembly();
                var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

                using (Stream stream = asm.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
                {
                    Configs.Translations = reader.ReadToEnd();
                    var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

                }

                var Metadata = "ISN_Forecast.Win7.Metadata.json";
                using (Stream stream = asm.GetManifestResourceStream(Metadata))
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
                {
                    Configs.Meta = reader.ReadToEnd();
                    var ForkieData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Meta);

                    Configs.Weatherkey = ForkieData["APIKeyWeather"];
                    Configs.IPInfoKey = ForkieData["IPInfoKey"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetIP()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringAsync(new Uri("https://api64.ipify.org/"));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ProcessIP);
        }

        private void ProcessIP(object sender, DownloadStringCompletedEventArgs e)
        {
            Status.Text = "Locating...";
            GlobalStrings.IP = e.Result;

            WebClient webClient = new WebClient();
            webClient.DownloadStringAsync(new Uri("http://ip-api.com/json/" + GlobalStrings.IP));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ProcessLocation);
        }

        private void ProcessLocation(object sender, DownloadStringCompletedEventArgs e)
        {
            //Status.Text = "Preparing weather data...";
            GlobalStrings.IP = e.Result;
            var IP = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(GlobalStrings.IP);
            GlobalStrings.City = IP["city"];
            GlobalStrings.Country = IP["country"];


            Status.Text = "Downloading weather data...";
            Statusbar.Instance.Status.Text = "Current Weather";
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(new Uri("https://api.weatherapi.com/v1/forecast.json?key=" + Configs.Weatherkey + "&q=" + GlobalStrings.City + ", " + GlobalStrings.Country + "&days=10&aqi=yes&alerts=yes"));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WeatherData);
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FutureForecasting);
        }

        private async void WeatherData(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                GlobalStrings.WeatherData = e.Result;
                var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(GlobalStrings.WeatherData);

                City.Text = GlobalStrings.City;
                Temperature.Text = WeatherData["current"][Configs.Unit];

                if (Configs.Unit == "temp_c")
                {
                    Configs.UnitChar = "°C";
                    Type.Text = Configs.UnitChar;
                }
                if (Configs.Unit == "temp_f")
                {
                    Configs.UnitChar = "°F";
                    Type.Text = Configs.UnitChar;
                }
                ConditionText.Text = WeatherData["current"]["condition"]["text"];

                String BS = WeatherData["current"]["condition"]["text"];

                var asm = Assembly.GetExecutingAssembly();
                var resourceName = "ISN_Forecast.Win7.Metadata.json";
                using (Stream stream = asm.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
                {
                    var Condition = reader.ReadToEnd();
                    var Data = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Condition);

                    try
                    {
                        String TopGradient = Data["Conditions"][BS]["Top"];
                        String BottomGradient = Data["Conditions"][BS]["Bottom"];
                        String BoxColor = Data["Conditions"][BS]["Box"];

                        ConditionText.Text = Data["Conditions"][BS]["Name"];

                        MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString(TopGradient);
                        MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString(BottomGradient);
                        TwoDayForecast.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(BoxColor);
                        UVBox.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(BoxColor);
                        AQIBox.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(BoxColor);
                        Astronomy.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(BoxColor);
                    }
                    catch
                    {
                        MessageBox.Show("I'm still building out this database and looks like that \"" + BS + "\" was not added in yet. Please report it an issue on github.", "Important Notice.");
                    }
                }

                Updated.Text = "Updated as of " + WeatherData["current"]["last_updated"];

                Header.Opacity = 1;
                ProcessRing.IsEnabled = false;
                ProcessRing.Opacity = 0;
                Status.Opacity = 0;

                #region Emergency Alerts
                try
                {
                    Source.Text = WeatherData["alerts"]["alert"][0]["headline"];
                    Headlines.Text = WeatherData["alerts"]["alert"][0]["event"];
                }
                catch (Exception ex)
                {
                    Warnings.Opacity = 0;
                    Warnings.Margin = new Thickness(0, -150, 0, 0);
                    Header.Margin = new Thickness(0, 60, 0, 0);
                    BottomInfo.Margin = new Thickness(50, 220, 50, 0);
                }
                #endregion


                #region UV-Rating + AQI Value
                try
                {
                    UValue.Text = WeatherData["current"]["uv"];

                    var UVValue = WeatherData["current"]["uv"].ToString();
                    if (WeatherData["current"]["uv"] == "1") { UVIndicator.Margin = new Thickness(30, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "2") { UVIndicator.Margin = new Thickness(58, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "3") { UVIndicator.Margin = new Thickness(86, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "4") { UVIndicator.Margin = new Thickness(114, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "5") { UVIndicator.Margin = new Thickness(142, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "6") { UVIndicator.Margin = new Thickness(170, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "7") { UVIndicator.Margin = new Thickness(198, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "8") { UVIndicator.Margin = new Thickness(226, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "9") { UVIndicator.Margin = new Thickness(254, -210, 0, 0); }
                    if (WeatherData["current"]["uv"] == "10") { UVIndicator.Margin = new Thickness(282, -210, 0, 0); }

                    var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
                    UVInfo.Text = Lang["UVIndex"][UVValue];
                }
                catch (Exception ex)
                {

                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LastUpdated(double unixTimeStamp)
        {
            try
            {
                var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(GlobalStrings.WeatherData);

                var UnixTime = WeatherData["current"]["last_updated_epoch"];
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                Updated.Text = "Updated as of " + dateTime;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public int ReturnTime(int time)
        {
            int resultTime = 0;
            int currTime = DateTime.Now.Hour;

            DateTime today = DateTime.Now;
            double remaining = (today.AddDays(1).Date - today).TotalHours;

            var times = new Dictionary<int, int>()
            {
                {24, 0 },
                {25, 1 },
                {26, 2 },
                {27, 3 },
                {28, 4 },
                {29, 5 },
                {30, 6 },
                {31, 7 },
                {32, 8 },
                {33, 9 },
                {34, 10 },
                {35, 11 },
                {36, 12 },
                {37, 13 },
                {38, 14 },
                {40, 15 },
                {41, 16},
                {42, 17},
                {43, 18},
                {44, 19 },
                {45, 20 },
                {46, 21 },
                {47, 22 },
                {48, 23 },
            };


            if (currTime + time > 23)
            {
                int val;

                times.TryGetValue(currTime + time, out val);
                resultTime = val;
            }
            else
            {
                resultTime = currTime + time;
            }
            return resultTime;
        }

        private async void FutureForecasting(object sender, DownloadStringCompletedEventArgs e)
        {
            await Task.Delay(500);
            //This is likely the most complicated feature yet. I will prob end up rewriting this feature soon.
            var FutureData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result);
            int count1 = 0;
            int CurrentHour = DateTime.Now.Hour;

            String TextHour = DateTime.Now.ToString("HH");

            foreach (TextBox tempText in FutureTwoDays.Children.OfType<TextBox>())
            {
                tempText.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(count1)][Configs.Unit] + "°C";
                count1++;

                if(count1 == 23)
                {
                    count1 = 0;
                }
            }

            Hour1.Text = "Now";
            Hour2.Text = ReturnTime(1) + ":00";
            Hour3.Text = ReturnTime(2) + ":00";
            Hour4.Text = ReturnTime(3) + ":00";
            Hour5.Text = ReturnTime(4) + ":00";
            Hour6.Text = ReturnTime(5) + ":00";
            Hour7.Text = ReturnTime(6) + ":00";
            Hour8.Text = ReturnTime(7) + ":00";
            Hour9.Text = ReturnTime(8) + ":00";
            Hour10.Text = ReturnTime(9) + ":00";
            Hour11.Text = ReturnTime(10) + ":00";
            Hour12.Text = ReturnTime(11) + ":00";
            Hour13.Text = ReturnTime(12) + ":00";
            Hour14.Text = ReturnTime(13) + ":00";
            Hour15.Text = ReturnTime(14) + ":00";
            Hour16.Text = ReturnTime(15) + ":00";
            Hour17.Text = ReturnTime(16) + ":00";
            Hour18.Text = ReturnTime(17) + ":00";
            Hour19.Text = ReturnTime(18) + ":00";
            Hour20.Text = ReturnTime(19) + ":00";
            Hour21.Text = ReturnTime(20) + ":00";
            Hour22.Text = ReturnTime(21) + ":00";
            Hour23.Text = ReturnTime(23) + ":00";
            Hour24.Text = ReturnTime(24) + ":00";
        }
    }
}  
