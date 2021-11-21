using System;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TestProject
{
    /// <summary>
    /// 메인 페이지
    /// </summary>
    public sealed partial class MainPage : Page
    {

        #region 생성자 - MainPage()
        public MainPage()
        {
            InitializeComponent();

            double width = 800d;
            double height = 600d;

            double dpi = (double)DisplayInformation.GetForCurrentView().LogicalDpi;

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Size windowSize = new Size(width * 96d / dpi, height * 96d / dpi);

            ApplicationView.PreferredLaunchViewSize = windowSize;

            Window.Current.Activate();

            ApplicationView.GetForCurrentView().TryResizeView(windowSize);

            ApplicationView.GetForCurrentView().Title = "Weather";

            Initialization();
        }

        public void Initialization()
        {
            var Settings = File.ReadAllText(@"Settings.json");
            dynamic config = JToken.Parse(Settings);

            if (config.NavPanelStyle == 0)
            {
                this.navigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top;
                this.navigationView.IsPaneOpen = false;
            }
            if (config.NavPanelStyle == 1)
            {
                this.navigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Left;
                this.navigationView.IsPaneOpen = true;
            }
            if (config.NavPanelStyle == 2)
            {
                this.navigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top;
                this.navigationView.IsPaneOpen = false;
            }
            if (config.NavPanelStyle == 3)
            {
                this.navigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.LeftCompact;
                this.navigationView.IsPaneOpen = false;
            }
        }

        #endregion

        #region 네비게이션 뷰 선택 변경시 처리하기 - navigationView_SelectionChanged(sender, e)
        private void navigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs e)
        {
            if (e.IsSettingsSelected)
            {
                this.frame.Navigate(typeof(SampleSettingPage));
            }
            else
            {
                Microsoft.UI.Xaml.Controls.NavigationViewItem item = e.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;

                string itemTag = item.Tag.ToString();

                string pageName = "TestProject." + itemTag;

                Type pageType = Type.GetType(pageName);

                this.frame.Navigate(pageType);
            }
        }

        #endregion
        #region 왼쪽 창 위치 라디오 버튼 체크시 처리하기 - leftPanePositionRadioButton_Checked(sender, e)
        public void leftPanePositionRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.navigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Left;
            this.navigationView.IsPaneOpen = true;
        }

        #endregion
        #region 위쪽 창 위치 라디오 버튼 체크시 처리하기 - topPanePositionRadioButton_Checked(sender, e)
        public void topPanePositionRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.navigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top;
            this.navigationView.IsPaneOpen = false;
        }

        #endregion
        #region 왼쪽 컴팩트 창 위치 라디오 버튼 체크시 처리하기 - leftCompactPanePositionRadioButton_Checked(sender, e)

        public void leftCompactPanePositionRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.navigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.LeftCompact;
            this.navigationView.IsPaneOpen = false;
        }

        #endregion
    }
}