namespace Tests
{
	using System.IO;
	using NUnit.Framework;

	public class UserSettings
	{
		[Test]
		public void UserSettingsSimplePasses()
		{
			UserConfiguration.SaveSettings();
			string path = "Assets/Resources/UserConfigurationFiles/UserConfiguration.json";
			Assert.IsTrue(File.Exists(path));
		}
	}
}