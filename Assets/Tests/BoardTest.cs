using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Tests
{
    class BoardTest
    {
        [Test]
        public void GetSortedDeck()
        {            
            Room roomTest = new Room();
            roomTest.Host.Email = "alexisao@hotmail.com";
            roomTest.GameMode = "Cutom";
            roomTest.NumberPlayers = 4;
            roomTest.Rounds = 1;
            roomTest.Speed = 2;
            string idRoom = "c1c0f";           
            int[] sortedDeck = Board.GetSortedDeck(idRoom, "alexis@hotmail.com");
            
            Debug.Log(sortedDeck[0] + "-" + sortedDeck[1] + "-" + sortedDeck[2] + "-" + sortedDeck[3] + "-" +
                      sortedDeck[4] + "-" + sortedDeck[5]);
            
            Assert.IsTrue(sortedDeck.Length > 0);
        }
        [Test]
        public void SavePattern()
        {
            //Mover SavePattern a Player
            Player player = new Player();
            player.Board.GameMode = "Hardcore mode";
            bool[,] boolPattern = new bool[,]{{true, true, true, true, true},
                                              {false,false,false,false,false},
                                              {true,false,false,false,false},
                                              {true,false,false,false,false},
                                              {true,false,false,false,false}};
            player.Board.Pattern = boolPattern;
            
            Assert.AreEqual("OK", player.Board.SavePattern("alexisao@hotmail.com"));
        }
    }
}