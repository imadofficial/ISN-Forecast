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

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        public static Search Instance;
        public Search()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Statusbar.Instance.Status.Text = "Current Weather";
            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0, -10240, 0, 0);
            NewWeather.Instance.Effect = null;
            NewWeather.Instance.ScrollPerms.IsEnabled = true;

            Statusbar.Instance.Settings.IsEnabled = true; Statusbar.Instance.Settings.Opacity = 1;
            Statusbar.Instance.Refresh.IsEnabled = true; Statusbar.Instance.Refresh.Opacity = 1;
            Statusbar.Instance.Globe.IsEnabled = true; Statusbar.Instance.Globe.Opacity = 1;
        }

        private void Query_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringAsync(new Uri("https://api.weatherapi.com/v1/search.json?key=b48046722eb448daafa173827211511&q=" + Query.Text));
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ProcessLocation);
            }catch(Exception ex)
            {

            }
        }

        private void ProcessLocation(object sender, DownloadStringCompletedEventArgs e)
        {
            Result1.Opacity = 0; Result2.Opacity = 0; Result3.Opacity = 0; Result4.Opacity = 0; Result5.Opacity = 0;
            Result1.IsEnabled = false; Result2.IsEnabled = false; Result3.IsEnabled = false; Result4.IsEnabled = false; Result5.IsEnabled = false;
            try
            {
                var QueryString = e.Result;
                var SearchData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(QueryString);

                City1.Text = SearchData[0]["name"];
                Country1.Text = SearchData[0]["region"] + ", " + SearchData[0]["country"];
                Result1.Opacity = 1; Result1.IsEnabled = true;

                City2.Text = SearchData[1]["name"];
                Country2.Text = SearchData[1]["region"] + ", " + SearchData[1]["country"];
                Result2.Opacity = 1; Result2.IsEnabled = true;

                City3.Text = SearchData[2]["name"];
                Country3.Text = SearchData[2]["region"] + ", " + SearchData[2]["country"];
                Result3.Opacity = 1; Result3.IsEnabled = true;

                City4.Text = SearchData[3]["name"];
                Country4.Text = SearchData[3]["region"] + ", " + SearchData[3]["country"];
                Result4.Opacity = 1; Result4.IsEnabled = true;

                City5.Text = SearchData[4]["name"];
                Country5.Text = SearchData[4]["region"] + ", " + SearchData[4]["country"];
                Result5.Opacity = 1; Result5.IsEnabled = true;
            }
            catch(Exception ex)
            {

            }
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
