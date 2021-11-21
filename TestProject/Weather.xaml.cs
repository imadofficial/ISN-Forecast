using Newtonsoft.Json;
using System;
using Windows.Devices.Geolocation;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Linq;
using Windows.UI.Xaml.Media;
using System.Drawing;
using Windows.UI.Xaml.Data;
using Windows.UI;

namespace TestProject
{
    /// <summary>
    /// 샘플 페이지 1
    /// </summary>
    /// 
    public sealed partial class SamplePage1 : Page
    {
        public SamplePage1()
        {
            ApplicationView.GetForCurrentView().Title = "Current weather";
            InitializeComponent();
            Engine();
        }

        public async void Engine()
        {
            try
            {
                //Get coordinates
                var geoLocator = new Geolocator();
                geoLocator.DesiredAccuracy = PositionAccuracy.High;
                var pos = await geoLocator.GetGeopositionAsync();
                string latitude = pos.Coordinate.Point.Position.Latitude.ToString();
                string longitude = pos.Coordinate.Point.Position.Longitude.ToString();


                using (var webClient = new System.Net.WebClient())
                {
                    var globaltemps = webClient.DownloadString("https://forecast.imadsnetwork.net/api/forecast/" + "?API=global&q=" + latitude + "," + longitude);
                    var Forecast = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(globaltemps);

                    //Parsing Weather data
                    string temperature = Forecast["current"]["temp_c"].ToString();
                    string city = Forecast["location"]["name"].ToString();
                    string country = Forecast["location"]["country"].ToString();
                    string condition = Forecast["current"]["condition"]["text"].ToString();

                    LoadingCircle.IsActive = false;

                    Temperature.Text = temperature + "°C";
                    Coordinates.Text = city + ", " + country;
                    Coordinates.Opacity = 1;
                    Temperature.Opacity = 1;
                    Condition.Text = condition;
                    Condition.Opacity = 1;

                    try
                    {
                        
                        var globalAQI = webClient.DownloadString("https://api.ambeedata.com/latest/by-lat-lng?lat=" + latitude + "&lng=" + longitude);
                        var AQI = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(globalAQI);

                        string aqi = AQI["stations"][0]["AQI"].ToString();
                        AQI_Value.Text = aqi;
                        //Converting string to Int
                        int AqiInt = Int32.Parse(aqi);

                        LoadingCircle.IsActive = false;

                        if (Enumerable.Range(1, 50).Contains(AqiInt))
                        {
                            AQIColor.Fill = new SolidColorBrush(Colors.Green);

                            AQIColor.Opacity = 1;
                            AQITXT.Opacity = 1;
                            AQI_Value.Opacity = 1;
                        }
                        if (Enumerable.Range(51, 100).Contains(AqiInt))
                        {
                            AQIColor.Fill = new SolidColorBrush(Colors.Yellow);
                            AQITXT.Foreground = new SolidColorBrush(Colors.Black);
                            AQI_Value.Foreground = new SolidColorBrush(Colors.Black);

                            AQIColor.Opacity = 1;
                            AQITXT.Opacity = 1;
                            AQI_Value.Opacity = 1;
                        }
                        if (Enumerable.Range(101, 150).Contains(AqiInt))
                        {
                            AQIColor.Fill = new SolidColorBrush(Colors.Orange);

                            AQIColor.Opacity = 1;
                            AQITXT.Opacity = 1;
                            AQI_Value.Opacity = 1;
                        }
                        if (Enumerable.Range(151, 200).Contains(AqiInt))
                        {
                            AQIColor.Fill = new SolidColorBrush(Colors.Red);

                            AQIColor.Opacity = 1;
                            AQITXT.Opacity = 1;
                            AQI_Value.Opacity = 1;
                        }

                        if (Enumerable.Range(201, 300).Contains(AqiInt))
                        {
                            AQIColor.Fill = new SolidColorBrush(Colors.MediumPurple);

                            AQIColor.Opacity = 1;
                            AQITXT.Opacity = 1;
                            AQI_Value.Opacity = 1;
                        }
                        if (Enumerable.Range(301, 999).Contains(AqiInt))
                        {
                            AQIColor.Opacity = 1;
                            AQITXT.Opacity = 1;
                            AQI_Value.Opacity = 1;
                        }

                        if (country == "China")
                        {
                            var China = webClient.DownloadString("https://forecast.imadsnetwork.net/api/forecast/" + "?API=global&q=" + latitude + "," + longitude);
                        }

                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
                var dialog = new MessageDialog("Location services are not enabled.");
                await dialog.ShowAsync();
            }
        }

        public static DateTime CalculateUpdated(double LastUpdated)
        {
            System.DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(LastUpdated).ToLocalTime();
            return dt;
        }
        public void PrepareConnectedAnimation(ConnectedAnimationConfiguration configuration)
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", this.sourceGrid);

            if (configuration != null && ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                animation.Configuration = configuration;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardAnimation");

            if (animation != null)
            {
                animation.TryStart(this.sourceGrid);
            }
        }
    }
}
