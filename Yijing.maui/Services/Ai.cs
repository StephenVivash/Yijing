//using Microsoft.Extensions.AI;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
//using System.Text.Json;

using Yijing.Views;

namespace Yijing.Services;

public class Ai
{
	public string[] _systemPrompts = [""];

	public List<string> _contextSessions = [];

	public List<List<string>> _userPrompts = [[], []];
	public List<List<string>> _chatReponses = [[], []];

	public ChatHistory _chatHistory = new();
	public List<OpenAI.Chat.ChatMessage> _chatHistory1 = [];

	public async Task ChatAsync(int aiService, string prompt)
	{
		try
		{
			var builder = Kernel.CreateBuilder();
			var http = new HttpClient { Timeout = TimeSpan.FromMinutes(
				aiService == (int)eAiService.eOllama ? 10 : 2) };

			builder.AddOpenAIChatCompletion(AppPreferences.AiModelId[aiService],
				new Uri(AppPreferences.AiEndPoint[aiService]), 
				AppPreferences.AiKey[aiService], httpClient: http);

			//builder.AddOpenAITextToAudio(AppPreferences.AiModelId[aiService], AppPreferences.AiKey[aiService]);

			//builder.Plugins.AddFromType<YijingPlugin>("Yijing");
			Kernel kernel = builder.Build();

			var chatService = kernel.GetRequiredService<IChatCompletionService>();

			_chatHistory = new ChatHistory();
			foreach (var s1 in _systemPrompts)
				_chatHistory.AddSystemMessage(s1);

			for (int i = 0; i < 2; ++i)
				for (int j = 0; j < _userPrompts[i].Count(); ++j)
				{
					_chatHistory.AddUserMessage(_userPrompts[i][j]);
					_chatHistory.AddAssistantMessage(_chatReponses[i][j]);
				}

			_chatHistory.AddUserMessage(prompt);
			var settings = new OpenAIPromptExecutionSettings
			{
				Temperature = AppPreferences.AiTemperature,
				TopP = AppPreferences.AiTopP,
				MaxTokens = AppPreferences.AiMaxTokens,
				ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
				//AudioOptions =
			};

			var reply = await chatService.GetChatMessageContentAsync(_chatHistory, settings, kernel);
			string response = reply.Content;

			response = response.Replace("**", ""); // Remove .md
			response = response.Replace("###", "");
			response = response.Replace("---", "");

			_userPrompts[1].Add(prompt);
			_chatReponses[1].Add(response);
		}
		catch (Exception ex)
		{
			_userPrompts[1].Add(prompt);
			_chatReponses[1].Add(ex.Message +
				"\n\nEdit the Documents\\Yijing\\appsettings.json file to correct the configuration and restart the application.");
		}
	}

	public async Task ChatAsync1(int aiService, string prompt)
	{
		try
		{
			OpenAIClientOptions openAIClientOptions = new()
			{
				NetworkTimeout = TimeSpan.FromSeconds(aiService == (int)eAiService.eOllama ? 600 : 120),
				Endpoint = new Uri(AppPreferences.AiEndPoint[aiService])
			};

			var requestOptions = new ChatCompletionOptions()
			{
				Temperature = AppPreferences.AiTemperature,
				TopP = AppPreferences.AiTopP,
				MaxOutputTokenCount = AppPreferences.AiMaxTokens,
				//AudioOptions = 
			};

			ApiKeyCredential credential = new(AppPreferences.AiKey[aiService]);
			var chatClient = new ChatClient(AppPreferences.AiModelId[aiService], credential, openAIClientOptions);

			_chatHistory1 = [];
			foreach (var s1 in _systemPrompts)
				_chatHistory1.Add(new SystemChatMessage(s1)); // UserChatMessage SystemChatMessage

			for (int i = 0; i < 2; ++i)
				for (int j = 0; j < _userPrompts[i].Count(); ++j)
				{
					_chatHistory1.Add(new UserChatMessage(_userPrompts[i][j]));
					_chatHistory1.Add(new AssistantChatMessage(_chatReponses[i][j]));
				}

			_chatHistory1.Add(new UserChatMessage(prompt));

			var completion = await chatClient.CompleteChatAsync(_chatHistory1, requestOptions);
			string response = completion.Value.Content[0].Text;

			response = response.Replace("**", ""); // Remove .md
			response = response.Replace("###", "");
			response = response.Replace("---", "");

			_userPrompts[1].Add(prompt);
			_chatReponses[1].Add(response);

			}
		catch (Exception ex)
		{
			_userPrompts[1].Add(prompt);
			_chatReponses[1].Add(ex.Message +
				"\n\nEdit the Documents\\Yijing\\appsettings.json file to correct the configuration and restart the application.");
		}
	}
}

public class YijingPlugin
{

	[KernelFunction("autocast_hexagram")]
	public static void autocast_hexagram()
	{
		DiagramView.AutoCastHexagram();
	}

	[KernelFunction("set_hexagram")]
	public static void set_hexagram(int sequence)
	{
		DiagramView.SetHexagram(sequence);
	}

	[KernelFunction("get_hexagram")]
	public static int get_hexagram()
	{
		return DiagramView.GetHexagram();
	}
	
	[KernelFunction("first_hexagram")]
	public static void first_hexagram()
	{
		DiagramView.SetFirst();
	}

	[KernelFunction("previous_hexagram")]
	public static void previous_hexagram()
	{
		DiagramView.SetPrevious();
	}

	[KernelFunction("next_hexagram")]
	public static void next_hexagram()
	{
		DiagramView.SetNext();
	}

	[KernelFunction("last_hexagram")]
	public static void last_hexagram()
	{
		DiagramView.SetLast();
	}

	[KernelFunction("move_hexagram")]
	public static void move_hexagram()
	{
		DiagramView.SetMove();
	}

	[KernelFunction("last_cast_hexagram")]
	public static void last_cast_hexagram()
	{
		DiagramView.SetHome();
	}

	[KernelFunction("inverse_hexagram")]
	public static void inverse_hexagram()
	{
		DiagramView.SetInverse();
	}

	[KernelFunction("opposite_hexagram")]
	public static void opposite_hexagram()
	{
		DiagramView.SetOpposite();
	}

	[KernelFunction("transverse_hexagram")]
	public static void transverse_hexagram()
	{
		DiagramView.SetTransverse();
	}

	[KernelFunction("nuclear_hexagram")]
	public static void nuclear_hexagram()
	{
		DiagramView.SetNuclear();
	}
}

/*
	public async Task TestSkOpenAiTools(int aiService)
	{
		var builder = Kernel.CreateBuilder();
		builder.AddOpenAIChatCompletion(
			modelId: AppPreferences.AiModelId[aiService],
			apiKey: AppPreferences.AiKey[aiService]);

		builder.Plugins.AddFromType<ClockPlugin>("Utils");
		Kernel kernel = builder.Build();

		var chatService = kernel.GetRequiredService<IChatCompletionService>();
		var history = new ChatHistory();
		history.AddSystemMessage("You may call functions when needed.");
		history.AddUserMessage("What time is it in Sydney?");

		var settings = new OpenAIPromptExecutionSettings
		{
			ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
		};

		ApiKeyCredential credential = new(AppPreferences.AiKey[aiService]);
		var chatClient = new ChatClient(AppPreferences.AiModelId[aiService], credential, openAIClientOptions);

		var tools = new List<ChatTool> {
			ChatTool.CreateFunctionTool(
				"Utils_get_time",
				"Get the local time in a given city.",
				BinaryData.FromString("""
				{
				  "type": "object",
				  "properties": { "city": { "type": "string", "description": "City name" } },
				  "required": ["city"]
				}
				"""))
		};

		var messages = new List<ChatMessage> {
			new SystemChatMessage("You may call tools."),
			new UserChatMessage("What time is it in Sydney?")
		};

		var options = new ChatCompletionOptions
		{
			//Temperature = 0.7f,
			//TopP = 0.95f,
			//MaxOutputTokenCount = 1000,
			//AllowParallelToolCalls = true,
			//EndUserId = "Stephen",
			//ToolChoice = ChatToolChoice.CreateAutoChoice //.Auto
		};
		options.Tools.Add(tools[0]);

		while (true)
		{
			ChatCompletion completion = chatClient.CompleteChat(messages, options);
			messages.Add(new AssistantChatMessage(completion));

			bool handledToolCall = false;
			foreach (var call in completion.ToolCalls)
			{
				if (call is ChatToolCall.Function function && function.Name == "Utils_get_time")
				{
					using var doc = JsonDocument.Parse(function.Arguments);
					string city = doc.RootElement.GetProperty("city").GetString()!;
					var result1 = await kernel.InvokeAsync<string>("Utils", "get_time", new() { ["city"] = city });
					messages.Add(new ToolChatMessage(function.Id, result1));
					handledToolCall = true;
				}
			}
			if (!handledToolCall)
			{
				Console.WriteLine(completion.Content[0].Text);
				break;
			}
		}
	}
 
	public async void TestOpenAITools(int aiService)
	{

		OpenAIClientOptions openAIClientOptions = new()
		{
			NetworkTimeout = TimeSpan.FromSeconds(aiService == (int)eAiService.eOllama ? 600 : 120),
			Endpoint = new Uri(AppPreferences.AiEndPoint[aiService])
		};

		System.ClientModel.ApiKeyCredential credential = new(AppPreferences.AiKey[aiService]);
		var openAiChatClient = new ChatClient(AppPreferences.AiModelId[aiService], credential, openAIClientOptions);

		// Advertise a function tool
		var tools = new List<ChatTool> {
		ChatTool.CreateFunctionTool(
			"get_time", "Return ISO8601 time for an IANA timezone.",
			BinaryData.FromString(
			"""
			{"type":"object","properties":{"tz":{"type":"string"}},"required":["tz"]}
			"""))
		};

		var messages = new List<OpenAI.Chat.ChatMessage> {
			new SystemChatMessage("You may call tools."),
			new UserChatMessage("What's the time in Australia/Sydney?")
		};

		var opts = new ChatCompletionOptions();// { ToolChoice = ChatToolChoice.CreateNoneChoice };
		opts.Tools.Add(tools[0]);

		while (true)
		{
			ChatCompletion completion = await openAiChatClient.CompleteChatAsync(messages, opts);
			messages.Add(new AssistantChatMessage(completion));

			if (completion.ToolCalls.Count == 0)
			{
				Console.WriteLine(completion.Content[0].Text);
				break;
			}
			foreach (var tool in completion.ToolCalls.OfType<ChatToolCall.CreateFunctionToolCall>())
			{
				if (tool.Name == "get_time")
				{
					string tz = JsonDocument.Parse(tool.Arguments).RootElement.GetProperty("tz").GetString()!;
					var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(tz)).ToString("O");
					messages.Add(new ToolChatMessage(tool.Id, now));
				}
			}
		}
	}

	public async void TestExtensionTools(int aiService)
	{
		OpenAIClientOptions openAIClientOptions = new()
		{
			NetworkTimeout = TimeSpan.FromSeconds(aiService == (int)eAiService.eOllama ? 600 : 120),
			Endpoint = new Uri(AppPreferences.AiEndPoint[aiService])
		};

		System.ClientModel.ApiKeyCredential credential = new(AppPreferences.AiKey[aiService]);
		var openAiChatClient = new ChatClient(AppPreferences.AiModelId[aiService], credential, openAIClientOptions);

		IChatClient chat = openAiChatClient.AsIChatClient();
		
		//IChatClient chat = new ChatClientBuilder(new OpenAIClient(Environment.GetEnvironmentVariable("OPENAI_API_KEY"))
		//	.GetChatClient("gpt-4o").AsIChatClient())
		//	.UseFunctionInvocation()
		//	.Build();

		var options = new ChatOptions
		{
			Tools = [AIFunctionFactory.Create((string location) => "15°C and drizzly", "get_current_weather", "Weather by location")]
		};

		var response = await chat.GetResponseAsync("Do I need a jacket in Sydney?", options);
		string x = response.Text;
	}

	public async Task ChatAsync(int aiService, string prompt)
	{
		string response = "";
		try
		{
			OllamaApiClient client = new OllamaApiClient(new Uri(AppPreferences.AiEndPoint[aiService]));
			Chat chat = new(client);
			chat.Model = AppPreferences.AiModelId[aiService];
			//chat.Options = options;
			chat.Messages.Add(new Message { Role = "user", Content = msg });

			var response1 = await chat.SendAsync("Hello") // .GetResponseAsync(options);

			// With the following code to properly consume the IAsyncEnumerable<string>:
			string str1 = "";
			await foreach (var chunk in chat.SendAsync("Hello"))
			{
				str1 += chunk;
			}

			string str2 = response1.Choices[0].Message.Content;

			client.ChatAsync("llama3.1:latest", new List<OllamaSharp.Message> 
				{ new OllamaSharp. Message { Role = "user", Content = msg } },
				options).Subscribe((response) =>
			{
				// Handle each response chunk as it arrives
				if (response != null && response.Choices != null && response.Choices.Count > 0)
				{
					var chunk = response.Choices[0].Message.Content;
					Console.WriteLine(chunk);
				}
			},
			() =>
			{
				// Handle completion of the stream
				Console.WriteLine("Stream completed.");
			});

			if (aiService == (int)eAiService.eOllama)
			{
				var ollamaClient = new OllamaApiClient(new Uri(AppPreferences.AiEndPoint[aiService]));

				ChatRequest request = new()
				{
					Model = AppPreferences.AiModelId[aiService],
					Messages = []
				};

				foreach (var s1 in _aiSystemPrompts)
					request.Messages.Add(new Message(ChatRole.System, s1));

				for (int i = 0; i < _aiUserPrompts[1].Count(); ++i)
				{
					request.Messages.Add(new Message(ChatRole.User, _aiUserPrompts[1][i]));
					request.Messages.Add(new Message(ChatRole.Assistant, _aiChatReponses[1][i]));
				}

				request.Messages.Add(new Message(ChatRole.User, msg));

				var completion = await ollamaClient.ChatAsync(request);
				string str = completion.Message.Content;
				str = str.Replace("**", "");
				str = str.Replace("###", "");
				str = str.Replace("---", "");

				_aiUserPrompts[1].Add(msg);
				_aiChatReponses[1].Add(str);
			}

            if (aiService == (int)eAiService.eOllama)
			{
				ChatOptions options = new()
				{
					Temperature = AppPreferences.AiTemperature,
					TopP = AppPreferences.AiTopP,
					MaxOutputTokens = AppPreferences.AiMaxTokens,

					//AllowParallelToolCalls = true,
					//EndUserId = "Stephen",
					//Functions = new List<ChatCompletionFunction> { ChatCompletionFunction.Chat },	
				};

				var ollamaChatClient = new OllamaChatClient(new Uri(AppPreferences.AiEndPoint[aiService]),
					AppPreferences.AiModelId[aiService]);

				List<Microsoft.Extensions.AI.ChatMessage> chatHistory = [];
				foreach (var s1 in _systemPrompts)
					chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(ChatRole.System, s1));
				for (int i = 0; i < _userPrompts[1].Count(); ++i)
				{
					chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, _userPrompts[1][i]));
					chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(ChatRole.Assistant, _chatReponses[1][i]));
				}
				chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, prompt));

				var completion = await ollamaChatClient.GetResponseAsync(chatHistory, options);
                response = completion.Messages[0].Text;
			}
			else
			{
				OpenAIClientOptions openAIClientOptions = new()
				{
					NetworkTimeout = TimeSpan.FromSeconds(aiService == (int)eAiService.eOllama ? 360 : 120),
					Endpoint = new Uri(AppPreferences.AiEndPoint[aiService])
				};
				//if (aiService != (int)eAiService.eOpenAi)
				//openAIClientOptions.Endpoint = new Uri(AppPreferences.AiEndPoint[aiService]);

				var requestOptions = new ChatCompletionOptions()
				{
					Temperature = AppPreferences.AiTemperature,
					TopP = AppPreferences.AiTopP,
					MaxOutputTokenCount = AppPreferences.AiMaxTokens,
					//AudioOptions = 
				};

				System.ClientModel.ApiKeyCredential credential = new(AppPreferences.AiKey[aiService]);
				var openAiChatClient = new ChatClient(AppPreferences.AiModelId[aiService], credential, openAIClientOptions); // AppPreferences.OpenAiKey

				List<OpenAI.Chat.ChatMessage> chatHistory = [];
				foreach (var s1 in _systemPrompts)
					chatHistory.Add(new SystemChatMessage(s1)); // UserChatMessage SystemChatMessage

				for (int i = 0; i < 2; ++i)
					for (int j = 0; j < _userPrompts[i].Count(); ++j)
					{
						chatHistory.Add(new UserChatMessage(_userPrompts[i][j]));
						chatHistory.Add(new AssistantChatMessage(_chatReponses[i][j]));
					}

				chatHistory.Add(new UserChatMessage(prompt));

				var completion = await openAiChatClient.CompleteChatAsync(chatHistory, requestOptions);
				response = completion.Value.Content[0].Text;
			}

			response = response.Replace("**", ""); // Remove .md
			response = response.Replace("###", "");
			response = response.Replace("---", "");

			_userPrompts[1].Add(prompt);
			_chatReponses[1].Add(response);

		}
		catch (Exception ex)
		{
			_userPrompts[1].Add(prompt);
			_chatReponses[1].Add(ex.Message +
				"\n\nEdit the Documents\\Yijing\\appsettings.json file to correct the configuration and restart the application.");
		}
	}
}
*/

