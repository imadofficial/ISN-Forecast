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
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Net;

namespace Setup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            InitializeComponent();
            Check();
            Prepare();
            Instance = this;
        }

        public void EULALoad()
        {
            
        }

        public void Prepare()
        {

            Main.Content = new Welcome();
        }

        public void Check()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            var buildNumber = registryKey.GetValue("CurrentBuildNumber").ToString();

            if (buildNumber == "19044" || buildNumber == "19043" || buildNumber == "19042" || buildNumber == "19041" || buildNumber == "18363" || buildNumber == "18362")
            {
                var Edition = registryKey.GetValue("EditionID").ToString();
                if (Edition == "ServerStandard")
                {

                }
            }

            else
            {
                if (TaskDialog.OSSupportsTaskDialogs)
                {
                    using (TaskDialog dialog = new TaskDialog())
                    {
                        dialog.WindowTitle = "Unsupported OS";
                        dialog.MainInstruction = "Unfortunately, your Windows version is unsupported.";
                        dialog.Content = "This usually happens when ISN Studios doesn't provide direct support for Windows. If this is a new Windows feature update, chances are that ISN Forecast will be updated soon.";
                        dialog.ExpandedInformation = "You can still bypass this warning. But ISN is NOT responsible for the damage that we did your Windows install and/or your hardware. If you'd like to ignore this warning, feel free to click the \"Run anyway\" button";
                        dialog.Footer = "Regardless of what you pick. UWP versions will be disabled for download.";
                        dialog.FooterIcon = TaskDialogIcon.Information;
                        dialog.EnableHyperlinks = true;
                        TaskDialogButton customButton = new TaskDialogButton("View compatibility list");
                        TaskDialogButton okButton = new TaskDialogButton("Run anyway");
                        TaskDialogButton cancelButton = new TaskDialogButton("Exit");
                        dialog.Buttons.Add(customButton);
                        dialog.Buttons.Add(okButton);
                        dialog.Buttons.Add(cancelButton);
                        //dialog.HyperlinkClicked += new EventHandler<HyperlinkClickedEventArgs>(TaskDialog_HyperLinkClicked);
                        TaskDialogButton button = dialog.ShowDialog(this);
                        if (button == customButton)
                        {
                            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/1-3bqIhhqXw9Vlp-XyIGzD--45fjVxLoT69nf8Lg0UmM/edit?usp=sharing");
                            Environment.Exit(0);
                        }
                        else if (button == cancelButton)
                            Environment.Exit(0);
                    }
                }
                else
                {
                    var result = MessageBox.Show("This version of Windows™ is not officially supported. Are you sure you want to continue?", "Unsupported Operating System", MessageBoxButton.YesNo, MessageBoxImage.Question);


                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            break;

                        case MessageBoxResult.No:
                            Environment.Exit(0);
                            break;
                    }
                }
            }
        }
    }
}
