
// dotnet nuget why Yijing.maui.csproj Microsoft.SemanticKernel

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory;

//using Microsoft.SemanticKernel.Embeddings;
//using Microsoft.SemanticKernel.Connectors.Ollama;
//using OllamaSharp;
//using OllamaSharp.Models.Chat;
//using Microsoft.Extensions.Azure;
//using Microsoft.SemanticKernel.Memory;
//using OpenAI;
//using OpenAI.Chat;
//using Microsoft.Extensions.AI;

//using Microsoft.SemanticKernel;
//using Microsoft.SemanticKernel.Plugins.Core;
//using Microsoft.SemanticKernel.Connectors.OpenAI;

using CommunityToolkit.Maui;

using SkiaSharp.Views.Maui.Controls.Hosting;

using Yijing.Services;
using Yijing.Views;
using LiveChartsCore.SkiaSharpView.Maui;

namespace Yijing;

public class YijingKernel
{

	//[KernelFunction]
	//public void SelectHexagram(int number)
	//{
	//	DiagramView.SetHexagramValue(number);
	//}

	//[KernelFunction("auto_cast_hexagram")]
	public static void auto_cast_hexagram()
	{
		DiagramView.AutoCastHexagram();
	}

	//[KernelFunction("set_hexagram")]
	public static void set_hexagram(int sequence)
	{
		DiagramView.SetHexagram(sequence);
	}

	//[KernelFunction("get_hexagram")]
	public static int get_hexagram()
	{
		return DiagramView.GetHexagram();
	}

	//[KernelFunction("first_hexagram")]
	public static void first_hexagram()
	{
		DiagramView.SetFirst();
	}

	//[KernelFunction("previous_hexagram")]
	public void previous_hexagram()
	{
		DiagramView.SetPrevious();
	}

	//[KernelFunction("next_hexagram")]
	public void next_hexagram()
	{
		DiagramView.SetNext();
	}

	//[KernelFunction("last_hexagram")]
	public static void last_hexagram()
	{
		DiagramView.SetLast();
	}

	//[KernelFunction("move_hexagram")]
	public static void move_hexagram()
	{
		DiagramView.SetMove();
	}

	//[KernelFunction("last_cast_hexagram")]
	public static void last_cast_hexagram()
	{
		DiagramView.SetHome();
	}

	//[KernelFunction("inverse_hexagram")]
	public static void inverse_hexagram()
	{
		DiagramView.SetInverse();
	}

	//[KernelFunction("opposite_hexagram")]
	public static void opposite_hexagram()
	{
		DiagramView.SetOpposite();
	}

	//[KernelFunction("transverse_hexagram")]
	public static void transverse_hexagram()
	{
		DiagramView.SetTransverse();
	}

	//[KernelFunction("nuclear_hexagram")]
	public static void nuclear_hexagram()
	{
		DiagramView.SetNuclear();
	}
}

public static class MauiProgram
{

	public static MauiApp CreateMauiApp()
	{
		AppSettings.SetDocumentHome();
		AppPreferences.Load();

		//BuildKernel();

		MauiAppBuilder builder = MauiApp.CreateBuilder();
#if WINDOWS || ANDROID
		builder
			.UseMauiApp<App>()
			.UseLiveCharts()
			.UseSkiaSharp()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#endif

#if DEBUG
		builder.Logging.AddDebug();
#endif
		//builder.Services.AddSingleton<Views.DiagramView>();
		//builder.Services.AddSingleton<Views.EegView>();
		//builder.Services.AddSingleton<Pages.DiagramPage>();
		//builder.Services.AddSingleton<Pages.EegPage>();

#if ANDROID || IOS
		//builder.Services.AddTransient<Services.SampleDataService>();

		//builder.Services.AddSingleton<Pages.WebPage>();
		//builder.Services.AddSingleton<Pages.ChartPage>();
		//builder.Services.AddSingleton<Pages.ListDetailPage>();
		//builder.Services.AddTransient<Pages.ListDetailDetailPage>();
#endif

		Services.AudioPlayer.Load();

		return builder.Build();
	}

	public static void BuildKernel()
	{
		if (!string.IsNullOrEmpty(AppPreferences.OpenAiModelId))
		{

#pragma warning disable SKEXP0010

			//IKernelBuilder kernelBuilder1 = Kernel.CreateBuilder();
			////kernelBuilder.Services.ConfigureHttpClientDefaults(c => c.AddStandardResilienceHandler());
			//var kernel = kernelBuilder1
			//	.AddHuggingFaceTextEmbeddingGeneration("nomic-ai/nomic-embed-text-v1.5-GGUF/nomic-embed-text-v1.5.Q8_0.gguf",
			//	new Uri("http://localhost:1234/v1"), apiKey: "lm-studio", serviceId: null)
			//	.Build();

			//var embeddingGenerator = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
			//var memoryBuilder = new MemoryBuilder();
			//memoryBuilder.WithTextEmbeddingGeneration(embeddingGenerator);
			//memoryBuilder.WithMemoryStore(new VolatileMemoryStore());
			//var memory = memoryBuilder.Build();
			//var embeddingGenerator = kernel.Services.GetRequiredService<ITextEmbeddingGenerationService>();
			//var memory = new SemanticTextMemory(new VolatileMemoryStore(), embeddingGenerator);


			

			/*
			IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
			//if (!string.IsNullOrEmpty(AppPreferences.AiEndPoint))
			//	kernelBuilder.AddOpenAIChatCompletion(AppPreferences.AiModelId, new Uri(AppPreferences.AiEndPoint),null, AppPreferences.OpenAiKey);
			//else
				if (!string.IsNullOrEmpty(AppPreferences.OpenAiKey))
					kernelBuilder.AddOpenAIChatCompletion(AppPreferences.OpenAiModelId, AppPreferences.OpenAiKey);

			kernelBuilder.Plugins.AddFromType<YijingKernel>();
			AppSettings._kernel = kernelBuilder.Build();
			*/






			//AppSettings._kernel.ImportPluginFromType<YijingKernel>();
			//KernelArguments args = new KernelArguments();
			//args.Add("name", "Stephen");
			//var x = await AppSettings._kernel.InvokeAsync<int>("YijingKernel", "GetPersonAge", args);

			//var openAIConnector = new OpenAIConnector(AppPreferences.AiApiKey);
			//AppSettings._kernel.AddConnector(openAIConnector);
		}
	}

	// ...

	public async static void BuildKernelMemory()
	//public static Task<string> BuildKernelMemory()
	{

		//.WithOllamaTextEmbeddingGeneration(AppPreferences.AiModelId, AppPreferences.AiEndPoint);// + "/api/generate");
		//.WithOpenAITextGeneration(AppPreferences.AiModelId, AppPreferences.AiApiKey);
		//.WithOllamaTextEmbeddingGeneration(AppPreferences.AiModelId, AppPreferences.AiEndPoint + "/api/generate");
		//.Configure(null) C:\Users\Stephen Vivash\.ollama

		if (AppSettings._memoryServerless == null)
		{
			//var hostServices = new ServiceCollection();
			//hostServices.AddOpenAIChatCompletion(AppPreferences.AiModelId, AppPreferences.AiApiKey);
			//hostServices.AddOpenAITextEmbeddingGeneration(AppPreferences.AiModelId, AppPreferences.AiApiKey);

			OpenAIConfig openAIConfig = new OpenAIConfig()
			{
				APIKey = AppPreferences.OpenAiKey,
				TextModel = "gpt-4o", //AppPreferences.OpenAiModelId
				TextGenerationType = OpenAIConfig.TextGenerationTypes.Auto, //Chat,
				TextModelMaxTokenTotal = 8192,
				EmbeddingModel = "text-embedding-3-large", // text-embedding-ada-002 text-embedding-3-large
				EmbeddingModelMaxTokenTotal = 8192,
				//EmbeddingDimensions = 512,
				//MaxEmbeddingBatchSize = 1,
				//MaxRetries = 3,
				//Endpoint = "",
				//OrgId = "",
			}; 

			openAIConfig.Validate();

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0070

			//MemoryBuilder mb = new();
			//mb.WithOpenAITextEmbeddingGeneration("text-embedding-ada-002", AppPreferences.AiApiKey);
			//mb.WithMemoryStore(new VolatileMemoryStore());
			//var m = mb.Build();
			//m.AddDocumentAnalysisClient(new OpenAIAnalysisClient(AppPreferences.AiApiKey, AppPreferences.AiModelId));
			//m.GetAsync<MemoryServerless>().Wait();

			// Constructing an Open AI embedding generation service directly.
			//ITextEmbeddingGenerationService openAITES = new OpenAITextEmbeddingGenerationService(
			//	"text-embedding-ada-002", AppPreferences.AiApiKey);
			//openAITES.AsEmbeddingGenerator();
			//IList<ReadOnlyMemory<float>> embeddings =
			//	await openAITES.GenerateEmbeddingsAsync(
			//	[
			//		"sample text 1",
			//		"sample text 2"
			//	]);

			// Constructing an Olama embedding generation service directly.
			//ITextEmbeddingGenerationService olamaTES = new OllamaTextEmbeddingGenerationService(
			//ITextEmbeddingGenerationService olamaTES = new OllamaApiClient.AsEmbeddingGenerationService(
			//	"mxbai-embed-large", new Uri("http://localhost:11434"));

			var _kernelMemoryBuilder = new KernelMemoryBuilder()
				.WithOpenAI(openAIConfig);
				//.WithOpenAIDefaults(AppPreferences.AiApiKey);

				//.WithDefaultHandlersAsHostedServices(hostServices);
				//.AddOpenAITextEmbeddingGeneration(AppPreferences.AiModelId, AppPreferences.AiApiKey)

			AppSettings._memoryServerless = _kernelMemoryBuilder.Build<MemoryServerless>();
			//return AppSettings._memoryServerless.ImportDocumentAsync("C:\\Src\\IChing.308\\Resources\\Books\\B-YiTran.pdf", "B-YiTran");
			await AppSettings._memoryServerless.ImportDocumentAsync("C:\\Src\\IChing.308\\Resources\\Books\\B-YiTran.pdf", "B-YiTran");
			// https://scriptures.ru/buddhist_meditation.htm#google_vignette
		}
		//return null;
	}
}
