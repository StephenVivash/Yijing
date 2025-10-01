
using System.Collections.ObjectModel;
using System.ComponentModel;
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
	private ObservableCollection<Session> _sessions = new();
	private Session? _selectedSession;
	private readonly Ai _ai = new();

	//public ObservableCollection<Session> Sessions => _sessions;

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

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		if ((width == -1) || (height == -1))
			return;

		double w = width - 20;
		if (width < 380)
			w = width - 30;

		w = width - 40;
		//lblHexagram.WidthRequest = w;

		w /= 2;

		//lblDiagramMode.WidthRequest = w;

		w -= 5;

		//lblSession.WidthRequest = w - 50;
		//picSession.WidthRequest = w;

		w /= 2;
		btnAdd.WidthRequest = w;
		btnDelete.WidthRequest = w;
	}

	private void OnAddSessionClicked(object? sender, EventArgs e)
	{
		string fileName = AppSettings.ReverseDateString();
		var summary = CreateSession(fileName);
		_sessions.Insert(0, summary);
		sessionCollection.SelectedItem = summary;
	}

	private async void OnDeleteSessionClicked(object? sender, EventArgs e)
	{
		if (_selectedSession is null)
			return;

		bool confirm = await Window.Page!.DisplayAlert("Delete Session", $"Delete {_selectedSession.Name}?", "Yes", "No");
		if (!confirm)
			return;

		try
		{
			string path = Path.Combine(AppSettings.DocumentHome(), "Questions", _selectedSession.FileName + ".txt");
			if (File.Exists(path))
				File.Delete(path);

			path = Path.Combine(AppSettings.DocumentHome(), "Answers", _selectedSession.FileName + ".txt");
			if (File.Exists(path))
				File.Delete(path);

			int i = _sessions.IndexOf(_selectedSession);
			_sessions.Remove(_selectedSession);
			if (_sessions.Count > 0)
			{
				sessionCollection.SelectedItem = _sessions[i == _sessions.Count ? --i : i];
				sessionCollection.ScrollTo(i, position: ScrollToPosition.Start, animate: false);
			}
		}
		catch (Exception ex)
		{
			await Window.Page!.DisplayAlert("Delete Session", $"Unable to delete the session. {ex.Message}", "OK");
		}
	}

	private void OnSessionsSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		_selectedSession = e.CurrentSelection.FirstOrDefault() as Session;
		LoadSelectedSession(_selectedSession);
		if (!string.IsNullOrEmpty(_selectedSession?.YijingCast))
			UI.Call<DiagramView>(v => v.SetHexagramCast(_selectedSession?.YijingCast));
	}

	public void UpdateChat()
	{
		String strText = Sequences.strDiagramSettings[16, Sequences.HexagramText + 1];
		string strBC = App.Current?.RequestedTheme == AppTheme.Dark ? "black" : "white";
		string strFC = App.Current?.RequestedTheme == AppTheme.Dark ? "white" : "black";
		string strAC = App.Current?.RequestedTheme == AppTheme.Dark ? "gray" : "gray";

		var sb = new StringBuilder();
		sb.Append("<html><head><meta charset=\"utf-8\"/><style>");
		sb.Append("body{");
		sb.Append($"background-color:{strBC};color:{strFC};font-family:'Open Sans',sans-serif;font-size:16px;line-height:1.5;");
		sb.Append("}");
		sb.Append("strong{color:" + strFC + ";}");
		sb.Append("a {" + $" color: {strAC};" + "} ");
		sb.Append("h1 {" + $" color: {strAC};" + "} ");
		sb.Append("h2 {" + $" color: {strAC};" + "} ");
		sb.Append("h4 {" + $" color: {strAC};" + "} ");
		sb.Append("</style></head><body>");

		if (_selectedSession is not null)
		{
			sb.Append($"<h2>Session: {WebUtility.HtmlEncode(_selectedSession.Name)}");
			string yjingCast = WebUtility.HtmlEncode(_selectedSession.YijingCast);
			if (!string.IsNullOrEmpty(yjingCast))
				sb.Append($" - ({yjingCast})");
		}

		if (_ai._contextSessions.Count() > 0)
			sb.Append("</p>Context Sessions: ");
		foreach (var s in _ai._contextSessions)
			sb.Append(s + " ");

		sb.Append("</h2>");

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
			string title = WebUtility.HtmlEncode(_selectedSession.Name);
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
		List<Session> sessions = new List<Session>();
		try
		{
			IEnumerable<string>[] files = new IEnumerable<string>[3];
			string[] folder = [Path.Combine(AppSettings.DocumentHome(), "Questions"),
							   Path.Combine(AppSettings.DocumentHome(), "Muse"),
							   Path.Combine(AppSettings.DocumentHome(), "Emotiv")];

			for (int i = 0; i < 3; ++i)
			{
				if (!Directory.Exists(folder[i]))
					Directory.CreateDirectory(folder[i]);

				files[i] = Directory.EnumerateFiles(folder[i], i == 0 ? "*.txt" : "*.csv", SearchOption.TopDirectoryOnly)
						.OrderByDescending(f => f, StringComparer.OrdinalIgnoreCase);

				foreach (string file in files[i])
				{
					var summary = CreateSession(file);
					var match = sessions.FirstOrDefault(s => s.Name.Equals(summary.Name, StringComparison.OrdinalIgnoreCase));
					if (match is not null)
						match.Description = "* " + match.Description;
					else
						sessions.Add(summary);
				}
			}

			_sessions = new ObservableCollection<Session>(sessions.OrderByDescending(s => s.FileName, StringComparer.OrdinalIgnoreCase));
			sessionCollection.ItemsSource = _sessions;
			/*_sessions = new ObservableCollection<SessionSummary>(_sessions.OrderByDescending(s =>
			{
				if (DateTime.TryParseExact(s.FileName, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture,
						DateTimeStyles.AssumeLocal, out DateTime dt))
					return dt;
				return DateTime.MinValue;
			}));*/

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

	private Session CreateSession(string filePath)
	{
		string fileName = Path.GetFileNameWithoutExtension(filePath);
		string extension = Path.GetExtension(filePath);
		string name = FormatSessionName(fileName);
		string description = "New session";
		string yijingCast = "";

		if (!string.IsNullOrEmpty(extension))
			if (extension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
				ReadText(filePath, ref description, ref yijingCast);
			else if (extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
				description = "EEG data";

		return new Session(0, name, fileName, description, yijingCast);
	}

	private static string FormatSessionName(string fileName)
	{
		if (fileName.EndsWith("-Muse"))
			fileName = fileName.Substring(0, fileName.Length - 5);
		else if (fileName.EndsWith("-Emotiv"))
			fileName = fileName.Substring(0, fileName.Length - 7);

		if (DateTime.TryParseExact(fileName, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture,
			DateTimeStyles.AssumeLocal, out DateTime dt))
			return dt.ToString("MMM dd HH:mm:ss", CultureInfo.InvariantCulture);
		//return dt.ToString("yyyy MMM dd HH:mm:ss", CultureInfo.InvariantCulture);
		return fileName;
	}

	private static void ReadText(string filePath, ref string description, ref string yijingCast)
	{
		string s = "Yijing responded with hexagram";
		try
		{
			if (!File.Exists(filePath))
				return;

			string text = File.ReadAllText(filePath);
			if (string.IsNullOrWhiteSpace(text))
				return;

			string? castLine = text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
					.Select(line => line.Trim())
					.FirstOrDefault(line => line.Contains(s, StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrEmpty(castLine))
			{
				int i = castLine.IndexOf(s, StringComparison.OrdinalIgnoreCase);
				castLine = castLine.Substring(i + s.Length + 1).Trim();
				yijingCast = castLine;
			}

			var words = text.Replace("$(Question)", "").Split([ ' ', '\r', '\n', '\t' ], StringSplitOptions.RemoveEmptyEntries).Take(10);
			description = string.Join(" ", words);

		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to read session '{filePath}': {ex.Message}");
			return;
		}
	}

	private void LoadSelectedSession(Session? summary)
	{
		ClearChatState();
		if (summary is null)
		{
			UpdateChat();
			return;
		}
		LoadChat(summary.FileName);
		UpdateChat();
	}

	public async Task AiChatAsync(string prompt, bool includeCast)
	{

		includeCast = false;

		if (AppPreferences.AiChatService == (int)eAiService.eNone)
		{
			await Window.Page!.DisplayAlert("No Service", "Please select the AI service.", "OK");
			return;
		}

		string s = "";
		if (includeCast)
		{
			UI.Try<DiagramView>(v => s = v.DescribrCastHexagram());
			if (string.IsNullOrWhiteSpace(s))
			{
				await Window.Page!.DisplayAlert("No Cast", "Please cast a hexagram first.", "OK");
				return;
			}
			s = prompt + (prompt.Length > 0 ? "\n" : "") + "I consulted the oracle and the Yijing responded with hexagram " + s;
		}
		else
			s = prompt;

		if (string.IsNullOrWhiteSpace(s))
		{
			await Window.Page!.DisplayAlert("No Prompt", "Please enter a prompt.", "OK");
			return;
		}

		await _ai.ChatAsync(AppPreferences.AiChatService, s);
		SaveChat(_selectedSession!.FileName);
		UpdateChat();
		UpdateSessionLog("", false, false);
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

	public void UpdateSessionLog(string str, bool append, bool newline)
	{
		if (append)
			UI.Call<SessionPage>(p => p.SessionLog().Text += str + (newline ? "\n" : ""));
		else
			UI.Call<SessionPage>(p => p.SessionLog().Text = str + (newline ? "\n" : ""));
	}

	public void SaveChat(string name)
	{
		if ((_ai._userPrompts[1].Count > 0) || (_ai._chatReponses[1].Count > 0))
		{
			SaveChat(name, "Question", _ai._userPrompts[1]);
			SaveChat(name, "Answer", _ai._chatReponses[1]);
		}
	}

	public void SaveChat(string name, string type, List<string> list)
	{
		string str = Path.Combine(AppSettings.DocumentHome(), $"{type}s", name + ".txt");
		using (FileStream fs = new(str, FileMode.Create, FileAccess.Write))
			foreach (string s in list)
			{
				byte[] val = Encoding.UTF8.GetBytes($"$({type})\n" + s + "\n");
				fs.Write(val, 0, val.Length);
			}
	}

	public void LoadChat(string session)
	{
		for (int i = 0; i < _ai._contextSessions.Count; ++i)
		{
			LoadChat(_ai._contextSessions[i], "Question", _ai._userPrompts[0]);
			LoadChat(_ai._contextSessions[i], "Answer", _ai._chatReponses[0]);
		}
		if (!string.IsNullOrEmpty(session))
		{
			//EegView._strSession = session;
			LoadChat(session, "Question", _ai._userPrompts[1]);
			LoadChat(session, "Answer", _ai._chatReponses[1]);
			if ((_ai._userPrompts[1].Count() > 0) || (_ai._chatReponses[1].Count() > 0))
				UpdateChat();
		}
	}

	public void LoadChat(string name, string type, List<string> list)
	{
		string? str = Path.Combine(AppSettings.DocumentHome(), $"{type}s", name + ".txt");
		string entry = "";
		if (File.Exists(str))
			using (StreamReader sr = File.OpenText(str))
			{
				while ((str = sr.ReadLine()) != null)
					if (!string.IsNullOrEmpty(str))
						if (str == $"$({type})")
						{
							if (!string.IsNullOrEmpty(entry))
							{
								list.Add(entry);
								entry = "";
							}
						}
						else
							entry += str + "\n";
				if (!string.IsNullOrEmpty(entry))
					list.Add(entry);
			}
		else
			UpdateSessionLog("Failed to load " + str, true, true);
	}
}

public class Session
{
	public Session(int id, string name, string fileName, string description, string yijingCast)
	{
		Id = id;
		Name = name;
		FileName = fileName;
		Description = description;
		YijingCast = yijingCast;
	}

	public int Id { get; set; }
	public string Name { get; set; }
	public string FileName { get; set; }
	public string Description { get; set; }
	public string YijingCast { get; set; }
}
