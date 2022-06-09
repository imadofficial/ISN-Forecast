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
                Setup.Instance.Extra.Text = Lang["Setup"]["ExtraUpdateInfo"];
                Setup.Instance.Status.Text = Lang["Setup"]["UpdateTitle"];
                Status.Text = Lang["PlsWait"];
            }

            Checker();
        }

        public void Checker()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringAsync(new Uri("https://raw.githubusercontent.com/imadofficial/ISN-Forecast/main/CurrentVersion.json"));
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Process);
            }
            catch (Exception)
            {

            }
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

            var UpdateFile = e.Result;
            var ISNU = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(UpdateFile); //ISNUSERV = ISN Update Service

            if (Version.Build < (int)ISNU["Build"])
            {
                Status.Text = "A Build Update was found";
            }

            if (Version.Build == (int)ISNU["Build"])
            {
                if (Version.SubBuild < (int)ISNU["SubBuild"])
                {
                    Status.Text = "A smol update was found.";
                }

                if (Version.SubBuild == (int)ISNU["SubBuild"])
                {
                    Setup.Instance.MainContent.Content = new Appearance();
                    Setup.Instance.Step2.Opacity = 0.5;
                    Setup.Instance.Step3.Opacity = 1;
                }
            }

            
        }
    }
} 