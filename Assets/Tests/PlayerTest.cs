using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	class PlayerTest
	{
		[Test]
		public void LogIn()
		{
			string response = null;
			Player alexito = new Player
			                 {
				                 Email = "alexisao@hotmail.com",
				                 Password = "charlotte123"
			                 };
			response = alexito.LogIn();
			Debug.Log(response);
			Assert.IsTrue(response.Equals("OK"));
		}

		[Test]
		public void SignUp()
		{
			Player alexito = new Player
			                 {
				                 Email = "alexisao@hotmail.com",
				                 Password = "charlotte123",
				                 NickName = "VeggieCode",
				                 Names = "Alexis",
				                 LastName = "Alvarez Ortega",
				                 Code = "8cd6f"
			                 };
			string response = alexito.SignUp();
			Debug.Log(response);
			Assert.IsTrue(response.Equals("OK"));
		}

		[Test]
		public void SendCode()
		{
			Player alexito = new Player
			                 {
				                 Email = "alexisao@hotmail.com",
				                 Password = "charlotte123",
				                 NickName = "VeggieCode",
				                 Names = "Alexis",
				                 LastName = "Alvarez Ortega"
			                 };
			string response = alexito.SendCode();
			Debug.Log(response);
			Assert.IsTrue(response.Equals("OK"));
		}

		[Test]
		public void IsName()
		{
			Assert.IsTrue(Player.IsName("Efrain Razziel"));
		}

		[Test]
		public void IsEmail()
		{
			Assert.IsTrue(Player.IsEmail("grimlicrash@outlook.com"));
		}

		[Test]
		public void GetPlayerFromServer()
		{
			Player player = new Player
			{
				Email = "efrain_escuela@outlook.com"
			};
			Assert.IsTrue(player.GetPlayerFromServer());
			Debug.Log(player.NickName);
		}
	}
}