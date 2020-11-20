namespace Tests
{
	using NUnit.Framework;

	public class SettingsTest
	{
		[Test]
		public void SettingTest()
		{
			TCPSocket socket;
			TCPSocketConfiguration.BuildDefaultConfiguration(out socket);
			Assert.AreEqual(
				socket.Server,
				"201.105.97.102"
			);
		}
	}
}