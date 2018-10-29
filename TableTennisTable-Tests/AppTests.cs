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
        [TestMethod]
        public void TestPrintsCurrentState()
        {
            var league = new Mock<ILeague>();
            var renderer = new Mock<ILeagueRenderer>();
            renderer.Setup(r => r.Render(league.Object)).Returns("Rendered League");

            var app = new App(league.Object, renderer.Object, null);

            Assert.AreEqual("Rendered League", app.SendCommand("print"));
        }

        [TestMethod]
        public void TestAddPlayer()
        {
            var league = new Mock<ILeague>();

            var app = new App(league.Object, null, null);

            Assert.AreEqual("Added player Mike", app.SendCommand("add player Mike"));
        }

        [TestMethod]
        public void TestWinner()
        {
            var league = new Mock<ILeague>();
            league.Setup(l => l.GetWinner()).Returns("Carlos");

            var app = new App(league.Object, null, null);

            Assert.AreEqual("Carlos", app.SendCommand("winner"));
        }

        [TestMethod]
        public void TestLoadLeague()
        {
            var fileService = new Mock<IFileService>();
            fileService.Setup(f => f.Load("test")).Returns(new League());

            var app = new App(null, null, fileService.Object);

            Assert.AreEqual("Loaded test", app.SendCommand("load test"));
        }

        [TestMethod]
        public void TestSaveLeague()
        {
            var fileService = new Mock<IFileService>();
            League league = new League();

            var app = new App(null, null, fileService.Object);

            Assert.AreEqual("Saved test", app.SendCommand("save test"));
        }

        [TestMethod]
        public void TestRecordWin()
        {
            var league = new Mock<ILeague>();

            var app = new App(league.Object, null, null);

            Assert.AreEqual("Recorded barry win against larry", app.SendCommand("record win barry larry"));
        }

        [TestMethod]
        public void TestForfeit()
        {
            var league = new Mock<ILeague>();

            var app = new App(league.Object, null, null);

            Assert.AreEqual("Player larry forfeited to barry", app.SendCommand("forfeit barry larry"));
        }

        [TestMethod]
        public void TestUnrecognisedCommand()
        {
            var league = new Mock<ILeague>();

            var app = new App(league.Object, null, null);

            Assert.AreEqual("Unknown command: Invade Crimea", app.SendCommand("Invade Crimea"));
        }
    }
}
