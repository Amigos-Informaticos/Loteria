namespace Tests
{
	using NUnit.Framework;

	public class NewTestScript
	{
		[Test]
		public void SendCodeToEmailTest()
		{
			TCPSocket socket;
			TCPSocketConfiguration.BuildDefaultConfiguration(out socket);
			Command command = new Command("send_code_to_email");
			command.AddArgument("email", "efrain_escuela@outlook.com");
			socket.AddCommand(command);
			socket.SendCommand();
			Assert.AreEqual(
				"OK",
				socket.GetResponse(true)
			);
		}
	}
}