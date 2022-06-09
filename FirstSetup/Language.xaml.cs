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
        }

        #region Making every single button save a string globally.
        private void en_US(object sender, RoutedEventArgs e)
        {
            Configs.Language = "en_US";
            ApplySetting();
        }

        private void jp(object sender, RoutedEventArgs e)
        {
            Configs.Language = "jp";
            ApplySetting();
        }

        private void cn(object sender, RoutedEventArgs e)
        {
            Configs.Language = "cn";
            ApplySetting();
        }

        private void nl_BE(object sender, RoutedEventArgs e)
        {
            Configs.Language = "nl_BE";
            ApplySetting();
        }

        private void ru(object sender, RoutedEventArgs e)
        {
            Configs.Language = "ru";
            ApplySetting();
        }
        #endregion

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
                Setup.Instance.Extra.Text = Lang["Setup"]["ChangeLanguageLater"];
                Setup.Instance.Status.Text = Lang["Setup"]["Language Select"];
                Recommended.Text = Lang["Setup"]["Recommended"];
                Other.Text = Lang["Setup"]["OtherLang"];

                //string message = "Is the following information correct?\nLanguage:" + Configs.Language + "\nContent:" + Contents;
                //string title = "Verification";
                //System.Windows.MessageBox.Show(message, title);

            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Setup.Instance.MainContent.Content = new Updates();
        }
    }
}
