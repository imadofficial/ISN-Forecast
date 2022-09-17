using ISN_Forecast.Win7.SetupPages;
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

            //MicaImage.ImageSource = new BitmapImage(new Uri("/Assets/Icons/Clown.png", UriKind.Relative));

            Instance = this;

            ColorfulSelected.Opacity = 0;
            DarkSelected.Opacity = 0;
            LightSelected.Opacity = 0;
            MicaSelected.Opacity = 0;

            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                Configs.Translations = reader.ReadToEnd();
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

                //Setup.Instance.Status.Text = Lang["Setup"]["AppearanceTitle"];
                Title.Text = Lang["Setup"]["ChooseLook"];
                Description.Text = Lang["Setup"]["ChooseLookWording"];
                Title.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontBold"].ToString());
                Description.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
                Colorful.Text = Lang["Setup"]["GradientOnly"];
                Dark.Text = Lang["Setup"]["Black"];
                Light.Text = Lang["Setup"]["White"];
            }

            Init();
        }

        public void Init()
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            ThicknessAnimation ButtonLeft = new ThicknessAnimation()
            {
                From = new Thickness(-100,0,0,0),
                To = new Thickness(30, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            ThicknessAnimation ButtonRight = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, -100, 0),
                To = new Thickness(0, 0, 30, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation FadeWindow = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
                //EasingFunction = c
            };

            PanelBG.BeginAnimation(Border.OpacityProperty, FadeWindow);
            Back.BeginAnimation(System.Windows.Controls.Button.MarginProperty, ButtonLeft);
            Next.BeginAnimation(System.Windows.Controls.Button.MarginProperty, ButtonRight);
        }

        private void GradientOnly(object sender, RoutedEventArgs e)
        {
            SolidColorBrush TextColor = new SolidColorBrush(Colors.White);

            SolidColorBrush BoxColor = new SolidColorBrush(Colors.Black);
            SolidColorBrush BoxTextColor = new SolidColorBrush(Colors.White);


            Configs.Look = "Gradient";
            MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#0D52AA");
            //Setup.Instance.DarkMode.IsEnabled = false; Setup.Instance.DarkMode.Opacity = 0;
            MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#7CC0E8");

            Statusbar.Instance.Status.Foreground = TextColor;
            Statusbar.Instance.Time.Foreground = TextColor;
            Statusbar.Instance.Date.Foreground = TextColor;

            Title.Foreground = TextColor;
            Setup.Instance.Step1.Fill = TextColor;
            Setup.Instance.Step2.Fill = TextColor;
            Setup.Instance.Step3.Fill = TextColor;
            Setup.Instance.Step4.Fill = TextColor;
            Setup.Instance.DarkMode.Foreground = TextColor;
            Statusbar.Instance.Status.Foreground = TextColor;
            Statusbar.Instance.Time.Foreground = TextColor;
            Statusbar.Instance.Date.Foreground = TextColor;

            Appearance.Instance.Title.Foreground = TextColor;
            //Appearance.Instance.BackAndWhite.Foreground = BoxColor;
            //Appearance.Instance.GradOnly.Foreground = BoxColor;

            //Appearance.Instance.BackAndWhite.Background = BoxTextColor;
            //Appearance.Instance.GradOnly.Background = BoxTextColor;

            SaveState();
        }

        private void BlackAndWhite(object sender, RoutedEventArgs e)
        {
            Configs.Look = "BlackAndWhite";
            MainWindow.Instance.GradientTop.Color = (Color)ColorConverter.ConvertFromString("#000000");
            //Setup.Instance.DarkMode.IsEnabled = true; Setup.Instance.DarkMode.Opacity = 1;
            MainWindow.Instance.GradientBottom.Color = (Color)ColorConverter.ConvertFromString("#000000");
            SaveState();
        }

        public void SaveState()
        {
            Next.IsEnabled = true;
            Next.Opacity = 1;
        }

        private async void Back_Click(object sender, RoutedEventArgs e)
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation Current = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step2 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step3 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -2000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step4 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -3000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step5 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -4000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation FadeButtons = new ThicknessAnimation()
            {
                To = new Thickness(-100, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            Back.BeginAnimation(System.Windows.Controls.Button.MarginProperty, FadeButtons);
            Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
            Setup.Instance.Status.Opacity = 1;
            Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
            Setup.Instance.Status2.Opacity = 0.5;
            Setup.Instance.Status3.Opacity = 0.5;
            Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
            Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
            Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);
            await Task.Delay(1000);
            Setup.Instance.MainContent.Content = new Language();
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            Setup.Instance.Step3.Opacity = 0.5;
            Setup.Instance.Step4.Opacity = 1;


            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseInOut;

            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            ThicknessAnimation Current = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 3000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step2 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 2000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step3 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step4 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step5 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            if (Configs.Look == "Mica")
            {
                DoubleAnimation FadeWindow = new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.2),
                    EasingFunction = c
                };

                DoubleAnimation AppearText = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.2),
                    //EasingFunction = c
                };

                ColorAnimation Gradient = new ColorAnimation()
                {
                    To = (Color)ColorConverter.ConvertFromString("#000000"),
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                ThicknessAnimation Ass = new ThicknessAnimation()
                {
                    From = new Thickness(0, 10, 0, 0),
                    To = new Thickness(0, -50, 0, 0),
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = b
                };

                PanelBG.BeginAnimation(Border.OpacityProperty, FadeWindow);
                Next.BeginAnimation(Button.OpacityProperty, FadeWindow);
                Back.BeginAnimation(Button.OpacityProperty, FadeWindow);

                var converter = new System.Windows.Media.BrushConverter();

                TextBlock RebootMessage = new TextBlock();
                RebootMessage.Text = "ISN Forecast will reboot to finish the setup.";
                RebootMessage.VerticalAlignment = VerticalAlignment.Center;
                RebootMessage.HorizontalAlignment = HorizontalAlignment.Center;
                RebootMessage.Margin = new Thickness(0, 0, 0, 0);
                RebootMessage.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                RebootMessage.Opacity = 0;
                RebootMessage.FontSize = 32;
                RebootMessage.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");
                MicaConfirmation.Children.Add(RebootMessage);

                RebootMessage.BeginAnimation(TextBlock.OpacityProperty, AppearText);
                RebootMessage.BeginAnimation(TextBlock.MarginProperty, Ass);

                await Task.Delay(1000);

                return;
            }

            Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
            Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
            Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
            Setup.Instance.Status3.Opacity = 0.5;

            Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
            Setup.Instance.Status4.Opacity = 1;

            Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);

            Setup.Instance.MainContent.Content = new LocationServices();
        }

        private void Colorful_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            Next.Opacity = 1;

            Configs.Look = "Colorful";
            ColorAnimation SetBox = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#0051FF"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation GradientPart1 = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#0D52AA"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation GradientPart2 = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#7CC0E8"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorfulSelected.Opacity = 1;
            DarkSelected.Opacity = 0;
            LightSelected.Opacity = 0;
            MicaSelected.Opacity = 0;

            Title.Foreground =  new SolidColorBrush(Colors.White);
            Description.Foreground = new SolidColorBrush(Colors.White);
            Colorful.Foreground = new SolidColorBrush(Colors.White);
            Dark.Foreground = new SolidColorBrush(Colors.White);
            Light.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status2.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status3.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status4.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status5.Foreground = new SolidColorBrush(Colors.White);
            Statusbar.Instance.Status.Foreground = new SolidColorBrush(Colors.White);
            Statusbar.Instance.Time.Foreground = new SolidColorBrush(Colors.White);
            Statusbar.Instance.Date.Foreground = new SolidColorBrush(Colors.White);

            PanelBG.Background.BeginAnimation(SolidColorBrush.ColorProperty, SetBox);
            MainWindow.Instance.GradientTop.BeginAnimation(GradientStop.ColorProperty, GradientPart1);
            MainWindow.Instance.GradientBottom.BeginAnimation(GradientStop.ColorProperty, GradientPart2);
        }

        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            Next.Opacity = 1;

            Configs.Look = "Dark";
            ColorfulSelected.Opacity = 0;
            DarkSelected.Opacity = 1;
            LightSelected.Opacity = 0;
            MicaSelected.Opacity = 0;

            Title.Foreground = new SolidColorBrush(Colors.White);
            Description.Foreground = new SolidColorBrush(Colors.White);
            Colorful.Foreground = new SolidColorBrush(Colors.White);
            Dark.Foreground = new SolidColorBrush(Colors.White);
            Light.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status2.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status3.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status4.Foreground = new SolidColorBrush(Colors.White);
            Setup.Instance.Status5.Foreground = new SolidColorBrush(Colors.White);
            Statusbar.Instance.Status.Foreground = new SolidColorBrush(Colors.White);
            Statusbar.Instance.Time.Foreground = new SolidColorBrush(Colors.White);
            Statusbar.Instance.Date.Foreground = new SolidColorBrush(Colors.White);

            ColorAnimation SetBox = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#595959"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation GradientPart1 = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#000000"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation GradientPart2 = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#000000"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation TXT = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#FFFFFF"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            PanelBG.Background.BeginAnimation(SolidColorBrush.ColorProperty, SetBox);
            MainWindow.Instance.GradientTop.BeginAnimation(GradientStop.ColorProperty, GradientPart1);
            MainWindow.Instance.GradientBottom.BeginAnimation(GradientStop.ColorProperty, GradientPart2);
        }

        private void BrightMode_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            Next.Opacity = 1;

            Configs.Look = "Bright";
            ColorfulSelected.Opacity = 0;
            DarkSelected.Opacity = 0;
            MicaSelected.Opacity = 0;
            LightSelected.Opacity = 1;

            Title.Foreground = new SolidColorBrush(Colors.Black);
            Description.Foreground = new SolidColorBrush(Colors.Black);
            Colorful.Foreground = new SolidColorBrush(Colors.Black);
            Dark.Foreground = new SolidColorBrush(Colors.Black);
            Light.Foreground = new SolidColorBrush(Colors.Black);
            Setup.Instance.Status.Foreground = new SolidColorBrush(Colors.Black);
            Setup.Instance.Status2.Foreground = new SolidColorBrush(Colors.Black);
            Setup.Instance.Status3.Foreground = new SolidColorBrush(Colors.Black);
            Setup.Instance.Status4.Foreground = new SolidColorBrush(Colors.Black);
            Setup.Instance.Status5.Foreground = new SolidColorBrush(Colors.Black);
            Statusbar.Instance.Status.Foreground = new SolidColorBrush(Colors.Black);
            Statusbar.Instance.Time.Foreground = new SolidColorBrush(Colors.Black);
            Statusbar.Instance.Date.Foreground = new SolidColorBrush(Colors.Black);


            ColorAnimation SetBox = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#a8a8a8"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation GradientPart1 = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#FFFFFF"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation GradientPart2 = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#FFFFFF"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation TXT = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#FFFFFF"),
                Duration = TimeSpan.FromSeconds(0.5)
            };

            //Setup.Instance.LineTop.Background = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            PanelBG.Background.BeginAnimation(SolidColorBrush.ColorProperty, SetBox);
            MainWindow.Instance.GradientTop.BeginAnimation(GradientStop.ColorProperty, GradientPart1);
            MainWindow.Instance.GradientBottom.BeginAnimation(GradientStop.ColorProperty, GradientPart2);
        }

        private void MicaMode_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            Next.Opacity = 1;

            Configs.Look = "Mica";

            ColorfulSelected.Opacity = 0;
            DarkSelected.Opacity = 0;
            MicaSelected.Opacity = 1;
            LightSelected.Opacity = 0;
        }
    }
}
