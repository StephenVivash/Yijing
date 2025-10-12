using System.Diagnostics;

using Yijing.Services;
using YijingData;

namespace Yijing.Views;

#nullable enable

public partial class MeditationView : ContentView
{
	private DateTime? _dtElapsed;
	private DateTime? _dtTimer;
	private IDispatcherTimer? _timer;
	private TimeSpan? _targetDuration;
	private bool _isMeditating;

	public event EventHandler<MeditationSessionCompletedEventArgs>? MeditationCompleted;

	public MeditationView()
	{
		var behavior = new RegisterInViewDirectoryBehavior();
		Behaviors.Add(behavior);
		InitializeComponent();
	}

	private void ContentView_Loaded(object sender, EventArgs e)
	{
		picGoal.SelectedIndex = AppPreferences.EegGoal;
		picAmbience.SelectedIndex = AppPreferences.Ambience;
		picTimer.SelectedIndex = AppPreferences.Timer;
		//UpdateStatus("Meditation is idle");
		UpdateElapsed(TimeSpan.Zero);
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

		double w = width - 10;

		w = width - 40;

		w /= 2;

		lblGoal.WidthRequest = w;
		lblAmbience.WidthRequest = w;
		lblTimer.WidthRequest = w;
		lblElapsed1.WidthRequest = w;
		lblMeditation.WidthRequest = w;

		picGoal.WidthRequest = w;
		picAmbience.WidthRequest = w;
		picTimer.WidthRequest = w;
		lblElapsed2.WidthRequest = w;
		btnMeditation.WidthRequest = w - 20;
		//btnMeditation.HeightRequest = 20;

		w -= 5;

		w /= 2;

		base.OnSizeAllocated(width, height);
	}

	private void btnMeditation_Clicked(object sender, EventArgs e)
	{
		if (_isMeditating)
			StopMeditation(false);
		else
			StartMeditation();
	}

	private void StartMeditation()
	{
		_dtElapsed = DateTime.Now;
		_dtTimer = _dtElapsed;
		_isMeditating = true;
		btnMeditation.Text = "Stop";
		//UpdateStatus("Meditation in progress...");
		UpdateElapsed(TimeSpan.Zero);
		_targetDuration = SelectedTimerDuration();
		EnsureTimer();
		_timer?.Start();
		AudioPlayer.Ambience(Dispatcher, true);
	}

	private void EnsureTimer()
	{
		_timer ??= Dispatcher.CreateTimer();
		if (_timer is null)
			return;

		_timer.Interval = TimeSpan.FromSeconds(1);
		_timer.Tick -= Timer_Tick;
		_timer.Tick += Timer_Tick;
	}

	private void Timer_Tick(object? sender, EventArgs e)
	{
		//if (_dtElapsed is null)
		//	return;

		TimeSpan elapsed = DateTime.Now - _dtElapsed!.Value;
		UpdateElapsed(elapsed);

		elapsed = DateTime.Now - _dtTimer!.Value;
		if (_targetDuration.HasValue && elapsed >= _targetDuration.Value)
		{
			_dtTimer = DateTime.Now;
			AudioPlayer.PlayTimer(Dispatcher);
		//	StopMeditation(true);
		}
	}

	private void StopMeditation(bool fromTimer)
	{
		if (_dtElapsed is null)
			return;

		_timer?.Stop();
		TimeSpan elapsed = DateTime.Now - _dtElapsed.Value;
		int durationMinutes = Math.Max(1, (int)Math.Round(elapsed.TotalMinutes));

		SaveMeditation(_dtElapsed.Value, durationMinutes);

		_dtElapsed = null;
		_isMeditating = false;
		btnMeditation.Text = "Start";
		//UpdateStatus(fromTimer ? "Meditation completed" : "Meditation stopped");
		UpdateElapsed(TimeSpan.Zero);
		AudioPlayer.Ambience(Dispatcher, false);
	}

	private void SaveMeditation(DateTime start, int durationMinutes)
	{
		using var context = new YijingDbContext();
		var meditation = new Meditation
		{
			Start = start,
			Duration = durationMinutes
		};
		context.Meditations.Add(meditation);
		YijingDatabase.SaveChanges(context);
		MeditationCompleted?.Invoke(this, new MeditationSessionCompletedEventArgs(meditation));
	}

	private void UpdateStatus(string message)
	{
		//lblStatus.Text = message;
	}

	private void UpdateElapsed(TimeSpan elapsed)
	{
		lblElapsed2.Text = $"{elapsed:hh\\:mm\\:ss}";
	}

	private TimeSpan? SelectedTimerDuration()
	{
		return AppPreferences.Timer switch
		{
			(int)eTimer.eTen => TimeSpan.FromMinutes(10),
			(int)eTimer.eFifteen => TimeSpan.FromMinutes(15),
			(int)eTimer.eTwenty => TimeSpan.FromMinutes(20),
			(int)eTimer.eThirty => TimeSpan.FromMinutes(30),
			(int)eTimer.eSixty => TimeSpan.FromMinutes(60),
			_ => null
		};
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
		_targetDuration = SelectedTimerDuration();
	}

	public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(
			nameof(CardTitle), typeof(string), typeof(MeditationView), string.Empty);

	public static readonly BindableProperty CardColorProperty = BindableProperty.Create(
			nameof(CardColor), typeof(Color), typeof(MeditationView),
			App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White);

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
}

public sealed class MeditationSessionCompletedEventArgs : EventArgs
{
	public MeditationSessionCompletedEventArgs(Meditation meditation)
	{
		Meditation = meditation;
	}

	public Meditation Meditation { get; }
}
