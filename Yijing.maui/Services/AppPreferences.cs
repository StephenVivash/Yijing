
using Microsoft.Extensions.Configuration;
using ValueSequencer;

namespace Yijing.Services;

public enum eDiagramMode { eExplore, eAnimate, eTouchCast, eAutoCast, eMindCast };
public enum eDiagramType { eLine, eTrigram, eHexagram };
public enum eDiagramSpeed { eSlow, eMedium, eFast };
public enum eDiagramColor { eMono, eDual, eTrigram, eHexagram };

public enum eEegDevice { eMuse, eEmotiv };
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

public enum eAiService { eOpenAi, eDeepseek, eGithub, eOllama, eNone };
//public enum eAiAnalysis { eNone, eOpenAI };
public enum eAiEegMlModel { eStephenV, eNone }; // eJohnD

public static class AppPreferences
{
	public static void Load()
	{

		var configuration = new ConfigurationBuilder()
			.SetBasePath(AppSettings.DocumentHome())
			.AddJsonFile("appsettings.json", optional: true)
			.Build();

		DiagramLsb = Preferences.Get("DiagramLsb", Sequences.DiagramLsb);

		DiagramMode = Preferences.Get("DiagramMode", (int)eDiagramMode.eExplore);
		DiagramType = Preferences.Get("DiagramType", (int)eDiagramType.eHexagram);
		DiagramColor = Preferences.Get("DiagramColor", (int)eDiagramColor.eTrigram);
		DiagramSpeed = Preferences.Get("DiagramSpeed", (int)eDiagramSpeed.eMedium);

		HexagramText = Preferences.Get("HexagramText", Sequences.HexagramText);
		HexagramLabel = Preferences.Get("HexagramLabel", Sequences.HexagramLabel);
		HexagramSequence = Preferences.Get("HexagramSequence", Sequences.HexagramSequence);
		HexagramRatio = Preferences.Get("HexagramRatio", Sequences.HexagramRatio);

		TrigramText = Preferences.Get("TrigramText", Sequences.TrigramText);
		TrigramLabel = Preferences.Get("TrigramLabel", Sequences.TrigramLabel);
		TrigramSequence = Preferences.Get("TrigramSequence", Sequences.TrigramSequence);
		TrigramRatio = Preferences.Get("TrigramRatio", Sequences.TrigramRatio);

		LineText = Preferences.Get("LineText", Sequences.LineText);
		LineLabel = Preferences.Get("LineLabel", Sequences.LineLabel);
		LineSequence = Preferences.Get("LineSequence", Sequences.LineSequence);
		LineRatio = Preferences.Get("LineRatio", Sequences.LineRatio);

		EegDevice = Preferences.Get("EegDevice", (int)eEegDevice.eMuse);
		EegGoal = Preferences.Get("EegGoal", (int)eGoal.eMeditation);
		Ambience = Preferences.Get("Ambience", (int)eAmbience.eLightRain);
		Timer = Preferences.Get("Timer", (int)eTimer.eTen);

		ReplaySpeed = Preferences.Get("ReplaySpeed", (int)eReplaySpeed.eNormal);
		ChartBands = Preferences.Get("ChartBands", (int)eChartBands.eFront);
		ChartTime = Preferences.Get("ChartTime", (int)eChartTime.eTwoAndHalf);

		TriggerBand = Preferences.Get("TriggerBand", (int)eTriggerBand.eGamma);
		TriggerChannel = Preferences.Get("TriggerChannel", (int)eTriggerChannel.eFrontLeft);
		TriggerRange = Preferences.Get("TriggerRange", (int)eTriggerRange.eFourSix);

		TriggerFixed = Preferences.Get("TriggerFixed", false);
		TriggerSounding = Preferences.Get("TriggerSounding", true);
		RawData = Preferences.Get("RawData", false);

		AiChatService = Preferences.Get("AiChatService", (int)eAiService.eNone);
		
		AiEegService = Preferences.Get("AiEegService", (int)eAiService.eNone);
		AiEegMlModel = Preferences.Get("AiEegMlModel", (int)eAiEegMlModel.eNone);

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

		AiModelId[(int)eAiService.eOpenAi] = configuration["AI:Providers:OpenAI:Model"] ?? "";
		AiEndPoint[(int)eAiService.eOpenAi] = configuration["AI:Providers:OpenAI:EndPoint"] ?? "";
		AiKey[(int)eAiService.eOpenAi] = configuration["AI:Providers:OpenAI:Key"] ?? "";

		AiModelId[(int)eAiService.eDeepseek] = configuration["AI:Providers:Deepseek:Model"] ?? "";
		AiEndPoint[(int)eAiService.eDeepseek] = configuration["AI:Providers:Deepseek:EndPoint"] ?? "";
		AiKey[(int)eAiService.eDeepseek] = configuration["AI:Providers:Deepseek:Key"] ?? "";

		AiModelId[(int)eAiService.eGithub] = configuration["AI:Providers:Github:Model"] ?? "";
		AiEndPoint[(int)eAiService.eGithub] = configuration["AI:Providers:Github:EndPoint"] ?? "";
		AiKey[(int)eAiService.eGithub] = configuration["AI:Providers:Github:Key"] ?? "";

		AiModelId[(int)eAiService.eOllama] = configuration["AI:Providers:Ollama:Model"] ?? "";
		AiEndPoint[(int)eAiService.eOllama] = configuration["AI:Providers:Ollama:EndPoint"] ?? "";
		AiKey[(int)eAiService.eOllama] = configuration["AI:Providers:Ollama:Key"] ?? "";

		MuseScale = 1;
		AudioScale = 10;
	}

	public static void Save()
	{
		Preferences.Set("DiagramLsb", DiagramLsb);
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

	public static bool TriggerFixed;
	public static bool TriggerSounding;
	public static bool RawData;

	public static int AiChatService;

	public static int AiEegService;
	public static int AiEegMlModel;

	public static float AiTemperature;
	public static float AiTopP;
	public static int AiMaxTokens;

	public static string[] AiModelId = ["", "", "", ""];
	public static string[] AiEndPoint = ["", "", "", ""];
	public static string[] AiKey = ["", "", "", ""];

	public static int TriggerIndex;

	public static int MuseScale;
	public static int AudioScale;

}
