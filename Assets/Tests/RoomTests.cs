using NUnit.Framework;

namespace Tests
{
    class RoomTests
    {
        [Test]
        public void MakeRoom()
        {
            Room roomTest = new Room();
            roomTest.Host.Email = "alexisao@hotmail.com";
            roomTest.GameMode = "Cutom";
            roomTest.NumberPlayers = 4;
            roomTest.Rounds = 1;
            roomTest.Speed = 2;
            Assert.AreNotEqual("ERROR",roomTest.MakeRoom());
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
