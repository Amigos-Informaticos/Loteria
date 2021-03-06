﻿using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	class PlayerTest
	{
		[Test]
		public void LogIn()
		{
			string response;
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

		[Test]
		public void WriteScore()
		{
			Player playerForTest = new Player
			{
				Email = "efrain_escuela@outlook.com"
			};
			int score = 1000;
			string response = playerForTest.SaveScore(score);
			Assert.IsTrue(response.Equals("OK"));
		}
		
		[Test]
		public void ChangePassword()
		{
			Player playerForTest = new Player
			{
				Email = "efrain_escuela@outlook.com",
				Password = "beethoven123"
			};
			string newPassword = "delorean2015";
			string response = playerForTest.ChangePassword(newPassword);
			Debug.Log(response);
			Assert.IsTrue(response.Equals("OK"));
		}
	}
}