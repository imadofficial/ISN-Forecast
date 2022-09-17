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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ISN_Forecast.Win7.FirstSetup
{
    /// <summary>
    /// Interaction logic for GPSWarning.xaml
    /// </summary>
    public partial class GPSWarning : Window
    {
        public GPSWarning()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            var Lang = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Configs.Translations);

            Title1.Text = Lang["Setup"]["AreYouSure"];
            Description.Text = Lang["Setup"]["AreYouSureDesc"];
            Reason1.Text = "- " + Lang["Setup"]["IncorrectLocation"];
            Reason2.Text = "- " + Lang["Setup"]["IPmgmnt"];

            UseIP.Content = Lang["Setup"]["ContinueAnyway"];
            SetLocation.Content = Lang["Setup"]["SetLocationManually"];
            Back.Content = Lang["Setup"]["Back"];
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void UseIP_Click(object sender, RoutedEventArgs e)
        {
            Configs.GPS = "False";
            this.Close();
            await Task.Delay(1000);
            Setup.Instance.Step4.Opacity = 0.5;
            Setup.Instance.Step5.Opacity = 1;

            QuinticEase b = new QuinticEase();
            b.EasingMode = EasingMode.EaseInOut;

            ThicknessAnimation Current = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 4000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step2 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 3000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step3 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 2000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step4 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 1000, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            ThicknessAnimation Step5 = new ThicknessAnimation()
            {
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = b
            };

            Setup.Instance.Status.BeginAnimation(TextBlock.MarginProperty, Current);
            Setup.Instance.Status2.BeginAnimation(TextBlock.MarginProperty, Step2);
            Setup.Instance.Status3.BeginAnimation(TextBlock.MarginProperty, Step3);
            Setup.Instance.Status4.BeginAnimation(TextBlock.MarginProperty, Step4);
            Setup.Instance.Status4.Opacity = 0.5;

            Setup.Instance.Status5.BeginAnimation(TextBlock.MarginProperty, Step5);
            Setup.Instance.Status5.Opacity = 1;
            Setup.Instance.MainContent.Content = new SmallerSettings();
        }
    }
}
