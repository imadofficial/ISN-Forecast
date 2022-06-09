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
using ISN_Forecast.Win7.FirstSetup;
using ISN_Forecast.Win7.SetupPages;
using ModernWpf.Controls;

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : System.Windows.Controls.Page
    {
        public static Setup Instance;

        public Setup()
        {
            InitializeComponent();
            Instance = this;
            MainContent.Content = new Language();
        }

        private void DarkMode_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true) //Light mode
                {
                    MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                    MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                    SolidColorBrush TextColor = new SolidColorBrush(Colors.Black);

                    SolidColorBrush BoxColor = new SolidColorBrush(Colors.White);
                    SolidColorBrush BoxTextColor = new SolidColorBrush(Colors.Black);

                    Status.Foreground = TextColor;
                    Extra.Foreground = TextColor;
                    Step1.Fill = TextColor;
                    Step2.Fill = TextColor;
                    Step3.Fill = TextColor;
                    Step4.Fill = TextColor;
                    DarkMode.Foreground = TextColor;
                    Statusbar.Instance.Status.Foreground = TextColor;
                    Statusbar.Instance.Time.Foreground = TextColor;
                    Statusbar.Instance.Date.Foreground = TextColor;
                    LineTop.Background = TextColor;
                    LineBottom.Background = TextColor;

                    Appearance.Instance.Title.Foreground = TextColor;
                    Appearance.Instance.BackAndWhite.Foreground = BoxColor;
                    Appearance.Instance.GradOnly.Foreground = BoxColor;

                    Appearance.Instance.BackAndWhite.Background = BoxTextColor;
                    Appearance.Instance.GradOnly.Background = BoxTextColor;
                }
                else //Dark mode
                {
                    MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#000000");
                    MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#000000");

                    SolidColorBrush TextColor = new SolidColorBrush(Colors.White);

                    SolidColorBrush BoxColor = new SolidColorBrush(Colors.Black);
                    SolidColorBrush BoxTextColor = new SolidColorBrush(Colors.White);

                    Status.Foreground = TextColor;
                    Extra.Foreground = TextColor;
                    Step1.Fill = TextColor;
                    Step2.Fill = TextColor;
                    Step3.Fill = TextColor;
                    Step4.Fill = TextColor;
                    DarkMode.Foreground = TextColor;
                    Statusbar.Instance.Status.Foreground = TextColor;
                    Statusbar.Instance.Time.Foreground = TextColor;
                    Statusbar.Instance.Date.Foreground = TextColor;
                    LineTop.Background = TextColor;
                    LineBottom.Background = TextColor;

                    Appearance.Instance.Title.Foreground = TextColor;
                    Appearance.Instance.BackAndWhite.Foreground = BoxColor;
                    Appearance.Instance.GradOnly.Foreground = BoxColor;

                    Appearance.Instance.BackAndWhite.Background = BoxTextColor;
                    Appearance.Instance.GradOnly.Background = BoxTextColor;
                }
            }

        }
    }
}
