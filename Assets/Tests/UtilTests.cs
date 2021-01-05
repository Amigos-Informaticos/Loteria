using NUnit.Framework;
using UnityEngine;
using System.Security.Cryptography;

namespace Tests
{
	public class UtilTests
	{
		[Test]
		public void GetHashString()
		{
			Debug.Log(Util.GetHashString("Alexis123"));
		}
	}
}