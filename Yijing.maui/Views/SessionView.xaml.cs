
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
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
	private List<SessionSearchEntry> _searchResults = [];
	private List<string> _searchTerms = [];
	private bool _searchResultsActive;

	private const string SearchUriScheme = "yijing-search";

	private sealed class SessionSearchEntry
	{
		public int SessionId { get; init; }
		public string Name { get; init; } = string.Empty;
		public string FileName { get; init; } = string.Empty;
		public string YijingCast { get; init; } = string.Empty;
		public string Keywords { get; init; } = string.Empty;
		public string Summary { get; init; } = string.Empty;
	}

	private const string SummarySystemPrompt = @"
You are an indexing engine for a personal archive of AI chat sessions that mix technical engineering
 discussion and spiritual/reflection (including possible Yijing casts).

Your job is to extract a compact, highly searchable record from the provided transcript.

Output ONLY valid JSON. No markdown, no commentary, no extra keys, no trailing text.

You will be given the full transcript containing User/Assistant turns.

Write a retrieval record that helps the user later find the session by searching with a few remembered concepts.

Don't ifentify the user or assistant, just record the dialog.

Don't try to capture everything, just the most distinctive and memorable elements that would help someone find this session later.

Don't include Yijing cast descriptions or hexagram numbers just use hexagram names as relevant natural language.

Hard requirements:
- Follow the schema exactly.
- Respect word/item limits exactly.
- Prefer concrete, distinctive nouns and phrases over generic terms.
- Do not invent facts; if something is unclear, omit it.
- Keep summary plain prose (no bullets, no numbering).

Schema (exact keys, exact types):
{
""Keywords"": string[],
""Summary"": string,
}

Field rules:
- Keywords: exactly 10 items. Each item is 1 word, lowercase, no punctuation. Avoid near-duplicates. Mix technical + reflective terms if present.
- Summary: 90-110 words exactly. Must include: the user's goal, notable constraints, the key turn/insight, and the outcome or next step.

Keyword selection guidance:
- Include at least 3 technical keywords if technical content exists, and at least 3 reflective/spiritual keywords if that content exists.
- Good keywords name: features, design, structure, retrieval, strategy, emotion, decision
- DO NOT use the foloowing keywords: yijing, hexagram, meditation, contemplation, cast, ai, assistant, user, session, summary

Return only the JSON object.";

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

		double w = Math.Max(0, (width - 40) / 3);
		btnAdd.WidthRequest = w;
		btnDelete.WidthRequest = w;
		btnSearch.WidthRequest = w;

		base.OnSizeAllocated(width, height);
	}

	private void OnAddSessionClicked(object? sender, EventArgs e)
	{
		AddSession(AppSettings.ReverseDateString());
	}

	private async void OnSearchSessionClicked(object? sender, EventArgs e)
	{
		await SearchAsync();
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
		if (_searchResultsActive)
			AbandonSearchResults();

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
		btnSearch.Padding = thickness;
	}

	public void UpdateChat()
	{
		if (_searchResultsActive)
		{
			RenderSearchResults();
			return;
		}

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

	public bool HandleWebViewNavigation(string? url)
	{
		if (string.IsNullOrWhiteSpace(url))
			return false;

		if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
			return false;

		if (!uri.Scheme.Equals(SearchUriScheme, StringComparison.OrdinalIgnoreCase))
			return false;

		if (!_searchResultsActive)
			return true;

		HashSet<int> selectedSessionIds = ParseSelectedSessionIds(uri);
		if (uri.Host.Equals("add", StringComparison.OrdinalIgnoreCase))
			AddSelectedSearchResultsToContextSessions(selectedSessionIds);
		else if (uri.Host.Equals("delete", StringComparison.OrdinalIgnoreCase))
			RemoveSearchResults(selectedSessionIds);
		else if (uri.Host.Equals("cancel", StringComparison.OrdinalIgnoreCase))
		{
			AbandonSearchResults();
			UpdateChat();
		}

		return true;
	}

	private async Task SearchAsync()
	{
		string? input = await Window.Page!.DisplayPromptAsync("Search Sessions",
			"Enter one or more words to search in YijingCast and Keywords.", "Search", "Cancel");
		if (input is null)
			return;

		List<string> terms = TokenizeSearchTerms(input)
			.Distinct(StringComparer.OrdinalIgnoreCase)
			.ToList();
		if (terms.Count == 0)
		{
			await Window.Page!.DisplayAlertAsync("Search Sessions", "Please enter at least one word.", "OK");
			return;
		}

		_searchTerms = terms;
		_searchResults = FindSearchResults(terms);
		_searchResultsActive = _searchResults.Count > 0;

		if (_searchResultsActive)
		{
			RenderSearchResults();
			return;
		}

		AbandonSearchResults();
		UpdateChat();
		await Window.Page!.DisplayAlertAsync("Search Sessions", "No matching sessions were found.", "OK");
	}

	private List<SessionSearchEntry> FindSearchResults(IEnumerable<string> terms)
	{
		HashSet<string> searchWords = new(terms, StringComparer.OrdinalIgnoreCase);

		using var yc = new YijingDbContext();
		List<SessionSearchEntry> entries = (
			from session in yc.Sessions.AsNoTracking()
			join sessionSummary in yc.SessionSummaries.AsNoTracking()
				on session.Id equals sessionSummary.SessionId into summaryJoin
			from summary in summaryJoin.DefaultIfEmpty()
			select new SessionSearchEntry
			{
				SessionId = session.Id,
				Name = session.Name,
				FileName = session.FileName ?? string.Empty,
				YijingCast = session.YijingCast ?? string.Empty,
				Keywords = summary != null ? summary.Keywords : string.Empty,
				Summary = summary != null ? summary.Summary : string.Empty
			}).ToList();

		return entries
			.Where(entry => MatchesSearchTerms(entry, searchWords))
			.OrderByDescending(entry => entry.FileName, StringComparer.OrdinalIgnoreCase)
			.ThenBy(entry => entry.Name, StringComparer.OrdinalIgnoreCase)
			.ToList();
	}

	private static bool MatchesSearchTerms(SessionSearchEntry entry, HashSet<string> searchWords)
	{
		HashSet<string> columnWords = new(TokenizeSearchTerms(entry.YijingCast), StringComparer.OrdinalIgnoreCase);
		foreach (string word in TokenizeSearchTerms(entry.Keywords))
			columnWords.Add(word);

		return searchWords.Any(columnWords.Contains);
	}

	private static List<string> TokenizeSearchTerms(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return [];

		return Regex.Matches(value, @"[\p{L}\p{N}]+")
			.Select(match => match.Value.Trim())
			.Where(word => !string.IsNullOrWhiteSpace(word))
			.ToList();
	}

	private void RenderSearchResults()
	{
		string strBC = App.Current?.RequestedTheme == AppTheme.Dark ? "black" : "white";
		string strFC = App.Current?.RequestedTheme == AppTheme.Dark ? "white" : "black";
		string strAC = App.Current?.RequestedTheme == AppTheme.Dark ? "gray" : "gray";

		var sb = new StringBuilder();
		sb.Append("<html><head><meta charset=\"utf-8\"/><style>");
		sb.Append("body{");
		sb.Append($"background-color:{strBC};color:{strFC};font-family:'Open Sans',sans-serif;font-size:16px;line-height:1.5;");
		sb.Append("}");
		sb.Append("h2{margin:0 0 10px 0;color:" + strAC + ";}");
		sb.Append("p{margin:4px 0;}");
		sb.Append(".toolbar{display:flex;gap:8px;flex-wrap:wrap;margin:8px 0 12px 0;}");
		sb.Append(".toolbar button{border:1px solid " + strAC + ";border-radius:6px;padding:4px 12px;background-color:" + strBC + ";color:" + strFC + ";}");
		sb.Append(".entry{border:1px solid " + strAC + ";border-radius:8px;padding:10px;margin:10px 0;}");
		sb.Append(".selector{display:flex;align-items:center;gap:8px;margin-bottom:8px;}");
		sb.Append("</style></head><body>");

		sb.Append("<form method=\"get\">");
		sb.Append($"<h2>Search Results ({_searchResults.Count})</h2>");
		if (_searchTerms.Count > 0)
		{
			sb.Append("<p><strong>Words:</strong> ");
			sb.Append(WebUtility.HtmlEncode(string.Join(" ", _searchTerms)));
			sb.Append("</p>");
		}

		sb.Append("<div class=\"toolbar\">");
		sb.Append($"<button type=\"submit\" formaction=\"{SearchUriScheme}://add\">Add</button>");
		sb.Append($"<button type=\"submit\" formaction=\"{SearchUriScheme}://delete\">Delete</button>");
		sb.Append($"<button type=\"submit\" formaction=\"{SearchUriScheme}://cancel\">Cancel</button>");
		sb.Append("</div>");

		for (int i = 0; i < _searchResults.Count; ++i)
		{
			SessionSearchEntry entry = _searchResults[i];

			sb.Append("<div class=\"entry\">");
			sb.Append("<label class=\"selector\">");
			sb.Append($"<input type=\"checkbox\" name=\"id\" value=\"{entry.SessionId.ToString(CultureInfo.InvariantCulture)}\" />");
			sb.Append("<span>Select</span>");
			sb.Append("</label>");

			AppendSearchField(sb, "Name", entry.Name);
			AppendSearchField(sb, "YijingCast", entry.YijingCast);
			AppendSearchField(sb, "Keywords", entry.Keywords);
			AppendSearchField(sb, "Summary", entry.Summary);

			sb.Append("</div>");
		}

		sb.Append("</form>");
		sb.Append("</body></html>");
		UI.Try<SessionPage>(p => p.WebView().Source = new HtmlWebViewSource { Html = sb.ToString() });
	}

	private static HashSet<int> ParseSelectedSessionIds(Uri uri)
	{
		HashSet<int> selectedSessionIds = [];
		if (string.IsNullOrWhiteSpace(uri.Query))
			return selectedSessionIds;

		string[] pairs = uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
		foreach (string pair in pairs)
		{
			string[] kv = pair.Split('=', 2);
			if ((kv.Length != 2) || !kv[0].Equals("id", StringComparison.OrdinalIgnoreCase))
				continue;

			string rawValue = Uri.UnescapeDataString(kv[1]);
			if (int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int sessionId))
				selectedSessionIds.Add(sessionId);
		}

		return selectedSessionIds;
	}

	private static void AppendSearchField(StringBuilder sb, string name, string? value)
	{
		sb.Append("<p><strong>");
		sb.Append(WebUtility.HtmlEncode(name));
		sb.Append(":</strong> ");
		sb.Append(WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(value) ? "-" : value));
		sb.Append("</p>");
	}

	private void RemoveSearchResults(HashSet<int> selectedSessionIds)
	{
		if (!_searchResultsActive || (_searchResults.Count == 0) || (selectedSessionIds.Count == 0))
			return;

		int removed = _searchResults.RemoveAll(entry => selectedSessionIds.Contains(entry.SessionId));
		if (removed == 0)
			return;

		if (_searchResults.Count == 0)
		{
			AbandonSearchResults();
			UpdateChat();
			return;
		}

		RenderSearchResults();
	}

	private void AddSelectedSearchResultsToContextSessions(HashSet<int> selectedSessionIds)
	{
		if (!_searchResultsActive || (_searchResults.Count == 0) || (selectedSessionIds.Count == 0))
			return;

		bool changed = false;
		foreach (SessionSearchEntry entry in _searchResults)
		{
			if (!selectedSessionIds.Contains(entry.SessionId))
				continue;

			string contextName = !string.IsNullOrWhiteSpace(entry.FileName) ? entry.FileName : entry.Name;
			if (string.IsNullOrWhiteSpace(contextName))
				continue;
			if (_selectedSession?.FileName is not null &&
				contextName.Equals(_selectedSession.FileName, StringComparison.OrdinalIgnoreCase))
				continue;
			if (_ai._contextSessions.Any(s => s.Equals(contextName, StringComparison.OrdinalIgnoreCase)))
				continue;

			_ai._contextSessions.Add(contextName);
			changed = true;
		}

		if (changed)
		{
			SortContextSessions();
			_contextSelectionDirty = true;
			UpdateContextSelectionFlags();
		}

		RemoveSearchResults(selectedSessionIds);
	}

	private void AbandonSearchResults()
	{
		_searchResultsActive = false;
		_searchResults = [];
		_searchTerms = [];
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

	public void AddSession(string fileName, bool meditation = false, eEegDevice eegDevice = eEegDevice.eNone)
	{
		var summary = CreateSession(fileName);
		summary.Meditation = meditation;
		summary.EegDevice = eegDevice;
		if (eegDevice != eEegDevice.eNone)
			summary.Description = "EEG session";
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
			AddSession(AppSettings.ReverseDateString(), true);
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

	public async Task GenerateSummaryAsync()
	{
		if (_selectedSession is null)
		{
			await Window.Page!.DisplayAlertAsync("AI Summary", "Please select a session first.", "OK");
			return;
		}

		if (_selectedSession.Id == 0)
		{
			await Window.Page!.DisplayAlertAsync("AI Summary", "Please save the session before summarising.", "OK");
			return;
		}

		if (AiPreferences.IsNoneService(AppPreferences.AiChatService))
		{
			await Window.Page!.DisplayAlertAsync("AI Summary", "Select an AI chat service in settings.", "OK");
			return;
		}

		string transcript = BuildTranscriptForSummary(_selectedSession);
		if (string.IsNullOrWhiteSpace(transcript))
		{
			await Window.Page!.DisplayAlertAsync("AI Summary", "No transcript available for this session.", "OK");
			return;
		}

		try
		{
			string userPrompt = $"Transcript for session '{_selectedSession.Name}' ({_selectedSession.FileName}):\n\n{transcript}";
			string response = await _ai.ChatOnceAsync(AppPreferences.AiChatService, SummarySystemPrompt, userPrompt, false);
			string json = ExtractJsonObject(response);

			if (!TryParseSummaryJson(json, out string summary, out string keywordsCsv, out string error))
			{
				await Window.Page!.DisplayAlertAsync("AI Summary", $"Unable to store summary. {error}", "OK");
				return;
			}

			SaveSessionSummary(_selectedSession.Id, summary, keywordsCsv);
			await Window.Page!.DisplayAlertAsync("AI Summary", summary + "\n\n" + keywordsCsv /*"Summary saved for this session."*/, "OK");
		}
		catch (Exception ex)
		{
			await Window.Page!.DisplayAlertAsync("AI Summary", $"Failed to generate summary. {ex.Message}", "OK");
		}
	}

	private string BuildTranscriptForSummary(Session session)
	{
		var sb = new StringBuilder();

		sb.AppendLine($"Session: {session.Name}");
		/*
		if (!string.IsNullOrWhiteSpace(session.FileName))
			sb.AppendLine($"File: {session.FileName}");
		if (!string.IsNullOrWhiteSpace(session.YijingCast))
			sb.AppendLine($"YijingCast: {session.YijingCast}");
		if (!string.IsNullOrWhiteSpace(session.Description))
		{
			sb.AppendLine("Description:");
			sb.AppendLine(session.Description);
		}
		*/
		if (_sessionNotes.Count > 0)
		{
			sb.AppendLine();
			sb.AppendLine("Notes:");
			foreach (string note in _sessionNotes)
				sb.AppendLine(note.Trim());
		}

		int count = int.Max(_ai._chatReponses[1].Count, _ai._userPrompts[1].Count);
		if (count > 0)
		{
			sb.AppendLine();
			sb.AppendLine("Transcript:");
			for (int i = 0; i < count; ++i)
			{
				if (i < _ai._userPrompts[1].Count)
					sb.AppendLine("User: " + _ai._userPrompts[1][i]);
				if (i < _ai._chatReponses[1].Count)
					sb.AppendLine("Assistant: " + _ai._chatReponses[1][i]);
			}
		}

		return sb.ToString().Trim();
	}

	private static string ExtractJsonObject(string response)
	{
		if (string.IsNullOrWhiteSpace(response))
			return string.Empty;

		string trimmed = response.Trim();
		int start = trimmed.IndexOf('{');
		int end = trimmed.LastIndexOf('}');
		if ((start >= 0) && (end >= start))
			return trimmed.Substring(start, end - start + 1).Trim();

		return trimmed;
	}

	private static bool TryParseSummaryJson(string json, out string summary, out string keywordsCsv, out string error)
	{
		summary = string.Empty;
		keywordsCsv = string.Empty;
		error = string.Empty;

		if (string.IsNullOrWhiteSpace(json))
		{
			error = "AI returned an empty response.";
			return false;
		}

		try
		{
			using JsonDocument doc = JsonDocument.Parse(json);
			var root = doc.RootElement;
			if (root.ValueKind != JsonValueKind.Object)
			{
				error = "Response must be a JSON object.";
				return false;
			}

			if (!root.TryGetProperty("Keywords", out JsonElement keywords) || (keywords.ValueKind != JsonValueKind.Array))
			{
				error = "Missing Keywords array.";
				return false;
			}
			if (keywords.GetArrayLength() > 15)
			{
				error = "Keywords must contain 15 items.";
				return false;
			}
			List<string> keywordsList = new();
			foreach (JsonElement item in keywords.EnumerateArray())
			{
				if (item.ValueKind != JsonValueKind.String)
				{
					error = "Each keyword must be a string.";
					return false;
				}
				string? kw = item.GetString();
				if (string.IsNullOrWhiteSpace(kw))
				{
					error = "Keywords must not be empty.";
					return false;
				}
				keywordsList.Add(kw.Trim());
			}

			if (!root.TryGetProperty("Summary", out JsonElement summaryElement) || (summaryElement.ValueKind != JsonValueKind.String))
			{
				error = "Missing Summary string.";
				return false;
			}

			summary = summaryElement.GetString() ?? string.Empty;

			int wordCount = CountWords(summary);
			if ((wordCount < 80) || (wordCount > 150))
			{
				error = $"Summary must be 80-150 words, found {wordCount}.";
				return false;
			}

			/*	Prompt text
				""YijingCasts"": string[],
				- YijingCasts: 0-5 items. Each must be verbatim cast description in this format if present: ""41.3 Decrease > 26 Discipline"". If none, empty array.

			if (!root.TryGetProperty("YijingCasts", out JsonElement casts) || (casts.ValueKind != JsonValueKind.Array))
			{
				error = "Missing YijingCasts array.";
				return false;
			}
			
			foreach (JsonElement item in casts.EnumerateArray())
			{
				if (item.ValueKind != JsonValueKind.String)
				{
					error = "Each YijingCast must be a string.";
					return false;
				}
			}
			*/

			keywordsCsv = string.Join(",", keywordsList);
			return true;
		}
		catch (JsonException ex)
		{
			error = ex.Message;
			return false;
		}
	}

	private static int CountWords(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return 0;

		return value.Split([' ', '\r', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries).Length;
	}

	private void SaveSessionSummary(int sessionId, string summary, string keywordsCsv)
	{
		using var yc = new YijingDbContext();
		var existing = yc.SessionSummaries.SingleOrDefault(ss => ss.SessionId == sessionId);
		if (existing is null)
			yc.SessionSummaries.Add(new SessionSummary { SessionId = sessionId, Summary = summary, Keywords = keywordsCsv });
		else
		{
			existing.Summary = summary;
			existing.Keywords = keywordsCsv;
			yc.SessionSummaries.Update(existing);
		}

		YijingDatabase.SaveChanges(yc);
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
		if (_selectedSession?.FileName is null)
		{
			await Window.Page!.DisplayAlertAsync("No Session", "Please select a session.", "OK");
			return;
		}

		bool hadSearchResults = _searchResultsActive;

		if (AiPreferences.IsNoneService(AppPreferences.AiChatService))
			_sessionNotes.Add(prompt);
		else
		{
			EnsureContextSessionsLoadedForChat();
			await _ai.ChatAsync(AppPreferences.AiChatService, prompt, false);
		}

		if (hadSearchResults)
			AbandonSearchResults();

		SaveChat(_selectedSession.FileName);
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
