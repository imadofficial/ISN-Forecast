using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for SmallerSettings.xaml
    /// </summary>
    public partial class SmallerSettings : Page
    {
        public class data
        {
            public String OOBESetup { get; set; }
            public String Language { get; set; }
            public String Appearance { get; set; }
            public String TimeFormat { get; set; }
            public String DateFormat { get; set; }
            public String Country { get; set; }
            public String AllowAlerts { get; set; }
            public String AllowAutoUpdate { get; set; }
            public String Unit { get; set; }

        }

        public SmallerSettings()
        {
            InitializeComponent();

            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                //string Settings = File.ReadAllText(reader.ReadToEnd());
                //SysUtil.StringEncodingConvert(content, "ISO-8859-1", "UTF-8");
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);

                Description.Text = Lang["Setup"]["LastStepDesc"];
                Title.Text = Lang["Setup"]["LastStepTitle"];
                TimeFormat.Text = Lang["Setup"]["TimeFormat"];
                Two4Hour.Content = Lang["Setup"]["24Hour"];
                One2Hour.Content = Lang["Setup"]["12Hour"];
                Date.Text = Lang["Setup"]["DateFormat"];
                Country.Text = Lang["Setup"]["Country"];
                EmergencyCheckbox.Content = Lang["Setup"]["UseEmergencyServices"];
                AutoUpdates.Content = Lang["Setup"]["AllowBackgroundUpdate"];
                Unit.Text = Lang["Setup"]["tempunit"];


                CountrySelection.Items.Insert(0, Lang["Countries"]["BE"]);
                CountrySelection.Items.Insert(1, Lang["Countries"]["ES"]);
                CountrySelection.Items.Insert(2, Lang["Countries"]["FR"]);
                CountrySelection.Items.Insert(3, Lang["Countries"]["UK"]);
                CountrySelection.Items.Insert(4, Lang["Countries"]["NL"]);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            CheckTime();
            CheckDate();
            
            GetCheckboxes();
            GetUnit();
            GetCountry(); //Gets country selected and converts it into a 2 Character code
        }
        
        public void GetUnit()
        {
            if(UnitBox.Text == "Celsius")
            {
                Configs.Unit = "temp_c";
            }
            else
            {
                Configs.Unit = "temp_f";
            }
        }

        public void GetCheckboxes()
        {
            Configs.AllowAlerts = EmergencyCheckbox.IsChecked.ToString();
            Configs.AllowAutoUpdate = AutoUpdates.IsChecked.ToString();
        }

        public async void GetCountry()
        {
            if (CountrySelection.Text.Length == 0)
            {
                MessageBox.Show("No country was selected");
                return;
            }
            else
            {
                var asm = Assembly.GetExecutingAssembly();
                var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

                using (Stream stream = asm.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
                {
                    var Contents = reader.ReadToEnd();
                    var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);

                    Configs.Country = Lang["Countries"][CountrySelection.Text];
                }

                NextText.Opacity = 1;
                await Task.Delay(300);

                List<data> _data = new List<data>();
                _data.Add(new data()
                {
                    OOBESetup = "False",
                    Language = Configs.Language,
                    Appearance = Configs.Look,
                    TimeFormat = Configs.TimeFormat,
                    DateFormat = Configs.DateFormat,
                    Country = Configs.Country,
                    AllowAlerts = Configs.AllowAlerts,
                    AllowAutoUpdate = Configs.AllowAutoUpdate,
                    Unit = Configs.Unit
                });

                string json = JsonConvert.SerializeObject(_data.ToArray());

                //write string to file
                System.IO.File.WriteAllText(@"Assets/Settings.json", json);

                Setup.Instance.MainContent.Content = new SetupCompleted();
            }
        }

         
        public void CheckDate()
        {
            Configs.DateFormat = DateFormat.Text;
        }

        public void CheckTime()
        {
            if(Two4Hour.IsChecked == true) //Checks if 24-Hours were selected
            {
                Configs.TimeFormat = "24";
            }
            else
            {
                Configs.TimeFormat = "12";
            }
        }
    }
}
