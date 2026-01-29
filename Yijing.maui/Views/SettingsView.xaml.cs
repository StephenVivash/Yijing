using System.Collections.ObjectModel;

namespace Yijing.Views;

public enum SettingsSection
{
	AiPreferences
}

public class SettingsTreeItem
{
	public string Title { get; }
	public SettingsSection Section { get; }
	public FontAttributes FontAttributes { get; }
	public Thickness ItemMargin { get; }

	public SettingsTreeItem(string title, SettingsSection section, FontAttributes fontAttributes, Thickness itemMargin)
	{
		Title = title;
		Section = section;
		FontAttributes = fontAttributes;
		ItemMargin = itemMargin;
	}
}

public partial class SettingsView : ContentView
{
	public ObservableCollection<SettingsTreeItem> TreeItems { get; } = new();

	public event EventHandler<SettingsSection>? SectionSelected;
	public event EventHandler? SaveClicked;
	public event EventHandler? ResetClicked;

	public SettingsView()
	{
		InitializeComponent();
		BindingContext = this;

		if (App.Current!.RequestedTheme == AppTheme.Dark)
		{
			hslButtons.BackgroundColor = Colors.Black;
			settingsTree.BackgroundColor = Colors.Black;
		}

		TreeItems.Add(new SettingsTreeItem("Application", SettingsSection.AiPreferences, FontAttributes.Bold, new Thickness(10, 12, 0, 6)));
		TreeItems.Add(new SettingsTreeItem("AI Preferences", SettingsSection.AiPreferences, FontAttributes.None, new Thickness(28, 6, 0, 6)));

		settingsTree.SelectedItem = TreeItems[1];
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

		double w = width - 10;

		w = width - 40;

		w /= 2;

		w /= 2;
		btnSave.WidthRequest = w;
		btnReset.WidthRequest = w;

		base.OnSizeAllocated(width, height);
	}

	public void ButtonPadding(Thickness thickness)
	{
		btnSave.Padding = thickness;
		btnReset.Padding = thickness;
	}

	private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is not SettingsTreeItem item)
			return;

		SectionSelected?.Invoke(this, item.Section);
	}

	private void OnSaveClicked(object? sender, EventArgs e)
	{
		SaveClicked?.Invoke(this, EventArgs.Empty);
	}

	private void OnResetClicked(object? sender, EventArgs e)
	{
		ResetClicked?.Invoke(this, EventArgs.Empty);
	}
}
