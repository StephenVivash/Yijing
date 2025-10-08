
using Yijing.Services;
using Yijing.Views;

namespace Yijing.Pages;

public partial class DiagramPage : ContentPage
{
	//public Editor SessionLog() => edtSessionLog;
	public WebView WebView() => webview;

	public DiagramPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();

		//picAiChatService.SelectedIndex = AppPreferences.AiChatService;
		//picAiMode.SelectedIndex = 0;
		//chbIncludeCast.IsChecked = true;
	}

	private void Page_Loaded(object sender, EventArgs e)
	{
		//var button = new Button { Text = "New Bottom", HeightRequest = 200 };
		//DockLayout.SetDockPosition(button, DockPosition.Bottom);
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

#if ANDROID || IOS
		//edtSessionLog.HeightRequest = 100;
		if (width > height)
			diagramView.WidthRequest = 200;
		else
			diagramView.WidthRequest = 340;
#endif

		base.OnSizeAllocated(width, height);
	}

	private void webview_Navigating(object sender, WebNavigatingEventArgs e)
	{
#if WINDOWS
		string s = "https://appdir/Hexagram";
#elif ANDROID
		string s = "file:///android_asset/Hexagram";
#elif MACCATALYST
		string s = "file:///xxx/Hexagram";
#elif IOS
		string s = "file:///xxx/Hexagram";
#endif

		//if (e.NavigationEvent == WebNavigationEvent.Back)
		if (e.Url.StartsWith(s))
		{
			s = e.Url.Substring(s.Length, e.Url.Length - s.Length);
			bool b = false;
			UI.Call<DiagramView>(v => b = v.IsExploreMode());
			if (b && !string.IsNullOrEmpty(s))
				UI.Call<DiagramView>(v => v.SetHexagramValue(int.Parse(s)));
			e.Cancel = true;
		}
	}

	protected void picAiChatService_SelectedIndexChanged(object sender, EventArgs e)
	{
		//AppPreferences.AiChatService = picAiChatService.SelectedIndex;
		//if (AppPreferences.AiChatService != (int)eAiService.eNone)
		//!string.IsNullOrEmpty(AppPreferences.AiModelId[AppPreferences.AiChatService]) &&
		//(((AppPreferences.AiChatService == (int)eAiChatService.eOllama)) || 
		//!string.IsNullOrEmpty(AppPreferences.AiKey[AppPreferences.AiChatService])))
		//btnAskAi.IsEnabled = true;
		//else
		//btnAskAi.IsEnabled = false;
	}

	//protected void picAiMode_SelectedIndexChanged(object sender, EventArgs e)
	//{
	//	if (picAiMode.SelectedIndex == 1)
	//	{
	//MauiProgram.BuildKernelMemory();
	//DiagramView.UpdateSessionLog(" ***** Started Kernel Memory", true, true);
	//	}
	//}

	protected void btnAskAi_Clicked(object sender, EventArgs e)
	{
		//diagram.AiChat(chbIncludeCast.IsChecked);
		//chbIncludeCast.IsChecked = false;
	}

	private void chbIncludeCast_CheckedChanged(object sender, EventArgs e)
	{
	}

	/*
	private void webview_Navigated(object sender, WebNavigatedEventArgs e)
	{
		if (e.Result == WebNavigationResult.Success)
		{
			//BackButton.IsEnabled = webview.CanGoBack;
		}
		else
		{
			//await DisplayAlert("Navigation failed", e.Result.ToString(), "OK");
		}
	}

	private void Back_Clicked(object sender, EventArgs e)
	{
		webview.GoBack();
	}

	private void Forward_Clicked(object sender, EventArgs e)
	{
		webview.GoForward();
	}

	private void Refresh_Clicked(object sender, EventArgs e)
	{
		webview.Reload();
	}
	*/
}
