using System.Collections.ObjectModel;
using System.Globalization;

using Yijing.Services;

namespace Yijing.Views;

public partial class SettingsView : ContentView
{
	private SettingsNode? _selectedNode;
	private string _selectedNodeTitle = string.Empty;
	private string _selectedNodeDescription = string.Empty;
	private bool _isAiPreferencesSelected;
	private string _aiTemperatureText = string.Empty;
	private string _aiTopPText = string.Empty;
	private string _aiMaxTokensText = string.Empty;
	private string _newServiceName = string.Empty;

	public ObservableCollection<SettingsNode> PropertyNodes { get; } = new();
	public ObservableCollection<AiServiceEditor> AiServices { get; } = new();

	public SettingsView()
	{
		InitializeComponent();
		BindingContext = this;

		BuildPropertyNodes();
		SelectedNode = PropertyNodes.FirstOrDefault(node => node.Section == SettingsSection.AiPreferences);
		propertyTree.SelectedItem = SelectedNode;
		LoadAiPreferences();
	}

	public SettingsNode? SelectedNode
	{
		get => _selectedNode;
		set
		{
			if (_selectedNode == value)
				return;

			_selectedNode = value;
			UpdateSelectedNode();
			OnPropertyChanged(nameof(SelectedNode));
		}
	}

	public string SelectedNodeTitle
	{
		get => _selectedNodeTitle;
		private set
		{
			if (_selectedNodeTitle == value)
				return;

			_selectedNodeTitle = value;
			OnPropertyChanged(nameof(SelectedNodeTitle));
		}
	}

	public string SelectedNodeDescription
	{
		get => _selectedNodeDescription;
		private set
		{
			if (_selectedNodeDescription == value)
				return;

			_selectedNodeDescription = value;
			OnPropertyChanged(nameof(SelectedNodeDescription));
		}
	}

	public bool IsAiPreferencesSelected
	{
		get => _isAiPreferencesSelected;
		private set
		{
			if (_isAiPreferencesSelected == value)
				return;

			_isAiPreferencesSelected = value;
			OnPropertyChanged(nameof(IsAiPreferencesSelected));
		}
	}

	public string AiTemperatureText
	{
		get => _aiTemperatureText;
		set
		{
			if (_aiTemperatureText == value)
				return;

			_aiTemperatureText = value;
			OnPropertyChanged(nameof(AiTemperatureText));
		}
	}

	public string AiTopPText
	{
		get => _aiTopPText;
		set
		{
			if (_aiTopPText == value)
				return;

			_aiTopPText = value;
			OnPropertyChanged(nameof(AiTopPText));
		}
	}

	public string AiMaxTokensText
	{
		get => _aiMaxTokensText;
		set
		{
			if (_aiMaxTokensText == value)
				return;

			_aiMaxTokensText = value;
			OnPropertyChanged(nameof(AiMaxTokensText));
		}
	}

	public string NewServiceName
	{
		get => _newServiceName;
		set
		{
			if (_newServiceName == value)
				return;

			_newServiceName = value;
			OnPropertyChanged(nameof(NewServiceName));
		}
	}

	private void BuildPropertyNodes()
	{
		PropertyNodes.Clear();
		PropertyNodes.Add(new SettingsNode("Application", "", 0, SettingsSection.None, false));
		PropertyNodes.Add(new SettingsNode(
			"AI Preferences",
			"Configure AI services and generation settings.",
			1,
			SettingsSection.AiPreferences,
			true));
	}

	private void UpdateSelectedNode()
	{
		if (_selectedNode is null)
			return;

		SelectedNodeTitle = _selectedNode.Title;
		SelectedNodeDescription = _selectedNode.Description;
		IsAiPreferencesSelected = _selectedNode.Section == SettingsSection.AiPreferences;
	}

	private void LoadAiPreferences()
	{
		AiTemperatureText = AiPreferences.AiTemperature.ToString(CultureInfo.InvariantCulture);
		AiTopPText = AiPreferences.AiTopP.ToString(CultureInfo.InvariantCulture);
		AiMaxTokensText = AiPreferences.AiMaxTokens.ToString(CultureInfo.InvariantCulture);
		NewServiceName = string.Empty;

		AiServices.Clear();
		foreach (var serviceName in AiPreferences.AiServiceNames)
		{
			var info = AiPreferences.AiService(serviceName);
			AiServices.Add(new AiServiceEditor
			{
				Name = serviceName,
				ModelId = info.ModelId,
				EndPoint = info.EndPoint,
				Key = info.Key
			});
		}
	}

	private void OnPropertySelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is SettingsNode node)
		{
			if (!node.IsSelectable)
			{
				propertyTree.SelectedItem = _selectedNode;
				return;
			}

			SelectedNode = node;
		}
	}

	private void OnSaveClicked(object sender, EventArgs e)
	{
		var serviceNames = new List<string>();
		var serviceMap = new Dictionary<string, AiPreferences.AiServiceInfo>(StringComparer.OrdinalIgnoreCase);

		foreach (var service in AiServices)
		{
			if (string.IsNullOrWhiteSpace(service.Name))
				continue;

			var name = service.Name.Trim();
			if (name.Equals(AiPreferences.AiServiceNone, StringComparison.OrdinalIgnoreCase))
				continue;

			if (serviceMap.ContainsKey(name))
				continue;

			serviceNames.Add(name);
			serviceMap[name] = new AiPreferences.AiServiceInfo(
				service.ModelId?.Trim() ?? string.Empty,
				service.EndPoint?.Trim() ?? string.Empty,
				service.Key ?? string.Empty);
		}

		AiPreferences.AiServiceNames = serviceNames.ToArray();
		AiPreferences.AiServices = serviceMap;
		AiPreferences.AiTemperature = ParseFloat(AiTemperatureText, AiPreferences.AiTemperature);
		AiPreferences.AiTopP = ParseFloat(AiTopPText, AiPreferences.AiTopP);
		AiPreferences.AiMaxTokens = ParseInt(AiMaxTokensText, AiPreferences.AiMaxTokens);

		AiPreferences.Save();
		AppPreferences.AiChatService = AiPreferences.NormalizeServiceName(AppPreferences.AiChatService);
		AppPreferences.AiEegService = AiPreferences.NormalizeServiceName(AppPreferences.AiEegService);
		LoadAiPreferences();
	}

	private void OnResetClicked(object sender, EventArgs e)
	{
		AiPreferences.Reset();
		AppPreferences.AiChatService = AiPreferences.NormalizeServiceName(AppPreferences.AiChatService);
		AppPreferences.AiEegService = AiPreferences.NormalizeServiceName(AppPreferences.AiEegService);
		LoadAiPreferences();
	}

	private void OnAddServiceClicked(object sender, EventArgs e)
	{
		var name = NewServiceName?.Trim();
		if (string.IsNullOrWhiteSpace(name))
			return;

		if (AiServices.Any(service => service.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
			return;

		AiServices.Add(new AiServiceEditor { Name = name });
		NewServiceName = string.Empty;
	}

	private void OnDeleteServiceClicked(object sender, EventArgs e)
	{
		if (sender is not Button button)
			return;

		if (button.CommandParameter is not AiServiceEditor service)
			return;

		AiServices.Remove(service);
	}

	private static float ParseFloat(string value, float fallback)
	{
		if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed))
			return parsed;

		if (float.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out parsed))
			return parsed;

		return fallback;
	}

	private static int ParseInt(string value, int fallback)
	{
		if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
			return parsed;

		if (int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsed))
			return parsed;

		return fallback;
	}

	
}

public enum SettingsSection
{
	None,
	AiPreferences
}

public class SettingsNode
{
	public SettingsNode(string title, string description, int level, SettingsSection section, bool isSelectable)
	{
		Title = title;
		Description = description;
		Section = section;
		IsSelectable = isSelectable;
		Indent = new Thickness(level * 16, 6, 6, 6);
	}

	public string Title { get; }
	public string Description { get; }
	public SettingsSection Section { get; }
	public bool IsSelectable { get; }
	public Thickness Indent { get; }
}

public class AiServiceEditor
{
	public string Name { get; set; } = string.Empty;
	public string ModelId { get; set; } = string.Empty;
	public string EndPoint { get; set; } = string.Empty;
	public string Key { get; set; } = string.Empty;
}
