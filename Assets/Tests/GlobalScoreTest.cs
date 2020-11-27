namespace Assets.Tests
{
	using System.Collections.Generic;
	using NUnit.Framework;

	class GlobalScoreTest
	{
		[Test]
		public void GetGlobalScore()
		{
			Dictionary<string, Dictionary<string, string>> globalScore = Player.GetGlobalScore();
			Assert.NotNull(globalScore);
		}
	}
}