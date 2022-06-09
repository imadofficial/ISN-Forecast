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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ISN_Forecast.Win7.FirstSetup
{
    /// <summary>
    /// Interaction logic for Appearance.xaml
    /// </summary>
    public partial class Appearance : Page
    {
        public static Appearance Instance;
        public Appearance()
        {
            InitializeComponent();
            
            Instance = this;

            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                Configs.Translations = reader.ReadToEnd();
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

                Setup.Instance.Status.Text = Lang["Setup"]["AppearanceTitle"];
                Setup.Instance.Extra.Text = Lang["Setup"]["AppearanceExtra"];
                Title.Text = Lang["Setup"]["ChooseLook"];
                Description.Text = Lang["Setup"]["ChooseLookWording"];
            }
        }

        private void GradientOnly(object sender, RoutedEventArgs e)
        {
            SolidColorBrush TextColor = new SolidColorBrush(Colors.White);

            SolidColorBrush BoxColor = new SolidColorBrush(Colors.Black);
            SolidColorBrush BoxTextColor = new SolidColorBrush(Colors.White);


            Configs.Look = "Gradient";
            MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#0D52AA");
            Setup.Instance.DarkMode.IsEnabled = false; Setup.Instance.DarkMode.Opacity = 0;
            MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#7CC0E8");

            Statusbar.Instance.Status.Foreground = TextColor;
            Statusbar.Instance.Time.Foreground = TextColor;
            Statusbar.Instance.Date.Foreground = TextColor;

            Title.Foreground = TextColor;
            Setup.Instance.Extra.Foreground = TextColor;
            Setup.Instance.Step1.Fill = TextColor;
            Setup.Instance.Step2.Fill = TextColor;
            Setup.Instance.Step3.Fill = TextColor;
            Setup.Instance.Step4.Fill = TextColor;
            Setup.Instance.DarkMode.Foreground = TextColor;
            Statusbar.Instance.Status.Foreground = TextColor;
            Statusbar.Instance.Time.Foreground = TextColor;
            Statusbar.Instance.Date.Foreground = TextColor;
            Setup.Instance.LineTop.Background = TextColor;
            Setup.Instance.LineBottom.Background = TextColor;

            Appearance.Instance.Title.Foreground = TextColor;
            Appearance.Instance.BackAndWhite.Foreground = BoxColor;
            Appearance.Instance.GradOnly.Foreground = BoxColor;

            Appearance.Instance.BackAndWhite.Background = BoxTextColor;
            Appearance.Instance.GradOnly.Background = BoxTextColor;

            SaveState();
        }

        private void BlackAndWhite(object sender, RoutedEventArgs e)
        {
            Configs.Look = "BlackAndWhite";
            MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#000000");
            Setup.Instance.DarkMode.IsEnabled = true; Setup.Instance.DarkMode.Opacity = 1;
            MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#000000");
            SaveState();
        }

        public void SaveState()
        {
            Next.IsEnabled = true;
            Next.Opacity = 1;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Setup.Instance.Extra.Text = "";
            Setup.Instance.Step3.Opacity = 0.5;
            Setup.Instance.Step4.Opacity = 1;

            Setup.Instance.MainContent.Content = new SmallerSettings();
        }
    }
}
