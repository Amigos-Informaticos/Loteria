using System.Collections.Generic;
using System.IO;
using GitHub.Unity.Json;

public static class Localization
{
	private static Dictionary<string, Dictionary<string, Dictionary<string, string>>> _messages =
		new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

	public static void LoadMessages()
	{
		if (_messages.Count != 0) return;
		string messagesPath = MainConfiguration.GetSetting("LanguagesFile");
		using (StreamReader reader = new StreamReader(messagesPath))
		{
			string contents = reader.ReadToEnd();
			_messages = SimpleJson.DeserializeObject<
				Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(contents);
		}
	}

	public static string GetMessage(string scene, string item)
	{
		if (_messages.Count == 0)
		{
			LoadMessages();
		}
		return _messages[scene][item][UserConfiguration.GetSetting("Language")];
	}
}