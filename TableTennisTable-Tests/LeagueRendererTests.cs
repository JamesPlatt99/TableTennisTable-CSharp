using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TableTennisTable_CSharp;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class LeagueRendererTests
    {
        LeagueRenderer leagueRenderer;
        League league;

        [TestInitialize]
        public void Initilize()
        {
            leagueRenderer = new LeagueRenderer();
            league = new League();
        }

        [TestMethod]
        public void TestNameInOutput()
        {
            league.AddPlayer("Kyle");
            string output = leagueRenderer.Render(league);
            Assert.IsTrue(output.Contains("Kyle"));
        }

        [TestMethod]
        public void TestOrderOfOutput()
        {
            league.AddPlayer("Kyle");
            league.AddPlayer("Barry");
            league.AddPlayer("Stevie");
            string output = leagueRenderer.Render(league);
            Assert.IsTrue(output.IndexOf("Kyle") < output.IndexOf("Barry") && output.IndexOf("Barry") < output.IndexOf("Stevie"));
        }

        [TestMethod]
        public void TestRowIndex()
        {
            league.AddPlayer("Kyle");
            league.AddPlayer("Barry");
            league.AddPlayer("Stevie");
            string output = leagueRenderer.Render(league);
            string[] splitOutput = output.Split('\r');
            Assert.IsTrue(splitOutput[1].Contains("Kyle"));
            Assert.IsTrue(splitOutput[4].Contains("Barry") && splitOutput[4].Contains("Stevie"));
        }
    }
}
