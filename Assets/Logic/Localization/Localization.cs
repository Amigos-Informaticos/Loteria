using System.Collections.Generic;
using System.IO;
using GitHub.Unity.Json;
using UnityEngine;

public abstract class Localization
{
	public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> Messages =
		new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

	public static void LoadMessages()
	{
		if (Messages.Count == 0)
		{
			string messagesPath = MainConfiguration.GetSetting("LanguagesFile");
			try
			{
				using (StreamReader reader = new StreamReader(messagesPath))
				{
					string contents = reader.ReadToEnd();
					Messages = SimpleJson
						.DeserializeObject<
							Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(
							contents);
				}
			}
			catch (FileNotFoundException fileNotFoundException)
			{
				Debug.Log("LoadMessages"+fileNotFoundException);
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