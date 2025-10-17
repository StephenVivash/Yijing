
using Yijing.Pages;

namespace Yijing;

public partial class AppShell : Shell
{
	private static AppShell thisShell;
	public AppShell()
	{
		thisShell = this;
		InitializeComponent();

		//ShellContent.ContentTemplate = new DataTemplate { "local:Pages.DiagramPage" };
		//ShellContent.Route = "Pages.DiagramPage";

		DataTemplate x = new DataTemplate();// {   "local:Pages.DiagramPage" },
		ShellContent sc = new ShellContent()
		{
			Title = "Diagram",
			Icon = "iconblank.png",
			//ContentTemplate = new DataTemplate { "local:Pages.DiagramPage" },
			ContentTemplate = x,
			Route = "Pages.DiagramPage"
		};

		/*
		<ShellContent
			x:Name="pagDiagram"
			Shell.NavBarIsVisible="false"
			Title="Diagram"
			Icon="iconblank.png"
			ContentTemplate="{DataTemplate pages:DiagramPage}"
			Route="Pages.DiagramPage" />
		<ShellContent
			x:Name="pagEeg"
			Shell.NavBarIsVisible="false"
			Title="Eeg"
			Icon="iconlistdetail.png"
			ContentTemplate="{DataTemplate pages:EegPage}"
			Route="Pages.EegPage" />
		*/

		Routing.RegisterRoute(nameof(Pages.ListDetailDetailPage), typeof(Pages.ListDetailDetailPage));

		Routing.RegisterRoute(nameof(SessionPage), typeof(SessionPage));
		Routing.RegisterRoute(nameof(DiagramPage), typeof(DiagramPage));
		Routing.RegisterRoute(nameof(EegPage), typeof(EegPage));
		Routing.RegisterRoute(nameof(MeditationPage), typeof(MeditationPage));
	}

	private void Page_Loaded(object sender, EventArgs e)
	{
	}

	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	public static DiagramPage GetDiagramPage()
	{
		ShellContent x = thisShell.pagDiagram;
		return (DiagramPage)x.Content;
	}

	public static EegPage GetEegPage()
	{
		ShellContent x = thisShell.pagEeg;
		return (EegPage)x.Content;
	}

	public static MeditationPage GetMeditationPage()
	{
		ShellContent x = thisShell.pagMeditation;
		return (MeditationPage)x.Content;
	}

	public static App GetApp()
	{
		Window w = thisShell.Parent as Window;
		AppShell das = w.Page as AppShell;
		DiagramPage dp = das.CurrentPage as DiagramPage;
		App a = w.Parent as App;

		ShellContent sc1 = NameScopeExtensions.FindByName<ShellContent>(thisShell, "pagDiagram");
		ShellContent sc2 = NameScopeExtensions.FindByName<ShellContent>(thisShell, "pagEeg");
		Window w1 = sc1.Window;

		return a;
	}
	*/
}
