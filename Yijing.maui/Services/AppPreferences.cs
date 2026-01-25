
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

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

	public const int DiagramLsbDefault = 1;
	public const int DiagramModeDefault = (int)eDiagramMode.eExplore;
	public const int DiagramTypeDefault = (int)eDiagramType.eHexagram;
	public const int DiagramColorDefault = (int)eDiagramColor.eTrigram;
	public const int DiagramSpeedDefault = (int)eDiagramSpeed.eMedium;
	public const int HexagramTextDefault = 1;
	public const int HexagramLabelDefault = 9;
	public const int HexagramSequenceDefault = 2;
	public const int HexagramRatioDefault = 0;
	public const int TrigramTextDefault = 0;
	public const int TrigramLabelDefault = 2;
	public const int TrigramSequenceDefault = 1;
	public const int TrigramRatioDefault = 0;
	public const int LineTextDefault = 0;
	public const int LineLabelDefault = 3;
	public const int LineSequenceDefault = 1;
	public const int LineRatioDefault = 1;
	public const int EegDeviceDefault = (int)eEegDevice.eMuse;
	public const int EegGoalDefault = (int)eGoal.eYijingCast;
	public const int AmbienceDefault = (int)eAmbience.eLightRain;
	public const int TimerDefault = (int)eTimer.eTwenty;
	public const int ReplaySpeedDefault = (int)eReplaySpeed.eNormal;
	public const int ChartBandsDefault = (int)eChartBands.eFront;
	public const int ChartTimeDefault = (int)eChartTime.eTwoAndHalf;
	public const int TriggerBandDefault = (int)eTriggerBand.eGamma;
	public const int TriggerChannelDefault = (int)eTriggerChannel.eFrontLeft;
	public const int TriggerRangeDefault = (int)eTriggerRange.eTwoFour;
	public const int TriggerHunterDefault = (int)eTriggerHunter.eFive;
	public const int TriggerScheduleDefault = (int)eTriggerSchedule.eSixty;
	public const bool TriggerSoundingDefault = true;
	public const bool RawDataDefault = false;
	public const int AiChatServiceDefault = (int)eAiService.eNone;
	public const int AiEegServiceDefault = (int)eAiService.eNone;
	public const int AiEegMlModelDefault = (int)eAiEegMlModel.eNone;

	public static void Load()
	{

		DiagramLsb = Preferences.Get(DiagramLsbKey, DiagramLsbDefault);

		DiagramMode = Preferences.Get(DiagramModeKey, DiagramModeDefault);
		DiagramType = Preferences.Get(DiagramTypeKey, DiagramTypeDefault);
		DiagramColor = Preferences.Get(DiagramColorKey, DiagramColorDefault);
		DiagramSpeed = Preferences.Get(DiagramSpeedKey, DiagramSpeedDefault);

		HexagramText = Preferences.Get(HexagramTextKey, HexagramTextDefault);
		HexagramLabel = Preferences.Get(HexagramLabelKey, HexagramLabelDefault);
		HexagramSequence = Preferences.Get(HexagramSequenceKey, HexagramSequenceDefault);
		HexagramRatio = Preferences.Get(HexagramRatioKey, HexagramRatioDefault);

		TrigramText = Preferences.Get(TrigramTextKey, TrigramTextDefault);
		TrigramLabel = Preferences.Get(TrigramLabelKey, TrigramLabelDefault);
		TrigramSequence = Preferences.Get(TrigramSequenceKey, TrigramSequenceDefault);
		TrigramRatio = Preferences.Get(TrigramRatioKey, TrigramRatioDefault);

		LineText = Preferences.Get(LineTextKey, LineTextDefault);
		LineLabel = Preferences.Get(LineLabelKey, LineLabelDefault);
		LineSequence = Preferences.Get(LineSequenceKey, LineSequenceDefault);
		LineRatio = Preferences.Get(LineRatioKey, LineRatioDefault);

		EegDevice = Preferences.Get(EegDeviceKey, EegDeviceDefault);
		EegGoal = Preferences.Get(EegGoalKey, EegGoalDefault);
		Ambience = Preferences.Get(AmbienceKey, AmbienceDefault);
		Timer = Preferences.Get(TimerKey, TimerDefault);

		ReplaySpeed = Preferences.Get(ReplaySpeedKey, ReplaySpeedDefault);
		ChartBands = Preferences.Get(ChartBandsKey, ChartBandsDefault);
		ChartTime = Preferences.Get(ChartTimeKey, ChartTimeDefault);

		TriggerBand = Preferences.Get(TriggerBandKey, TriggerBandDefault);
		TriggerChannel = Preferences.Get(TriggerChannelKey, TriggerChannelDefault);
		TriggerRange = Preferences.Get(TriggerRangeKey, TriggerRangeDefault);
		TriggerHunter = Preferences.Get(TriggerHunterKey, TriggerHunterDefault);
		TriggerSchedule = Preferences.Get(TriggerScheduleKey, TriggerScheduleDefault);
		TriggerSounding = Preferences.Get(TriggerSoundingKey, TriggerSoundingDefault);
		RawData = Preferences.Get(RawDataKey, RawDataDefault);

		AiChatService = Preferences.Get(AiChatServiceKey, AiChatServiceDefault);

		AiEegService = Preferences.Get(AiEegServiceKey, AiEegServiceDefault);
		AiEegMlModel = Preferences.Get(AiEegMlModelKey, AiEegMlModelDefault);
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

	public static void Reset()
	{
		DiagramLsb = DiagramLsbDefault;
		DiagramMode = DiagramModeDefault;
		DiagramType = DiagramTypeDefault;
		DiagramColor = DiagramColorDefault;
		DiagramSpeed = DiagramSpeedDefault;
		HexagramText = HexagramTextDefault;
		HexagramLabel = HexagramLabelDefault;
		HexagramSequence = HexagramSequenceDefault;
		HexagramRatio = HexagramRatioDefault;
		TrigramText = TrigramTextDefault;
		TrigramLabel = TrigramLabelDefault;
		TrigramSequence = TrigramSequenceDefault;
		TrigramRatio = TrigramRatioDefault;
		LineText = LineTextDefault;
		LineLabel = LineLabelDefault;
		LineSequence = LineSequenceDefault;
		LineRatio = LineRatioDefault;
		EegDevice = EegDeviceDefault;
		EegGoal = EegGoalDefault;
		Ambience = AmbienceDefault;
		Timer = TimerDefault;
		ReplaySpeed = ReplaySpeedDefault;
		ChartBands = ChartBandsDefault;
		ChartTime = ChartTimeDefault;
		TriggerBand = TriggerBandDefault;
		TriggerChannel = TriggerChannelDefault;
		TriggerRange = TriggerRangeDefault;
		TriggerHunter = TriggerHunterDefault;
		TriggerSchedule = TriggerScheduleDefault;
		TriggerSounding = TriggerSoundingDefault;
		RawData = RawDataDefault;
		AiChatService = AiChatServiceDefault;
		AiEegService = AiEegServiceDefault;
		AiEegMlModel = AiEegMlModelDefault;
		Save();
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
