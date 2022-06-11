using ISN_Forecast.Win7.FirstSetup;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        
        public MainWindow()
        {
            #region Play the bootup sound
            //var reader = new Mp3FileReader("Assets/Boot.mp3");
            //var waveOut = new WaveOut();
            //waveOut.Init(reader);
            //waveOut.Play();
            #endregion

            InitializeComponent();
            Instance = this;

            PreStart();
        }

        public void PreStart()
        {
            StatusBar.Content = new Statusbar();
            try
            {
                string Settings = File.ReadAllText("Assets/Settings.json");
                var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

                if (SettingsJSON[0]["OOBESetup"] == "True")
                {
                    Configs.SetupProcess = "NotComplete";
                    Statusbar.Instance.Status.Text = "Welcome!";
                    MainContents.Content = new Setup();
                    MainContents.Margin = new Thickness(0, 60, 0, 0);
                    
    }
                if (SettingsJSON[0]["OOBESetup"] == "False")
                {
                    Configs.SetupProcess = "Complete";
                    MainContents.Content = new NewWeather();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var ScreenWidth = Screen.ActualWidth;
            var ScreenWidthString = Screen.Width.ToString();


            if (Configs.SetupProcess == "Complete")
            {
                if (ScreenWidth > 1400)
                {
                    NewWeather.Instance.SevenDay.Margin = new Thickness(0, -120, 0, 0);
                    NewWeather.Instance.SevenDay.HorizontalAlignment = HorizontalAlignment.Left;
                }
                if (ScreenWidth >= 1100 && ScreenWidth <= 1400)
                {
                    NewWeather.Instance.AQIBox.Margin = new Thickness(0, -100, 570, 0);
                    NewWeather.Instance.AQIBox.HorizontalAlignment = HorizontalAlignment.Right;
                    NewWeather.Instance.SevenDay.HorizontalAlignment = HorizontalAlignment.Center;
                    NewWeather.Instance.SevenDay.Margin = new Thickness(0, 10, 0, 0);

                }
                if (ScreenWidth < 1100)
                {
                    NewWeather.Instance.AQIBox.Margin = new Thickness(0, 10, 0, 0);
                    NewWeather.Instance.AQIBox.HorizontalAlignment = HorizontalAlignment.Right;
                    NewWeather.Instance.SevenDay.HorizontalAlignment = HorizontalAlignment.Left;
                    NewWeather.Instance.SevenDay.Margin = new Thickness(0, -120, 0, 0);
                }
            }
            if (Configs.SetupProcess == "NotComplete")
            {
                return;
            }
        }
    }
}