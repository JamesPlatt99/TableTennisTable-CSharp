using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableTennisTable_CSharp;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class LeagueTests
    {
        League league;

        [TestInitialize]
        public void Initilize()
        {
            league = new League();
        }

        [TestCategory("AddPlayer"), TestCategory("GetRows"), TestMethod]
        public void TestAddPlayer()
        {
            // When
            league.AddPlayer("Bob");

            // Then
            var rows = league.GetRows();
            Assert.AreEqual(1, rows.Count);
            var firstRowPlayers = rows.First().GetPlayers();
            Assert.AreEqual(1, firstRowPlayers.Count);
            CollectionAssert.Contains(firstRowPlayers, "Bob");
        }

        [TestCategory("AddPlayer"), TestMethod]
        public void TestAddDuplicatePlayer()
        {
            league.AddPlayer("Bob");
            Assert.ThrowsException<System.ArgumentException>(() => league.AddPlayer("Bob"));
        }

        [TestCategory("AddPlayer"), TestMethod]
        public void TestAddInvalidPlayerName()
        {
            Assert.ThrowsException<System.ArgumentException>(() => league.AddPlayer("Bob "));
        }

        [TestCategory("AddPlayer"), TestCategory("GetRows"), TestMethod]
        public void TestNewRowCreated()
        {
            league.AddPlayer("Bob");
            int numRows = league.GetRows().Count();
            Assert.AreEqual(1, numRows);

            league.AddPlayer("Steve");
            numRows = league.GetRows().Count();
            Assert.AreEqual(2, numRows);

            league.AddPlayer("John");
            numRows = league.GetRows().Count();
            Assert.AreEqual(2, numRows);

            league.AddPlayer("Carlos");
            numRows = league.GetRows().Count();
            Assert.AreEqual(3, numRows);
        }

        [TestCategory("AddPlayer"), TestCategory("GetRows"), TestMethod]
        public void TestPlayerIsAddedToLastRow()
        {
            league.AddPlayer("Bob");
            league.AddPlayer("Marcus");
            List<LeagueRow> rows = league.GetRows();
            Assert.IsTrue(rows.Last().GetPlayers().Contains("Marcus"));
        }

        [TestCategory("GetWinner"), TestCategory("AddPlayer"), TestMethod]
        public void TestGetWinner()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");
            league.AddPlayer("Stanley");
            league.AddPlayer("Samuel");
            Assert.AreEqual("Boris", league.GetWinner());
        }

        [TestCategory("RecordWin"), TestCategory("GetRows"), TestCategory("AddPlayer"), TestCategory("GetWinner"), TestMethod]
        public void TestRecordWinSwapPlaces()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");
            league.RecordWin("Manuel", "Boris");
            string winner = league.GetWinner();
            Assert.AreEqual("Manuel", winner);
        }

        [TestCategory("RecordWin"), TestCategory("GetRows"), TestCategory("AddPlayer"), TestMethod]
        public void TestPlayersTooFarApartError()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");
            league.AddPlayer("Peter");
            league.AddPlayer("Charlie");
            Assert.ThrowsException<System.ArgumentException>(() => league.RecordWin("Charlie", "Boris"));
        }

        [TestCategory("AddPlayer"), TestCategory("Forfeit"), TestMethod]
        public void TestForfeitNotOneRowBelow()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");
            league.AddPlayer("Pete");
            league.AddPlayer("Carlos");
            Assert.ThrowsException<System.ArgumentException>(()=>league.Forfeit("Manuel", "Boris"));
            Assert.ThrowsException<System.ArgumentException>(()=>league.Forfeit("Boris", "Carlos"));
        }

        [TestCategory("RecordWin"), TestCategory("GetRows"), TestCategory("AddPlayer"), TestMethod]
        public void TestRecordWinWinnerDoesNotExist()
        {
            league.AddPlayer("Boris");
            Assert.ThrowsException<System.ArgumentException>(() => league.RecordWin("Charlie", "Boris"));
        }

        [TestCategory("RecordWin"), TestCategory("GetRows"), TestCategory("AddPlayer"), TestMethod]
        public void TestRecordWinLoserDoesNotExist()
        {
            league.AddPlayer("Charlie");
            Assert.ThrowsException<System.ArgumentException>(() => league.RecordWin("Charlie", "Boris"));
        }

        [TestCategory("AddPlayer"), TestCategory("Forfeit"), TestMethod]
        public void TestForfeitSwitchesPlaces()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");

            league.Forfeit("Boris", "Manuel");
            league.Forfeit("Boris", "Manuel");
            league.Forfeit("Boris", "Manuel");

            string winner = league.GetWinner();
            Assert.AreEqual("Manuel", winner);
        }

        [TestCategory("AddPlayer"), TestCategory("Forfeit"), TestMethod]
        public void TestForfeitDoesNotSwitchPlacesBefore3()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");

            league.Forfeit("Boris", "Manuel");
            league.Forfeit("Boris", "Manuel");

            string winner = league.GetWinner();
            Assert.AreEqual("Boris", winner);
        }

        [TestCategory("AddPlayer"), TestCategory("Forfeit"), TestMethod]
        public void TestForfeitStreakRetained()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");
            league.AddPlayer("Peter");
            league.AddPlayer("Rob");

            league.Forfeit("Boris", "Manuel");
            league.Forfeit("Boris", "Manuel");
            league.Forfeit("Boris", "Manuel");
            league.Forfeit("Boris", "Rob");

            var rows = league.GetRows();
            Assert.IsTrue(rows[2].GetPlayers().Contains("Boris"));
        }


        [TestCategory("AddPlayer"), TestCategory("Forfeit"), TestMethod]
        public void TestForfeitResets()
        {
            league.AddPlayer("Boris");
            league.AddPlayer("Manuel");
            league.AddPlayer("Steve");
            league.AddPlayer("Carlos");

            league.Forfeit("Steve", "Carlos");
            league.Forfeit("Steve", "Carlos");

            league.RecordWin("Steve", "Boris");

            league.Forfeit("Steve", "Manuel");

            string winner = league.GetWinner();
            Assert.AreEqual("Steve", winner);
        }

        [TestCategory("AddPlayer"), TestCategory("Forfeit"), TestMethod]
        public void TestForfeitChallengerDoesNotExist()
        {
            league.AddPlayer("Boris");
            Assert.ThrowsException<System.ArgumentException>(() => league.Forfeit("Boris", "Steve"));
        }

        [TestCategory("AddPlayer"), TestCategory("Forfeit"), TestMethod]
        public void TestForfeitChallengeeDoesNotExist()
        {
            league.AddPlayer("Steve");
            Assert.ThrowsException<System.ArgumentException>(() => league.Forfeit("Boris", "Steve"));
        }
    }
}
