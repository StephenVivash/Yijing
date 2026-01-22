using System.Collections.ObjectModel;

using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;

#if WINDOWS
using Windows.Win32;
using Windows.Win32.Foundation;
#endif

using Platform = Yijing.Platforms.Platform;
using Yijing.Models;
using Yijing.Services;
using Yijing.Pages;
using YijingData;

namespace Yijing.Views;

// https://lvcharts.com/docs/Maui/2.0.0-beta.330/Overview.Installation
// https://github.com/jsuarezruiz/awesome-dotnet-maui
// https://lvcharts.com/docs/Maui/2.0.0-beta.330/gallery

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////

public class DescendingOrder : Comparer<string>
{
	public override int Compare(string x, string y)
	{
		if (x.StartsWith("museMonitor_"))
			x = x.Remove(0, 12);
		if (y.StartsWith("museMonitor_"))
			y = y.Remove(0, 12);
		return y.CompareTo(x);
	}
}

public partial class EegView : ContentView
{
	public int m_nEegMode = (int)eEegMode.eIdle;
	private int m_nSeriesMax = 2000;

	public static string _strSession = "";

	private Task m_tskReplay = null;
	//private Task m_tskLoadAnalysis = null;

	private Eeg _eeg = new();
	private Ai _ai = new();

	public EegView()
	{
		var b = new RegisterInViewDirectoryBehavior(); // { Key = "DiagramView1" };
		Behaviors.Add(b);
		InitializeComponent();
		BindingContext = this;

		Eeg.SetEegView(this);
		_eeg.InitialiseChannels();

		picMode.SelectedIndex = (int)eEegMode.eIdle;
		picGoal.SelectedIndex = AppPreferences.EegGoal;
		picAmbience.SelectedIndex = AppPreferences.Ambience;
		picTimer.SelectedIndex = AppPreferences.Timer;
		picReplaySpeed.SelectedIndex = _eeg.m_nReplaySpeed == 1 ? (int)eReplaySpeed.eNormal : (int)eReplaySpeed.eFast;
		picChartBands.SelectedIndex = AppPreferences.ChartBands;
		picChartTime.SelectedIndex = AppPreferences.ChartTime;
		picTriggerBand.SelectedIndex = AppPreferences.TriggerBand;
		picTriggerChannel.SelectedIndex = AppPreferences.TriggerChannel;
		picTriggerRange.SelectedIndex = AppPreferences.TriggerRange;
		picTriggerHunter.SelectedIndex = AppPreferences.TriggerHunter;
		picAiAnalysis.SelectedIndex = AppPreferences.AiEegService;
		picAiModel.SelectedIndex = AppPreferences.AiEegMlModel;
		chbTriggerSounding.IsChecked = AppPreferences.TriggerSounding;

		//_ai.AddSystemMessage(
		_ai._systemPrompts[0] =
			"I will upload Muse EEG brain wave data in dB while I perform a focused meditation. " +
			"The Prediction represents the degree of meditative state as a percentage. " +
			"Don't report the Prediction just use it as a primary factor in your analysis. " +
			"The Minute represents the time from the start. " +
			"Don't report the Minute just use it as a factor in your analysis " +
			"eg. You can't conclude much about state at the start. " +
			"Focus on readings at Gamma_AF7 and Gamma_AF8 and contrast their values. " +
			"Don't disgregard the other frequencies or sensors. " +
			"Determine general meditative state from any patterns you discern. " +
			"Keep commentary very short and focus on significant current brainwave trends, don't rehash past trends. " +
			"Don't use extraneous comments or excessive punctuation or extra spaces or new lines. " +
			"Don't repeat the raw data or ever specify any numeric vales. Don't use document point form. " +
			"Dont request further data I will send it automatically.";

		picDevice.SelectedIndex = AppPreferences.EegDevice;
	}

	protected void Page_Loaded(object sender, EventArgs e)
	{
		if (picMode.SelectedIndex == (int)eEegMode.eIdle)
		{
			picMode.SelectedIndex = (int)eEegMode.eSummary;
			picSession.Focus();
		}
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

		double w = width - 10;

		w = width - 40;
		lblTime.WidthRequest = w;

		w /= 2;
		lblDevice.WidthRequest = w;
		lblMode.WidthRequest = w;
		lblSession.WidthRequest = w;
		lblGoal.WidthRequest = w;
		lblAmbience.WidthRequest = w;
		lblTimer.WidthRequest = w;
		lblReplaySpeed.WidthRequest = w;
		lblChartBands.WidthRequest = w;
		lblChartTime.WidthRequest = w;
		lblTriggerBand.WidthRequest = w;
		lblTriggerChannel.WidthRequest = w;
		lblAiAnalysis.WidthRequest = w;
		lblAiModel.WidthRequest = w;
		lblTriggerRange.WidthRequest = w;
		lblTriggerHunter.WidthRequest = w;
		lblTriggerSounding.WidthRequest = w;
		lblRawData.WidthRequest = w;

		picDevice.WidthRequest = w;
		picMode.WidthRequest = w;
		picSession.WidthRequest = w;
		picGoal.WidthRequest = w;
		picAmbience.WidthRequest = w;
		picTimer.WidthRequest = w;
		picReplaySpeed.WidthRequest = w;
		picChartBands.WidthRequest = w;
		picChartTime.WidthRequest = w;
		picTriggerBand.WidthRequest = w;
		picTriggerChannel.WidthRequest = w;
		picAiAnalysis.WidthRequest = w;
		picAiModel.WidthRequest = w;
		picTriggerRange.WidthRequest = w;
		picTriggerHunter.WidthRequest = w;
		chbTriggerSounding.WidthRequest = w;
		chbRawData.WidthRequest = w;

		w /= 2;
		base.OnSizeAllocated(width, height);
	}

	private void picDevice_SelectedIndexChanged(object sender, EventArgs e)
	{
		_eeg.m_bCancelReplay = true;
		_eeg.Disconnect();
		AppPreferences.EegDevice = picDevice.SelectedIndex;

		EegCreate();

		AppSettings.SetDocumentHome();
		LoadSessions();

		CartesianChart cc;
		if ((cc = UI.Get<EegPage>()?.CartesianChart()) == null)
			return;

		SKColor c = AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? SKColors.Olive : SKColors.Red;
		IEnumerable<ISeries> ies = cc.Series;
		for (int i = 0; i < 5; ++i)
			((LineSeries<float>)ies.ElementAt(i)).Stroke = new SolidColorPaint(c) { StrokeThickness = EegSeries.m_fThinStoke };
	}

	private async void picMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		_eeg.m_bCancelReplay = true;
		_eeg.Disconnect();
		if ((m_nEegMode == (int)eEegMode.eLive) || (m_nEegMode == (int)eEegMode.eReplay)) // was
			await Task.Delay(1000);

		if (m_nEegMode == (int)eEegMode.eLive) // was live
		{
			AudioPlayer.Ambience(Dispatcher, false);
			string s = _strSession;
			LoadSessions();
			if (s == _strSession)
				SaveAnalysis();
		}

		m_nEegMode = (int)picMode.SelectedIndex;
		if (m_nEegMode == (int)eEegMode.eLive)
		{
			picReplaySpeed.SelectedIndex = (int)eReplaySpeed.eNormal;
			picReplaySpeed.IsEnabled = false;

			UI.Call<EegPage>(p => p.SessionLog().Text = "");
			//UI.Call<DiagramView>(v => v.StartNewChat());
			AudioPlayer.Ambience(Dispatcher, true);
			_eeg.Connect();
		}
		else
		if (m_nEegMode == (int)eEegMode.eReplay)
		{
			string s = (string)picSession.SelectedItem;
			if (!string.IsNullOrEmpty(s))
			{
				if (picAiAnalysis.SelectedIndex != (int)eAiService.eNone)
					UI.Call<EegPage>(p => p.SessionLog().Text = "");
				_eeg.m_bCancelReplay = false;
				void action() => _eeg.Replay(Path.Combine(AppSettings.EegDataHome(), s + (AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? "-Emotiv.csv" : "-Muse.csv")));
				m_tskReplay = new Task(action);
				m_tskReplay.Start();
			}
		}
		else
		if (m_nEegMode == (int)eEegMode.eSummary)
		{
			string s = (string)picSession.SelectedItem;
			if (!string.IsNullOrEmpty(s))
			{
				//UI.Call<DiagramView>(v => v.SelectChat(s));
				_strSession = s;
				LoadAnalysis();
				_eeg.m_bCancelReplay = false;
				void action1() => _eeg.Summary(Path.Combine(AppSettings.EegDataHome(), s + (AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? "-Emotiv.csv" : "-Muse.csv")));
				m_tskReplay = new Task(action1);
				m_tskReplay.Start();
			}
		}
	}

	private void picSession_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (picSession.SelectedIndex == -1)
			return;
		picMode_SelectedIndexChanged(null, null);
	}

	private void picGoal_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.EegGoal = picGoal.SelectedIndex;
	}

	private void picAmbience_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.Ambience = picAmbience.SelectedIndex;
	}

	private void picTimer_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.Timer = picTimer.SelectedIndex;
	}

	private void picReplaySpeed_SelectedIndexChanged(object sender, EventArgs e)
	{
		_eeg.m_nReplaySpeed = picReplaySpeed.SelectedIndex == 0 ? 1 : 10;
	}

	private void picChartBands_SelectedIndexChanged(object sender, EventArgs e)
	{
		CartesianChart cc;
		if ((cc = UI.Get<EegPage>()?.CartesianChart()) == null)
			return;

		AppPreferences.ChartBands = picChartBands.SelectedIndex;
		if (m_nEegMode == (int)eEegMode.eSummary)
		{
			picMode_SelectedIndexChanged(null, null);
			return;
		}

		IEnumerable<ISeries> ies = cc.Series;
		foreach (var s in ies)
		{
			ObservableCollection<float> v = (ObservableCollection<float>)s.Values;
			v.Clear();
		}
	}

	private void picChartTime_SelectedIndexChanged(object sender, EventArgs e)
	{
		m_nSeriesMax = (picChartTime.SelectedIndex + 1) * 1000;
	}

	private void picTriggerChannel_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (picTriggerChannel.SelectedIndex == -1)
			return;

		CartesianChart cc;
		if ((cc = UI.Get<EegPage>()?.CartesianChart()) == null)
			return;

		IEnumerable<ISeries> ies = cc.Series;
		((LineSeries<float>)ies.ElementAt(AppPreferences.TriggerIndex)).Stroke.StrokeThickness = EegSeries.m_fThinStoke;
		_eeg.m_eegChannel[AppPreferences.TriggerIndex].m_isTrigger = false;
		AppPreferences.TriggerIndex = (picTriggerBand.SelectedIndex * 5) + picTriggerChannel.SelectedIndex;
		_eeg.m_eegChannel[AppPreferences.TriggerIndex].m_isTrigger = true;
		((LineSeries<float>)ies.ElementAt(AppPreferences.TriggerIndex)).Stroke.StrokeThickness = EegSeries.m_fThickStoke;

		picTriggerRange_SelectedIndexChanged(null, null);

		if (m_nEegMode == (int)eEegMode.eSummary)
		{
			ObservableCollection<float> v = (ObservableCollection<float>)((LineSeries<float>)ies.ElementAt(AppPreferences.TriggerIndex)).Values;
			if (v.Count > 0)
			{
				float f = v.ElementAt(0);
				v.RemoveAt(0);
				v.Insert(0, f);
			}
		}
	}

	private void picTriggerRange_SelectedIndexChanged(object sender, EventArgs e)
	{
		String[] str = ((String)picTriggerRange.SelectedItem).Split(" - ");
		_eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fLow = _eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fInitialLow = float.Parse(str[0]); // picTriggerRange.SelectedIndex;
		_eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fHigh = _eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fInitialHigh = float.Parse(str[1]); // picTriggerRange.SelectedIndex + 1;
		_eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fDifference = _eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fInitialHigh - _eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fInitialLow;
	}

	private void picTriggerHunter_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.TriggerHunter = picTriggerHunter.SelectedIndex;
	}

	private void picAiAnalysis_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.AiEegService = picAiAnalysis.SelectedIndex;
		if (AppPreferences.AiEegService == (int)eAiService.eNone)
			UI.Call<EegPage>(p => p.ShowSessionLog(false));
		else
			UI.Call<EegPage>(p => p.ShowSessionLog(true));
	}

	private void picAiModel_SelectedIndexChanged(object sender, EventArgs e)
	{
		AppPreferences.AiEegMlModel = picAiModel.SelectedIndex;
	}

	private void chbTriggerSounding_CheckedChanged(object sender, EventArgs e)
	{
		if (chbTriggerSounding is not null)
			AppPreferences.TriggerSounding = chbTriggerSounding.IsChecked;
	}

	private void chbRawData_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (chbRawData is not null)
			AppPreferences.RawData = chbRawData.IsChecked;

		if (m_nEegMode == (int)eEegMode.eSummary)
			picMode_SelectedIndexChanged(null, null);
	}

	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////

	public void LoadSessions()
	{
		IEnumerable<string> ief = Directory.EnumerateFiles(AppSettings.EegDataHome(), "*.csv", SearchOption.TopDirectoryOnly);
		List<string> lf = new();
		foreach (string f in ief)
		{
			string s = Path.GetFileNameWithoutExtension(f);
			if (s.EndsWith("-Muse"))
				s = s.Substring(0, s.Length - 5);
			else
				if (s.EndsWith("-Emotiv"))
				s = s.Substring(0, s.Length - 7);

			lf.Add(s);
		}
		lf.Sort(new DescendingOrder());
		picSession.SelectedIndex = -1;
		picSession.ItemsSource = lf;
		picSession.SelectedIndex = 0;

		//_strSession = picSession.SelectedItem.ToString();

		picSession.Focus();
	}

	public void SelectSession(string name)
	{
		picSession.SelectedItem = name;
	}

	public void PlayTimer()
	{
		AudioPlayer.PlayTimer(Dispatcher);
	}
	/*
	public async Task SoundTrigger(float fBand, float fTrigger)
	{
#if WINDOWS

		TimeSpan ts = DateTime.Now - AppSettings._lastEegDataTime;

		if (ts.TotalSeconds > 30)
		{
			fBand = 0.01f;
			fTrigger = 0.01f;
		}

		//PInvoke.Beep(261, 2000); // Middle C
		uint freq = (uint)((fBand + 11.0) * 23.7);
		PInvoke.Beep(freq, 200);
		freq = (uint)((fTrigger + 11.0) * 23.7);
		if (fTrigger != 0.0f)
			PInvoke.Beep(freq, 200);
#endif
	}
	*/
	public void AiData(string data)
	{
		void action() => AiData(data, true);
		Dispatcher.Dispatch(action);
	}

	public async void AiData(string data, bool dummy)
	{
		await _ai.ChatAsync(AppPreferences.AiEegService, data, false);
		int i = _ai._userPrompts[1].Count() - 1;
		//UpdateSessionLog(_ai._userPrompts[1][i] + "\n\n");
		UI.Call<EegPage>(p => p.SessionLog().Text += _ai._chatReponses[1][i] + "\n\n");
	}

	//var result = await FilePicker.PickAsync(new PickOptions
	//{
	//	FileTypes = fpftCVS,
	//	PickerTitle = "Pick a .cvs file",
	//	 
	//});
	//if ((result != null) && File.Exists(result.FullPath))

	protected FilePickerFileType fpftCVS = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
	{
		{ DevicePlatform.iOS, null }, // new[] { "public.text", "public.csv" }
		{ DevicePlatform.macOS,new [] { ".txt", ".csv" } },
		{ DevicePlatform.Android, null }, // new[] { "text/plain", "text/csv" }
		{ DevicePlatform.WinUI,new [] { ".txt", ".csv" } },
	});

	public void SaveAnalysis()
	{
		string text = "";
		UI.Call<EegPage>(p => text = p.SessionLog().Text);
		if (text.Length > 0)
		{
			string str = Path.Combine(AppSettings.DocumentHome(), "Analysis");
			if (!Directory.Exists(str))
				Directory.CreateDirectory(str);
			str = Path.Combine(str, Path.GetFileNameWithoutExtension(_strSession) + "-Muse.txt");
			FileStream fs = new FileStream(str, FileMode.Append, FileAccess.Write);
			byte[] val = System.Text.Encoding.UTF8.GetBytes(text);
			fs.Write(val, 0, val.Length);
			fs.Close();
		}
	}

	public void LoadAnalysis()
	{
		string text = "";
		string str = Path.Combine(AppSettings.DocumentHome(), "Analysis");
		if (!Directory.Exists(str))
			Directory.CreateDirectory(str);
		str = Path.Combine(str, Path.GetFileNameWithoutExtension(_strSession) + "-Muse.txt");
		if (File.Exists(str))
			using (StreamReader sr = File.OpenText(str))
				while ((str = sr.ReadLine()) != null)
					text += str + "\n";
		UI.Call<EegPage>(p => p.SessionLog().Text = text);
	}

	public void SetAppTitle(string title)
	{
		void action()
		{
			Window[] w = Application.Current.Windows.ToArray();
			w[0].Title = title;
		}
		Dispatcher.Dispatch(action);
	}

	public void EnableEegControls(bool bEnable, bool bLive)
	{
		void action1() => picReplaySpeed.IsEnabled = bEnable;
		void action2() => picTriggerChannel.IsEnabled = bEnable;
		void action3() => picTriggerRange.IsEnabled = bEnable;
		void action4() => picTriggerHunter.IsEnabled = bEnable;

		if (bEnable)
		{
			Dispatcher.Dispatch(action1);
			Dispatcher.Dispatch(action2);
			Dispatcher.Dispatch(action3);
			Dispatcher.Dispatch(action4);
		}
		//else
		//if (bLive)
		//	Dispatcher.Dispatch(action1);
	}

	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////

	public void InitialiseChart()
	{
		CartesianChart cc;
		if ((cc = UI.Get<EegPage>()?.CartesianChart()) == null)
			return;

		IEnumerable<ISeries> ies = cc.Series;
		foreach (var s in ies)
		{
			ObservableCollection<float> v = (ObservableCollection<float>)s.Values;
			v.Clear();
			//if (++index < 25)
			//	v.Add(AppSettings.EegChannel(index).m_fCurrentValue);
			//else
			v.Add(0.0f);
		}
		ObservableCollection<string> l = UI.Get<EegPage>().TimeAxisLabels();
		l.Clear();
	}

	public void InitialseTriggers()
	{
		picTriggerChannel_SelectedIndexChanged(null, null);
	}

	public void UpdateTime(DateTime dtStart, DateTime dtEnd)
	{
		TimeSpan ts = dtEnd - dtStart;
		void action() => lblTime.Text = $"{dtStart.ToLongTimeString()} - {ts.Hours,1:#00}:{ts.Minutes,1:#00}:{ts.Seconds,1:#00}";
		Dispatcher.Dispatch(action);
	}

	public void UpdateTime(DateTime dtCurrent)
	{
		TimeSpan ts = DateTime.Now - _eeg.m_dtEegStart;
		ts = ts.Multiply(_eeg.m_nReplaySpeed);
		void action() => lblTime.Text = $"{dtCurrent.ToLongTimeString()} - {ts.Hours,1:#00}:{ts.Minutes,1:#00}:{ts.Seconds,1:#00} " + Eeg.m_strPrediction;
		Dispatcher.Dispatch(action);
	}

	public void UpdateData()
	{
		void action() => UpdateChart();
		Dispatcher.Dispatch(action);
	}

	private void UpdateChart()
	{
		int index = -1;
		IEnumerable<ISeries> ies = UI.Get<EegPage>()?.CartesianChart().Series;
		foreach (var s in ies)
		{
			bool bDisplay = false;
			++index;
			ObservableCollection<float> v = (ObservableCollection<float>)s.Values;

			if (index < 25)
			{
				//  0  1  2  3  4 - Delta / BetaL
				//  5  6  7  8  9 - Theta
				// 10 11 12 13 14 - Alpha
				// 15 16 17 18 19 - Beta / BetaH
				// 20 21 22 23 24 - Gamma
				if ((AppPreferences.ChartBands == (int)eChartBands.eFront &&
					((index == 1) || (index == 3) ||
					(index == 6) || (index == 8) ||
					(index == 11) || (index == 13) ||
					(index == 16) || (index == 18) ||
					(index == 21) || (index == 23)))
					||

					(AppPreferences.ChartBands == (int)eChartBands.eBack &&
					((index == 0) || (index == 2) || (index == 4) ||
					(index == 5) || (index == 7) || (index == 9) ||
					(index == 10) || (index == 12) || (index == 14) ||
					(index == 15) || (index == 17) || (index == 19) ||
					(index == 20) || (index == 22) || (index == 24)))
					||

					(AppPreferences.ChartBands == (int)eChartBands.eMixed &&
					((index == 0) || (index == 2) || (index == 4) ||
					(index == 5) || (index == 7) || (index == 9) ||
					(index == 10) || (index == 12) || (index == 14) ||
					(index == 16) || (index == 18) ||
					(index == 21) || (index == 23)))
					||

					(AppPreferences.ChartBands == (int)eChartBands.eAll))
					bDisplay = true;

				if (bDisplay)
					v.Add(_eeg.m_eegChannel[index].m_fCurrentValue);
			}
			else
			if (index == 25)
				v.Add(0);
			else
			if (index == 26)
			{
				if (m_nEegMode != (int)eEegMode.eSummary)
					v.Add(_eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fHigh);
			}
			else
			if (index == 27)
			{
				if (m_nEegMode != (int)eEegMode.eSummary)
					v.Add(_eeg.m_eegChannel[AppPreferences.TriggerIndex].m_fLow);
			}

			if (m_nEegMode != (int)eEegMode.eSummary)
				while (v.Count > m_nSeriesMax)
					v.RemoveAt(0);
		}
		ObservableCollection<string> l = UI.Get<EegPage>().TimeAxisLabels();
		TimeSpan ts = DateTime.Now - _eeg.m_dtEegStart;
		ts = ts.Multiply(_eeg.m_nReplaySpeed);
		l.Add($"{ts.Minutes:00}:{ts.Seconds:00}");
		if (l.Count > m_nSeriesMax)
			l.RemoveAt(0);
	}

	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////

	public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle),
		typeof(string), typeof(EegView), string.Empty);

	public static readonly BindableProperty CardColorProperty = BindableProperty.Create(nameof(CardColor),
		typeof(Color), typeof(EegView), App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White);

	public string CardTitle
	{
		get => (string)GetValue(CardTitleProperty);
		set => SetValue(CardTitleProperty, value);
	}

	public Color CardColor
	{
		get => (Color)GetValue(CardColorProperty);
		set => SetValue(CardColorProperty, value);
	}
	public void EegCreate()
	{
		_eeg = AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? new EmotivEeg() : new MuseEeg();
	}

	public bool EegIsConnected()
	{
		return _eeg.m_bConnected;
	}

	public int EegReplaySpeed()
	{
		return _eeg.m_nReplaySpeed;
	}

	public void EegSetTriggers(bool bHigh, bool bLow)
	{
		_eeg.SetTriggers(bHigh, bLow);
	}

	public void EegCalculateTriggers()
	{
		_eeg.CalculateTriggers();
	}

	public void EegIncreaseTriggers(float amount)
	{
		_eeg.IncreaseTriggers(amount);
	}

	public void EegDecreaseTriggers(float amount)
	{
		_eeg.DecreaseTriggers(amount);
	}

	public bool EegIsTriggerOn()
	{
		return _eeg.IsTriggerOn();
	}

	public bool EegIsTriggerOff()
	{
		return _eeg.IsTriggerOff();
	}

	public EegChannel EegChannel(int index)
	{
		return _eeg.m_eegChannel[index];
	}
}
