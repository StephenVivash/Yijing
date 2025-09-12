using Microsoft.Extensions.AI;

using OpenAI;
using OpenAI.Chat;

//using OllamaSharp.Models;
//using OllamaSharp.Models.Chat;
//using OllamaSharp;

namespace Yijing.Services;

public class Ai
{
	public string[] _systemPrompts = [""];

	public List<string> _contextSessions = [];

	public List<List<string>> _userPrompts = [[], []];
	public List<List<string>> _chatReponses = [[], []];

	public async Task ChatAsync(int aiService, string prompt)
	{
		string response = "";
		try
		{
            /*			
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
			*/

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
					NetworkTimeout = TimeSpan.FromSeconds(60),
				};
				if (aiService != (int)eAiService.eOpenAi)
					openAIClientOptions.Endpoint = new Uri(AppPreferences.AiEndPoint[aiService]);

				var requestOptions = new ChatCompletionOptions()
				{
					Temperature = AppPreferences.AiTemperature,
					TopP = AppPreferences.AiTopP,
					MaxOutputTokenCount = AppPreferences.AiMaxTokens,
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

            response = response.Replace("**", ""); // Remove .MD
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

