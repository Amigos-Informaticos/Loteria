﻿namespace Tests
{
	using NUnit.Framework;

	public class NewTestScript
	{
		[Test]
		public void SendCodeToEmailTest()
		{
			TCPSocket socket = new TCPSocket("localhost", 42069);
			Command command = new Command("send_code_to_email");
			command.AddArgument("email", "edsonmanuelcarballovera@gmail.com");
			socket.AddCommand(command);
			socket.SendCommand();
			Assert.AreEqual(
				"OK",
				socket.GetResponse(true)
			);
		}
	}
}