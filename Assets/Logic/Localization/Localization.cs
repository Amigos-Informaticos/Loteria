using System.Collections.Generic;
using System.IO;
using GitHub.Unity.Json;

public abstract class Localization
{
	public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> Messages =
		new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

	public static void LoadMessages()
	{
		if (Messages.Count == 0)
		{
			string messagesPath = MainConfiguration.GetSetting("LanguagesFile");
			using (StreamReader reader = new StreamReader(messagesPath))
			{
				string contents = reader.ReadToEnd();
				Messages = SimpleJson
					.DeserializeObject<
						Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(
						contents);
			}
		}
	}

	public static string GetMessage(string scene, string item)
	{
		if (Messages.Count == 0)
		{
			LoadMessages();
		}
		return Messages[scene][item][UserConfiguration.GetSetting("Language")];
	}
}