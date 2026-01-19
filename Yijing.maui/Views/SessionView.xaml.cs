
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using ValueSequencer;

using Yijing.Pages;
using Yijing.Services;
using YijingData;

namespace Yijing.Views;

#nullable enable

public partial class SessionView : ContentView
{
	private ObservableCollection<Session> _sessions = new();
	private Session? _selectedSession;

	private readonly Ai _ai = new();
	private List<string> _sessionNotes = [];
	private bool _isSyncingContexts;
	private bool _contextSelectionDirty;

	public event EventHandler<string>? ChatUpdated;

	public SessionView()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();
		BindingContext = this;

		if (App.Current!.RequestedTheme == AppTheme.Dark)
		{
			hslButtons.BackgroundColor = Colors.Black;
			sessionCollection.BackgroundColor = Colors.Black;
		}

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

			"Always use the cast description format where the hexagram number is seperated from the moving lines with 1 period eg: 12.356 Stagnation > 62 Surplus " +
			"The example 12.356 represents hexagram 12 with 3 moving lines 3, 5 and 6 " +

			"You may call functions when needed. " +
			"You may call autocast_hexagram() initially if a question is asked without a cast description. " +
			"If a cast description is provided don't call autocast_hexagram() and don't call on subsequent questions in that session unless asked to do so. " +
			"Always report the autocast_hexagram() description in this format in your response eg: The Yijing cast yielded hexagram 12.356 Stagnation > 62 Surplus " +
			"Obviously if a question is asked about the weather or something factual it may not be appropriate to call autocast_hexagram() ";
	}

	private void OnLoaded(object? sender, EventArgs e)
	{
		Loaded -= OnLoaded;
		ResetChat();
		LoadSessions();
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

		double w = width - 10;

		w = width - 40;

		w /= 2;

		//w -= 5;

		w /= 2;
		btnAdd.WidthRequest = w;
		btnDelete.WidthRequest = w;

		base.OnSizeAllocated(width, height);
	}

	private void OnAddSessionClicked(object? sender, EventArgs e)
	{
		AddSession();
	}

	private async void OnDeleteSessionClicked(object? sender, EventArgs e)
	{
		if (_selectedSession is null)
			return;

		bool confirm = await Window.Page!.DisplayAlertAsync("Delete Session", $"Delete {_selectedSession.Name}?", "Yes", "No");
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

			using var yc = new YijingDbContext();
			yc.Sessions.Where(s => s.Id == _selectedSession.Id).ExecuteDelete();

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
			await Window.Page!.DisplayAlertAsync("Delete Session", $"Unable to delete the session. {ex.Message}", "OK");
		}
	}

	private void OnSessionsSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		_selectedSession = e.CurrentSelection.FirstOrDefault() as Session;
		LoadSelectedSession(_selectedSession);
		if (!string.IsNullOrEmpty(_selectedSession?.YijingCast))
			UI.Call<DiagramView>(v => v.SetHexagramCast(_selectedSession?.YijingCast));

		string? file = Path.GetFileNameWithoutExtension(_selectedSession?.FileName);
		if (_selectedSession?.EegDevice != eEegDevice.eNone)
			UI.Call<EegView>(v => v.SelectSession(file));
	}

	private void OnContextSelectionChanged(object? sender, CheckedChangedEventArgs e)
	{
		if (_isSyncingContexts)
			return;
		if (sender is not CheckBox checkbox || checkbox.BindingContext is not Session session)
			return;
		if (string.IsNullOrEmpty(session.FileName))
			return;

		if (e.Value)
		{
			if (!_ai._contextSessions.Any(s => s.Equals(session.FileName, StringComparison.OrdinalIgnoreCase)))
				_ai._contextSessions.Add(session.FileName);
		}
		else
			_ai._contextSessions.RemoveAll(s => s.Equals(session.FileName, StringComparison.OrdinalIgnoreCase));

		SortContextSessions();
		_contextSelectionDirty = true;
		UpdateChat();
	}

	public void ButtonPadding(Thickness thickness)
	{
		btnAdd.Padding = thickness;
		btnDelete.Padding = thickness;
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
		sb.Append("h2 {" + $" color: {strAC};" + "} ");
		sb.Append("h2 {" + $" color: {strAC};" + "} ");
		sb.Append("h4 {" + $" color: {strAC};" + "} ");
		sb.Append("</style></head><body>");

		if (_selectedSession is not null)
		{
			sb.Append($"<h2>{WebUtility.HtmlEncode(_selectedSession.Name)}");
			string? yjingCast = WebUtility.HtmlEncode(_selectedSession.YijingCast);
			if (!string.IsNullOrEmpty(yjingCast))
				sb.Append($" - {yjingCast}");
		}

		if (_ai._contextSessions.Count() > 0)
			sb.Append("</p>Context Sessions: ");
		foreach (var s in _ai._contextSessions)
			sb.Append(s + " ");

		sb.Append("</h2>");

		foreach (string s in _sessionNotes)
			sb.Append("<p><h4>" + s.Replace("\n", "</p>") + "</h4></p>");

		int count = int.Max(_ai._chatReponses[1].Count(), _ai._userPrompts[1].Count());
		for (int i = 0; i < count; ++i)
		{
			if (i < _ai._userPrompts[1].Count())
				sb.Append("<p><h4>" + _ai._userPrompts[1][i].Replace("\n", "</p>") + "</h4></p>");
			if (i < _ai._chatReponses[1].Count())
				sb.Append("<p>" + _ai._chatReponses[1][i].Replace("\n", "</p>") + "</p>");
		}

		sb.Append("</body></html>");
		UI.Try<SessionPage>(p => p.WebView().Source = new HtmlWebViewSource { Html = sb.ToString() });
		//ChatUpdated?.Invoke(this, sb.ToString());
	}

	private void LoadSessions(bool reload = false)
	{
		try
		{
			using (var yc = new YijingDbContext())
			{
				if ((reload || yc.Migrated("SessionDevice")) && yc.Sessions.Any())
					yc.Sessions.ExecuteDelete();
				if ((reload || yc.Migrated("AddMeditation")) && yc.Meditations.Any())
					yc.Meditations.ExecuteDelete();

				List<Meditation> lm = [];
				bool anyMeditations = yc.Meditations.Any();

				if (!yc.Sessions.Any())
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
							var session = CreateSession(file);

							if (i > 0)
								if (DateTime.TryParseExact(session.FileName, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture,
									DateTimeStyles.AssumeLocal, out DateTime dt))
									lm.Add(new Meditation { Start = dt, Duration = 60 }); // ??????????????????

							var match = _sessions.FirstOrDefault(s => s.FileName!.Equals(session.FileName,
								StringComparison.OrdinalIgnoreCase));
							if (match is not null)
							{
								if (i > 0)
								{
									match.Meditation = true;
									match.EegAnalysis = File.Exists(Path.Combine(AppSettings.DocumentHome(), "Analysis",
										Path.GetFileNameWithoutExtension(file) + ".txt"));
								}
								match.EegDevice = i == 1 ? eEegDevice.eMuse : eEegDevice.eEmotiv;
							}
							else
							{
								if (i > 0)
								{
									session.Meditation = true;
									session.EegAnalysis = File.Exists(Path.Combine(AppSettings.DocumentHome(), "Analysis",
										Path.GetFileNameWithoutExtension(file) + ".txt"));
									session.EegDevice = i == 1 ? eEegDevice.eMuse : eEegDevice.eEmotiv;
								}
								_sessions.Add(session);
							}
						}
					}
					yc.Sessions.AddRange(_sessions);
					yc.Meditations.AddRange(lm);
					YijingDatabase.SaveChanges(yc);
				}

				string filePath = Path.Combine(AppSettings.DocumentHome(), "Meditation.txt");
				if (!anyMeditations && File.Exists(filePath))
				{
					lm = [];
					IEnumerable<string> text = File.ReadLines(filePath);
					foreach (string line in text)
					{
						string[] parts = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
						if (parts.Length > 3)
							if (DateTime.TryParseExact(parts[2], "yyyy-MM-ddTHH:mm:ss.ffffff", CultureInfo.InvariantCulture,
								DateTimeStyles.AssumeLocal, out DateTime dt1) &&
								DateTime.TryParseExact(parts[3], "yyyy-MM-ddTHH:mm:ss.ffffff", CultureInfo.InvariantCulture,
								DateTimeStyles.AssumeLocal, out DateTime dt2))
							{
								TimeSpan ts = dt2 - dt1;
								dt1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, dt1.Second);
								lm.Add(new Meditation { Start = dt1, Duration = (int)ts.TotalMinutes });
								string matchname = dt1.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
								var match = _sessions.FirstOrDefault(s => s.FileName!.StartsWith(matchname,
									StringComparison.OrdinalIgnoreCase));
								if (match is null)
								{
									match = new Session(0, dt1.ToString("MMM dd HH:mm", CultureInfo.InvariantCulture),
										"Meditation session", dt1.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture),
										"", true);
									yc.Sessions.Add(match);
								}
								else
									match.Meditation = true;
								YijingDatabase.SaveChanges(yc);
							}
					}
					yc.Meditations.AddRange(lm);
					YijingDatabase.SaveChanges(yc);
				}

				_sessions = new ObservableCollection<Session>(yc.Sessions.AsNoTracking().ToList()
					.OrderByDescending(s => s.FileName, StringComparer.OrdinalIgnoreCase));
			}

			sessionCollection.ItemsSource = _sessions;
			SelectSession(null);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to load sessions: {ex.Message}");
		}
	}

	private Session CreateSession(string filePath)
	{
		string fileName = Path.GetFileNameWithoutExtension(filePath);

		if (fileName.EndsWith("-Muse"))
			fileName = fileName.Substring(0, fileName.Length - 5);
		else if (fileName.EndsWith("-Emotiv"))
			fileName = fileName.Substring(0, fileName.Length - 7);

		string extension = Path.GetExtension(filePath);
		string name = FormatSessionName(fileName);
		string description = "New session";
		string yijingCast = "";

		if (!string.IsNullOrEmpty(extension))
			if (extension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
				ReadText(filePath, ref description, ref yijingCast);
			else if (extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
				description = "EEG session";

		bool meditation = false;
		if (DateTime.TryParseExact(fileName, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture,
			DateTimeStyles.AssumeLocal, out var dt))
		{
			var dayStart = dt.Date;
			var dayEnd = dayStart.AddDays(1);
			using var yc = new YijingDbContext();
			meditation = yc.Meditations.AsNoTracking()
				.Any(m => (m.Start >= dayStart) && (m.Start < dayEnd));
			if (meditation && (description == "New session"))
				description = "Meditation session";
		}

		return new Session(0, name, description, fileName, yijingCast, meditation);
	}

	private static string FormatSessionName(string fileName)
	{
		if (DateTime.TryParseExact(fileName, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture,
			DateTimeStyles.AssumeLocal, out DateTime dt))
			return dt.ToString("MMM dd HH:mm", CultureInfo.InvariantCulture);
		return fileName;
	}

	private static void ReadText(string filePath, ref string description, ref string yijingCast)
	{
		string s1 = "Yijing responded with hexagram";
		string s2 = "Yijing cast yielded hexagram";
		try
		{
			if (!File.Exists(filePath))
				return;

			string text = File.ReadAllText(filePath);
			if (string.IsNullOrWhiteSpace(text))
				return;

			text = Regex.Replace(text, @"\$\((Context)\)\s*[^\r\n]*\r?\n?", "", RegexOptions.IgnoreCase);

			string? castLine = text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
					.Select(line => line.Trim())
					.FirstOrDefault(line => line.Contains(s1, StringComparison.OrdinalIgnoreCase) ||
						line.Contains(s2, StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrEmpty(castLine))
			{
				int index;
				if ((index = castLine.IndexOf(s1, StringComparison.OrdinalIgnoreCase)) != -1)
					castLine = castLine.Substring(index + s1.Length + 1).Trim();
				else if ((index = castLine.IndexOf(s2, StringComparison.OrdinalIgnoreCase)) != -1)
					castLine = castLine.Substring(index + s2.Length + 1).Trim();
				yijingCast = castLine;
			}

			var words = text.Replace("$(Question)", "").Replace("$(Note)", "").Split([' ', '\r', '\n', '\t'],
				StringSplitOptions.RemoveEmptyEntries).Take(10);
			description = string.Join(" ", words);

		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to read session '{filePath}': {ex.Message}");
			return;
		}
	}

	public void AddSession()
	{
		string fileName = AppSettings.ReverseDateString();
		var summary = CreateSession(fileName);
		_sessions.Insert(0, summary);
		sessionCollection.SelectedItem = summary;

		using var yc = new YijingDbContext();
		var x = yc.Sessions.Add(summary);
		YijingDatabase.SaveChanges(yc);
	}

	public void AddMeditationSession()
	{
		DateTime dt = DateTime.Now;
		string matchname = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		var match = _sessions.FirstOrDefault(s => s.FileName!.StartsWith(matchname,
			StringComparison.OrdinalIgnoreCase) && !s.Meditation);
		if (match is not null)
		{
			using var yc = new YijingDbContext();
			sessionCollection.SelectedItem = match;
			int i = _sessions.IndexOf(_selectedSession!);
			match = yc.Sessions.Find(match.Id);
			if (match is not null)
			{
				match.Meditation = true;
				if (match.Description == "New session")
					match.Description = "Meditation session";
				YijingDatabase.SaveChanges(yc);
				_sessions[i] = match;
				sessionCollection.SelectedItem = match;
			}
		}
		else
			AddSession();
	}

	private void SelectSession(string? selectSession)
	{
		if (!string.IsNullOrEmpty(selectSession))
		{
			var match = _sessions.FirstOrDefault(s => s.FileName!.Equals(selectSession, StringComparison.OrdinalIgnoreCase));
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

	private void LoadSelectedSession(Session? session)
	{
		ClearChatState();
		if (session is not null)
			LoadChatSessionData(session.FileName!);
		UpdateChat();
	}

	public async void AiOrNote(string prompt, bool includeCast)
	{
		string s = string.Empty;
		if (includeCast)
		{
			UI.Try<DiagramView>(v => s = v.DescribrCastHexagram());
			if (string.IsNullOrWhiteSpace(s))
			{
				await Window.Page!.DisplayAlertAsync("No Cast", "Please cast a hexagram first.", "OK");
				return;
			}
			prompt += (prompt.Length > 0 ? "\n" : "") + "The Yijing cast yielded hexagram " + s;
		}

		if (string.IsNullOrWhiteSpace(prompt))
		{
			await Window.Page!.DisplayAlertAsync("No Prompt", "Please enter a prompt.", "OK");
			return;
		}

		if (AppPreferences.AiChatService == (int)eAiService.eNone)
			_sessionNotes.Add(prompt);
		else
		{
			EnsureContextSessionsLoadedForChat();
			await _ai.ChatAsync(AppPreferences.AiChatService, prompt, false);
		}

		SaveChat(_selectedSession!.FileName!);
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
		_sessionNotes = [];

		_ai._userPrompts = [[], []];
		_ai._chatReponses = [[], []];
		_ai._contextSessions = [];
	}

	private void EnsureContextSessionsLoadedForChat()
	{
		if (!_contextSelectionDirty)
			return;
		if (_selectedSession?.FileName is null)
			return;

		SortContextSessions();
		SaveChat(_selectedSession.FileName);
		ClearChatState();
		LoadChatSessionData(_selectedSession.FileName);
		//_contextSelectionDirty = false;
	}

	private void SetContextSessions(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return;
		foreach (string context in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			if (!_ai._contextSessions.Any(s => s.Equals(context, StringComparison.OrdinalIgnoreCase)))
				_ai._contextSessions.Add(context);
	}

	private void SortContextSessions()
	{
		_ai._contextSessions = _ai._contextSessions
			.Distinct(StringComparer.OrdinalIgnoreCase)
			.OrderBy(session => TryParseSessionDate(session, out DateTime date) ? date : DateTime.MaxValue)
			.ThenBy(session => session, StringComparer.OrdinalIgnoreCase)
			.ToList();
	}

	private static bool TryParseSessionDate(string session, out DateTime date)
	{
		return DateTime.TryParseExact(session, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture,
			DateTimeStyles.AssumeLocal, out date);
	}

	private void UpdateContextSelectionFlags()
	{
		_isSyncingContexts = true;
		try
		{
			HashSet<string> contextLookup = new(_ai._contextSessions, StringComparer.OrdinalIgnoreCase);
			foreach (Session session in _sessions)
			{
				if (string.IsNullOrEmpty(session.FileName))
					continue;
				session.IsContextSelected = contextLookup.Contains(session.FileName);
			}
		}
		finally
		{
			_isSyncingContexts = false;
		}
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
		if ((_ai._userPrompts[1].Count > 0) || (_ai._chatReponses[1].Count > 0) || (_sessionNotes.Count > 0))
		{
			SaveChat(name, "Question", _ai._userPrompts[1]);
			SaveChat(name, "Answer", _ai._chatReponses[1]);

			var summary = CreateSession(Path.Combine(AppSettings.DocumentHome(), "Questions", name + ".txt"));
			summary.Id = _selectedSession?.Id ?? 0;

			using var yc = new YijingDbContext();
			yc.Sessions.Update(summary);
			YijingDatabase.SaveChanges(yc);

			int i = _sessions.IndexOf(_selectedSession!);
			_sessions[i] = summary;
			sessionCollection.SelectedItem = summary;
		}
	}

	public void SaveChat(string name, string type, List<string> list)
	{
		string str = Path.Combine(AppSettings.DocumentHome(), $"{type}s", name + ".txt");
		using (FileStream fs = new(str, FileMode.Create, FileAccess.Write))
		{
			if (type == "Question")
			{
				SortContextSessions();
				string contexts = string.Join(",", _ai._contextSessions);
				byte[] contextVal = Encoding.UTF8.GetBytes("$(Context)\n" + contexts + "\n");
				fs.Write(contextVal, 0, contextVal.Length);
				foreach (string s in _sessionNotes)
				{
					byte[] val = Encoding.UTF8.GetBytes($"$(Note)\n" + s + "\n");
					fs.Write(val, 0, val.Length);
				}
			}
			foreach (string s in list)
			{
				byte[] val = Encoding.UTF8.GetBytes($"$({type})\n" + s + "\n");
				fs.Write(val, 0, val.Length);
			}
		}
	}

	public void LoadChatSessionData(string session)
	{
		if (string.IsNullOrEmpty(session))
			return;

		_ai._contextSessions = [];
		LoadChatFile(session, "Question", _ai._userPrompts[1], true, true);
		LoadChatFile(session, "Answer", _ai._chatReponses[1], false, false);
		SortContextSessions();
		UpdateContextSelectionFlags();
		_contextSelectionDirty = false;

		for (int i = 0; i < _ai._contextSessions.Count; ++i)
		{
			LoadChatFile(_ai._contextSessions[i], "Question", _ai._userPrompts[0], false, false);
			LoadChatFile(_ai._contextSessions[i], "Answer", _ai._chatReponses[0], false, false);
		}

		if ((_ai._userPrompts[1].Count > 0) || (_ai._chatReponses[1].Count > 0))
			UpdateChat();
	}

	private void LoadChatFile(string name, string type, List<string> list, bool allowNotes, bool allowContext)
	{
		string path = Path.Combine(AppSettings.DocumentHome(), $"{type}s", name + ".txt");
		if (!File.Exists(path))
		{
			UpdateSessionLog("Failed to load " + path, true, true);
			return;
		}

		string? currentSection = null;
		StringBuilder buffer = new();

		void FlushEntry()
		{
			if (string.IsNullOrEmpty(currentSection))
				return;

			string entry = buffer.ToString().TrimEnd(); // '\n'
			buffer.Clear();

			if (string.IsNullOrEmpty(entry))
				return;

			if (currentSection == type)
				list.Add(entry);
			else if (currentSection == "Note" && allowNotes)
				_sessionNotes.Add(entry);
			else if (currentSection == "Context" && allowContext)
				SetContextSessions(entry);
		}

		using StreamReader sr = File.OpenText(path);
		string? line;
		while ((line = sr.ReadLine()) != null)
		{
			if (string.IsNullOrWhiteSpace(line))
				continue;

			if (line.StartsWith("$(") && line.EndsWith(")"))
			{
				FlushEntry();
				currentSection = line[2..^1];
				continue;
			}

			buffer.AppendLine(line);
		}

		FlushEntry();
	}

	public async Task NavigateToDialogPage()
	{
		await Shell.Current.GoToAsync("//Diagram/DiagramRoot", true);
	}

}
