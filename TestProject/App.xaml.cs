using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TestProject
{
    /// <summary>
    /// 앱
    /// </summary>
    sealed partial class App : Application
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - App()

        /// <summary>
        /// 생성자
        /// </summary>
        public App()
        {
            InitializeComponent();

            Suspending += Application_Suspending;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Protected

        #region 시작시 처리하기 - OnLaunched(e)

        /// <summary>
        /// 시작시 처리하기
        /// </summary>
        /// <param name="e">이벤트 인자</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += rootFrame_NavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                Window.Current.Activate();
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region 애플리케이션 정지시 처리하기 - Application_Suspending(sender, e)

        /// <summary>
        /// 애플리케이션 정지시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }

        #endregion
        #region 루트 프레임 네비게이션 실패시 처리하기 - rootFrame_NavigationFailed(sender, e)

        /// <summary>
        /// 루트 프레임 네비게이션 실패시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void rootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"페이지 로드를 실패했습니다 : {e.SourcePageType.FullName}");
        }

        #endregion
    }
}