using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Tests
{
    class GlobalScoreTest
    {        
        [Test]
        public void GetGlobalScore()
        {
            Dictionary<int, Dictionary<string, string>> globalScore = Player.GetGlobalScore();            
            Assert.NotNull(globalScore);
        }
    }
}
