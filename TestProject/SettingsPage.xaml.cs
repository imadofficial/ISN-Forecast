using System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Windows.Storage;

namespace TestProject
{
    public sealed partial class SampleSettingPage : Page
    {
        class Configurations
        {
            public int NavPanelStyle { get; set; }
            public string Degrees { get; set; }
        }

        public SampleSettingPage()
        {
            ApplicationView.GetForCurrentView().Title = "Settings";
            InitializeComponent();
            navSetup.Items.Add(new ComboBoxItem() { Content = "Auto" });
            navSetup.Items.Add(new ComboBoxItem() { Content = "Detailed (Side)" });
            navSetup.Items.Add(new ComboBoxItem() { Content = "Top" });
            navSetup.Items.Add(new ComboBoxItem() { Content = "Icons only (Side)" });
            LoadSettings();
        }

        public void LoadSettings()
        {
            //dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(@"Settings.json"));
            var Settings = File.ReadAllText(@"Settings.json");
            dynamic config = JToken.Parse(Settings);

            if (config.NavPanelStyle == 0)
            {
                navSetup.SelectedItem = navSetup.Items[0];
            }
            if (config.NavPanelStyle == 1)
            {
                navSetup.SelectedItem = navSetup.Items[1];
            }
            if (config.NavPanelStyle == 2)
            {
                navSetup.SelectedItem = navSetup.Items[2];
            }
            if (config.NavPanelStyle == 3)
            {
                navSetup.SelectedItem = navSetup.Items[3];
            }
            else
            {

            }
        }

        public void NavPanelChange(object sender, SelectionChangedEventArgs e)
        {
            //Getting the Index int of the panels.
            var combo = sender as ComboBox;
            var selecteditem = combo.SelectedIndex;

            SaveButton.IsEnabled = true;

            if (selecteditem == 0)
            {

            }
            if (selecteditem == 1)
            {
                //Detailed (Side)
            }
            if (selecteditem == 1)
            {
                //Top Panel
            }
        }

        private async void SaveSettings(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            LoadingCircle.IsActive = true;
            var selecteditem = navSetup.SelectedIndex;

            Configurations settings = new Configurations()
            {
                NavPanelStyle = selecteditem,
                Degrees = "C"
            };

            string strResultJson = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(@"Settings.json", strResultJson);
            LoadingCircle.IsActive = false;
        }
    }
}