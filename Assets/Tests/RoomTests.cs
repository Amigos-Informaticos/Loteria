﻿using NUnit.Framework;
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
            roomTest.Host.Email = "alexisao@hotmail.com";
            roomTest.IdRoom = "4792d";
            string response = roomTest.ExitRoom("efrain_escuela@outlook.com");
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
    }
}
