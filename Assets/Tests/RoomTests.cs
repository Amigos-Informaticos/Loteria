using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class RoomTests
    {
        [Test]
        public void MakeRoom()
        {
            Room roomTest = new Room();
            roomTest.Host.Email = "alexisao@hotmail.com";
            Assert.NotNull(roomTest.MakeRoom());
        }
        [Test]
        public void ExitRoom()
        {
            Room roomTest = new Room();
            roomTest.Host.Email = "alexisao@hotmail.com";
            roomTest.MakeRoom();
            string response = roomTest.ExitRoom("alexisao@hotmail.com");
            Assert.AreEqual("OK",response);
        }
    }
}
