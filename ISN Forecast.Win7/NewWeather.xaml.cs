using ISN_Forecast.Win7.FirstSetup;
using ISN_Forecast.Win7.WeatherComponents;
using ISN_Forecast.Win7.API;
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
using Newtonsoft.Json;
using System.Device.Location;
using System.Threading;
using System.Windows.Media.Animation;
using System.Net.Http;

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for NewWeather.xaml
    /// </summary>
    public partial class NewWeather : Page
    {
        GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        public static NewWeather Instance;
        public static class GlobalStrings
        {
            public static String IP;
            public static String WeatherData;
            public static String City;
            public static String Country;
            public static int Appearance;

            public static String AQI;
            public static String UV;
        }

        public NewWeather()
        {
            AppBehavior.Hamburger = "True";
            InitializeComponent();
            MainWindow.Instance.MadeBy.Opacity = 1;

            Instance = this;
            Init();
        }

        public async void Init()
        {
            DoubleAnimation Fade2 = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
            };

            Warnings.Opacity = 0;
            Header.Opacity = 0;
            BottomInfo.Opacity = 0;
            MainWindow.Instance.ProcessRing.IsActive = true;

            string Settings = File.ReadAllText("Assets/Settings.json");
            var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            if(SettingsJSON[0]["Appearance"] == 2)
            {
                String Code = "#000000";
                var converter = new System.Windows.Media.BrushConverter();
                Statusbar.Instance.Status.Foreground = (Brush)converter.ConvertFromString(Code);
                City.Foreground = (Brush)converter.ConvertFromString(Code);
                Temperature.Foreground = (Brush)converter.ConvertFromString(Code);
                Type.Foreground = (Brush)converter.ConvertFromString(Code);
                ConditionText.Foreground = (Brush)converter.ConvertFromString(Code);
                Updated.Foreground = (Brush)converter.ConvertFromString(Code);
                Statusbar.Instance.Time.Foreground = (Brush)converter.ConvertFromString(Code);
                Statusbar.Instance.Date.Foreground = (Brush)converter.ConvertFromString(Code);

                Statusbar.Instance.Cities.Background = (Brush)converter.ConvertFromString(Code);
                Statusbar.Instance.Settings.Background = (Brush)converter.ConvertFromString(Code);
                Statusbar.Instance.Globe.Background = (Brush)converter.ConvertFromString(Code);
                Statusbar.Instance.Search.Background = (Brush)converter.ConvertFromString(Code);
            }

            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            Statusbar.Instance.Status.Text = Lang["Weather"]["Prep"];
            DayForecast.Text = Lang["Weather"]["24HrForecast"];
            Astronomy.Text = Lang["Weather"]["Astronomy"];
            SunriseTitle.Text = Lang["Weather"]["Sunrise"];
            SunsetTitle.Text = Lang["Weather"]["Sunset"];
            UVTitle.Text = Lang["Weather"]["UV-Index"];
            AQITitle.Text = Lang["Weather"]["AQI"];
            FutureTitle.Text = Lang["Weather"]["5DayForecast"];
            Rainfall.Text = Lang["Weather"]["Rainfall"];
            RainfallContext.Text = Lang["Weather"]["Past24"];
            WindDir.Text = Lang["Weather"]["Wind"];
            WindTitle.Text = Lang["Weather"]["WindSpeed"];
            WindDirTitle.Text = Lang["Weather"]["WindDirection"];

            await Task.Delay(1500);

            MainWindow.Instance.MadeBy.BeginAnimation(StackPanel.OpacityProperty, Fade2);

            await Task.Delay(1500);

            GlobalStrings.Appearance = Int32.Parse(SettingsJSON[0]["Appearance"].ToString()); //converts value to string

            ApplyColorscheme.Get(Int32.Parse(SettingsJSON[0]["Appearance"].ToString()), 0); //Sets color scheme

            if(SettingsJSON[0]["GPS"].ToString() == "True")
            {
                LocateGPS();
            }
            if (SettingsJSON[0]["GPS"].ToString() == "False")
            {
                NoGPS();
            }


            BackgroundAni();
        }

        public void BackgroundAni() 
        {
            if(GlobalStrings.Appearance == 0) //Gradient
            {
                ColorAnimation Gradient1 = new ColorAnimation()
                {
                    To = (Color)ColorConverter.ConvertFromString("#0D52AA"),
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                ColorAnimation Gradient2 = new ColorAnimation()
                {
                    To = (Color)ColorConverter.ConvertFromString("#7CC0E8"),
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                MainWindow.Instance.GradientTop.BeginAnimation(GradientStop.ColorProperty, Gradient1);
                MainWindow.Instance.GradientBottom.BeginAnimation(GradientStop.ColorProperty, Gradient2);
            }
            if (GlobalStrings.Appearance == 1) //Dark
            {
                ColorAnimation Gradient = new ColorAnimation()
                {
                    To = (Color)ColorConverter.ConvertFromString("#000000"),
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                MainWindow.Instance.GradientTop.BeginAnimation(GradientStop.ColorProperty, Gradient);
                MainWindow.Instance.GradientBottom.BeginAnimation(GradientStop.ColorProperty, Gradient);
            }
            if (GlobalStrings.Appearance == 2) //Bright
            {
                ColorAnimation Gradient = new ColorAnimation()
                {
                    To = (Color)ColorConverter.ConvertFromString("#FFFFFF"),
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                MainWindow.Instance.GradientTop.BeginAnimation(GradientStop.ColorProperty, Gradient);
                MainWindow.Instance.GradientBottom.BeginAnimation(GradientStop.ColorProperty, Gradient);
            }

        }

        public void LocateGPS()
        {
            watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
            Thread.Sleep(2000);
            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                String a = coord.Latitude.ToString();
                String b = coord.Longitude.ToString();

                string newStr1 = a.Replace(",", ".");
                string newStr2 = b.Replace(",", ".");

                AppBehavior.Latitude = newStr1;
                AppBehavior.Longitude = newStr2;
                WeatherSearch();
            }
            else
            {
                NoGPS();
            }
        }

        public void NoGPS() //Method that runs before the actual lookup
        {
            // best practice to create one HttpClient per Application and inject it
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ip-api.com/json");
            httpWebRequest.Method = "GET";

            using (WebResponse response = httpWebRequest.GetResponse())
            {
                HttpWebResponse httpResponse = response as HttpWebResponse;
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var IPConfiguration = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(reader.ReadToEnd());

                    AppBehavior.Latitude = IPConfiguration["lat"];
                    AppBehavior.Longitude = IPConfiguration["lon"];
                }
            }

            WeatherSearch();
        }

        public void WeatherSearch()
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            string Settings = File.ReadAllText("Assets/Settings.json");
            var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.weatherapi.com/v1/forecast.json?key=PASTEKEYHERE" + "&q=" + AppBehavior.Latitude + ", " + AppBehavior.Longitude + "&days=10&aqi=yes&alerts=yes&lang=" + Lang["Weather"]["lang"]);
            httpWebRequest.Method = "GET";

            using (WebResponse response = httpWebRequest.GetResponse())
            {
                HttpWebResponse httpResponse = response as HttpWebResponse;
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Weather.FullText = reader.ReadToEnd();
                    var Forecast = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.FullText);

                    //Here it sets all the values inside the textboxes
                    Sunrise.Text = Forecast["forecast"]["forecastday"][0]["astro"]["sunrise"];
                    Sunset.Text = Forecast["forecast"]["forecastday"][0]["astro"]["sunset"];
                    UValue.Text = Forecast["current"]["uv"];
                    Updated.Text = Forecast["current"]["last_updated"];
                    ConditionText.Text = Forecast["current"]["condition"]["text"];
                    City.Text = Forecast["location"]["name"];
                    GlobalStrings.AQI = Forecast["current"]["air_quality"]["gb-defra-index"];
                    GlobalStrings.UV = Forecast["current"]["uv"];
                    AQIValue.Text = Forecast["current"]["air_quality"]["gb-defra-index"];

                    if (SettingsJSON[0]["Unit"] == "temp_c")
                    {
                        Temperature.Text = Forecast["current"]["temp_c"];
                        Type.Text = "°C";
                    }
                    if (SettingsJSON[0]["Unit"] == "temp_f")
                    {
                        Temperature.Text = Forecast["current"]["temp_f"];
                        Type.Text = "°F";
                    }


                    if(SettingsJSON[0]["Percipitation"] == "precip_mm")
                    {
                        RainfallValue.Text = Forecast["current"]["precip_mm"] + " mm";
                    }
                    if (SettingsJSON[0]["Percipitation"] == "precip_in")
                    {
                        RainfallValue.Text = Forecast["current"]["precip_in"] + " ″";
                    }


                    if (SettingsJSON[0]["Speed"] == "kmh")
                    {
                        WindValue.Text = Forecast["current"]["wind_kph"] + " km/h";
                    }
                    if (SettingsJSON[0]["Speed"] == "mph")
                    {
                        WindValue.Text = Forecast["current"]["wind_mph"] + " mph";
                    }
                    WindDirValue.Text = Forecast["current"]["wind_degree"] + "°";
                }
            }

            Processing();
        }

        public void Processing()
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            string Settings = File.ReadAllText("Assets/Settings.json");
            var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

            AQIStandard.Text = Lang["AQI"][GlobalStrings.AQI + "T"];
            AQIInfo.Text = Lang["AQI"][GlobalStrings.AQI];

            UVStandard.Text = Lang["UVIndex"][GlobalStrings.UV + "T"]; 
            UVInfo.Text = Lang["UVIndex"][GlobalStrings.UV];

            Warnings.Opacity = 1;
            Header.Opacity = 1;
            BottomInfo.Opacity = 1;
            MainWindow.Instance.ProcessRing.IsActive = false;

            Statusbar.Instance.Status.Text = Lang["Weather"]["CurrentWeather"];

            #region 3-Day Forecast
            var Forecast = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.FullText);
            Day1TempLow.Text = Forecast["forecast"]["forecastday"][0]["day"]["min"+ SettingsJSON[0]["Unit"]] + "°";
            Day1TempHigh.Text = Forecast["forecast"]["forecastday"][0]["day"]["max" + SettingsJSON[0]["Unit"]] + "°";
            Condition1.Text = Forecast["forecast"]["forecastday"][0]["day"]["condition"]["text"];

            Day2TempLow.Text = Forecast["forecast"]["forecastday"][1]["day"]["min" + SettingsJSON[0]["Unit"]] + "°";
            Day2TempHigh.Text = Forecast["forecast"]["forecastday"][1]["day"]["max" + SettingsJSON[0]["Unit"]] + "°";
            Condition2.Text = Forecast["forecast"]["forecastday"][1]["day"]["condition"]["text"];

            Day3TempLow.Text = Forecast["forecast"]["forecastday"][2]["day"]["min" + SettingsJSON[0]["Unit"]] + "°";
            Day3TempHigh.Text = Forecast["forecast"]["forecastday"][2]["day"]["max" + SettingsJSON[0]["Unit"]] + "°";
            Condition3.Text = Forecast["forecast"]["forecastday"][2]["day"]["condition"]["text"];
            #endregion
        }

    }
}
