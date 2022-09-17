using ISN_Forecast.Win7.FirstSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ISN_Forecast.Win7.API
{
    internal class SearchBasedLookup
    {
        public static class GlobalStrings
        {
            public static int ID;
        }
        public static void Start(int ID)
        {
            GlobalStrings.ID = ID;
            Search.Instance.QueryResult.Content = new SearchedLocation();
            var SearchData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.Search);
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(new Uri("https://api.weatherapi.com/v1/forecast.json?key=b48046722eb448daafa173827211511&q=" + SearchData[ID]["lat"] + ", " + SearchData[ID]["lon"] + "&days=10&aqi=yes&alerts=yes"));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WeatherData);
        }

        private static void WeatherData(object sender, DownloadStringCompletedEventArgs e)
        {
            DownloadCondition();
            var Data = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result);
            SearchedLocation.Instance.City.Text = Data["location"]["name"];
            SearchedLocation.Instance.Temperature.Text = Data["current"][Configs.Unit];
            SearchedLocation.Instance.Updated.Text = "Updated as of " + Data["current"]["last_updated"];

            SearchedLocation.Instance.Warnings.Opacity = 1;
            SearchedLocation.Instance.Header.Opacity = 1;
            SearchedLocation.Instance.ProcessRing.IsActive = false;


        }

        public static void DownloadCondition() //Sets color scheme depending on the time & weather
        {
            var ID = GlobalStrings.ID;
            var SearchData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.Search);

            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(new Uri("https://api.openweathermap.org/data/2.5/forecast?lat=" + SearchData[ID]["lat"] + "&lon=" + SearchData[ID]["lon"] + "&appid=3a527f6b49287d153185316c83efdc9a"));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SetColors);
        }

        public async static void SetColors(object sender, DownloadStringCompletedEventArgs e) //Sets color scheme depending on the time & weather
        {
            try
            {
                var Data = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result); //Data from OpenWeather
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations); //Translations
                var ForkieData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Meta); //Metadata where all conditions are stored

                var SearchData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.Search);

                var Category = Data["list"][0]["weather"][0]["id"].ToString();
                var Detailed = Data["list"][0]["weather"][0]["icon"].ToString();

                var Indentification = ForkieData["Conditions"][Category]["ID"].ToString();
                SearchedLocation.Instance.Condition.Text = Lang["Condition"][Indentification];

                SearchedLocation.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString(ForkieData["Conditions"][Category][Detailed]["Top"].ToString());
                SearchedLocation.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString(ForkieData["Conditions"][Category][Detailed]["Bottom"].ToString());

                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                webClient.DownloadStringAsync(new Uri("https://api.openweathermap.org/data/2.5/onecall?lat=" + SearchData[GlobalStrings.ID]["lat"] + "&lon=" + SearchData[GlobalStrings.ID]["lon"] + "&exclude={part}&appid=3a527f6b49287d153185316c83efdc9a"));
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Forkie);
            }
            catch(Exception)
            {
                var Data = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result); //Data from OpenWeather
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations); //Translations
                var ForkieData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Meta); //Metadata where all conditions are stored

                var Category = Data["list"][0]["weather"][0]["id"].ToString();
                var Detailed = Data["list"][0]["weather"][0]["icon"].ToString();

                MessageBox.Show("Category: " + Category + ", Detailed: " + Detailed);
            }
            
        }

        public static void Forkie(object sender, DownloadStringCompletedEventArgs e)
        {
            var WeatherData2 = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result);
            #region Emergency Alerts

            if (WeatherData2.alerts == null)
            {
                SearchedLocation.Instance.Warnings.Opacity = 0;
                SearchedLocation.Instance.Warnings.Margin = new Thickness(0, -420, 0, 0);
                SearchedLocation.Instance.Header.Margin = new Thickness(0, 50, 0, 0);
                return;
            }

            if (WeatherData2.alerts.Count == 1)
            {
                SearchedLocation.Instance.Source.Text = WeatherData2["alerts"][0]["sender_name"];
                SearchedLocation.Instance.Headlines.Text = WeatherData2["alerts"][0]["event"];
            }
            if (WeatherData2.alerts.Count > 1)
            {
                SearchedLocation.Instance.Source.Text = WeatherData2["alerts"][0]["sender_name"];
                SearchedLocation.Instance.Headlines.Text = "Currently " + WeatherData2.alerts.Count + " ongoing warnings";
            }

            #endregion
        }
    }
}
