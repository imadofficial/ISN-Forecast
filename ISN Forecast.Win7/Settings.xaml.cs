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
using ISN_Forecast.Win7.Diagnostics;

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
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
            Statusbar.Instance.Search.IsEnabled = true; Statusbar.Instance.Search.Opacity = 1;
        }

        private async void ConnectTest(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#000000");
            MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#000000");
            await Task.Delay(500);
            MainWindow.Instance.MainContents.Opacity = 0;
            MainWindow.Instance.ButtonedScreen.Margin = new Thickness(0,-10240,0,0);
            await Task.Delay(1000);
            Statusbar.Instance.Status.Text = "Diagnostics Mode";
            MainWindow.Instance.MainContents.Margin = new Thickness(0, 60, 0, 0);
            await Task.Delay(2000);
            MainWindow.Instance.MainContents.Content = new Setup();
            Setup.Instance.MainContent.Content = new Splash();
            Setup.Instance.Status.Text = "Please select an action";
            Setup.Instance.Extra.Text = "";
            Setup.Instance.Progress.Opacity = 0;
            MainWindow.Instance.MainContents.Opacity = 1;
        }
    }
}
