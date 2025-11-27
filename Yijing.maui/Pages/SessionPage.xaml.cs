using Yijing.Services;
using Yijing.Views;

namespace Yijing.Pages;

public partial class SessionPage : ContentPage
{
	public Editor SessionLog() => edtSessionLog;
	public WebView WebView() => webview;

	public SessionPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();

		if (App.Current!.RequestedTheme == AppTheme.Dark)
		{
			edtSessionLog.BackgroundColor = Colors.Black;
			vslInput.BackgroundColor = Colors.Black;
		}

		picAiChatService.SelectedIndex = AppPreferences.AiChatService;
		chbIncludeCast.IsChecked = true;

		horMenu.Create(ePages.eSession, StackOrientation.Horizontal);
		verMenu.Create(ePages.eSession, StackOrientation.Vertical);
	}

	private void Page_Loaded(object sender, EventArgs e)
	{
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

#if ANDROID || IOS
		edtSessionLog.HeightRequest = 100;
		if (width > height)
		{
			horMenu.IsVisible = false;
			verMenu.IsVisible = true;
			sessionView.WidthRequest = 200;
		}
		else
		{
			horMenu.IsVisible = true;
			verMenu.IsVisible = false;
			sessionView.WidthRequest = width;
		}
#else
		horMenu.IsVisible = false;
		verMenu.IsVisible = true;
#endif

		base.OnSizeAllocated(width, height);
	}

	private void picAiChatService_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.AiChatService = picAiChatService.SelectedIndex;
		//btnAiOrNote.Text = AppPreferences.AiChatService == (int)eAiService.eNone ? "Add Note" : "Ask AI";
		//btnAiOrNote.IsEnabled = AppPreferences.AiChatService != (int)eAiService.eNone;
	}

	private void SessionView_ChatUpdated(object sender, string html)
	{
		webview.Source = new HtmlWebViewSource { Html = html };
	}

	private void btnAiOrNote_Clicked(object sender, EventArgs e)
	{
		sessionView.AiOrNote(edtSessionLog.Text ?? string.Empty, chbIncludeCast.IsChecked);
		chbIncludeCast.IsChecked = false;
	}

	private void chbIncludeCast_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void edtSessionLog_Focused(object sender, FocusEventArgs e)
	{
		vslInput.HeightRequest = 200;
		edtSessionLog.HeightRequest = 160;
	}

	private void edtSessionLog_Unfocused(object sender, FocusEventArgs e)
	{
	}

	private void webview_Focused(object sender, FocusEventArgs e)
	{
		vslInput.HeightRequest = 100;
		edtSessionLog.HeightRequest = 100;
	}
}
