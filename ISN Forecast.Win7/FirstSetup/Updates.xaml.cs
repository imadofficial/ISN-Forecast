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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ISN_Forecast.Win7.FirstSetup
{
    /// <summary>
    /// Interaction logic for Updates.xaml
    /// </summary>
    public partial class Updates : Page
    {
        public Updates()
        {
            InitializeComponent();

            

            Setup.Instance.Step1.Opacity = 0.5;
            Setup.Instance.Step2.Opacity = 1;

            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);

                Statusbar.Instance.Status.Text = Lang["Setup"]["Welcome"];
                //Setup.Instance.Status.Text = Lang["Setup"]["UpdateTitle"];
                Status.Text = Lang["PlsWait"];
            }

            Checker();
        }

        public async void Checker()
        {
            await Task.Delay(500);
            WebClient webClient = new WebClient();
                webClient.DownloadStringAsync(new Uri("https://raw.githubusercontent.com/imadofficial/ISN-Forecast/main/CurrentVersion.json"));
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Process);
        }

        private void Process(object sender, DownloadStringCompletedEventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Metadata.json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                var Metadata = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);

                Version.Build = Metadata["Build"];
                Version.SubBuild = Metadata["SubBuild"];
            }

            try
            {
                var UpdateFile = e.Result;

                var ISNU = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(UpdateFile);

                if (Version.Build < (int)ISNU["Build"])
                {
                    Status.Text = "A Build Update was found";
                }

                if (Version.Build > (int)ISNU["Build"])
                {
                    Status.Text = "A big update was found";
                }

                if (Version.Build == (int)ISNU["Build"])
                {
                    if (Version.SubBuild < (int)ISNU["SubBuild"])
                    {
                        Status.Text = "A smol update was found.";
                    }

                    if (Version.SubBuild == (int)ISNU["SubBuild"])
                    {

                        var StartMargin1 = new Thickness(0, 0, 1000, 0);
                        var EndMargin1 = new Thickness(0, 0, 2000, 0);

                        var StartMargin2 = new Thickness(0, 0, 0, 0);
                        var EndMargin2 = new Thickness(0, 0, 1000, 0);

                        var StartMargin3 = new Thickness(0, 0, -1000, 0);
                        var EndMargin3 = new Thickness(0, 0, 0, 0);


                        QuinticEase b = new QuinticEase();
                        b.EasingMode = EasingMode.EaseInOut;

                        ThicknessAnimation Current = new ThicknessAnimation()
                        {
                            To = EndMargin1,
                            Duration = TimeSpan.FromSeconds(1),
                            EasingFunction = b
                        };

                        ThicknessAnimation Step2 = new ThicknessAnimation()
                        {
                            To = EndMargin2,
                            Duration = TimeSpan.FromSeconds(1),
                            EasingFunction = b
                        };

                        ThicknessAnimation Step3 = new ThicknessAnimation()
                        {
                            To = EndMargin3,
                            Duration = TimeSpan.FromSeconds(1),
                            EasingFunction = b
                        };

                        ThicknessAnimation Step4 = new ThicknessAnimation()
                        {
                            To = new Thickness(0, 0, -1000, 0),
                            Duration = TimeSpan.FromSeconds(1),
                            EasingFunction = b
                        };

                        ThicknessAnimation Step5 = new ThicknessAnimation()
                        {
                            To = new Thickness(0, 0, -2000, 0),
                            Duration = TimeSpan.FromSeconds(1),
                            EasingFunction = b
                        };

                        Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
                        Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
                        Setup.Instance.Status2.Opacity = 0.5;
                        Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
                        Setup.Instance.Status3.Opacity = 1;

                        Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
                        Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);

                        Setup.Instance.MainContent.Content = new Appearance();
                        Setup.Instance.Step2.Opacity = 0.5;
                        Setup.Instance.Step3.Opacity = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Status.Text = "No internet connection was found.";

                Task.Delay(1000);

                var StartMargin1 = new Thickness(0, 0, 1000, 0);
                var EndMargin1 = new Thickness(0, 0, 2000, 0);

                var StartMargin2 = new Thickness(0, 0, 0, 0);
                var EndMargin2 = new Thickness(0, 0, 1000, 0);

                var StartMargin3 = new Thickness(0, 0, -1000, 0);
                var EndMargin3 = new Thickness(0, 0, 0, 0);


                QuinticEase b = new QuinticEase();
                b.EasingMode = EasingMode.EaseInOut;

                ThicknessAnimation Current = new ThicknessAnimation()
                {
                    To = EndMargin1,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = b
                };

                ThicknessAnimation Step2 = new ThicknessAnimation()
                {
                    To = EndMargin2,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = b
                };

                ThicknessAnimation Step3 = new ThicknessAnimation()
                {
                    To = EndMargin3,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = b
                };

                ThicknessAnimation Step4 = new ThicknessAnimation()
                {
                    To = new Thickness(0, 0, -1000, 0),
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = b
                };

                ThicknessAnimation Step5 = new ThicknessAnimation()
                {
                    To = new Thickness(0, 0, -2000, 0),
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = b
                };

                Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
                Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
                Setup.Instance.Status2.Opacity = 0.5;
                Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
                Setup.Instance.Status3.Opacity = 1;

                Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
                Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);

                Setup.Instance.MainContent.Content = new Appearance();
                Setup.Instance.Step2.Opacity = 0.5;
                Setup.Instance.Step3.Opacity = 1;
            }
        }
    }
}