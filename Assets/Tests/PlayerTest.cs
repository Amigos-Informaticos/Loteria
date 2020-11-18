using NUnit.Framework;

namespace Tests
{
	public class PlayerTest
	{
		[Test]
		public void IsName()
		{
			Assert.IsTrue(Player.IsName("Alexis"));
		}

		[Test]
		public void IsEmail()
		{
			Assert.IsTrue(Player.IsEmail("alexisao@hotmail.com"));
		}
	}
}