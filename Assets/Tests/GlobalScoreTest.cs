using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Tests
{
    class GlobalScoreTest
    {
        [Test]
        public void GetGlobalScore()
        {                                 
            Assert.NotNull(Player.GetGlobalScore());
        }
    }
}
