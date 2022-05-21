using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace Setup
{
    /// <summary>
    /// Interaction logic for EULA.xaml
    /// </summary>
    public partial class EULA : Page
    {
        static class GlobalStrings
        {
            public static String DisplayText;
        }

        public EULA()
        {
            InitializeComponent();
            DownloadEULA();
        }

        public async void DownloadEULA()
        {
            await Task.Delay(1000);
            WebClient webClient = new WebClient();
            webClient.DownloadStringAsync(new Uri("https://raw.githubusercontent.com/imadofficial/ISN-Forecast-Channel/main/EULA.txt"));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadCompleted);
        }

        private void DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            GlobalStrings.DisplayText = e.Result;
            EULA_TEXT.Text = GlobalStrings.DisplayText;

            LoadingScreen.Opacity = 0;
            EulaAccept.Opacity = 1;
            EulaDeny.Opacity = 1;
            NextBtn.Opacity = 1;
            NextBtn.IsEnabled = true;
        }

        public void Next(object sender, RoutedEventArgs e)
        {
            if (EulaAccept.IsChecked == true)
            {
                MainWindow.Instance.EULA.Opacity = 0.5;
                MainWindow.Instance.Edition.Opacity = 1;
                MainWindow.Instance.Main.Content = new Edition();
                
            }
            if (EulaDeny.IsChecked == true)
            {
                MessageBoxResult result = MessageBox.Show("You chose not to agree to the TOS, correct?", "EULA Manager", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MessageBox.Show("This setup will now close.", "EULA Manager");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
