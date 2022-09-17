using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json.Serialization;
using System.Threading;

namespace ISN_Forecast.Win7.FirstSetup
{
    /// <summary>
    /// Interaction logic for Saving.xaml
    /// </summary>
    public partial class Saving : Page
    {
        public Saving()
        {
            InitializeComponent();
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            Status.Text = Lang["Setup"]["IntialSaving"];
            SaveSettings();
            
        }

        public class data
        {
            public String OOBESetup { get; set; }
            public String Language { get; set; }
            public String Appearance { get; set; }
        }

        public async void SaveSettings()
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);
            Status.Text = Lang["Setup"]["WritingToFile"];

            List<data> _data = new List<data>();
            _data.Add(new data()
            {
                OOBESetup = "false",
                Language = Configs.Language,
                Appearance = Configs.Look
            }) ;

            string json = JsonConvert.SerializeObject(_data.ToArray());

            //write string to file
            System.IO.File.WriteAllText(@"Assets/Settings.json", json);

            await Task.Delay(300);
            Status.Text = Lang["PlsWait"];

            Cleanup();
        }

        public void Cleanup()
        {
            //Setup.Instance.MainContent.Content = new SetupCompleted();
        }
    }
}
