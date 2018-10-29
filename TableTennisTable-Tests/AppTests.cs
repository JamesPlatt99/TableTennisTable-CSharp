using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using TableTennisTable_CSharp;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class AppTests
    {
        private ILeague league;
        private ILeagueRenderer _leagueRenderer;
        private IFileService _fileService;

        [TestInitialize]
        public void Initilize()
        {
            var leagueMock = new Mock<ILeague>();
            var leagueRendererMock = new Mock<ILeagueRenderer>();
            var fileServiceMock = new Mock<IFileService>();

            string[] namesTaken = { "test", "test$1" };
            fileServiceMock.Setup(f => f.Load(It.IsIn(namesTaken))).Returns(new League());
            fileServiceMock.Setup(f => f.Load(It.IsNotIn(namesTaken))).Throws(new ArgumentException());

            league = leagueMock.Object;
            _leagueRenderer = leagueRendererMock.Object;
            _fileService = fileServiceMock.Object;
        }


        [TestMethod]
        public void TestPrintsCurrentState()
        {
            var league = new Mock<ILeague>();
            var renderer = new Mock<ILeagueRenderer>();
            renderer.Setup(r => r.Render(league.Object)).Returns("Rendered League");

            var app = new App(league.Object, renderer.Object, _fileService);

            Assert.AreEqual("Rendered League", app.SendCommand("print"));
        }

        [TestMethod]
        public void TestAddPlayer()
        {
            var league = new Mock<ILeague>();

            var app = new App(league.Object, null, _fileService);

            Assert.AreEqual("Added player Mike", app.SendCommand("add player Mike"));
        }

        [TestMethod]
        public void TestWinner()
        {
            var league = new Mock<ILeague>();
            league.Setup(l => l.GetWinner()).Returns("Carlos");

            var app = new App(league.Object, null, _fileService);

            Assert.AreEqual("Carlos", app.SendCommand("winner"));
        }

        [TestMethod]
        public void TestLoadLeague()
        {
            var app = new App(null, null, _fileService);

            Assert.AreEqual("Loaded test", app.SendCommand("load test"));
        }

        [TestMethod]
        public void TestSaveLeague()
        {
            var app = new App(null, null, _fileService);

            Assert.AreEqual("Saved test", app.SendCommand("save test"));
        }

        [TestMethod]
        public void TestRecordWin()
        {
            var app = new App(league, null, _fileService);

            Assert.AreEqual("Recorded barry win against larry", app.SendCommand("record win barry larry"));
        }

        [TestMethod]
        public void TestForfeit()
        {
            var app = new App(this.league, null, _fileService);

            Assert.AreEqual("Player larry forfeited to barry", app.SendCommand("forfeit barry larry"));
        }

        [TestMethod]
        public void TestUnrecognisedCommand()
        {
            var app = new App(league, null, _fileService);

            Assert.AreEqual("Unknown command: Invade Crimea", app.SendCommand("Invade Crimea"));
        }

        [TestMethod]
        public void TestAutosaveNameNotTaken()
        {
            var app = new App(league, null, _fileService);
            string filename = app.GenerateAutoSaveName();

            Assert.IsTrue(filename.StartsWith("saved_games/autosave"));
        }

        [TestMethod]
        public void TestAutosaveNameTaken()
        {
            var app = new App(league, null, _fileService);
            string filename = app.IncrementFileNameIfTaken("test");

            Assert.AreEqual(filename, "test$2");
        }
    }
}
