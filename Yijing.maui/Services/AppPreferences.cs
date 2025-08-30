
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

public enum eAiChatService { eOpenAi, eDeepseek, eGithub, eOllama, eNone };
public enum eAiAnalysis { eNone, eOpenAI };
public enum eAiModel { eNone, eStephenV, eJohnD };

public class AppPreferences
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

		AiAnalysis = Preferences.Get("AiAnalysis", (int)eAiAnalysis.eNone);
		AiModel = Preferences.Get("AiModel", (int)eAiModel.eStephenV);

		AiChatService = Preferences.Get("AiChatService", (int)eAiChatService.eNone);

		// gpt-4.5-preview o1-preview gpt-4o gpt-4o-mini
		// deepseek-reasoner deepseek-chat
		// gpt-4.1 grok-3 DeepSeek-V3-0324 Llama-4-Maverick-17B-128E-Instruct-FP8 Mistral-large-2407 Meta-Llama-3.1-405B-Instruct o1 o1-mini gpt-4o-mini
		// gpt-oss:20b qwen3:8b deepseek-r1:8b llama3.2:latest gemma3:latest

		// https://api.openai.com
		// https://api.deepseek.com
		// https://models.inference.ai.azure.com
		// http://localhost:11434


		AiModelId[(int)eAiChatService.eOpenAi] = configuration["OpenAI:Model"] ?? "";
		AiEndPoint[(int)eAiChatService.eOpenAi] = configuration["OpenAI:EndPoint"] ?? "";
		AiKey[(int)eAiChatService.eOpenAi] = configuration["OpenAI:Key"] ?? "";

		AiModelId[(int)eAiChatService.eDeepseek] = configuration["Deepseek:Model"] ?? "";
		AiEndPoint[(int)eAiChatService.eDeepseek] = configuration["Deepseek:EndPoint"] ?? "";
		AiKey[(int)eAiChatService.eDeepseek] = configuration["Deepseek:Key"] ?? "";

		AiModelId[(int)eAiChatService.eGithub] = configuration["Github:Model"] ?? "";
		AiEndPoint[(int)eAiChatService.eGithub] = configuration["Github:EndPoint"] ?? "";
		AiKey[(int)eAiChatService.eGithub] = configuration["Github:Key"] ?? "";

		AiModelId[(int)eAiChatService.eOllama] = configuration["Ollama:Model"] ?? "";
		AiEndPoint[(int)eAiChatService.eOllama] = configuration["Ollama:EndPoint"] ?? "";
		AiKey[(int)eAiChatService.eOllama] = configuration["Ollama:Key"] ?? "";


		OpenAiModelId = configuration["OpenAI:Model"] ?? "";
		OpenAiEndPoint = configuration["OpenAI:EndPoint"] ?? "";
		OpenAiKey = configuration["OpenAI:Key"] ?? "";

		DeepseekModelId = configuration["Deepseek:Model"] ?? "";
		DeepseekEndPoint = configuration["Deepseek:EndPoint"] ?? "";
		DeepseekKey = configuration["Deepseek:Key"] ?? "";

		GithubModelId = configuration["Github:Model"] ?? "";
		GithubEndPoint = configuration["Github:EndPoint"] ?? "";
		GithubKey = configuration["Github:Key"] ?? "";

		OllamaModelId = configuration["Ollama:Model"] ?? "";
		OllamaEndPoint = configuration["Ollama:EndPoint"] ?? "";
		OllamaKey = configuration["Ollama:Key"] ?? "";

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

	public static int AiAnalysis;
	public static int AiModel;

	public static int AiChatService;

	public static string[] AiModelId = ["", "", "", ""];
	public static string[] AiEndPoint = ["", "", "", ""];
	public static string[] AiKey = ["", "", "", ""];

	public static string OpenAiModelId;
	public static string DeepseekModelId;
	public static string GithubModelId;
	public static string OllamaModelId;

	public static string OpenAiEndPoint;
	public static string DeepseekEndPoint;
	public static string GithubEndPoint;
	public static string OllamaEndPoint;

	public static string OpenAiKey;
	public static string DeepseekKey;
	public static string GithubKey;
	public static string OllamaKey;

	public static int TriggerIndex;

	public static int MuseScale;
	public static int AudioScale;

}
