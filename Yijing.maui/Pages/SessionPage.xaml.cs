using Yijing.Services;

namespace Yijing.Pages;

public partial class SessionPage : ContentPage
{
	public Editor SessionLog() => edtSessionLog;
	public WebView WebView() => webview;

	public SessionPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();
		picAiChatService.SelectedIndex = AppPreferences.AiChatService;
		chbIncludeCast.IsChecked = true;
	}

	private void Page_Loaded(object sender, EventArgs e)
	{
	}

	private void picAiChatService_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.AiChatService = picAiChatService.SelectedIndex;
		btnAiOrNote.Text = AppPreferences.AiChatService == (int)eAiService.eNone ? "Add Note" : "Ask AI";
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

}
