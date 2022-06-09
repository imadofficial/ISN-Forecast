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

namespace ISN_Forecast.Win7
{
    /// <summary>
    /// Interaction logic for SearchedLocation.xaml
    /// </summary>
    public partial class SearchedLocation : Page
    {
        public static SearchedLocation Instance;
        public SearchedLocation()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainBox.Height = MainWindow.Instance.Screen.ActualHeight - 10;
            Statusbar.Instance.ActualPos.Text = MainWindow.Instance.Screen.ActualHeight.ToString();
        }
    }
}
