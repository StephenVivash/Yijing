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

                picAiChatService.SelectedIndex = AppPreferences.AiChatService;
                btnAskAi.IsEnabled = AppPreferences.AiChatService != (int)eAiService.eNone;
        }

        private void Page_Loaded(object sender, EventArgs e)
        {
        }

        private void SessionView_ChatUpdated(object sender, string html)
        {
                webview.Source = new HtmlWebViewSource { Html = html };
        }

        private async void btnAskAi_Clicked(object sender, EventArgs e)
        {
                await sessionView.AiChatAsync(edtSessionLog.Text ?? string.Empty);
                edtSessionLog.Text = string.Empty;
        }

        private void picAiChatService_SelectedIndexChanged(object sender, EventArgs e)
        {
                AppPreferences.AiChatService = picAiChatService.SelectedIndex;
                btnAskAi.IsEnabled = AppPreferences.AiChatService != (int)eAiService.eNone;
        }
}
