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

            int CurrentHour = DateTime.Now.Hour;
            String TextHour = DateTime.Now.ToString("HH");

            string One = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(0)][Configs.Unit] + "°C";
            string Two = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(1)][Configs.Unit] + "°C";
            string Three = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(2)][Configs.Unit] + "°C";
            string Four = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(3)][Configs.Unit] + "°C";
            string Five = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(4)][Configs.Unit] + "°C";
            string Six = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(5)][Configs.Unit] + "°C";
            string Seven = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(6)][Configs.Unit] + "°C";
            string Eight = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(7)][Configs.Unit] + "°C";
            string Nine = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(8)][Configs.Unit] + "°C";
            string Ten = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(9)][Configs.Unit] + "°C";
            string Eleven = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(10)][Configs.Unit] + "°C";
            string Twelve = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(11)][Configs.Unit] + "°C";
            string One3 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(12)][Configs.Unit] + "°C";
            string One4 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(13)][Configs.Unit] + "°C";
            string One5 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(14)][Configs.Unit] + "°C";
            string One6 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(15)][Configs.Unit] + "°C";
            string One7 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(16)][Configs.Unit] + "°C";
            string One8 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(17)][Configs.Unit] + "°C";
            string One9 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(18)][Configs.Unit] + "°C";
            string Two0 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(19)][Configs.Unit] + "°C";
            string Two1 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(20)][Configs.Unit] + "°C";
            string Two2 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(21)][Configs.Unit] + "°C";
            string Two3 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(22)][Configs.Unit] + "°C";
            string Two4 = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(23)][Configs.Unit] + "°C";
            int index = One.IndexOf(".");
            if (index >= 0)
                Temp1.Text = One.Substring(0, index) + Configs.UnitChar;
                Temp2.Text = Two.Substring(0, index) + Configs.UnitChar;
                Temp3.Text = Three.Substring(0, index) + Configs.UnitChar;
                Temp4.Text = Four.Substring(0, index) + Configs.UnitChar;
                Temp5.Text = Five.Substring(0, index) + Configs.UnitChar;
                Temp6.Text = Six.Substring(0, index) + Configs.UnitChar;
                Temp7.Text = Seven.Substring(0, index) + Configs.UnitChar;
                Temp8.Text = Eight.Substring(0, index) + Configs.UnitChar;
                Temp9.Text = Nine.Substring(0, index) + Configs.UnitChar;
                Temp10.Text = Ten.Substring(0, index) + Configs.UnitChar;
                Temp11.Text = Eleven.Substring(0, index) + Configs.UnitChar;
                Temp12.Text = Twelve.Substring(0, index) + Configs.UnitChar;
                Temp13.Text = One3.Substring(0, index) + Configs.UnitChar;
                Temp14.Text = One4.Substring(0, index) + Configs.UnitChar;
                Temp15.Text = One5.Substring(0, index) + Configs.UnitChar;
                Temp16.Text = One6.Substring(0, index) + Configs.UnitChar;
                Temp17.Text = One7.Substring(0, index) + Configs.UnitChar;
                Temp18.Text = One8.Substring(0, index) + Configs.UnitChar;
                Temp19.Text = One9.Substring(0, index) + Configs.UnitChar;
                Temp20.Text = Two0.Substring(0, index) + Configs.UnitChar;
                Temp21.Text = Two1.Substring(0, index) + Configs.UnitChar;
                Temp22.Text = Two2.Substring(0, index) + Configs.UnitChar;
                Temp23.Text = Two3.Substring(0, index) + Configs.UnitChar;
                Temp24.Text = Two4.Substring(0, index) + Configs.UnitChar;

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