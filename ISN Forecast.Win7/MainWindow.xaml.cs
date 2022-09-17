using ISN_Forecast.Win7.FirstSetup;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            InitializeComponent();
            Instance = this;

            PreStart();
        }

        public void PreStart()
        {
            StatusBar.Content = new Statusbar();

            var asm = Assembly.GetExecutingAssembly();
            var resourceName2 = "ISN_Forecast.Win7.Metadata.json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName2))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                Configs.Meta = reader.ReadToEnd();
            }

            try
            {
                string Settings = File.ReadAllText("Assets/Settings.json");
                var SettingsJSON = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Settings);

                if (SettingsJSON[0]["OOBESetup"] == "True")
                {
                    #region Play the setup sound
                    var reader = new Mp3FileReader("Assets/Setup.mp3");
                    var waveOut = new WaveOut();
                    waveOut.Init(reader);
                    waveOut.Play();
                    float volumevalue = 0.2f;
                    waveOut.Volume = volumevalue;
                    #endregion
                    Configs.SetupProcess = "NotComplete";
                    Statusbar.Instance.Status.Text = "Welcome!";
                    MainContents.Content = new Setup();
                    MainContents.Margin = new Thickness(0, 60, 0, 0);
                    ProcessRing.IsActive = false;
                    Statusbar.Instance.Cities.Margin = new Thickness(0, 6, -70, 0);
                    Statusbar.Instance.Cities.Opacity = 0;
                    Statusbar.Instance.Settings.Opacity = 0;
                    Statusbar.Instance.Settings.IsEnabled = false;
                    Statusbar.Instance.Cities.IsEnabled = false;
                    //Statusbar.Instance.MusicBox.Margin = new Thickness(-70, 0, 0, 0);
    }
                if (SettingsJSON[0]["OOBESetup"] == "False")
                {
                    Configs.SetupProcess = "Complete";

                    string SettingsFile = File.ReadAllText("Assets/Settings.json");
                    var JSONForkie = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(SettingsFile);

                    Configs.Language = JSONForkie[0]["Language"];

                    var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

                    using (Stream stream = asm.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
                    {
                        Configs.Translations = reader.ReadToEnd();
                        var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
                    }

                    Configs.Language = SettingsJSON[0]["Language"];
                    Configs.Unit = SettingsJSON[0]["Unit"];

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
                    if (ScreenWidth > 1920)
                    {

                    }
                    if (ScreenWidth >= 1180 && ScreenWidth <= 1920)
                    {
                        MainContents.Width = 1180;
                        NewWeather.Instance.SevenDay.Margin = new Thickness(50, -250, 0, 0);
                        NewWeather.Instance.AQIBox.Margin = new Thickness(0, 10, 230, 0);
                        NewWeather.Instance.UVBox.Margin = new Thickness(0, -35, 230, 0);
                        NewWeather.Instance.DewPoint.Margin = new Thickness(0, -140, 0, 0);
                        NewWeather.Instance.Wind.Margin = new Thickness(0, -510, 0, 0);
                }
                    if (ScreenWidth <= 1179)
                    {
                        MainContents.Width = 900;
                        NewWeather.Instance.SevenDay.Margin = new Thickness(0, -120, 0, 0);
                        NewWeather.Instance.DewPoint.Margin = new Thickness(0, -330, 217, 0);
                        NewWeather.Instance.Wind.Margin = new Thickness(0, -330, 0, 0);
                        NewWeather.Instance.AQIBox.Margin = new Thickness(0, 10, 0, 0);
                    }
                }
        }
    }
}
