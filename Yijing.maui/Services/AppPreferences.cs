
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

using ValueSequencer;
using YijingData;

namespace Yijing.Services;

#nullable enable

public enum eDiagramMode { eExplore, eAnimate, eTouchCast, eAutoCast, eMindCast };
public enum eDiagramType { eLine, eTrigram, eHexagram };
public enum eDiagramSpeed { eSlow, eMedium, eFast };
public enum eDiagramColor { eMono, eDual, eTrigram, eHexagram };

//public enum eEegDevice { eMuse, eEmotiv };
public enum eEegMode { eIdle, eLive, eReplay, eSummary };
public enum eGoal { eMeditation, eYijingCast };
public enum eTimer { eNone, eTen, eFifteen, eTwenty, eThirty, eSixty };
public enum eAmbience { eNone, eLightRain, eQuietForest, eMeadowBirds }; // , eKashmir, eMachineGun 
public enum eReplaySpeed { eNormal, eFast };
public enum eChartBands { eFront, eBack, eMixed, eAll };
public enum eChartTime { eTwoAndHalf, eFive };
public enum eTriggerBand { eDelta, eTheta, eAlpha, eBeta, eGamma };
public enum eTriggerChannel { eBackLeft, eFrontLeft, eBackCenter, eFrontRight, eBackRight };
public enum eTriggerRange { eZeroOne, eOneTwo, eTwoThree, eTwoFour, eThreeFour, eThreeFive, eFourFive, eFourSix };
public enum eTriggerHunter { eNone, eOne, eTwo, eThree, eFour, eFive, eSix, eSeven, eEight, eNine, eTen };
public enum eTriggerSchedule { eTwenty, eThirty, eForty, eSixty, eNinety, eOneTwenty };

public enum eAiService { eNone, eOpenAi, eDeepseek, eGithub, eOllama };
public enum eAiEegMlModel { eNone, eStephenV }; // eJohnD

public enum ePages { eNone, eSession, eDiagram, eEeg, eMeditation, eSettings, eAbout };

public static class AppPreferences
{
	private const string KeyDiagramLsb = "DiagramLsb";
	private const string KeyDiagramMode = "DiagramMode";
	private const string KeyDiagramType = "DiagramType";
	private const string KeyDiagramColor = "DiagramColor";
	private const string KeyDiagramSpeed = "DiagramSpeed";
	private const string KeyHexagramText = "HexagramText";
	private const string KeyHexagramLabel = "HexagramLabel";
	private const string KeyHexagramSequence = "HexagramSequence";
	private const string KeyHexagramRatio = "HexagramRatio";
	private const string KeyTrigramText = "TrigramText";
	private const string KeyTrigramLabel = "TrigramLabel";
	private const string KeyTrigramSequence = "TrigramSequence";
	private const string KeyTrigramRatio = "TrigramRatio";
	private const string KeyLineText = "LineText";
	private const string KeyLineLabel = "LineLabel";
	private const string KeyLineSequence = "LineSequence";
	private const string KeyLineRatio = "LineRatio";
	private const string KeyEegDevice = "EegDevice";
	private const string KeyEegGoal = "EegGoal";
	private const string KeyAmbience = "Ambience";
	private const string KeyTimer = "Timer";
	private const string KeyReplaySpeed = "ReplaySpeed";
	private const string KeyChartBands = "ChartBands";
	private const string KeyChartTime = "ChartTime";
	private const string KeyTriggerBand = "TriggerBand";
	private const string KeyTriggerChannel = "TriggerChannel";
	private const string KeyTriggerRange = "TriggerRange";
	private const string KeyTriggerHunter = "TriggerHunter";
	private const string KeyTriggerSchedule = "TriggerSchedule";
	private const string KeyTriggerSounding = "TriggerSounding";
	private const string KeyRawData = "RawData";
	private const string KeyAiChatService = "AiChatService";
	private const string KeyAiEegService = "AiEegService";
	private const string KeyAiEegMlModel = "AiEegMlModel";

	public static void Load()
	{

		var configuration = new ConfigurationBuilder()
			.SetBasePath(AppSettings.DocumentHome())
			.AddJsonFile("appsettings.json", optional: true)
			.Build();

		DiagramLsb = Preferences.Get(KeyDiagramLsb, Sequences.DiagramLsb);

		DiagramMode = Preferences.Get(KeyDiagramMode, (int)eDiagramMode.eExplore);
		DiagramType = Preferences.Get(KeyDiagramType, (int)eDiagramType.eHexagram);
		DiagramColor = Preferences.Get(KeyDiagramColor, (int)eDiagramColor.eTrigram);
		DiagramSpeed = Preferences.Get(KeyDiagramSpeed, (int)eDiagramSpeed.eMedium);

		HexagramText = Preferences.Get(KeyHexagramText, Sequences.HexagramText);
		HexagramLabel = Preferences.Get(KeyHexagramLabel, Sequences.HexagramLabel);
		HexagramSequence = Preferences.Get(KeyHexagramSequence, Sequences.HexagramSequence);
		HexagramRatio = Preferences.Get(KeyHexagramRatio, Sequences.HexagramRatio);

		TrigramText = Preferences.Get(KeyTrigramText, Sequences.TrigramText);
		TrigramLabel = Preferences.Get(KeyTrigramLabel, Sequences.TrigramLabel);
		TrigramSequence = Preferences.Get(KeyTrigramSequence, Sequences.TrigramSequence);
		TrigramRatio = Preferences.Get(KeyTrigramRatio, Sequences.TrigramRatio);

		LineText = Preferences.Get(KeyLineText, Sequences.LineText);
		LineLabel = Preferences.Get(KeyLineLabel, Sequences.LineLabel);
		LineSequence = Preferences.Get(KeyLineSequence, Sequences.LineSequence);
		LineRatio = Preferences.Get(KeyLineRatio, Sequences.LineRatio);

		EegDevice = Preferences.Get(KeyEegDevice, (int)eEegDevice.eMuse);
		EegGoal = Preferences.Get(KeyEegGoal, (int)eGoal.eYijingCast);
		Ambience = Preferences.Get(KeyAmbience, (int)eAmbience.eLightRain);
		Timer = Preferences.Get(KeyTimer, (int)eTimer.eTwenty);

		ReplaySpeed = Preferences.Get(KeyReplaySpeed, (int)eReplaySpeed.eNormal);
		ChartBands = Preferences.Get(KeyChartBands, (int)eChartBands.eFront);
		ChartTime = Preferences.Get(KeyChartTime, (int)eChartTime.eTwoAndHalf);

		TriggerBand = Preferences.Get(KeyTriggerBand, (int)eTriggerBand.eGamma);
		TriggerChannel = Preferences.Get(KeyTriggerChannel, (int)eTriggerChannel.eFrontLeft);
		TriggerRange = Preferences.Get(KeyTriggerRange, (int)eTriggerRange.eTwoFour);
		TriggerHunter = Preferences.Get(KeyTriggerHunter, (int)eTriggerHunter.eFive);
		TriggerSchedule = Preferences.Get(KeyTriggerSchedule, (int)eTriggerSchedule.eSixty);
		TriggerSounding = Preferences.Get(KeyTriggerSounding, true);
		RawData = Preferences.Get(KeyRawData, false);

		AiChatService = Preferences.Get(KeyAiChatService, (int)eAiService.eNone);

		AiEegService = Preferences.Get(KeyAiEegService, (int)eAiService.eNone);
		AiEegMlModel = Preferences.Get(KeyAiEegMlModel, (int)eAiEegMlModel.eNone);

		AiPreferences.Load(configuration);

		AppSettings.MuseScale = 1;
		AppSettings.AudioScale = 1;
	}

	public static void Save()
	{
		Preferences.Set(KeyDiagramLsb, DiagramLsb);
		Preferences.Set(KeyDiagramMode, DiagramMode);
		Preferences.Set(KeyDiagramType, DiagramType);
		Preferences.Set(KeyDiagramColor, DiagramColor);
		Preferences.Set(KeyDiagramSpeed, DiagramSpeed);
		Preferences.Set(KeyHexagramText, HexagramText);
		Preferences.Set(KeyHexagramLabel, HexagramLabel);
		Preferences.Set(KeyHexagramSequence, HexagramSequence);
		Preferences.Set(KeyHexagramRatio, HexagramRatio);
		Preferences.Set(KeyTrigramText, TrigramText);
		Preferences.Set(KeyTrigramLabel, TrigramLabel);
		Preferences.Set(KeyTrigramSequence, TrigramSequence);
		Preferences.Set(KeyTrigramRatio, TrigramRatio);
		Preferences.Set(KeyLineText, LineText);
		Preferences.Set(KeyLineLabel, LineLabel);
		Preferences.Set(KeyLineSequence, LineSequence);
		Preferences.Set(KeyLineRatio, LineRatio);
		Preferences.Set(KeyEegDevice, EegDevice);
		Preferences.Set(KeyEegGoal, EegGoal);
		Preferences.Set(KeyAmbience, Ambience);
		Preferences.Set(KeyTimer, Timer);
		Preferences.Set(KeyReplaySpeed, ReplaySpeed);
		Preferences.Set(KeyChartBands, ChartBands);
		Preferences.Set(KeyChartTime, ChartTime);
		Preferences.Set(KeyTriggerBand, TriggerBand);
		Preferences.Set(KeyTriggerChannel, TriggerChannel);
		Preferences.Set(KeyTriggerRange, TriggerRange);
		Preferences.Set(KeyTriggerHunter, TriggerHunter);
		Preferences.Set(KeyTriggerSchedule, TriggerSchedule);
		Preferences.Set(KeyTriggerSounding, TriggerSounding);
		Preferences.Set(KeyRawData, RawData);
		Preferences.Set(KeyAiChatService, AiChatService);
		Preferences.Set(KeyAiEegService, AiEegService);
		Preferences.Set(KeyAiEegMlModel, AiEegMlModel);
	}

	public static int DiagramLsb;

	public static int DiagramMode;
	public static int DiagramType;
	public static int DiagramColor;
	public static int DiagramSpeed;

	public static int HexagramText;
	public static int HexagramLabel;
	public static int HexagramSequence;
	public static int HexagramRatio;

	public static int TrigramText;
	public static int TrigramLabel;
	public static int TrigramSequence;
	public static int TrigramRatio;

	public static int LineText;
	public static int LineLabel;
	public static int LineSequence;
	public static int LineRatio;

	public static int EegDevice;
	public static int EegGoal;

	public static int Ambience;
	public static int Timer;

	public static int ReplaySpeed;
	public static int ChartBands;
	public static int ChartTime;
	public static int TriggerBand;
	public static int TriggerChannel;
	public static int TriggerRange;

	public static int TriggerHunter;
	public static int TriggerSchedule;
	public static bool TriggerSounding;
	public static bool RawData;

	public static int AiChatService;

	public static int AiEegService;
	public static int AiEegMlModel;
}

public static class AiPreferences
{
	public static void Load(IConfiguration configuration)
	{
		if (float.TryParse(configuration["AI:Temperature"], out float temp1))
			AiTemperature = temp1;
		else
			AiTemperature = 1.0f;
		if (float.TryParse(configuration["AI:TopP"], out float temp2))
			AiTopP = temp2;
		else
			AiTopP = 1.0f;
		if (int.TryParse(configuration["AI:MaxTokens"], out int temp3))
			AiMaxTokens = temp3;
		else
			AiMaxTokens = 10240;

		// gpt-4.5-preview o1-preview gpt-4o gpt-4o-mini
		// deepseek-reasoner deepseek-chat
		// gpt-4.1 grok-3 DeepSeek-V3-0324 Llama-4-Maverick-17B-128E-Instruct-FP8 Mistral-large-2407 Meta-Llama-3.1-405B-Instruct o1 o1-mini gpt-4o-mini
		// gpt-oss:20b qwen3:8b deepseek-r1:8b llama3.2:latest gemma3:latest

		AiModelId[(int)eAiService.eOpenAi - 1] = configuration["AI:Providers:OpenAI:Model"] ?? "";
		AiEndPoint[(int)eAiService.eOpenAi - 1] = configuration["AI:Providers:OpenAI:EndPoint"] ?? "";
		AiKey[(int)eAiService.eOpenAi - 1] = configuration["AI:Providers:OpenAI:Key"] ?? "";

		AiModelId[(int)eAiService.eDeepseek - 1] = configuration["AI:Providers:Deepseek:Model"] ?? "";
		AiEndPoint[(int)eAiService.eDeepseek - 1] = configuration["AI:Providers:Deepseek:EndPoint"] ?? "";
		AiKey[(int)eAiService.eDeepseek - 1] = configuration["AI:Providers:Deepseek:Key"] ?? "";

		AiModelId[(int)eAiService.eGithub - 1] = configuration["AI:Providers:Github:Model"] ?? "";
		AiEndPoint[(int)eAiService.eGithub - 1] = configuration["AI:Providers:Github:EndPoint"] ?? "";
		AiKey[(int)eAiService.eGithub - 1] = configuration["AI:Providers:Github:Key"] ?? "";

		AiModelId[(int)eAiService.eOllama - 1] = configuration["AI:Providers:Ollama:Model"] ?? "";
		AiEndPoint[(int)eAiService.eOllama - 1] = configuration["AI:Providers:Ollama:EndPoint"] ?? "";
		AiKey[(int)eAiService.eOllama - 1] = configuration["AI:Providers:Ollama:Key"] ?? "";

		SaveNewDefaults();
	}

	private static void SaveNewDefaults()
	{
		try
		{
			string settingsPath = Path.Combine(AppSettings.DocumentHome(), "appsettings.json");
			JsonObject rootObject = LoadConfigurationObject(settingsPath);
			JsonObject aiObject = rootObject["AI"] as JsonObject ?? new JsonObject();
			rootObject["AI"] = aiObject;
			JsonObject providersObject = aiObject["Providers"] as JsonObject ?? new JsonObject();
			aiObject["Providers"] = providersObject;

			bool save = false;

			JsonObject openAIObject = providersObject["OpenAI"] as JsonObject ?? new JsonObject();
			providersObject["OpenAI"] = openAIObject;
			if (string.IsNullOrWhiteSpace((string?)openAIObject["EndPoint"]))
			{
				openAIObject["EndPoint"] = DefaultOpenAiEndpoint;
				AiEndPoint[(int)eAiService.eOpenAi] = DefaultOpenAiEndpoint;
				save = true;
			}

			JsonObject ollamaObject = providersObject["Ollama"] as JsonObject ?? new JsonObject();
			providersObject["Ollama"] = ollamaObject;
			if (string.IsNullOrWhiteSpace((string?)ollamaObject["Key"]))
			{
				ollamaObject["Key"] = DefaultOllamaKey;
				AiKey[(int)eAiService.eOllama] = DefaultOllamaKey;
				save = true;
			}

			var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
			if (save)
				File.WriteAllText(settingsPath, rootObject.ToJsonString(jsonOptions));
		}
		catch (Exception) { }
	}

	private static JsonObject LoadConfigurationObject(string settingsPath)
	{
		if (File.Exists(settingsPath))
		{
			string json = File.ReadAllText(settingsPath);
			if (!string.IsNullOrWhiteSpace(json))
			{
				try
				{
					JsonNode? parsed = JsonNode.Parse(json);
					if (parsed is JsonObject jsonObject)
						return jsonObject;
				}
				catch (JsonException)
				{
				}
			}
		}
		return new JsonObject();
	}

	public static float AiTemperature;
	public static float AiTopP;
	public static int AiMaxTokens;

	public static string[] AiModelId = ["", "", "", ""];
	public static string[] AiEndPoint = ["", "", "", ""];
	public static string[] AiKey = ["", "", "", ""];

	private const string DefaultOllamaKey = "Ollama";
	private const string DefaultOpenAiEndpoint = "https://api.openai.com/v1";
}
