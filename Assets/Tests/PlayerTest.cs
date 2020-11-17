using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assets.Tests
{
    class PlayerTest
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
