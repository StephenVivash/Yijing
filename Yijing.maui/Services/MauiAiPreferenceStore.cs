namespace Yijing.Services;

public sealed class MauiAiPreferenceStore : IAiPreferenceStore
{
	public string Get(string key, string defaultValue)
	{
		return Preferences.Get(key, defaultValue);
	}

	public float Get(string key, float defaultValue)
	{
		return Preferences.Get(key, defaultValue);
	}

	public int Get(string key, int defaultValue)
	{
		return Preferences.Get(key, defaultValue);
	}

	public void Set(string key, string value)
	{
		Preferences.Set(key, value);
	}

	public void Set(string key, float value)
	{
		Preferences.Set(key, value);
	}

	public void Set(string key, int value)
	{
		Preferences.Set(key, value);
	}
}
