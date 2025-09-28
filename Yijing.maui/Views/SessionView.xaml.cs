
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using ValueSequencer;

using Yijing.Pages;
using Yijing.Services;

namespace Yijing.Views;

#nullable enable

public partial class SessionView : ContentView
{
	private readonly ObservableCollection<SessionSummary> _sessions = new();
	private SessionSummary? _selectedSession;
	private readonly Ai _ai = new();

	public ObservableCollection<SessionSummary> Sessions => _sessions;

	public event EventHandler<string>? ChatUpdated;

	public SessionView()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();
		BindingContext = this;

		_ai._systemPrompts[0] =
				"This app allows a user to consult the Yijing and engage in casual conversation with AI. " +
				"Don't explain what the Yijing is or how it works unless explicitly asked. " +
				"Not all consultations with the Yijing, and therefore AI, involve a question that needs an answer, like those " +
				"which are simply a reflective statement that seeks to explore for enjoyment rather than map for remedy. " +
				"The question/statement and the Yijing's response will then be sent to the AI for comment. " +
				"Respond in light of the Yijing's answer unless told to ignore it. " +
				"Focus on an explanation of question/statement and draw from any other relevant sources. " +
				"After the initial response don't repeat or rehash the answer refering to the hexagram again unless explicitly asked to do so. " +
				"Subsequent 'on topic' questions/statements won't necessarily be related to the cast hexagram, therefore don't include that information in the response. " +
				"At least reference the ideas associated with hexagrams cast if mentioned in the current prompt " +
				"Don't repeat or summarise previous answers unless explicitly ask to do so. " +
				"Allow the user to change the subject or ask for clarification about past responses. " +
				"Respond with prose rather than bullet points unless explicitly asked. " +
				"You may call functions when needed.";
	}

	private void OnLoaded(object? sender, EventArgs e)
	{
		Loaded -= OnLoaded;
		ResetChat();
		LoadSessions(null);
	}

	public async Task AiChatAsync(string prompt)
	{
		if (string.IsNullOrWhiteSpace(prompt))
			return;

		if (AppPreferences.AiChatService == (int)eAiService.eNone)
			return;

		await _ai.ChatAsync(AppPreferences.AiChatService, prompt);
		UpdateChat();
	}

	private void ResetChat()
	{
		ClearChatState();
		UpdateChat();
	}

	private void ClearChatState()
	{
		_ai._userPrompts = [[], []];
		_ai._chatReponses = [[], []];
		_ai._contextSessions = [];
	}

	public void UpdateChat()
	{
		String strText = Sequences.strDiagramSettings[16, Sequences.HexagramText + 1];
		string strBC = App.Current?.RequestedTheme == AppTheme.Dark ? "black" : "white";
		string strFC = App.Current?.RequestedTheme == AppTheme.Dark ? "white" : "black";
		string strAC = App.Current?.RequestedTheme == AppTheme.Dark ? "gray" : "gray";

		//string background = App.Current?.RequestedTheme == AppTheme.Dark ? "#000000" : "#FFFFFF";
		//string foreground = App.Current?.RequestedTheme == AppTheme.Dark ? "#FFFFFF" : "#000000";

		var sb = new StringBuilder();

		sb.Append("<html><head><meta charset=\"utf-8\"/><style>");
		sb.Append("body{");
		sb.Append($"background-color:{strBC};color:{strFC};font-family:'Open Sans',sans-serif;font-size:16px;line-height:1.5;");
		//sb.Append($"background-color:{background};color:{foreground};font-family:'Open Sans',sans-serif;font-size:16px;line-height:1.5;");
		sb.Append("}");
		sb.Append("strong{color:" + strFC + ";}");
		//sb.Append("strong{color:" + foreground + ";}");

		sb.Append("a {" + $" color: {strAC};" + "} ");
		sb.Append("h1 {" + $" color: {strAC};" + "} ");
		sb.Append("h2 {" + $" color: {strAC};" + "} ");
		sb.Append("h4 {" + $" color: {strAC};" + "} ");

		sb.Append("</style></head><body>");

		/*
		sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
			"<head><title>Yijing</title>" +
			"<link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\"/>" +
			"<style>" +
			"body {" +
			$" background-color: {strBC};" +
			$" color: {strFC};" +
			"} " +
			"html {" +
			" font-size: 16px;" +
			" font-family: \"Open Sans\", sans-serif;" +
			"} " +
			"a {" +
			$" color: {strAC};" +
			"} " +
			"h1 {" +
			$" color: {strAC};" +
			"} " +
			"h2 {" +
			$" color: {strAC};" +
			"} " +
			"h4 {" +
			$" color: {strAC};" +
			"} " +
			"</style></head><body><h1>" + " Chat Session: " + "");// _selectedSession.Session; // (picSession.SelectedItem as string);
		*/
		if (_ai._contextSessions.Count() > 0)
			sb.Append("</p>Context Sessions: ");
		foreach (var s in _ai._contextSessions)
			sb.Append(s + " ");

		sb.Append("</h1>");

		int count = int.Max(_ai._chatReponses[1].Count(), _ai._userPrompts[1].Count());
		for (int i = 0; i < count; ++i)
		{
			if (i < _ai._userPrompts[1].Count())
				sb.Append("<p><h4>" + _ai._userPrompts[1][i].Replace("\n", "</p>") + "</h4></p>");
			if (i < _ai._chatReponses[1].Count())
				sb.Append("<p>" + _ai._chatReponses[1][i].Replace("\n", "</p>") + "</p>");
		}
		/*
		for (int i = 0; i < 64; ++i)
			if (strHtml.Contains(Sequences.strHexagramLabels[9, i], StringComparison.CurrentCultureIgnoreCase))
			{
				String strHref = "<a href=\"Hexagram" + i + "\">" + Sequences.strHexagramLabels[9, i] + "</a>";
				Regex rgx = new Regex("\\b(?i)" + Sequences.strHexagramLabels[9, i] +
					"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b",
					RegexOptions.Compiled | RegexOptions.NonBacktracking | RegexOptions.IgnoreCase); // RegexOptions.ExplicitCapture
				strHtml = rgx.Replace(strHtml, strHref + "$1$2$3$4$5$6$7$8$9$10");
			}
		*/
		sb.Append("</body></html>");
		UI.Try<SessionPage>(p => p.WebView().Source = new HtmlWebViewSource { Html = sb.ToString() });
		//ChatUpdated?.Invoke(this, sb.ToString());
	}

	private void UpdateChat1()
	{
		string background = App.Current?.RequestedTheme == AppTheme.Dark ? "#000000" : "#FFFFFF";
		string foreground = App.Current?.RequestedTheme == AppTheme.Dark ? "#FFFFFF" : "#000000";

		var sb = new StringBuilder();
		sb.Append("<html><head><meta charset=\"utf-8\"/><style>");
		sb.Append("body{");
		sb.Append($"background-color:{background};color:{foreground};font-family:'Open Sans',sans-serif;font-size:16px;line-height:1.5;");
		sb.Append("strong{color:" + foreground + ";}");
		sb.Append("</style></head><body>");

		if (_selectedSession is not null)
		{
			string title = WebUtility.HtmlEncode(_selectedSession.Session);
			sb.Append($"<h1>Session: {title}</h1>");
		}

		int count = Math.Max(_ai._userPrompts[1].Count, _ai._chatReponses[1].Count);
		for (int i = 0; i < count; ++i)
		{
			if (i < _ai._userPrompts[1].Count)
			{
				string encodedPrompt = WebUtility.HtmlEncode(_ai._userPrompts[1][i]).Replace("\n", "<br/>");
				sb.Append($"<p><strong>User:</strong> {encodedPrompt}</p>");
			}
			if (i < _ai._chatReponses[1].Count)
			{
				string encodedResponse = WebUtility.HtmlEncode(_ai._chatReponses[1][i]).Replace("\n", "<br/>");
				sb.Append($"<p><strong>AI:</strong> {encodedResponse}</p>");
			}
		}

		sb.Append("</body></html>");
		ChatUpdated?.Invoke(this, sb.ToString());
	}

	private void LoadSessions(string? selectFile)
	{
		_sessions.Clear();

		try
		{
			string folder = GetQuestionsFolder();
			if (string.IsNullOrEmpty(folder))
				return;

			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			IEnumerable<string> files = Directory.EnumerateFiles(folder, "*.txt", SearchOption.TopDirectoryOnly)
					.OrderByDescending(f => f, StringComparer.OrdinalIgnoreCase);

			foreach (string file in files)
			{
				var summary = CreateSummary(file);
				_sessions.Add(summary);
			}

			if (!string.IsNullOrEmpty(selectFile))
			{
				var match = _sessions.FirstOrDefault(s => s.FileName.Equals(selectFile, StringComparison.OrdinalIgnoreCase));
				if (match is not null)
				{
					sessionCollection.SelectedItem = match;
					sessionCollection.ScrollTo(match, position: ScrollToPosition.Center, animate: false);
				}
			}
			else if (_sessions.Count > 0)
			{
				sessionCollection.SelectedItem = _sessions[0];
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to load sessions: {ex.Message}");
		}
	}

	private SessionSummary CreateSummary(string filePath)
	{
		string fileName = Path.GetFileNameWithoutExtension(filePath);
		string display = FormatSessionName(fileName);
		string description = GetSessionDescription(filePath);
		return new SessionSummary(fileName, display, description);
	}

	private static string FormatSessionName(string fileName)
	{
		if (DateTime.TryParseExact(fileName, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture,
				DateTimeStyles.AssumeLocal, out DateTime dt))
		{
			return dt.ToString("yyyy MMM dd HH:mm:ss", CultureInfo.InvariantCulture);
		}

		return fileName;
	}

	private static string GetSessionDescription(string filePath)
	{
		try
		{
			if (!File.Exists(filePath))
				return string.Empty;

			string text = File.ReadAllText(filePath);
			if (string.IsNullOrWhiteSpace(text))
				return string.Empty;

			string? castLine = text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
					.Select(line => line.Trim())
					.FirstOrDefault(line => line.Contains("hexagram", StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrEmpty(castLine))
				return castLine;

			return GetFirstWords(text, 10);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to read session '{filePath}': {ex.Message}");
			return string.Empty;
		}
	}

	private static string GetFirstWords(string text, int count)
	{
		if (string.IsNullOrWhiteSpace(text))
			return string.Empty;

		var words = text.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Take(count);
		return string.Join(" ", words);
	}

	private void LoadSelectedSession(SessionSummary? summary)
	{
		ClearChatState();

		if (summary is null)
		{
			UpdateChat();
			return;
		}

		LoadChat(summary.FileName, "Question", _ai._userPrompts[1]);
		LoadChat(summary.FileName, "Answer", _ai._chatReponses[1]);
		UpdateChat();
	}

	private void LoadChat(string name, string type, List<string> list)
	{
		string documentHome = GetDocumentHome();
		string directory = Path.Combine(documentHome, $"{type}s");
		string path = Path.Combine(directory, name + ".txt");

		if (!File.Exists(path))
			return;

		string entry = string.Empty;

		using StreamReader reader = File.OpenText(path);
		string? line;
		while ((line = reader.ReadLine()) != null)
		{
			if (string.IsNullOrEmpty(line))
				continue;

			if (line == $"$({type})")
			{
				if (!string.IsNullOrEmpty(entry))
				{
					list.Add(entry);
					entry = string.Empty;
				}
			}
			else
			{
				entry += line + "\n";
			}
		}

		if (!string.IsNullOrEmpty(entry))
			list.Add(entry);
	}

	private static string GetQuestionsFolder()
	{
		return Path.Combine(GetDocumentHome(), "Questions");
	}

	private static string GetDocumentHome()
	{
		string? documentHome = AppSettings.DocumentHome();
		if (string.IsNullOrWhiteSpace(documentHome))
		{
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			documentHome = Path.Combine(documents, "Yijing");
		}

		return documentHome;
	}

	private async void OnAddSessionClicked(object? sender, EventArgs e)
	{
		try
		{
			string folder = GetQuestionsFolder();
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			string fileName = AppSettings.ReverseDateString();
			string path = Path.Combine(folder, fileName + ".txt");
			if (!File.Exists(path))
				File.WriteAllText(path, string.Empty);

			LoadSessions(fileName);
		}
		catch (Exception ex)
		{
			await ShowAlert("Add Session", $"Unable to create a session. {ex.Message}");
		}
	}

	private async void OnDeleteSessionClicked(object? sender, EventArgs e)
	{
		if (_selectedSession is null)
			return;

		bool confirm = await ConfirmAsync("Delete Session",
				$"Delete {_selectedSession.Session}?");
		if (!confirm)
			return;

		try
		{
			string folder = GetQuestionsFolder();
			string path = Path.Combine(folder, _selectedSession.FileName + ".txt");
			if (File.Exists(path))
				File.Delete(path);

			string answersFolder = Path.Combine(GetDocumentHome(), "Answers");
			string answersPath = Path.Combine(answersFolder, _selectedSession.FileName + ".txt");
			if (File.Exists(answersPath))
				File.Delete(answersPath);

			LoadSessions(null);
		}
		catch (Exception ex)
		{
			await ShowAlert("Delete Session", $"Unable to delete the session. {ex.Message}");
		}
	}

	private void OnSessionsSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		_selectedSession = e.CurrentSelection.FirstOrDefault() as SessionSummary;
		LoadSelectedSession(_selectedSession);
	}

	private static Task ShowAlert(string title, string message)
	{
		if (Application.Current?.MainPage is Page page)
			return page.DisplayAlert(title, message, "OK");

		return Task.CompletedTask;
	}

	private static Task<bool> ConfirmAsync(string title, string message)
	{
		if (Application.Current?.MainPage is Page page)
			return page.DisplayAlert(title, message, "Yes", "No");

		return Task.FromResult(false);
	}
}

public class SessionSummary
{
	public SessionSummary(string fileName, string session, string description)
	{
		FileName = fileName;
		Session = session;
		Description = description;
	}

	public string FileName { get; }

	public string Session { get; }

	public string Description { get; }
}
