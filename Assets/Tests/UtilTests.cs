using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Assets.Logic.Util;

namespace Assets.Tests
{
    class UtilTests
    {
        [Test]
        public void Encrypt()
        {
            Debug.Log(Util.Encrypt("1234"));
        }
    }
}
