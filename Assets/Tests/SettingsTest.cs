namespace Tests
{
	using NUnit.Framework;

	public class SettingsTest
	{
		[Test]
		public void SettingTest()
		{
			TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket socket);
			Assert.AreEqual(
				socket.Server,
				TCPSocketConfiguration.GetSetting("Address")
			);
		}
	}
}