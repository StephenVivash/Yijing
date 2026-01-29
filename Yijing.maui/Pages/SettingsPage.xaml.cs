using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;

using Yijing.Services;
using Yijing.Views;

namespace Yijing.Pages;

public partial class SettingsPage : ContentPage
{
	public class AiServiceEditor : BindableObject
	{
		private string _name = string.Empty;
		private string _modelId = string.Empty;
		private string _endPoint = string.Empty;
		private string _key = string.Empty;

		public string Name
		{
			get => _name;
			set
			{
				if (_name == value)
					return;
				_name = value;
				OnPropertyChanged();
			}
		}

		public string ModelId
		{
			get => _modelId;
			set
			{
				if (_modelId == value)
					return;
				_modelId = value;
				OnPropertyChanged();
			}
		}

		public string EndPoint
		{
			get => _endPoint;
			set
			{
				if (_endPoint == value)
					return;
				_endPoint = value;
				OnPropertyChanged();
			}
		}

		public string Key
		{
			get => _key;
			set
			{
				if (_key == value)
					return;
				_key = value;
				OnPropertyChanged();
			}
		}
	}

	public ObservableCollection<AiServiceEditor> ServiceItems { get; } = new();

	public SettingsPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();
		BindingContext = this;

		if (App.Current!.RequestedTheme == AppTheme.Dark)
		{
			borDetails.BackgroundColor = Colors.Black;
			vslDetails.BackgroundColor = Colors.Black;
		}

		LoadAiPreferences();

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
			settingsView.WidthRequest = 200;
			settingsView.ButtonPadding(new Thickness(0, 5));
		}
		else
		{
			horMenu.IsVisible = true;
			verMenu.IsVisible = false;
			settingsView.WidthRequest = width;
			settingsView.ButtonPadding(new Thickness(5));
		}
#else
		horMenu.IsVisible = false;
		verMenu.IsVisible = true;
#endif

		base.OnSizeAllocated(width, height);
	}

	private void SettingsView_SectionSelected(object sender, SettingsSection section)
	{
		if (section == SettingsSection.AiPreferences)
		{
			lblSectionTitle.Text = "AI Preferences";
			lblSectionDescription.Text = "Configure AI provider defaults and services for chat and EEG analysis.";
		}
	}

	private async void SettingsView_SaveClicked(object sender, EventArgs e)
	{
		if (!await TryApplyPreferences())
			return;

		AiPreferences.Save();
		AppPreferences.AiChatService = AiPreferences.NormalizeServiceName(AppPreferences.AiChatService);
		AppPreferences.AiEegService = AiPreferences.NormalizeServiceName(AppPreferences.AiEegService);
	}

	private void SettingsView_ResetClicked(object sender, EventArgs e)
	{
		AiPreferences.Reset();
		LoadAiPreferences();
	}

	private async void OnAddServiceClicked(object sender, EventArgs e)
	{
		string name = edtNewServiceName.Text?.Trim() ?? string.Empty;
		if (string.IsNullOrWhiteSpace(name))
		{
			await DisplayAlert("AI Services", "Enter a service name before adding.", "OK");
			return;
		}

		if (ServiceItems.Any(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase)))
		{
			await DisplayAlert("AI Services", "That service already exists.", "OK");
			return;
		}

		ServiceItems.Add(new AiServiceEditor
		{
			Name = name,
			ModelId = string.Empty,
			EndPoint = string.Empty,
			Key = string.Empty
		});

		edtNewServiceName.Text = string.Empty;
	}

	private async void OnDeleteServiceClicked(object sender, EventArgs e)
	{
		if (sender is not Button button || button.BindingContext is not AiServiceEditor item)
			return;

		bool confirm = await DisplayAlert("Delete Service", $"Delete {item.Name}?", "Yes", "No");
		if (!confirm)
			return;

		ServiceItems.Remove(item);
	}

	private void LoadAiPreferences()
	{
		ServiceItems.Clear();

		foreach (var serviceName in AiPreferences.AiServiceNames)
		{
			if (!AiPreferences.AiServices.TryGetValue(serviceName, out var info))
				info = new AiPreferences.AiServiceInfo(string.Empty, string.Empty, string.Empty);

			ServiceItems.Add(new AiServiceEditor
			{
				Name = serviceName,
				ModelId = info.ModelId,
				EndPoint = info.EndPoint,
				Key = info.Key
			});
		}

		edtAiTemperature.Text = AiPreferences.AiTemperature.ToString(CultureInfo.InvariantCulture);
		edtAiTopP.Text = AiPreferences.AiTopP.ToString(CultureInfo.InvariantCulture);
		edtAiMaxTokens.Text = AiPreferences.AiMaxTokens.ToString(CultureInfo.InvariantCulture);
	}

	private async Task<bool> TryApplyPreferences()
	{
		if (!TryParseFloat(edtAiTemperature.Text, out float temperature))
		{
			await DisplayAlert("AI Preferences", "Temperature must be a number.", "OK");
			return false;
		}

		if (!TryParseFloat(edtAiTopP.Text, out float topP))
		{
			await DisplayAlert("AI Preferences", "Top P must be a number.", "OK");
			return false;
		}

		if (!int.TryParse(edtAiMaxTokens.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int maxTokens))
		{
			await DisplayAlert("AI Preferences", "Max Tokens must be a whole number.", "OK");
			return false;
		}

		var names = new List<string>();
		var serviceInfos = new Dictionary<string, AiPreferences.AiServiceInfo>(StringComparer.OrdinalIgnoreCase);
		var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		foreach (var item in ServiceItems)
		{
			string name = item.Name?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(name))
			{
				await DisplayAlert("AI Services", "Service names cannot be blank.", "OK");
				return false;
			}

			if (string.Equals(name, AiPreferences.AiServiceNone, StringComparison.OrdinalIgnoreCase))
			{
				await DisplayAlert("AI Services", "Service name 'None' is reserved.", "OK");
				return false;
			}

			if (!seen.Add(name))
			{
				await DisplayAlert("AI Services", $"Duplicate service name: {name}.", "OK");
				return false;
			}

			names.Add(name);
			serviceInfos[name] = new AiPreferences.AiServiceInfo(
				item.ModelId?.Trim() ?? string.Empty,
				item.EndPoint?.Trim() ?? string.Empty,
				item.Key ?? string.Empty);
		}

		AiPreferences.AiTemperature = temperature;
		AiPreferences.AiTopP = topP;
		AiPreferences.AiMaxTokens = maxTokens;
		AiPreferences.AiServiceNames = names.ToArray();
		AiPreferences.AiServices = serviceInfos;

		return true;
	}

	private static bool TryParseFloat(string? value, out float result)
	{
		if (float.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out result))
			return true;

		return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
	}
}
