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
using System.Windows.Media.Animation;
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

            public String GPS { get; set; }
            public String Percipitation { get; set; }
            public String Speed { get; set; }
            public String AutoHideDate { get; set; }

        }

        public SmallerSettings()
        {
            InitializeComponent();

            if (Configs.Look == "Colorful")
            {
                var converter = new System.Windows.Media.BrushConverter();
                PanelBG.Background = (Brush)converter.ConvertFromString("#0051FF");
            }

            if (Configs.Look == "Dark")
            {
                var converter = new System.Windows.Media.BrushConverter();
                PanelBG.Background = (Brush)converter.ConvertFromString("#595959");
            }

            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

            Description.Text = Lang["Setup"]["LastStepDesc"];
                Title.Text = Lang["Setup"]["LastStepTitle"];
                TimeFormat.Text = Lang["Setup"]["TimeFormat"];
                //Two4Hour.Content = Lang["Setup"]["24Hour"];
                //One2Hour.Content = Lang["Setup"]["12Hour"];
                Date.Text = Lang["Setup"]["DateFormat"];
                Country.Text = Lang["Setup"]["Country"];
                EmergencyCheckbox.Content = Lang["Setup"]["UseEmergencyServices"];
                AutoUpdates.Content = Lang["Setup"]["AllowBackgroundUpdate"];
                Unit.Text = Lang["Setup"]["tempunit"];
            RegionTitle.Text = Lang["Setup"]["Region"];
            AppSettingsTitle.Text = Lang["Setup"]["Application"];
            OptionalTitle.Text = Lang["Setup"]["Optional"];


            CountrySelection.Items.Insert(0, Lang["Countries"]["BE"]);
            CountrySelection.Items.Insert(1, Lang["Countries"]["ES"]);
            CountrySelection.Items.Insert(2, Lang["Countries"]["FR"]);
            CountrySelection.Items.Insert(3, Lang["Countries"]["UK"]);
            CountrySelection.Items.Insert(4, Lang["Countries"]["NL"]);

            TimeFormatSelection.Items.Insert(0, DateTime.Now.ToString("HH:mm"));
            TimeFormatSelection.Items.Insert(1, DateTime.Now.ToString("hh:mm tt"));

            SpeedUnitSelection.Items.Insert(0, Lang["Units"]["kmh"]);
            SpeedUnitSelection.Items.Insert(1, Lang["Units"]["mph"]);

            PercipitationSelection.Items.Insert(0, Lang["Units"]["mm"]);
            PercipitationSelection.Items.Insert(1, Lang["Units"]["inch"]);

        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

            CheckTime();
            CheckDate();
            
            GetCheckboxes();
            GetUnit();
            
            GetSpeed();
            GetPercip();
            GetAutoHideDate();//Gets country selected and converts it into a 2 Character code
            GetCountry();

            DoubleAnimation FadeWindow = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
                //EasingFunction = c
            };

            DoubleAnimation AppearText = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
                //EasingFunction = c
            };

            ColorAnimation GradientPart1 = new ColorAnimation()
            {
                To = (Color)ColorConverter.ConvertFromString("#000000"),
                Duration = TimeSpan.FromSeconds(0.3)
            };

            PanelBG.BeginAnimation(Border.OpacityProperty, FadeWindow);
            Setup.Instance.Status.BeginAnimation(TextBlock.OpacityProperty, FadeWindow);
            Setup.Instance.Status2.BeginAnimation(TextBlock.OpacityProperty, FadeWindow);
            Setup.Instance.Status3.BeginAnimation(TextBlock.OpacityProperty, FadeWindow);
            Setup.Instance.Status4.BeginAnimation(TextBlock.OpacityProperty, FadeWindow);
            Setup.Instance.Status5.BeginAnimation(TextBlock.OpacityProperty, FadeWindow);
            Next.BeginAnimation(Button.OpacityProperty, FadeWindow);

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseOut;

            MainWindow.Instance.GradientTop.BeginAnimation(GradientStop.ColorProperty, GradientPart1);
            MainWindow.Instance.GradientBottom.BeginAnimation(GradientStop.ColorProperty, GradientPart1);

            ThicknessAnimation Ass = new ThicknessAnimation()
            {
                From = new Thickness(0, 10, 0, 0),
                To = new Thickness(0, -50, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            var converter = new System.Windows.Media.BrushConverter();

            TextBlock ThanksMessage = new TextBlock();
            ThanksMessage.Text = Lang["Finished"];
            ThanksMessage.VerticalAlignment = VerticalAlignment.Center;
            ThanksMessage.HorizontalAlignment = HorizontalAlignment.Center;
            ThanksMessage.Margin = new Thickness(0, 0, 0, 0);
            ThanksMessage.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
            ThanksMessage.Opacity = 0;
            ThanksMessage.FontSize = 32;
            ThanksMessage.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");
            EndAnimation.Children.Add(ThanksMessage);

            ThanksMessage.BeginAnimation(TextBlock.OpacityProperty, AppearText);
            ThanksMessage.BeginAnimation(TextBlock.MarginProperty, Ass);

            
        }

        public void GetPercip()
        {
            if (PercipitationSelection.SelectedIndex == 0)
            {
                Configs.Percipitation = "precip_mm";
            }
            if (PercipitationSelection.SelectedIndex == 1)
            {
                Configs.Percipitation = "precip_in";
            }
        }

        public void GetSpeed()
        {
            if (TimeFormatSelection.SelectedIndex == 0)
            {
                Configs.Speed = "kmh";
            }
            if (TimeFormatSelection.SelectedIndex == 1)
            {
                Configs.Speed = "mph";
            }
        }


        public void GetAutoHideDate()
        {
            Configs.AutoHideDate = HideDate.IsChecked.ToString();
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
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

            await Task.Delay(1000);

            if (CountrySelection.Text.Length == 0)
            {
                MessageBox.Show("No country was selected");
                return;
            }
            else
            {
                Configs.Country = Lang["Countries"][CountrySelection.Text];

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
                    Unit = Configs.Unit,

                    GPS = Configs.GPS,
                    Speed = Configs.Speed,
                    Percipitation = Configs.Percipitation,
                    AutoHideDate = Configs.AutoHideDate
                });

                string json = JsonConvert.SerializeObject(_data.ToArray());

                //write string to file
                System.IO.File.WriteAllText(@"Assets/Settings.json", json);

                var converter = new System.Windows.Media.BrushConverter();

                TextBlock RebootMessage = new TextBlock();
                RebootMessage.Text = Lang["FinishedRestart"];
                RebootMessage.VerticalAlignment = VerticalAlignment.Center;
                RebootMessage.HorizontalAlignment = HorizontalAlignment.Center;
                RebootMessage.Margin = new Thickness(0, -20, 0, 0);
                RebootMessage.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                RebootMessage.Opacity = 0;
                RebootMessage.FontSize = 28;
                RebootMessage.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

                QuinticEase b = new QuinticEase();
                b.EasingMode = EasingMode.EaseOut;

                DoubleAnimation AppearText = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.2),
                    //EasingFunction = c
                };

                ThicknessAnimation Ass = new ThicknessAnimation()
                {
                    From = new Thickness(0, 70, 0, 0),
                    To = new Thickness(0, 30, 0, 0),
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = b
                };

                EndAnimation.Children.Add(RebootMessage);

                RebootMessage.BeginAnimation(TextBlock.OpacityProperty, AppearText);
                RebootMessage.BeginAnimation(TextBlock.MarginProperty, Ass);
                NextText.Opacity = 0;

                await Task.Delay(2000);

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }

         
        public void CheckDate()
        {
            Configs.DateFormat = DateFormat.Text;
        }

        public void CheckTime()
        {
            if(TimeFormatSelection.SelectedIndex == 0) //Checks if 24-Hours were selected
            {
                Configs.TimeFormat = "24";
            }
            if (TimeFormatSelection.SelectedIndex == 1)
            {
                Configs.TimeFormat = "12";
            }
        }
    }
}
