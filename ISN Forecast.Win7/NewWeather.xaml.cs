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
            webClient.DownloadStringAsync(new Uri("https://ipinfo.io/" + GlobalStrings.IP + "?token=" + Configs.IPInfoKey));
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
            webClient.DownloadStringAsync(new Uri("https://api.weatherapi.com/v1/forecast.json?key=" + Configs.Weatherkey + "&q=" + GlobalStrings.City + ", "+ GlobalStrings.Country + "&days=10&aqi=yes&alerts=yes"));
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

                if(Configs.Unit == "temp_c")
                {
                    Type.Text = "°C";
                }
                if (Configs.Unit == "temp_f")
                {
                    Type.Text = "°F";
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
                catch(Exception ex)
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

        private async void FutureForecasting(object sender, DownloadStringCompletedEventArgs e)
        {
            await Task.Delay(500);
            //This is likely the most complicated feature yet. I will prob end up rewriting this feature soon.
            var FutureData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result);

            int CurrentHour = DateTime.Now.Hour;
            String TextHour = DateTime.Now.ToString("HH");
            
            if (CurrentHour == 0)
            {
                Temp1.Text = FutureData["forecast"]["forecastday"][0]["hour"][0][Configs.Unit] + "°C";
                Temp2.Text = FutureData["forecast"]["forecastday"][0]["hour"][1][Configs.Unit] + "°C"; 
                Temp3.Text = FutureData["forecast"]["forecastday"][0]["hour"][2][Configs.Unit] + "°C";
                Temp4.Text = FutureData["forecast"]["forecastday"][0]["hour"][3][Configs.Unit] + "°C";
                Temp5.Text = FutureData["forecast"]["forecastday"][0]["hour"][4][Configs.Unit] + "°C";
                Temp6.Text = FutureData["forecast"]["forecastday"][0]["hour"][5][Configs.Unit] + "°C";
                Temp7.Text = FutureData["forecast"]["forecastday"][0]["hour"][6][Configs.Unit] + "°C";
                Temp8.Text = FutureData["forecast"]["forecastday"][0]["hour"][7][Configs.Unit] + "°C";
                Temp9.Text = FutureData["forecast"]["forecastday"][0]["hour"][8][Configs.Unit] + "°C";
                Temp10.Text = FutureData["forecast"]["forecastday"][0]["hour"][9][Configs.Unit] + "°C";
                Temp11.Text = FutureData["forecast"]["forecastday"][0]["hour"][10][Configs.Unit] + "°C";
                Temp12.Text = FutureData["forecast"]["forecastday"][0]["hour"][11][Configs.Unit] + "°C";
                Temp13.Text = FutureData["forecast"]["forecastday"][0]["hour"][12][Configs.Unit] + "°C";
                Temp14.Text = FutureData["forecast"]["forecastday"][0]["hour"][13][Configs.Unit] + "°C";
                Temp15.Text = FutureData["forecast"]["forecastday"][0]["hour"][14][Configs.Unit] + "°C";
                Temp16.Text = FutureData["forecast"]["forecastday"][0]["hour"][15][Configs.Unit] + "°C";
                Temp17.Text = FutureData["forecast"]["forecastday"][0]["hour"][16][Configs.Unit] + "°C";
                Temp18.Text = FutureData["forecast"]["forecastday"][0]["hour"][17][Configs.Unit] + "°C";
                Temp19.Text = FutureData["forecast"]["forecastday"][0]["hour"][18][Configs.Unit] + "°C";
                Temp20.Text = FutureData["forecast"]["forecastday"][0]["hour"][19][Configs.Unit] + "°C";
                Temp21.Text = FutureData["forecast"]["forecastday"][0]["hour"][20][Configs.Unit] + "°C";
                Temp22.Text = FutureData["forecast"]["forecastday"][0]["hour"][21][Configs.Unit] + "°C";
                Temp23.Text = FutureData["forecast"]["forecastday"][0]["hour"][22][Configs.Unit] + "°C";
                Temp24.Text = FutureData["forecast"]["forecastday"][0]["hour"][23][Configs.Unit] + "°C";

                Hour1.Text = "Now"; Hour2.Text = 1 + ":00"; Hour3.Text = 2 + ":00"; Hour4.Text = 3 + ":00"; Hour5.Text = 4 + ":00"; Hour6.Text = 5 + ":00";
                Hour7.Text = 6 + ":00"; Hour8.Text = 7 + ":00"; Hour9.Text = 8 + ":00"; Hour10.Text = 9 + ":00"; Hour11.Text = 10 + ":00"; Hour12.Text = 11 + ":00";
                Hour13.Text = 12 + ":00"; Hour14.Text = 13 + ":00"; Hour15.Text = 14 + ":00"; Hour16.Text = 15 + ":00"; Hour17.Text = 16 + ":00"; Hour18.Text = 17 + ":00";
                Hour19.Text = 18 + ":00"; Hour20.Text = 19 + ":00"; Hour21.Text = 20 + ":00"; Hour22.Text = 21 + ":00"; Hour23.Text = 22 + ":00"; Hour24.Text = 23 + ":00";
            }
            if (CurrentHour == 1)
            {
                Temp1.Text = FutureData["forecast"]["forecastday"][0]["hour"][1][Configs.Unit] + "°C";
                Temp2.Text = FutureData["forecast"]["forecastday"][0]["hour"][2][Configs.Unit] + "°C";
                Temp3.Text = FutureData["forecast"]["forecastday"][0]["hour"][3][Configs.Unit] + "°C";
                Temp4.Text = FutureData["forecast"]["forecastday"][0]["hour"][4][Configs.Unit] + "°C";
                Temp5.Text = FutureData["forecast"]["forecastday"][0]["hour"][5][Configs.Unit] + "°C";
                Temp6.Text = FutureData["forecast"]["forecastday"][0]["hour"][6][Configs.Unit] + "°C";
                Temp7.Text = FutureData["forecast"]["forecastday"][0]["hour"][7][Configs.Unit] + "°C";
                Temp8.Text = FutureData["forecast"]["forecastday"][0]["hour"][8][Configs.Unit] + "°C";
                Temp9.Text = FutureData["forecast"]["forecastday"][0]["hour"][9][Configs.Unit] + "°C";
                Temp10.Text = FutureData["forecast"]["forecastday"][0]["hour"][10][Configs.Unit] + "°C";
                Temp11.Text = FutureData["forecast"]["forecastday"][0]["hour"][11][Configs.Unit] + "°C";
                Temp12.Text = FutureData["forecast"]["forecastday"][0]["hour"][12][Configs.Unit] + "°C";
                Temp13.Text = FutureData["forecast"]["forecastday"][0]["hour"][13][Configs.Unit] + "°C";
                Temp14.Text = FutureData["forecast"]["forecastday"][0]["hour"][14][Configs.Unit] + "°C";
                Temp15.Text = FutureData["forecast"]["forecastday"][0]["hour"][15][Configs.Unit] + "°C";
                Temp16.Text = FutureData["forecast"]["forecastday"][0]["hour"][16][Configs.Unit] + "°C";
                Temp17.Text = FutureData["forecast"]["forecastday"][0]["hour"][17][Configs.Unit] + "°C";
                Temp18.Text = FutureData["forecast"]["forecastday"][0]["hour"][18][Configs.Unit] + "°C";
                Temp19.Text = FutureData["forecast"]["forecastday"][0]["hour"][19][Configs.Unit] + "°C";
                Temp20.Text = FutureData["forecast"]["forecastday"][0]["hour"][20][Configs.Unit] + "°C";
                Temp21.Text = FutureData["forecast"]["forecastday"][0]["hour"][21][Configs.Unit] + "°C";
                Temp22.Text = FutureData["forecast"]["forecastday"][0]["hour"][22][Configs.Unit] + "°C";
                Temp23.Text = FutureData["forecast"]["forecastday"][0]["hour"][23][Configs.Unit] + "°C";
                Temp24.Text = FutureData["forecast"]["forecastday"][0]["hour"][0][Configs.Unit] + "°C";

                Hour1.Text = "Now";
                Temp1.Text = FutureData["forecast"]["forecastday"][0]["hour"][1]["temp_c"] + "°C";

                Hour2.Text = 2 + ":00";
                Hour3.Text = 3 + ":00";
                Hour4.Text = 4 + ":00";
                Hour5.Text = 5 + ":00";
                Hour6.Text = 6 + ":00";
                Hour7.Text = 7 + ":00";
                Hour8.Text = 8 + ":00";
                Hour9.Text = 9 + ":00";
                Temp9.Text = FutureData["forecast"]["forecastday"][0]["hour"][9]["temp_c"] + "°C";

                Hour10.Text = 10 + ":00";
                Temp10.Text = FutureData["forecast"]["forecastday"][0]["hour"][10]["temp_c"] + "°C";

                Hour11.Text = 11 + ":00";
                Temp11.Text = FutureData["forecast"]["forecastday"][0]["hour"][11]["temp_c"] + "°C";

                Hour12.Text = 12 + ":00";
                Temp12.Text = FutureData["forecast"]["forecastday"][0]["hour"][12]["temp_c"] + "°C";

                Hour13.Text = 13 + ":00";
                Temp13.Text = FutureData["forecast"]["forecastday"][0]["hour"][13]["temp_c"] + "°C";

                Hour14.Text = 14 + ":00";
                Temp14.Text = FutureData["forecast"]["forecastday"][0]["hour"][14]["temp_c"] + "°C";

                Hour15.Text = 15 + ":00";
                Temp15.Text = FutureData["forecast"]["forecastday"][0]["hour"][15]["temp_c"] + "°C";

                Hour16.Text = 16 + ":00";
                Temp16.Text = FutureData["forecast"]["forecastday"][0]["hour"][16]["temp_c"] + "°C";

                Hour17.Text = 17 + ":00";
                Temp17.Text = FutureData["forecast"]["forecastday"][0]["hour"][17]["temp_c"] + "°C";
                
                Hour18.Text = 18 + ":00";
                Temp18.Text = FutureData["forecast"]["forecastday"][0]["hour"][18]["temp_c"] + "°C";

                Hour19.Text = 19 + ":00";
                Temp19.Text = FutureData["forecast"]["forecastday"][0]["hour"][19]["temp_c"] + "°C";

                Hour20.Text = 20 + ":00";
                Temp20.Text = FutureData["forecast"]["forecastday"][0]["hour"][20]["temp_c"] + "°C";

                Hour21.Text = 21 + ":00";
                Temp21.Text = FutureData["forecast"]["forecastday"][0]["hour"][21]["temp_c"] + "°C";

                Hour22.Text = 22 + ":00";
                Temp22.Text = FutureData["forecast"]["forecastday"][0]["hour"][22]["temp_c"] + "°C";

                Hour23.Text = 23 + ":00";
                Temp23.Text = FutureData["forecast"]["forecastday"][0]["hour"][23]["temp_c"] + "°C";

                Hour24.Text = 00 + ":00";
                Temp24.Text = FutureData["forecast"]["forecastday"][1]["hour"][0]["temp_c"] + "°C";
            }
            if (CurrentHour == 2)
            {
                Hour1.Text = "Now";
                Hour2.Text = 3 + ":00";
                Hour3.Text = 4 + ":00";
                Hour4.Text = 5 + ":00";
                Hour5.Text = 6 + ":00";
                Hour6.Text = 7 + ":00";
                Hour7.Text = 8 + ":00";
                Hour8.Text = 9 + ":00";
                Hour9.Text = 10 + ":00";
                Hour10.Text = 11 + ":00";
                Hour11.Text = 12 + ":00";
                Hour12.Text = 13 + ":00";
                Hour13.Text = 14 + ":00";
                Hour14.Text = 15 + ":00";
                Hour15.Text = 16 + ":00";
                Hour16.Text = 17 + ":00";
                Hour17.Text = 18 + ":00";
                Hour18.Text = 19 + ":00";
                Hour19.Text = 20 + ":00";
                Hour20.Text = 21 + ":00";
                Hour21.Text = 22 + ":00";
                Hour22.Text = 23 + ":00";
                Hour23.Text = 00 + ":00";
                Hour24.Text = 01 + ":00";
            }
            if (CurrentHour == 3)
            {
                Hour1.Text = "Now";
                Hour2.Text = 4 + ":00";
                Hour3.Text = 5 + ":00";
                Hour4.Text = 6 + ":00";
                Hour5.Text = 7 + ":00";
                Hour6.Text = 8 + ":00";
                Hour7.Text = 9 + ":00";
                Hour8.Text = 10 + ":00";
                Hour9.Text = 11 + ":00";
                Hour10.Text = 12 + ":00";
                Hour11.Text = 13 + ":00";
                Hour12.Text = 14 + ":00";
                Hour13.Text = 15 + ":00";
                Hour14.Text = 16 + ":00";
                Hour15.Text = 17 + ":00";
                Hour16.Text = 18 + ":00";
                Hour17.Text = 19 + ":00";
                Hour18.Text = 20 + ":00";
                Hour19.Text = 21 + ":00";
                Hour20.Text = 22 + ":00";
                Hour21.Text = 23 + ":00";
                Hour22.Text = 00 + ":00";
                Hour23.Text = 01 + ":00";
                Hour24.Text = 02 + ":00";
            }
            if (CurrentHour == 4)
            {
                Hour1.Text = "Now";
                Hour2.Text = 5 + ":00";
                Hour3.Text = 6 + ":00";
                Hour4.Text = 7 + ":00";
                Hour5.Text = 8 + ":00";
                Hour6.Text = 9 + ":00";
                Hour7.Text = 10 + ":00";
                Hour8.Text = 11 + ":00";
                Hour9.Text = 12 + ":00";
                Hour10.Text = 13 + ":00";
                Hour11.Text = 14 + ":00";
                Hour12.Text = 15 + ":00";
                Hour13.Text = 16 + ":00";
                Hour14.Text = 17 + ":00";
                Hour15.Text = 18 + ":00";
                Hour16.Text = 19 + ":00";
                Hour17.Text = 20 + ":00";
                Hour18.Text = 21 + ":00";
                Hour19.Text = 22 + ":00";
                Hour20.Text = 23 + ":00";
                Hour21.Text = 00 + ":00";
                Hour22.Text = 01 + ":00";
                Hour23.Text = 02 + ":00";
                Hour24.Text = 03 + ":00";
            }
            if (CurrentHour == 5)
            {
                Hour1.Text = "Now";
                Hour2.Text = 6 + ":00";
                Hour3.Text = 7 + ":00";
                Hour4.Text = 8 + ":00";
                Hour5.Text = 9 + ":00";
                Hour6.Text = 10 + ":00";
                Hour7.Text = 11 + ":00";
                Hour8.Text = 12 + ":00";
                Hour9.Text = 13 + ":00";
                Hour10.Text = 14 + ":00";
                Hour11.Text = 15 + ":00";
                Hour12.Text = 16 + ":00";
                Hour13.Text = 17 + ":00";
                Hour14.Text = 18 + ":00";
                Hour15.Text = 19 + ":00";
                Hour16.Text = 20 + ":00";
                Hour17.Text = 21 + ":00";
                Hour18.Text = 22 + ":00";
                Hour19.Text = 23 + ":00";
                Hour20.Text = 00 + ":00";
                Hour21.Text = 01 + ":00";
                Hour22.Text = 02 + ":00";
                Hour23.Text = 03 + ":00";
                Hour24.Text = 04 + ":00";
            }
            if (CurrentHour == 6)
            {
                Hour1.Text = "Now";
                Hour2.Text = 7 + ":00";
                Hour3.Text = 8 + ":00";
                Hour4.Text = 9 + ":00";
                Hour5.Text = 10 + ":00";
                Hour6.Text = 11 + ":00";
                Hour7.Text = 12 + ":00";
                Hour8.Text = 13 + ":00";
                Hour9.Text = 14 + ":00";
                Hour10.Text = 15 + ":00";
                Hour11.Text = 16 + ":00";
                Hour12.Text = 17 + ":00";
                Hour13.Text = 18 + ":00";
                Hour14.Text = 19 + ":00";
                Hour15.Text = 20 + ":00";
                Hour16.Text = 21 + ":00";
                Hour17.Text = 22 + ":00";
                Hour18.Text = 23 + ":00";
                Hour19.Text = 00 + ":00";
                Hour20.Text = 01 + ":00";
                Hour21.Text = 02 + ":00";
                Hour22.Text = 03 + ":00";
                Hour23.Text = 04 + ":00";
                Hour24.Text = 05 + ":00";
            }
            if (CurrentHour == 7)
            {
                Hour1.Text = "Now";
                Hour2.Text = 8 + ":00";
                Hour3.Text = 9 + ":00";
                Hour4.Text = 10 + ":00";
                Hour5.Text = 11 + ":00";
                Hour6.Text = 12 + ":00";
                Hour7.Text = 13 + ":00";
                Hour8.Text = 14 + ":00";
                Hour9.Text = 15 + ":00";
                Hour10.Text = 16 + ":00";
                Hour11.Text = 17 + ":00";
                Hour12.Text = 18 + ":00";
                Hour13.Text = 19 + ":00";
                Hour14.Text = 20 + ":00";
                Hour15.Text = 21 + ":00";
                Hour16.Text = 22 + ":00";
                Hour17.Text = 23 + ":00";
                Hour18.Text = 00 + ":00";
                Hour19.Text = 01 + ":00";
                Hour20.Text = 02 + ":00";
                Hour21.Text = 03 + ":00";
                Hour22.Text = 04 + ":00";
                Hour23.Text = 05 + ":00";
                Hour24.Text = 06 + ":00";
            }
            if (CurrentHour == 8)
            {
                Hour1.Text = "Now";
                Hour2.Text = 9 + ":00";
                Hour3.Text = 10 + ":00";
                Hour4.Text = 11 + ":00";
                Hour5.Text = 12 + ":00";
                Hour6.Text = 13 + ":00";
                Hour7.Text = 14 + ":00";
                Hour8.Text = 15 + ":00";
                Hour9.Text = 16 + ":00";
                Hour10.Text = 17 + ":00";
                Hour11.Text = 18 + ":00";
                Hour12.Text = 19 + ":00";
                Hour13.Text = 20 + ":00";
                Hour14.Text = 21 + ":00";
                Hour15.Text = 22 + ":00";
                Hour16.Text = 23 + ":00";
                Hour17.Text = 00 + ":00";
                Hour18.Text = 01 + ":00";
                Hour19.Text = 02 + ":00";
                Hour20.Text = 03 + ":00";
                Hour21.Text = 04 + ":00";
                Hour22.Text = 05 + ":00";
                Hour23.Text = 06 + ":00";
                Hour24.Text = 07 + ":00";
            }
            if (CurrentHour == 9)
            {
                Hour1.Text = "Now";
                Hour2.Text = 10 + ":00";
                Hour3.Text = 11 + ":00";
                Hour4.Text = 12 + ":00";
                Hour5.Text = 13 + ":00";
                Hour6.Text = 14 + ":00";
                Hour7.Text = 15 + ":00";
                Hour8.Text = 16 + ":00";
                Hour9.Text = 17 + ":00";
                Hour10.Text = 18 + ":00";
                Hour11.Text = 19 + ":00";
                Hour12.Text = 20 + ":00";
                Hour13.Text = 21 + ":00";
                Hour14.Text = 22 + ":00";
                Hour15.Text = 23 + ":00";
                Hour16.Text = 00 + ":00";
                Hour17.Text = 01 + ":00";
                Hour18.Text = 02 + ":00";
                Hour19.Text = 03 + ":00";
                Hour20.Text = 04 + ":00";
                Hour21.Text = 05 + ":00";
                Hour22.Text = 06 + ":00";
                Hour23.Text = 07 + ":00";
                Hour24.Text = 08 + ":00";
            }
            if (CurrentHour == 10)
            {
                Hour1.Text = "Now";
                Hour2.Text = 11 + ":00";
                Hour3.Text = 12 + ":00";
                Hour4.Text = 13 + ":00";
                Hour5.Text = 14 + ":00";
                Hour6.Text = 15 + ":00";
                Hour7.Text = 16 + ":00";
                Hour8.Text = 17 + ":00";
                Hour9.Text = 18 + ":00";
                Hour10.Text = 19 + ":00";
                Hour11.Text = 20 + ":00";
                Hour12.Text = 21 + ":00";
                Hour13.Text = 22 + ":00";
                Hour14.Text = 23 + ":00";
                Hour15.Text = 00 + ":00";
                Hour16.Text = 01 + ":00";
                Hour17.Text = 02 + ":00";
                Hour18.Text = 03 + ":00";
                Hour19.Text = 04 + ":00";
                Hour20.Text = 05 + ":00";
                Hour21.Text = 06 + ":00";
                Hour22.Text = 07 + ":00";
                Hour23.Text = 08 + ":00";
                Hour24.Text = 09 + ":00";
            }
            if (CurrentHour == 11)
            {
                Hour1.Text = "Now";
                Hour2.Text = 12 + ":00";
                Hour3.Text = 13 + ":00";
                Hour4.Text = 14 + ":00";
                Hour5.Text = 15 + ":00";
                Hour6.Text = 16 + ":00";
                Hour7.Text = 17 + ":00";
                Hour8.Text = 18 + ":00";
                Hour9.Text = 19 + ":00";
                Hour10.Text = 20 + ":00";
                Hour11.Text = 21 + ":00";
                Hour12.Text = 22 + ":00";
                Hour13.Text = 23 + ":00";
                Hour14.Text = 00 + ":00";
                Hour15.Text = 01 + ":00";
                Hour16.Text = 02 + ":00";
                Hour17.Text = 03 + ":00";
                Hour18.Text = 04 + ":00";
                Hour19.Text = 05 + ":00";
                Hour20.Text = 06 + ":00";
                Hour21.Text = 07 + ":00";
                Hour22.Text = 08 + ":00";
                Hour23.Text = 09 + ":00";
                Hour24.Text = 10 + ":00";
            }
            if (CurrentHour == 12)
            {
                Hour1.Text = "Now";
                Hour2.Text = 13 + ":00";
                Hour3.Text = 14 + ":00";
                Hour4.Text = 15 + ":00";
                Hour5.Text = 16 + ":00";
                Hour6.Text = 17 + ":00";
                Hour7.Text = 18 + ":00";
                Hour8.Text = 19 + ":00";
                Hour9.Text = 20 + ":00";
                Hour10.Text = 21 + ":00";
                Hour11.Text = 22 + ":00";
                Hour12.Text = 23 + ":00";
                Hour13.Text = 00 + ":00";
                Hour14.Text = 01 + ":00";
                Hour15.Text = 02 + ":00";
                Hour16.Text = 03 + ":00";
                Hour17.Text = 04 + ":00";
                Hour18.Text = 05 + ":00";
                Hour19.Text = 06 + ":00";
                Hour20.Text = 07 + ":00";
                Hour21.Text = 08 + ":00";
                Hour22.Text = 09 + ":00";
                Hour23.Text = 10 + ":00";
                Hour24.Text = 11 + ":00";
            }
            if (CurrentHour == 13)
            {
                Hour1.Text = "Now";
                Hour2.Text = 14 + ":00";
                Hour3.Text = 15 + ":00";
                Hour4.Text = 16 + ":00";
                Hour5.Text = 17 + ":00";
                Hour6.Text = 18 + ":00";
                Hour7.Text = 19 + ":00";
                Hour8.Text = 20 + ":00";
                Hour9.Text = 21 + ":00";
                Hour10.Text = 22 + ":00";
                Hour11.Text = 23 + ":00";
                Hour12.Text = 00 + ":00";
                Hour13.Text = 01 + ":00";
                Hour14.Text = 02 + ":00";
                Hour15.Text = 03 + ":00";
                Hour16.Text = 04 + ":00";
                Hour17.Text = 05 + ":00";
                Hour18.Text = 06 + ":00";
                Hour19.Text = 07 + ":00";
                Hour20.Text = 08 + ":00";
                Hour21.Text = 09 + ":00";
                Hour22.Text = 10 + ":00";
                Hour23.Text = 11 + ":00";
                Hour24.Text = 12 + ":00";
            }
            if (CurrentHour == 14)
            {
                Hour1.Text = "Now";
                Hour2.Text = 15 + ":00";
                Hour3.Text = 16 + ":00";
                Hour4.Text = 17 + ":00";
                Hour5.Text = 18 + ":00";
                Hour6.Text = 19 + ":00";
                Hour7.Text = 20 + ":00";
                Hour8.Text = 21 + ":00";
                Hour9.Text = 22 + ":00";
                Hour10.Text = 23 + ":00";
                Hour11.Text = 00 + ":00";
                Hour12.Text = 01 + ":00";
                Hour13.Text = 02 + ":00";
                Hour14.Text = 03 + ":00";
                Hour15.Text = 04 + ":00";
                Hour16.Text = 05 + ":00";
                Hour17.Text = 06 + ":00";
                Hour18.Text = 07 + ":00";
                Hour19.Text = 08 + ":00";
                Hour20.Text = 09 + ":00";
                Hour21.Text = 10 + ":00";
                Hour22.Text = 11 + ":00";
                Hour23.Text = 12 + ":00";
                Hour24.Text = 13 + ":00";
            }
            if (CurrentHour == 15)
            {
                Hour1.Text = "Now";
                Hour2.Text = 16 + ":00";
                Hour3.Text = 17 + ":00";
                Hour4.Text = 18 + ":00";
                Hour5.Text = 19 + ":00";
                Hour6.Text = 20 + ":00";
                Hour7.Text = 21 + ":00";
                Hour8.Text = 22 + ":00";
                Hour9.Text = 23 + ":00";
                Hour10.Text = 00 + ":00";
                Hour11.Text = 01 + ":00";
                Hour12.Text = 02 + ":00";
                Hour13.Text = 03 + ":00";
                Hour14.Text = 04 + ":00";
                Hour15.Text = 05 + ":00";
                Hour16.Text = 06 + ":00";
                Hour17.Text = 07 + ":00";
                Hour18.Text = 08 + ":00";
                Hour19.Text = 09 + ":00";
                Hour20.Text = 10 + ":00";
                Hour21.Text = 11 + ":00";
                Hour22.Text = 12 + ":00";
                Hour23.Text = 13 + ":00";
                Hour24.Text = 14 + ":00";
            }
            if (CurrentHour == 16)
            {
                Hour1.Text = "Now";
                Hour2.Text = 17 + ":00";
                Hour3.Text = 18 + ":00";
                Hour4.Text = 19 + ":00";
                Hour5.Text = 20 + ":00";
                Hour6.Text = 21 + ":00";
                Hour7.Text = 22 + ":00";
                Hour8.Text = 23 + ":00";
                Hour9.Text = 00 + ":00";
                Hour10.Text = 01 + ":00";
                Hour11.Text = 02 + ":00";
                Hour12.Text = 03 + ":00";
                Hour13.Text = 04 + ":00";
                Hour14.Text = 05 + ":00";
                Hour15.Text = 06 + ":00";
                Hour16.Text = 07 + ":00";
                Hour17.Text = 08 + ":00";
                Hour18.Text = 09 + ":00";
                Hour19.Text = 10 + ":00";
                Hour20.Text = 11 + ":00";
                Hour21.Text = 12 + ":00";
                Hour22.Text = 13 + ":00";
                Hour23.Text = 14 + ":00";
                Hour24.Text = 15 + ":00";
            }
            if (CurrentHour == 17)
            {
                Hour1.Text = "Now";
                Hour2.Text = 18 + ":00";
                Hour3.Text = 19 + ":00";
                Hour4.Text = 20 + ":00";
                Hour5.Text = 21 + ":00";
                Hour6.Text = 22 + ":00";
                Hour7.Text = 23 + ":00";
                Hour8.Text = 00 + ":00";
                Hour9.Text = 01 + ":00";
                Hour10.Text = 02 + ":00";
                Hour11.Text = 03 + ":00";
                Hour12.Text = 04 + ":00";
                Hour13.Text = 05 + ":00";
                Hour14.Text = 06 + ":00";
                Hour15.Text = 07 + ":00";
                Hour16.Text = 08 + ":00";
                Hour17.Text = 09 + ":00";
                Hour18.Text = 10 + ":00";
                Hour19.Text = 11 + ":00";
                Hour20.Text = 12 + ":00";
                Hour21.Text = 13 + ":00";
                Hour22.Text = 14 + ":00";
                Hour23.Text = 15 + ":00";
                Hour24.Text = 16 + ":00";
            }
            if (CurrentHour == 18)
            {
                Hour1.Text = "Now";
                Hour2.Text = 19 + ":00";
                Hour3.Text = 20 + ":00";
                Hour4.Text = 21 + ":00";
                Hour5.Text = 22 + ":00";
                Hour6.Text = 23 + ":00";
                Hour7.Text = 00 + ":00";
                Hour8.Text = 01 + ":00";
                Hour9.Text = 02 + ":00";
                Hour10.Text = 03 + ":00";
                Hour11.Text = 04 + ":00";
                Hour12.Text = 05 + ":00";
                Hour13.Text = 06 + ":00";
                Hour14.Text = 07 + ":00";
                Hour15.Text = 08 + ":00";
                Hour16.Text = 09 + ":00";
                Hour17.Text = 10 + ":00";
                Hour18.Text = 11 + ":00";
                Hour19.Text = 12 + ":00";
                Hour20.Text = 13 + ":00";
                Hour21.Text = 14 + ":00";
                Hour22.Text = 15 + ":00";
                Hour23.Text = 16 + ":00";
                Hour24.Text = 17 + ":00";
            }
            if (CurrentHour == 19)
            {
                Hour1.Text = "Now";
                Hour2.Text = 20 + ":00";
                Hour3.Text = 21 + ":00";
                Hour4.Text = 22 + ":00";
                Hour5.Text = 23 + ":00";
                Hour6.Text = 00 + ":00";
                Hour7.Text = 01 + ":00";
                Hour8.Text = 02 + ":00";
                Hour9.Text = 03 + ":00";
                Hour10.Text = 04 + ":00";
                Hour11.Text = 05 + ":00";
                Hour12.Text = 06 + ":00";
                Hour13.Text = 07 + ":00";
                Hour14.Text = 08 + ":00";
                Hour15.Text = 09 + ":00";
                Hour16.Text = 10 + ":00";
                Hour17.Text = 11 + ":00";
                Hour18.Text = 12 + ":00";
                Hour19.Text = 13 + ":00";
                Hour20.Text = 14 + ":00";
                Hour21.Text = 15 + ":00";
                Hour22.Text = 16 + ":00";
                Hour23.Text = 17 + ":00";
                Hour24.Text = 18 + ":00";
            }
            if (CurrentHour == 20)
            {
                Hour1.Text = "Now";
                Hour2.Text = 21 + ":00";
                Hour3.Text = 22 + ":00";
                Hour4.Text = 23 + ":00";
                Hour5.Text = 00 + ":00";
                Hour6.Text = 01 + ":00";
                Hour7.Text = 02 + ":00";
                Hour8.Text = 03 + ":00";
                Hour9.Text = 04 + ":00";
                Hour10.Text = 05 + ":00";
                Hour11.Text = 06 + ":00";
                Hour12.Text = 07 + ":00";
                Hour13.Text = 08 + ":00";
                Hour14.Text = 09 + ":00";
                Hour15.Text = 10 + ":00";
                Hour16.Text = 11 + ":00";
                Hour17.Text = 12 + ":00";
                Hour18.Text = 13 + ":00";
                Hour19.Text = 14 + ":00";
                Hour20.Text = 15 + ":00";
                Hour21.Text = 16 + ":00";
                Hour22.Text = 17 + ":00";
                Hour23.Text = 18 + ":00";
                Hour24.Text = 19 + ":00";
            }
            if (CurrentHour == 21)
            {
                Hour1.Text = "Now";
                Hour2.Text = 22 + ":00";
                Hour3.Text = 23 + ":00";
                Hour4.Text = 00 + ":00";
                Hour5.Text = 01 + ":00";
                Hour6.Text = 02 + ":00";
                Hour7.Text = 03 + ":00";
                Hour8.Text = 04 + ":00";
                Hour9.Text = 05 + ":00";
                Hour10.Text = 06 + ":00";
                Hour11.Text = 07 + ":00";
                Hour12.Text = 08 + ":00";
                Hour13.Text = 09 + ":00";
                Hour14.Text = 10 + ":00";
                Hour15.Text = 11 + ":00";
                Hour16.Text = 12 + ":00";
                Hour17.Text = 13 + ":00";
                Hour18.Text = 14 + ":00";
                Hour19.Text = 15 + ":00";
                Hour20.Text = 16 + ":00";
                Hour21.Text = 17 + ":00";
                Hour22.Text = 18 + ":00";
                Hour23.Text = 19 + ":00";
                Hour24.Text = 20 + ":00";
            }
            if (CurrentHour == 22)
            {
                Hour1.Text = "Now";
                Hour2.Text = 23 + ":00";
                Hour3.Text = 00 + ":00";
                Hour4.Text = 01 + ":00";
                Hour5.Text = 02 + ":00";
                Hour6.Text = 03 + ":00";
                Hour7.Text = 04 + ":00";
                Hour8.Text = 05 + ":00";
                Hour9.Text = 06 + ":00";
                Hour10.Text = 07 + ":00";
                Hour11.Text = 08 + ":00";
                Hour12.Text = 09 + ":00";
                Hour13.Text = 10 + ":00";
                Hour14.Text = 11 + ":00";
                Hour15.Text = 12 + ":00";
                Hour16.Text = 13 + ":00";
                Hour17.Text = 14 + ":00";
                Hour18.Text = 15 + ":00";
                Hour19.Text = 16 + ":00";
                Hour20.Text = 17 + ":00";
                Hour21.Text = 18 + ":00";
                Hour22.Text = 19 + ":00";
                Hour23.Text = 20 + ":00";
                Hour24.Text = 21 + ":00";
            }
            if (CurrentHour == 23)
            {
                Hour1.Text = 23 + ":00";
                Hour2.Text = 0 + ":00";
                Hour3.Text = 1 + ":00";
                Hour4.Text = 2 + ":00";
                Hour5.Text = 3 + ":00";
                Hour6.Text = 4 + ":00";
                Hour7.Text = 5 + ":00";
                Hour8.Text = 6 + ":00";
                Hour9.Text = 7 + ":00";
                Hour10.Text = 8 + ":00";
                Hour11.Text = 9 + ":00";
                Hour12.Text = 10 + ":00";
                Hour13.Text = 11 + ":00";
                Hour14.Text = 12 + ":00";
                Hour15.Text = 13 + ":00";
                Hour16.Text = 14 + ":00";
                Hour17.Text = 15 + ":00";
                Hour18.Text = 16 + ":00";
                Hour19.Text = 17 + ":00";
                Hour20.Text = 18 + ":00";
                Hour21.Text = 19 + ":00";
                Hour22.Text = 20 + ":00";
                Hour23.Text = 21 + ":00";
                Hour24.Text = 22 + ":00";
            }
        }
    }
}
