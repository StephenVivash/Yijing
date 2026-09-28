using Foundation;

namespace Yijing
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

		public override void WillTerminate(UIKit.UIApplication application)
		{
			Yijing.Services.MacDocumentFolderAccess.Shared.Dispose();
			base.WillTerminate(application);
		}
	}
}
