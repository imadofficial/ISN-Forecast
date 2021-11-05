using Newtonsoft.Json;
using System;
using Windows.Devices.Geolocation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace TestProject
{
    /// <summary>
    /// 샘플 페이지 1
    /// </summary>
    /// 
    public sealed partial class SamplePage1 : Page
    {

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - SamplePage1()

        /// <summary>
        /// 생성자
        /// </summary>
        public SamplePage1()
        {
            InitializeComponent();
            Engine();
        }

        public async void Engine()
        {
            var APIKey = "INSERT_KEY_HERE";

            //Get coordinates
            var geoLocator = new Geolocator();
            geoLocator.DesiredAccuracy = PositionAccuracy.High;
            var pos = await geoLocator.GetGeopositionAsync();
            string latitude = pos.Coordinate.Point.Position.Latitude.ToString();
            string longitude = pos.Coordinate.Point.Position.Longitude.ToString();


            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString("https://api.weatherapi.com/v1/current.json" + "?key=" + APIKey + "&q=" + latitude + "," + longitude);
                //Now parse with JSON.Net
                var Forecast = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(json);


                string temperature = Forecast["current"]["temp_c"].ToString();
                string city = Forecast["location"]["name"].ToString();
                string country = Forecast["location"]["country"].ToString();

                Temperature.Text = temperature + "°C";
                Coordinates.Text = city + ", " + country;
                Coordinates.Opacity = 1;
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 연결 애니메이션 준비하기 - PrepareConnectedAnimation(configuration)

        /// <summary>
        /// 연결 애니메이션 준비하기
        /// </summary>
        /// <param name="configuration">연결 애니메이션 구성</param>
        public void PrepareConnectedAnimation(ConnectedAnimationConfiguration configuration)
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", this.sourceGrid);

            if (configuration != null && ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                animation.Configuration = configuration;
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Protected

        #region 탐색되는 경우 처리하기 - OnNavigatedTo(e)

        /// <summary>
        /// 탐색되는 경우 처리하기
        /// </summary>
        /// <param name="e">이벤트 인자</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardAnimation");

            if (animation != null)
            {
                animation.TryStart(this.sourceGrid);
            }
        }

        #endregion
    }

    public class Location
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("tz_id")]
        public string TzId { get; set; }

        [JsonProperty("localtime_epoch")]
        public int LocaltimeEpoch { get; set; }

        [JsonProperty("localtime")]
        public string Localtime { get; set; }
    }

    public class Condition
    {

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

    public class Current
    {

        [JsonProperty("last_updated_epoch")]
        public int LastUpdatedEpoch { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

        [JsonProperty("temp_c")]
        public double TempC { get; set; }

        [JsonProperty("temp_f")]
        public double TempF { get; set; }

        [JsonProperty("is_day")]
        public int IsDay { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("wind_mph")]
        public double WindMph { get; set; }

        [JsonProperty("wind_kph")]
        public double WindKph { get; set; }

        [JsonProperty("wind_degree")]
        public int WindDegree { get; set; }

        [JsonProperty("wind_dir")]
        public string WindDir { get; set; }

        [JsonProperty("pressure_mb")]
        public double PressureMb { get; set; }

        [JsonProperty("pressure_in")]
        public double PressureIn { get; set; }

        [JsonProperty("precip_mm")]
        public double PrecipMm { get; set; }

        [JsonProperty("precip_in")]
        public double PrecipIn { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("cloud")]
        public int Cloud { get; set; }

        [JsonProperty("feelslike_c")]
        public double FeelslikeC { get; set; }

        [JsonProperty("feelslike_f")]
        public double FeelslikeF { get; set; }

        [JsonProperty("vis_km")]
        public double VisKm { get; set; }

        [JsonProperty("vis_miles")]
        public double VisMiles { get; set; }

        [JsonProperty("uv")]
        public double Uv { get; set; }

        [JsonProperty("gust_mph")]
        public double GustMph { get; set; }

        [JsonProperty("gust_kph")]
        public double GustKph { get; set; }
    }

    public class Example
    {

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("current")]
        public Current Current { get; set; }
    }

}