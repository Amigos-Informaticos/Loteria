using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GitHub.Unity.Json;
using UnityEngine;

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

	public static string GetSetting(string settingName)
	{
		if (Settings.Count == 0)
		{
			LoadSettings();
		}
		string setting = null;
		if (SettingExists(settingName))
		{
			setting = Settings[settingName];
		}
		return setting;
	}

	public static void SetSetting(string settingName, string settingValue)
	{
		Settings[settingName] = settingValue;
	}

	public static void BuildDefaultConfiguration(out TCPSocket socket)
	{
		socket = new TCPSocket();
		try
        {
			
			WebClient client = new WebClient();
			string address = client.DownloadString(GetSetting("AddressServiceAddress"));
			int port = Convert.ToInt32(GetSetting("NumberServicePort"));
			socket = new TCPSocket(address, port);
			SetSetting("Address", address);
		}
        catch (WebException e)
        {
			Debug.Log(e);
        }		
	}
}