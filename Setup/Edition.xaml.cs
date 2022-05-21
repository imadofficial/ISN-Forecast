using Microsoft.Win32;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Setup
{
    /// <summary>
    /// Interaction logic for Edition.xaml
    /// </summary>
    public partial class Edition : Page
    {
        public Edition()
        {
            InitializeComponent();
            Checker();
            Preparation();
        }

        public void Preparation()
        {
            //Makes file if it doesn't exist
            using (StreamWriter w = File.AppendText("Configuration.json"));
        }

        public void IDR_Selected(object sender, MouseButtonEventArgs e) //Make recommended button clickable
        {

        }

        private void ID1_Selected(object sender, RoutedEventArgs e) //UWP
        {

        }

        public void Checker()
        {
            RegistryKey registryKey =  Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            var buildNumber = registryKey.GetValue("CurrentBuildNumber").ToString();

            if (buildNumber == "20348")
            {
                Recommended_Title.Text = "Win32 (Designed for Windows 7 -> 11)";
                Recommended_Desc.Text = "This is by far the best one you can come accross, powered by DirectX and OpenGL. You can get the best forecast experience ever to exist for older hardware.";
                Recommended_Support.Text = "Supported Versions: Windows 7, Windows 8.x, Windows 10 and Windows 11";

                Recommended_IMG.Source = new BitmapImage(new Uri(@"/Assets/Images/Windows7.png", UriKind.Relative));

                UWP_Support.Text = "Incompatible with your Windows version.";
                UWP_Title.Opacity = 0.5;
                UWP_IMG.Opacity = 0.5;
                UWP_Desc.Opacity = 0.5;
                UWP_Support.Opacity = 0.5;
                UWP_Checkbox.Opacity = 0.5;
                UWP_Checkbox.IsEnabled = false;
            }
            if (buildNumber == "22000" || buildNumber == "19044" || buildNumber == "19043" || buildNumber == "19042" || buildNumber == "19041" || buildNumber == "18363" || buildNumber == "18362")
            {
                Recommended_Title.Text = "Universal Windows Platform";
                Recommended_Desc.Text = "The UWP version of ISN Forecast provides you an easy way to check the weather, wherever you are. It features an easy setup, simplistic UI and feels like it comes from Microsoft themselves. It works seemlessly across Windows 10 and 11 devices.";
                Recommended_Support.Text = "Supported Versions: Windows 11 and Windows 10 Non-Server Editions (1809+)";
            }

            EditionsList.Opacity = 1;
            Status.Opacity = 0;
        }

        private void GoToLocation(object sender, RoutedEventArgs e) //UWP
        {
            if(Win32_7_Checkbox.IsChecked == false && Win32_XP_Checkbox.IsChecked == false && UWP_Checkbox.IsChecked == false)
            {
                MessageBox.Show("You have to select something to install.", "U serious?", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                using (StreamWriter file = File.CreateText(@"Configurations.json"))
                {
                }
            }

            
        }
    }
}
