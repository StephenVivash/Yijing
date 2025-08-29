using Plugin.Maui.Audio;

namespace Yijing.Services;

public class AudioPlayer
{
	//private static IAudioPlayer m_audLineStart;
	//private static IAudioPlayer m_audLineEnd;
	private static IAudioPlayer m_audHexagramEnd;

	private static IAudioPlayer m_audTimer;

	private static IAudioPlayer m_audLightRain;
	private static IAudioPlayer m_audMeadowBirds;
	private static IAudioPlayer m_audQuietForest;
	//private static IAudioPlayer m_audKashmir;
	//private static IAudioPlayer m_audMachineGun;

	private static IAudioPlayer m_audAmbience;

	public static async void Load()
	{
		//m_audLineStart = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Windows Foreground.wav"));
		//m_audLineEnd = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Windows Background.wav"));
		m_audHexagramEnd = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Ring06.wav"));

		m_audTimer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Ring02.wav"));

		m_audLightRain = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("mixkit-light-rain-looping-1249.wav"));
		m_audMeadowBirds = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("mixkit-meadow-birds-isolated-28.wav"));
		m_audQuietForest = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("mixkit-quiet-forest-ambience-1220.wav"));

		//m_audKashmir = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("06 Kashmir.m4a"));
		//m_audMachineGun = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("02 Machine Gun.m4a"));

		m_audLightRain.Loop = true;
		m_audMeadowBirds.Loop = true;
		m_audQuietForest.Loop = true;
		//m_audKashmir.Loop = true;
		//m_audMachineGun.Loop = true;

		m_audAmbience = m_audLightRain;
	}

	public static void PlayTimer(IDispatcher dispatcher)
	{
		void action() => m_audTimer.Play();
		dispatcher.Dispatch(action);
	}

	public static void PlayHexagramEnd(IDispatcher dispatcher)
	{
		void action() => m_audHexagramEnd.Play();
		dispatcher.Dispatch(action);
	}

	public static void Ambience(IDispatcher dispatcher, bool play)
	{
		if (!play)
		{
			void action1() => m_audAmbience.Stop();
			dispatcher.Dispatch(action1);
			return;
		}
		switch (AppPreferences.Ambience)
		{
			case (int)eAmbience.eLightRain:
				m_audAmbience = m_audLightRain;
				break;
			case (int)eAmbience.eMeadowBirds:
				m_audAmbience = m_audMeadowBirds;
				break;
			case (int)eAmbience.eQuietForest:
				m_audAmbience = m_audQuietForest;
				break;
			//case (int)eAmbience.eKashmir:
			//	m_audAmbience = m_audKashmir;
			//	break;
			//case (int)eAmbience.eMachineGun:
			//	m_audAmbience = m_audMachineGun;
			//	break;
			default:
				return;
		}
		void action2() => m_audAmbience.Play();
		dispatcher.Dispatch(action2);
	}
}
