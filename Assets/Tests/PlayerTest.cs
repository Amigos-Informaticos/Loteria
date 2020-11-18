using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests
{
    class PlayerTest
    {        
        [Test]
        public void LogIn()
        {
            Player edsonsito = new Player
            {
                Email = "efrain_escuela@outlook.com",
                Password = "beethoven123"
            };
            Assert.IsTrue(edsonsito.LogIn());
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
                LastName = "Alvarez Ortega"
            };
            string response = alexito.SignUp();
            Debug.Log(response);
            Assert.IsTrue(response.Equals("OK"));
        }
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
