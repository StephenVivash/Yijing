using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Yijing
{
	public sealed partial class MainPage : Page
	{
		public static MainPage m_mp;
		private static NavigationTransitionInfo m_nti = new SlideNavigationTransitionInfo();

		public MainPage()
		{
			m_mp = this;
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
			ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 640));
			SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
		}

		public static Frame MainFrame
		{
			get { return m_mp.frmMain; }
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			NavigationThemeTransition ntt = new NavigationThemeTransition();
			frmMain.ContentTransitions = new TransitionCollection();
			frmMain.ContentTransitions.Add(ntt);
			frmMain.Navigate(typeof(DiagramPage), null, m_nti);
			frmMain.Navigate(typeof(SettingsPage), null, m_nti);
			frmMain.Navigate(typeof(QuestionPage), null, m_nti);
		}

		private void Page_SizeChanged(object sender, RoutedEventArgs e)
		{
			if (ActualWidth < 360)
				spvMain.CompactPaneLength = 0;
			else
				spvMain.CompactPaneLength = 48;
		}

		private void btnHamburger_Click(object sender, RoutedEventArgs e)
		{
			spvMain.IsPaneOpen = !spvMain.IsPaneOpen;
		}

		private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
		{
			bool handled = e.Handled;
			BackRequested(ref handled);
			e.Handled = handled;
		}

		private void BackRequested(ref bool handled)
		{
			if (frmMain.CanGoBack && !handled)
			{
				handled = true;
				frmMain.GoBack();
			}
			if (!frmMain.CanGoBack)
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
		}

		private async void lbxIcons_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (lbxIcons.SelectedIndex == -1)
				return;
			if (!lbiWeb.IsSelected)
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

			if (lbiQuestion.IsSelected)
				frmMain.Navigate(typeof(QuestionPage), null, m_nti);
			else
			if (lbiDiagram.IsSelected)
				frmMain.Navigate(typeof(DiagramPage), null, m_nti);
			else
			if (lbiText.IsSelected)
				frmMain.Navigate(typeof(TextPage), null, m_nti);
			else
			if (lbiShare.IsSelected)
				DiagramPage.ShareCast();
			else
			if (lbiWeb.IsSelected)
			{
				await Launcher.LaunchUriAsync(new Uri("http://hermetica.info/"));
			}
			else
			if (lbiRate.IsSelected)
			{
				await Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productid=9n5q9qxxh7wj")); // 9wzdncrfj6qs 
				// bingmaps:?cp=40.726966~-74.006076 New York
				// bingmaps:?cp=-35.710013~150.18067 Home
				// ms-windows-store://navigatetopage/?Id=Apps
				// ms-windows-store://pdp/?productid=9wzdncrfj6qs 
			}
			else
			if (lbiSettings.IsSelected)
				frmMain.Navigate(typeof(SettingsPage), null, m_nti);

			if (spvMain.IsPaneOpen)
				spvMain.IsPaneOpen = !spvMain.IsPaneOpen;
			lbxIcons.SelectedIndex = -1;
		}

		public static void NavigateTo(Type typPage)
		{
			m_mp.frmMain.Navigate(typPage, null, m_nti);
		}

		public static void EnableControls(bool bEnable)
		{
			m_mp.lbiText.IsEnabled = bEnable;
			//m_mp.lbiSettings.IsEnabled = bEnable;
		}
	}
}
