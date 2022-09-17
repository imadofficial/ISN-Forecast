using ISN_Forecast.Win7.FirstSetup;
using ISN_Forecast.Win7.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Net.Http;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.IO;

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        public static Search Instance;

        public static class GlobalStrings
        {
            public static String Region;
            public static String Temperature;
            public static String Condition;
            public static String ReusultID;
            public static String LocalTime;
        }

        public Search()
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            InitializeComponent();

            Title1.Text = Lang["Search"]["NothingHere"];
            Title2.Text = Lang["Search"]["NothingInstruct"];
            Ani();
            Instance = this;
        }

        public async void Ani()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseInOut;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Fade = new DoubleAnimation()
            {
                From = 0,
                To = 0.5,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation Fade2 = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
                //EasingFunction = c
            };

            DoubleAnimation Fade3 = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
                //EasingFunction = c
            };

            ThicknessAnimation TextAni = new ThicknessAnimation()
            {
                From = new Thickness(0, 20, 0, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ForkieBG.BeginAnimation(Border.OpacityProperty, Fade);
            Title1.BeginAnimation(TextBlock.OpacityProperty, Fade3);

            Title1.BeginAnimation(TextBlock.MarginProperty, TextAni);
            Title2.BeginAnimation(TextBlock.OpacityProperty, Fade2);
            Title2.BeginAnimation(TextBlock.MarginProperty, TextAni);
        }

        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            Statusbar.Instance.Status.Text = Lang["Weather"]["CurrentWeather"];
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseInOut;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Fade = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = c
            };

            DoubleAnimation Fade2 = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                //EasingFunction = c
            };

            DoubleAnimation Fade3 = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                //EasingFunction = c
            };

            DoubleAnimation Gay = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
                //EasingFunction = c
            };

            ForkieBG.BeginAnimation(Border.OpacityProperty, Fade);
            Title1.BeginAnimation(TextBlock.OpacityProperty, Fade3);
            Title2.BeginAnimation(TextBlock.OpacityProperty, Fade2);
            Close.BeginAnimation(TextBlock.OpacityProperty, Fade2);
            Query.BeginAnimation(TextBlock.OpacityProperty, Fade2);
            DisplayResults.BeginAnimation(TextBlock.OpacityProperty, Fade2);
            NewWeather.Instance.Effect.BeginAnimation(BlurEffect.RadiusProperty, Gay);
            await Task.Delay(500);


            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, -10240, 0, 0);
            NewWeather.Instance.Effect = null;
            NewWeather.Instance.ScrollPerms.IsEnabled = true;

            //Statusbar.Instance.Settings.IsEnabled = true; Statusbar.Instance.Settings.Opacity = 1;
            Statusbar.Instance.Globe.IsEnabled = true; Statusbar.Instance.Globe.Opacity = 1;


            BackgroundManager.Children.Remove(ForkieBG);
        }

        private void Query_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            DisplayResults.Children.Clear();

            if (Query.Text.Length == 2)
            {
                Title1.Opacity = 1;
                Title2.Opacity = 1;
                Title1.Text = Lang["Search"]["InsuffChar"];
                Title2.Text = Lang["Search"]["Type1More"];
            }
            if (Query.Text.Length == 1)
            {
                Title1.Opacity = 1;
                Title2.Opacity = 1;
                Title1.Text = Lang["Search"]["InsuffChar"];
                Title2.Text = Lang["Search"]["Type2More"];
            }
            if (Query.Text.Length == 0)
            {
                Title1.Opacity = 1;
                Title2.Opacity = 1;
                Title1.Text = Lang["Search"]["NothingHere"];
                Title2.Text = Lang["Search"]["NothingInstruct"];
            }
            if (Query.Text.Length > 2)
            {
                
                try
                {
                    NoChars.Opacity = 0;
                    ProcessRing.IsActive = true;
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringAsync(new Uri("https://api.weatherapi.com/v1/search.json?key=PASTEHERE&q=" + Query.Text));
                    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ProcessLocation);
                }
                catch (Exception)
                {

                }
            }
        }

        public async void ProcessLocation(object sender, DownloadStringCompletedEventArgs e)
        {
            Weather.Search = e.Result;

            var SearchData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.Search);

            dynamic jObj = JsonConvert.DeserializeObject(Weather.Search);
            AmountFound.Text = SearchData.Count + " results were found.";

            ProcessRing.IsActive = false;
                for (var i = 0; i < jObj.Count; i++) //var Result in jObj
                {

                    var converter = new System.Windows.Media.BrushConverter();
                String Lat = jObj[i].lat;
                String Lon = jObj[i].lon;


                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://timeapi.io/api/Time/current/coordinate?latitude=" + Lat + "&longitude=" + Lon);
                httpWebRequest.Method = "GET";

                using (WebResponse response = httpWebRequest.GetResponse())
                {
                    HttpWebResponse httpResponse = response as HttpWebResponse;
                    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var JSONFile = reader.ReadToEnd();
                        var Data = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(JSONFile);
                        GlobalStrings.LocalTime = Data["time"];
                    }
                }
                    TextBlock City = new TextBlock();
                    City.Text = jObj[i].name;
                    City.VerticalAlignment = VerticalAlignment.Top;
                    City.HorizontalAlignment = HorizontalAlignment.Left;
                    City.Margin = new Thickness(20, -75, 20, 0);
                    City.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                    City.FontSize = 28;
                    City.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYBOLD.OTF#SF Pro Display");

                    String Place = jObj[i].region;

                    if (Place.Length == 0)
                    {
                        GlobalStrings.Region = jObj[i].country;
                    }
                    if (Place.Length > 0)
                    {
                        GlobalStrings.Region = jObj[i].region + ", " + jObj[i].country;
                    }

                    TextBlock Region = new TextBlock();
                    Region.Text = GlobalStrings.Region;
                    Region.VerticalAlignment = VerticalAlignment.Top;
                    Region.HorizontalAlignment = HorizontalAlignment.Left;
                    Region.Margin = new Thickness(20, -45, 20, 0);
                    Region.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                    Region.FontSize = 18;
                    Region.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

                    

                    TextBlock Condition = new TextBlock();
                    Condition.Text = "Condition Unknown";
                    Condition.VerticalAlignment = VerticalAlignment.Bottom;
                    Condition.HorizontalAlignment = HorizontalAlignment.Right;
                    Condition.Margin = new Thickness(20, -10, 20, 0);
                    Condition.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                    Condition.FontSize = 18;
                    Condition.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

                    TextBlock Time = new TextBlock();
                    
                    Time.Text = GlobalStrings.LocalTime;
                    Time.VerticalAlignment = VerticalAlignment.Top;
                    Time.HorizontalAlignment = HorizontalAlignment.Left;
                    Time.Margin = new Thickness(20, -10, 20, 0);
                    Time.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                    Time.FontSize = 18;
                    Time.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

                    TextBlock Temperatures = new TextBlock();

                    //string url = "https://api.weatherapi.com/v1/forecast.json?key=" + Configs.Weatherkey + "&q=" + jObj[i].lat + "," + jObj[i].lon + "&aqi=no&alerts=yes";

                    //HttpClient client = new HttpClient();

                    //using (HttpResponseMessage response = client.GetAsync(url).Result)
                    //{
                    //    using (HttpContent content = response.Content)
                    //    {
                    //        var json = await content.ReadAsStringAsync();
                   //         var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                   //
                    //        GlobalStrings.Temperature = WeatherData["current"][Configs.Unit] + "°";
                    //    }
                    //}

                    Temperatures.Text = "";
                    Temperatures.VerticalAlignment = VerticalAlignment.Top;
                    Temperatures.HorizontalAlignment = HorizontalAlignment.Right;
                    Temperatures.Margin = new Thickness(20, -75, 20, 0);
                    Temperatures.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                    Temperatures.FontSize = 48;
                    Temperatures.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

                    Button SelectionPoint = new Button();
                    SelectionPoint.Content = i.ToString();
                    SelectionPoint.FontSize = 24;
                    SelectionPoint.Height = 100;
                    SelectionPoint.Width = 600;
                    SelectionPoint.Margin = new Thickness(0, -92, 0, 0);
                    SelectionPoint.Opacity = 0;
                    SelectionPoint.Click += StartSearching_Click;

                    StackPanel Container = new StackPanel();
                    Container.Margin = new Thickness(0, 80, 0, 0);
                    Container.Children.Add(City);
                    Container.Children.Add(Region);
                    Container.Children.Add(Temperatures);
                    Container.Children.Add(Time);
                    Container.Children.Add(SelectionPoint);


                    Border ResultBox = new Border();
                    ResultBox.Height = 100;
                    ResultBox.Width = 600;
                    ResultBox.HorizontalAlignment = HorizontalAlignment.Center;
                    ResultBox.VerticalAlignment = VerticalAlignment.Top;
                    ResultBox.CornerRadius = new CornerRadius(10);
                    ResultBox.Background = (Brush)converter.ConvertFromString("#4E87AE");
                    ResultBox.Margin = new Thickness(0, 20, 0, 0);
                    ResultBox.Child = Container;

                    DisplayResults.Children.Add(ResultBox);
                }
        }

        void StartSearching_Click(object sender, RoutedEventArgs e)
        {
            var ID = (e.Source as Button).Content.ToString();
            SearchBasedLookup.Start(Int16.Parse(ID));
        }

        private async void WeatherData(object sender, DownloadStringCompletedEventArgs e)
        {
            var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result);
            GlobalStrings.Temperature = WeatherData["current"][Configs.Unit] + "°";
            GlobalStrings.Condition = WeatherData["current"]["condition"]["text"];
        }
        private void Enter1_Click(object sender, RoutedEventArgs e)
        {
            
            QueryResult.Margin = new Thickness(0, -10, 0, 0);
            QueryResult.Content = new SearchedLocation();
            MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#000000");
            MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#000000");
        }
    }
}
