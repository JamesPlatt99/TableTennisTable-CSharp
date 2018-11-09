using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TableTennisTable_CSharp;
using Moq;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private App _app;
        private Mock<IFileService> _fileServiceMock;
        private ILeague _league;

        [TestInitialize]
        public void Initilize()
        {
            string[] existingGameFiles = { "existingGame" };
            _fileServiceMock = new Mock<IFileService>();
            _fileServiceMock.Setup(f => f.Load("existingGame")).Returns(new League());
            _fileServiceMock.Setup(f => f.Load(It.IsNotIn<string>(existingGameFiles))).Throws(new ArgumentException());
            _league = new League();
            ILeagueRenderer leagueRenderer = new LeagueRenderer();
            IFileService fileService = _fileServiceMock.Object;
            
            _app = new App(_league, leagueRenderer, fileService);
        }

        [TestMethod]
        public void TestPrintsEmptyGame()
        {
            Assert.AreEqual("No players yet", _app.SendCommand("print"));
        }

        [TestMethod]
        public void TestAddedPlayer()
        {
            string player = "Bob";
            Assert.AreEqual($"Added player {player}", _app.SendCommand($"add player {player}"));
        }
        [TestMethod]
        public void TestPlayerAlreadyAdded()
        {
            string player = "Bob";
            _app.SendCommand($"add player {player}");
            Assert.AreEqual($"Cannot add player {player} because they are already in the game", _app.SendCommand($"add player {player}"));
        }

        [TestMethod]
        public void TestInvalidName()
        {
            string player = "$$$$";
            _app.SendCommand($"add player {player}");
            Assert.AreEqual($"Player name {player} contains invalid", _app.SendCommand($"add player {player}"));
        }

        [TestMethod]
        public void TestPlayerNotInGame()
        {
            string player = "Steve";
            _app.SendCommand("add player Bob");
            Assert.AreEqual($"Player {player} is not in the game", _app.SendCommand($"record win {player} Bob"));
            Assert.AreEqual($"Player {player} is not in the game", _app.SendCommand($"record win Bob {player}"));
        }

        [TestMethod]
        public void TestWinner()
        {
            string player = "Steve";
            _app.SendCommand($"add player {player}");
            Assert.AreEqual(player, _app.SendCommand("winner"));
        }

        [TestMethod]
        public void TestSave()
        {
            string saveName = "newGame";
            Assert.AreEqual($"Saved {saveName}", _app.SendCommand($"save {saveName}"));
            _fileServiceMock.Verify(n => n.Save(saveName, _league));
        }

        [TestMethod]
        public void TestLoad()
        {
            string saveName = "existingGame";
            Assert.AreEqual($"Loaded {saveName}", _app.SendCommand($"load {saveName}"));
            _fileServiceMock.Verify(n => n.Load(saveName));
        }

        [TestMethod]
        public void TestForfeit()
        {
            string forfeiter = "Carlos";
            string challenger = "Richard";
            _app.SendCommand($"add player {forfeiter}");
            _app.SendCommand($"add player {challenger}");
            Assert.AreEqual($"Player {forfeiter} forfeited to {challenger}", _app.SendCommand($"forfeit {challenger} {forfeiter}"));
        }
    }
}
