using System.Collections.Generic;
using System.IO;
using GitHub.Unity.Json;

public static class UserConfiguration
{
	public static Dictionary<string, string> Settings { get; set; } =
		new Dictionary<string, string>();

	private static readonly string SETTINGS_PATH =
		MainConfiguration.GetSetting("UserConfigurationFile");

	public static void LoadSettings()
	{
		string content = new StreamReader(SETTINGS_PATH).ReadToEnd();
		Settings = SimpleJson.DeserializeObject<Dictionary<string, string>>(content);
	}

	public static void SaveSettings()
	{
		string content = SimpleJson.SerializeObject(Settings);
		StreamWriter writer = new StreamWriter(SETTINGS_PATH, false);
		writer.Write(content);
		writer.Close();
	}

	private static bool SettingExists(string settingName)
	{
		if (Settings.Count == 0)
		{
			LoadSettings();
		}
		return Settings.ContainsKey(settingName);
	}

	public static string GetSetting(string settingName)
	{
		string setting = null;
		if (SettingExists(settingName))
		{
			setting = Settings[settingName];
		}
		return setting;
	}

	public static void SetSetting(string settingName, string settingValue)
	{
		if (SettingExists(settingName))
		{
			Settings[settingName] = settingValue;
		}
	}
}