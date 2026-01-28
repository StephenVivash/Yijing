using Yijing.Services;
using Yijing.Views;

namespace Yijing.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();

		horMenu.Create(ePages.eSettings, StackOrientation.Horizontal);
		verMenu.Create(ePages.eSettings, StackOrientation.Vertical);
	}

	private void Page_Loaded(object sender, EventArgs e)
	{
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

#if ANDROID || IOS
		if (width > height)
		{
			horMenu.IsVisible = false;
			verMenu.IsVisible = true;
			settingsView.ButtonPadding(new Thickness(0, 5));
		}
		else
		{
			horMenu.IsVisible = true;
			verMenu.IsVisible = false;
			settingsView.ButtonPadding(new Thickness(5));
		}
#else
		horMenu.IsVisible = false;
		verMenu.IsVisible = true;
		settingsView.ButtonPadding(new Thickness(0, 5));
#endif
		settingsView.UpdateLayout(width, height);

		base.OnSizeAllocated(width, height);
	}
}
