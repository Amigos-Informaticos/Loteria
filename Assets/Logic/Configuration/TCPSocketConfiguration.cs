using System.Collections.Generic;
using System.IO;
using System.Net;
using GitHub.Unity.Json;

public static class TCPSocketConfiguration
{
	private static readonly string SETTINGS_PATH =
		MainConfiguration.GetSetting("SocketConfigurationFile");

	private static Dictionary<string, string> Settings { get; set; } =
		new Dictionary<string, string>();

	private static void LoadSettings()
	{
		string content = new StreamReader(SETTINGS_PATH).ReadToEnd();
		Settings = SimpleJson.DeserializeObject<Dictionary<string, string>>(content);
	}

	private static bool SettingExists(string settingName)
	{
		if (Settings.Count == 0)
		{
			LoadSettings();
		}
		return Settings.ContainsKey(settingName);
	}

	private static string GetSetting(string settingName)
	{
		string setting = null;
		if (SettingExists(settingName))
		{
			setting = Settings[settingName];
		}
		return setting;
	}

	public static void BuildDefaultConfiguration(out TCPSocket socket)
	{
		socket = null;
		using (WebClient client = new WebClient())
		{
			string address = client.DownloadString(GetSetting("AddressServiceAddress"));
			socket = new TCPSocket(address, 42069);
		}
	}
}