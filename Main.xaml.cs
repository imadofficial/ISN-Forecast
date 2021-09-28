using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Weather_Channel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            mediaPlayer.Open(new Uri(System.Environment.CurrentDirectory + @"\Resources\Connecting.mp3"));
            mediaPlayer.Play();

            ContentRendered += (s, e) => { Connecting(); };


            //  string json = (new System.Net.WebClient()).DownloadString("https://dataservice.accuweather.com/forecasts/v1/daily/1day/27581?apikey=ms3GTDs39DhmtAUbs6iMglRqJQFEM7sd&language=en-us&details=true&metric=true");
        }

        void Connecting()
        {
            //oneShot.Stop();
            //oneShot.Start();

            Ping ping = new Ping();
            PingReply pingresult = ping.Send("92.123.50.79");
            if (pingresult.Status.ToString() == "Success")
            {
                Thread.Sleep(2000);
                this.Title = "ISN Weather | Connecting to AccuWeather...";
                ConnectionStatus.Content = "Connecting to AccuWeather...";
                Window window = new Window
                {
                    Title = "ISN Weather | Forecast in Brussels",
                    Content = new Weather()
                };

                window.ShowDialog();
            }
            else
            {
                this.Effect = new BlurEffect();
                this.Title = "ISN Weather | Connection Failed...";
                this.Opacity = 0.5;
            }

        }

        void Animation_INT()
        {
            //SUN_Int1 img1 = (SUN_Int1)sender;
            //DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            //img1.BeginAnimation(Canvas.OpacityProperty, ani);


        }
    }
}