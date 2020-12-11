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
            Room room = new Room();
            room.Host.Email = "alexisao@hotmail.com";
            Board board = new Board();
            Dictionary<string, string> sortedDeck = board.GetSortedDeck("alexisao@hotmail.com", room.MakeRoom());
            Debug.Log(sortedDeck);
            Assert.IsNotNull(sortedDeck);
        }
        [Test]
        public void SavePattern()
        {
            Board board = new Board();
            bool[,] boolPattern = new bool[,]{{true, true, true, true, true},
                                              {false,false,false,false,false},
                                              {true,false,false,false,false},
                                              {true,false,false,false,false},
                                              {true,false,false,false,false}};
            board.Pattern = boolPattern;
            Assert.AreEqual("OK", new Board().SavePattern());
        }
    }
}
