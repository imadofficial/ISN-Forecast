using ISN_Forecast.Win7.FirstSetup;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace ISN_Forecast.Win7.SetupPages
{
    /// <summary>
    /// Interaction logic for Language.xaml
    /// </summary>
    public partial class Language : Page
    {
        public Language()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Metadata.json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);
                dynamic jObj = JsonConvert.DeserializeObject(Contents);

                for (var i = 0; i < jObj.list.Count; i++) //var Result in jObj
                {
                    var converter = new System.Windows.Media.BrushConverter();

                    TextBlock LangTitle = new TextBlock();
                    LangTitle.TextAlignment = TextAlignment.Center;
                    LangTitle.Text = jObj.list[i].Name;
                    LangTitle.VerticalAlignment = VerticalAlignment.Top;
                    LangTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    LangTitle.Margin = new Thickness(0, 7, 0, 0);
                    LangTitle.Foreground = (Brush)converter.ConvertFromString("#FFFFFF");
                    LangTitle.FontSize = 21;
                    LangTitle.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/Latin-Based/SFPRODISPLAYREGULAR.OTF#SF Pro Display");

                    System.Windows.Controls.Button SelectionPoint = new System.Windows.Controls.Button();
                    SelectionPoint.Content = jObj.list[i].ID;
                    SelectionPoint.FontSize = 15;
                    SelectionPoint.Height = 30;
                    SelectionPoint.Width = 69420;
                    SelectionPoint.Margin = new Thickness(0, -25, 0, 0);
                    SelectionPoint.Opacity = 0;
                    SelectionPoint.Click += StartSearching_Click;

                    StackPanel Container = new StackPanel();
                    Container.Children.Add(LangTitle);
                    Container.Children.Add(SelectionPoint);


                    Border ResultBox = new Border();
                    ResultBox.Height = 40;
                    ResultBox.VerticalAlignment = VerticalAlignment.Top;
                    ResultBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    ResultBox.CornerRadius = new CornerRadius(10);
                    ResultBox.Background = (Brush)converter.ConvertFromString("#000000");
                    ResultBox.Margin = new Thickness(30, 15, 30, 0);
                    ResultBox.Child = Container;

                    LanguagePanel.Children.Add(ResultBox);
                }
            }
        }

        void StartSearching_Click(object sender, RoutedEventArgs e)
        {
            var ID = (e.Source as System.Windows.Controls.Button).Content.ToString();
            //System.Windows.MessageBox.Show(ID);

            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Metadata.json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);
                Configs.Language = Lang["list"][Int32.Parse(ID)]["Code"];
                ApplySetting();
                return;
            }
                
        }

        private void ApplySetting()
        {
            Next.Opacity = 1;
            Next.IsEnabled = true;

            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "ISN_Forecast.Win7.Assets.Translations." + Configs.Language + ".json";

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true))
            {
                var Contents = reader.ReadToEnd();
                //string Settings = File.ReadAllText(reader.ReadToEnd());
                //SysUtil.StringEncodingConvert(content, "ISO-8859-1", "UTF-8");
                var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Contents);

                Statusbar.Instance.Status.Text = Lang["Setup"]["Welcome"];
                Setup.Instance.Status.Text = Lang["Setup"]["Step1"];
                Setup.Instance.Status2.Text = Lang["Setup"]["Step2"];
                Setup.Instance.Status3.Text = Lang["Setup"]["Step3"];
                Setup.Instance.Status4.Text = Lang["Setup"]["Step4"];
                Setup.Instance.Status5.Text = Lang["Setup"]["Step5"];
                //Recommended.Text = Lang["Setup"]["Recommended"];
                Other.Text = Lang["Setup"]["Language Select"];

                Setup.Instance.Status.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
                Setup.Instance.Status2.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
                Setup.Instance.Status3.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
                Setup.Instance.Status4.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
                Setup.Instance.Status5.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());
                Other.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontBold"].ToString());
                Statusbar.Instance.Status.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Lang["Metadata"]["FontNormal"].ToString());

                if (Configs.Language == "kr" || Configs.Language == "cn_SP")
                {
                    Statusbar.Instance.Status.Margin = new Thickness(10, 0, 0, 0);
                }else
                {
                    Statusbar.Instance.Status.Margin = new Thickness(10, 5, 0, 0);
                }
            }
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            QuinticEase c = new QuinticEase();
            c.EasingMode = EasingMode.EaseOut;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation Current = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, 0, 0),
                To = new Thickness(0, 0, 1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step2 = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, -1000, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step3 = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, -2000, 0),
                To = new Thickness(0, 0, -1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step4 = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, -3000, 0),
                To = new Thickness(0, 0, -2000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step5 = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, -4000, 0),
                To = new Thickness(0, 0, -3000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation FadeButtons = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, -100, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = c
            };

            DoubleAnimation FadeWindow = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
                //EasingFunction = c
            };

            Next.BeginAnimation(System.Windows.Controls.Button.MarginProperty, FadeButtons);
            await Task.Delay(300);
            PanelBG.BeginAnimation(Border.OpacityProperty, FadeWindow);
            await Task.Delay(700);

            Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
            Setup.Instance.Status.Opacity = 0.5;
            Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
            Setup.Instance.Status2.Opacity = 1;
            Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
            Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
            Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);

            Setup.Instance.MainContent.Content = new Updates();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Lang.Height = MainWindow.Instance.Screen.ActualHeight - 250;
        }
    }
}
