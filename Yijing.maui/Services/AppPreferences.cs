
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
	public const string DiagramLsbKey = "DiagramLsb";
	public const string DiagramModeKey = "DiagramMode";
	public const string DiagramTypeKey = "DiagramType";
	public const string DiagramColorKey = "DiagramColor";
	public const string DiagramSpeedKey = "DiagramSpeed";
	public const string HexagramTextKey = "HexagramText";
	public const string HexagramLabelKey = "HexagramLabel";
	public const string HexagramSequenceKey = "HexagramSequence";
	public const string HexagramRatioKey = "HexagramRatio";
	public const string TrigramTextKey = "TrigramText";
	public const string TrigramLabelKey = "TrigramLabel";
	public const string TrigramSequenceKey = "TrigramSequence";
	public const string TrigramRatioKey = "TrigramRatio";
	public const string LineTextKey = "LineText";
	public const string LineLabelKey = "LineLabel";
	public const string LineSequenceKey = "LineSequence";
	public const string LineRatioKey = "LineRatio";
	public const string EegDeviceKey = "EegDevice";
	public const string EegGoalKey = "EegGoal";
	public const string AmbienceKey = "Ambience";
	public const string TimerKey = "Timer";
	public const string ReplaySpeedKey = "ReplaySpeed";
	public const string ChartBandsKey = "ChartBands";
	public const string ChartTimeKey = "ChartTime";
	public const string TriggerBandKey = "TriggerBand";
	public const string TriggerChannelKey = "TriggerChannel";
	public const string TriggerRangeKey = "TriggerRange";
	public const string TriggerHunterKey = "TriggerHunter";
	public const string TriggerScheduleKey = "TriggerSchedule";
	public const string TriggerSoundingKey = "TriggerSounding";
	public const string RawDataKey = "RawData";
	public const string AiChatServiceKey = "AiChatService";
	public const string AiEegServiceKey = "AiEegService";
	public const string AiEegMlModelKey = "AiEegMlModel";

	public static void Load()
	{

		DiagramLsb = Preferences.Get(DiagramLsbKey, Sequences.DiagramLsb);

		DiagramMode = Preferences.Get(DiagramModeKey, (int)eDiagramMode.eExplore);
		DiagramType = Preferences.Get(DiagramTypeKey, (int)eDiagramType.eHexagram);
		DiagramColor = Preferences.Get(DiagramColorKey, (int)eDiagramColor.eTrigram);
		DiagramSpeed = Preferences.Get(DiagramSpeedKey, (int)eDiagramSpeed.eMedium);

		HexagramText = Preferences.Get(HexagramTextKey, Sequences.HexagramText);
		HexagramLabel = Preferences.Get(HexagramLabelKey, Sequences.HexagramLabel);
		HexagramSequence = Preferences.Get(HexagramSequenceKey, Sequences.HexagramSequence);
		HexagramRatio = Preferences.Get(HexagramRatioKey, Sequences.HexagramRatio);

		TrigramText = Preferences.Get(TrigramTextKey, Sequences.TrigramText);
		TrigramLabel = Preferences.Get(TrigramLabelKey, Sequences.TrigramLabel);
		TrigramSequence = Preferences.Get(TrigramSequenceKey, Sequences.TrigramSequence);
		TrigramRatio = Preferences.Get(TrigramRatioKey, Sequences.TrigramRatio);

		LineText = Preferences.Get(LineTextKey, Sequences.LineText);
		LineLabel = Preferences.Get(LineLabelKey, Sequences.LineLabel);
		LineSequence = Preferences.Get(LineSequenceKey, Sequences.LineSequence);
		LineRatio = Preferences.Get(LineRatioKey, Sequences.LineRatio);

		EegDevice = Preferences.Get(EegDeviceKey, (int)eEegDevice.eMuse);
		EegGoal = Preferences.Get(EegGoalKey, (int)eGoal.eYijingCast);
		Ambience = Preferences.Get(AmbienceKey, (int)eAmbience.eLightRain);
		Timer = Preferences.Get(TimerKey, (int)eTimer.eTwenty);

		ReplaySpeed = Preferences.Get(ReplaySpeedKey, (int)eReplaySpeed.eNormal);
		ChartBands = Preferences.Get(ChartBandsKey, (int)eChartBands.eFront);
		ChartTime = Preferences.Get(ChartTimeKey, (int)eChartTime.eTwoAndHalf);

		TriggerBand = Preferences.Get(TriggerBandKey, (int)eTriggerBand.eGamma);
		TriggerChannel = Preferences.Get(TriggerChannelKey, (int)eTriggerChannel.eFrontLeft);
		TriggerRange = Preferences.Get(TriggerRangeKey, (int)eTriggerRange.eTwoFour);
		TriggerHunter = Preferences.Get(TriggerHunterKey, (int)eTriggerHunter.eFive);
		TriggerSchedule = Preferences.Get(TriggerScheduleKey, (int)eTriggerSchedule.eSixty);
		TriggerSounding = Preferences.Get(TriggerSoundingKey, true);
		RawData = Preferences.Get(RawDataKey, false);

		AiChatService = Preferences.Get(AiChatServiceKey, (int)eAiService.eNone);

		AiEegService = Preferences.Get(AiEegServiceKey, (int)eAiService.eNone);
		AiEegMlModel = Preferences.Get(AiEegMlModelKey, (int)eAiEegMlModel.eNone);
	}

	public static void Save()
	{
		Preferences.Set(DiagramLsbKey, DiagramLsb);
		Preferences.Set(DiagramModeKey, DiagramMode);
		Preferences.Set(DiagramTypeKey, DiagramType);
		Preferences.Set(DiagramColorKey, DiagramColor);
		Preferences.Set(DiagramSpeedKey, DiagramSpeed);
		Preferences.Set(HexagramTextKey, HexagramText);
		Preferences.Set(HexagramLabelKey, HexagramLabel);
		Preferences.Set(HexagramSequenceKey, HexagramSequence);
		Preferences.Set(HexagramRatioKey, HexagramRatio);
		Preferences.Set(TrigramTextKey, TrigramText);
		Preferences.Set(TrigramLabelKey, TrigramLabel);
		Preferences.Set(TrigramSequenceKey, TrigramSequence);
		Preferences.Set(TrigramRatioKey, TrigramRatio);
		Preferences.Set(LineTextKey, LineText);
		Preferences.Set(LineLabelKey, LineLabel);
		Preferences.Set(LineSequenceKey, LineSequence);
		Preferences.Set(LineRatioKey, LineRatio);
		Preferences.Set(EegDeviceKey, EegDevice);
		Preferences.Set(EegGoalKey, EegGoal);
		Preferences.Set(AmbienceKey, Ambience);
		Preferences.Set(TimerKey, Timer);
		Preferences.Set(ReplaySpeedKey, ReplaySpeed);
		Preferences.Set(ChartBandsKey, ChartBands);
		Preferences.Set(ChartTimeKey, ChartTime);
		Preferences.Set(TriggerBandKey, TriggerBand);
		Preferences.Set(TriggerChannelKey, TriggerChannel);
		Preferences.Set(TriggerRangeKey, TriggerRange);
		Preferences.Set(TriggerHunterKey, TriggerHunter);
		Preferences.Set(TriggerScheduleKey, TriggerSchedule);
		Preferences.Set(TriggerSoundingKey, TriggerSounding);
		Preferences.Set(RawDataKey, RawData);
		Preferences.Set(AiChatServiceKey, AiChatService);
		Preferences.Set(AiEegServiceKey, AiEegService);
		Preferences.Set(AiEegMlModelKey, AiEegMlModel);
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
	public static void Load()
	{

		var configuration = new ConfigurationBuilder()
			.SetBasePath(AppSettings.DocumentHome())
			.AddJsonFile("appsettings.json", optional: true)
			.Build();

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
