namespace Tests
{
	using NUnit.Framework;

	public class LocalizationTest
	{
		[Test]
		public void TestLocalizationClass()
		{
			Localization.LoadMessages();
			string spanishMessage = Localization.GetMessage("Login", "Login");
			Localization.Language = "English";
			string englishMessage = Localization.GetMessage("Login", "Login");
			Assert.AreEqual(spanishMessage, "Entrar");
			Assert.AreEqual(englishMessage, "Login");
		}
	}
}