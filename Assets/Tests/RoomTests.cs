using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    class RoomTests
    {
        [Test]
        public void MakeRoom()
        {
            Room roomTest = new Room();
            roomTest.Host.Email = "efrain_escuela@outlook.com";
            roomTest.GameMode = "Classic";
            roomTest.NumberPlayers = 4;
            roomTest.Rounds = 1;
            roomTest.Speed = 2;
            Assert.AreNotEqual("ERROR", roomTest.MakeRoom());
        }
        [Test]
        public void ExitRoom()
        {
            Room roomTest = new Room();
            roomTest.Host.Email = "alexis@hotmail.com";
            roomTest.IdRoom = "c1c0f";
            string response = roomTest.ExitRoom("alexisao@hotmail.com");
            Assert.AreEqual("OK", response);
        }
        [Test]
        public void GetGameModesFromPlayer()
        {
            Player player = new Player
            {
                Email = "alexisao@hotmail.com",
                Password = "charlotte123",
                NickName = "VeggieCode",
                Names = "Alexis",
                LastName = "Alvarez Ortega"
            };
            Room room = new Room
            {
                Host = player
            };

            List<string> listGameModes = room.GetGameModes();
            Debug.LogWarning(listGameModes[0]);
        }

        [Test]
        public void SendMessage()
        {
            Player player = new Player
            {
                Email = "efrain_escuela@outlook.com",
                NickName = "EfrainCami"
            };
            Room room = new Room
            {
                IdRoom = "c1c0f"
            };
            Assert.AreEqual("OK", room.SendMessage("Hola, soy Efrain fucker", player));
        }

        [Test]
        public void GetMessages()
        {
            Player player = new Player
            {
                Email = "alexis@hotmail.com",
            };
            Room room = new Room
            {
                IdRoom = "c1c0f"
            };
            string response = room.GetMessages("edsonmanuelcarballovera@gmail.com");
            Debug.Log(room.Messages[0].Key + ": " + room.Messages[0].Value);
            Debug.Log(room.Messages[1].Key + ": " + room.Messages[1].Value);
            
            Assert.AreEqual("OK", response);
        }
    }
}
