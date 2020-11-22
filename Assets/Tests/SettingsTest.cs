namespace Tests
{
	using NUnit.Framework;
    using System;

    public class SettingsTest
	{
		[Test]
		public void SettingServerAdressTest()
		{
			TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket socket);
			Assert.AreEqual(
				socket.Server,
				TCPSocketConfiguration.GetSetting("Address")
			);
		}
		[Test]
		public void SettingServerPortNumberTest()
        {
			TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket socket);
			string port = Convert.ToString(socket.Port);

			Assert.AreEqual(
				port,
				TCPSocketConfiguration.GetSetting("NumberServicePort")
			);
		}
	}
}