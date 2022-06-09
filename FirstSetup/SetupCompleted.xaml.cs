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

namespace ISN_Forecast.Win7.FirstSetup
{
    /// <summary>
    /// Interaction logic for SetupCompleted.xaml
    /// </summary>
    public partial class SetupCompleted : Page
    {
        public SetupCompleted()
        {
            InitializeComponent();
            Setup.Instance.Status.Text = "Thanks for installing me!";
            Setup.Instance.Extra.Text = "";
            Setup.Instance.Step3.Opacity = 0.5;
            Setup.Instance.Step4.Opacity = 1;
        }

        private void Reboot_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
