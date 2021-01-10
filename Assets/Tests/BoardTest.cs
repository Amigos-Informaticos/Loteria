﻿using NUnit.Framework;
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

        [Test]
        public void GetPatternByGameMode()
        {
            Player player = new Player();
            player.Board.GameMode = "Pool";
            player.Board.Pattern = player.Board.GetPatternByGameMode();
            Debug.Log(player.Board.Pattern[0, 0] + "-" + player.Board.Pattern[0, 1] + "-" + player.Board.Pattern[0, 2] +
                      "-" + player.Board.Pattern[0, 3] + "-" + player.Board.Pattern[0, 4]);
            Debug.Log(player.Board.Pattern[1, 0] + "-" + player.Board.Pattern[1, 1] + "-" + player.Board.Pattern[1, 2] +
                      "-" + player.Board.Pattern[1, 3] + "-" + player.Board.Pattern[1, 4]);
        }

        [Test]
        public void GetPatternsByGameMode()
        {
            Player player = new Player();
            player.Board.GameMode = "Classic";
            List<bool[,]> patterns = player.Board.GetPatternsByGameMode();
            Debug.Log(patterns[0][0, 0]+"-"+patterns[0][0, 1]+"-"+patterns[0][0, 2]+"-"+patterns[0][0, 3]+"-"+patterns[0][0, 4]);
            Debug.Log(patterns[0][1, 0]+"-"+patterns[0][1, 1]+"-"+patterns[0][1, 2]+"-"+patterns[0][1, 3]+"-"+patterns[0][1, 4]);
            Debug.Log(patterns[0][2, 0]+"-"+patterns[0][2, 1]+"-"+patterns[0][2, 2]+"-"+patterns[0][2, 3]+"-"+patterns[0][2, 4]);
            Debug.Log(patterns[0][3, 0]+"-"+patterns[0][3, 1]+"-"+patterns[0][3, 2]+"-"+patterns[0][3, 3]+"-"+patterns[0][3, 4]);
            Debug.Log(patterns[0][4, 0]+"-"+patterns[0][4, 1]+"-"+patterns[0][4, 2]+"-"+patterns[0][4, 3]+"-"+patterns[0][4, 4]);
            Debug.Log("");
            Debug.Log(patterns[1][0, 0]+"-"+patterns[1][0, 1]+"-"+patterns[1][0, 2]+"-"+patterns[1][0, 3]+"-"+patterns[1][0, 4]);
            Debug.Log(patterns[1][1, 0]+"-"+patterns[1][1, 1]+"-"+patterns[1][1, 2]+"-"+patterns[1][1, 3]+"-"+patterns[1][1, 4]);
            Debug.Log(patterns[1][2, 0]+"-"+patterns[1][2, 1]+"-"+patterns[1][2, 2]+"-"+patterns[1][2, 3]+"-"+patterns[1][2, 4]);
            Debug.Log(patterns[1][3, 0]+"-"+patterns[1][3, 1]+"-"+patterns[1][3, 2]+"-"+patterns[1][3, 3]+"-"+patterns[1][3, 4]);
            Debug.Log(patterns[1][4, 0]+"-"+patterns[1][4, 1]+"-"+patterns[1][4, 2]+"-"+patterns[1][4, 3]+"-"+patterns[1][4, 4]);
        }
    }
}