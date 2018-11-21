using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TableTennisTable_CSharp;
using TechTalk.SpecFlow;

namespace TableTennisTable_Tests
{
    [Binding]
    public class GameSteps
    {
        // Current app instance
        private App app;

        // Last response
        private String response;

        // Any test save games generated
        private List<String> saveGames;

        [BeforeScenario]
        public void CreateApp()
        {
            var league = new League();
            var leagueRenderer = new LeagueRenderer();
            var fileService = new FileService();
            saveGames = new List<String>();
            app = new App(league, leagueRenderer, fileService);
        }

        [AfterScenario]
        public void DeleteSaveGames()
        {
            foreach(string saveGame in saveGames)
            {
                if(System.IO.File.Exists(saveGame))
                {
                    System.IO.File.Delete(saveGame);
                }
            }
        }

        [Given("the league has no players")]
        public void GivenTheLeagueHasNoPlayers()
        {
            // Nothing to do - the default league starts with no players
        }

        [Given("the league has the players \"(.*)\"")]
        public void GivenTheLeagueHasThePlayers(string playersToAdd)
        {
            string[] players = playersToAdd.Split(',');
            foreach(string player in players)
            {
                app.SendCommand($"add player {player.Trim()}");
            }
        }

        [Given("There is a save game named \"(.*)\"")]
        public void GivenThereIsASavedGame(string saveName)
        {
            app.SendCommand($"save {saveName}");
        }

        [When("I print the league")]
        public void WhenIPrintTheLeague()
        {
            response = app.SendCommand("print");
        }

        [When("I add the player \"(.*)\"")]
        public void WhenIAddPlayer(string player)
        {
            response = app.SendCommand($"add player {player}");
        }

        [When("I record \"(.*)\" won against \"(.*)\"")]
        public void WhenIRecordWin(string winner, string loser)
        {
            response = app.SendCommand($"record win {winner} {loser}");
        }

        [When("Player \"(.*)\" forfeits to \"(.*)\" \"(.*)\" times")]
        public void WhenAPlayerForfeits(string challengee, string challenger, int times)
        {
            for(int i = 0; i < times; i++)
            {
                response = app.SendCommand($"forfeit {challenger} {challengee}");
            }
        }

        [When("The game is saved with the name \"(.*)\"")]
        public void WhenTheGameIsSaved(string fileName)
        {
            saveGames.Add(fileName);
            response = app.SendCommand($"save {fileName}");
        }

        [When("The game is loaded with the name \"(.*)\"")]
        public void WhenTheGameIsLoaded(string fileName)
        {
            saveGames.Add(fileName);
            response = app.SendCommand($"load {fileName}");
        }

        [Then("I should see \"(.*)\"")]
        public void ThenIShouldSee(string expected)
        {
            Assert.AreEqual(expected, response);
        }

        [Then("Then the winner should be \"(.*)\"")]
        public void ThenTheWinnerShouldBe(string player)
        {
            string winner = app.SendCommand("winner");
            Assert.AreEqual(winner, player);
        }

        [Then("Then the file \"(.*)\" should exist")]
        public void ThenFileShouldExist(string filePath)
        {
            Assert.IsTrue(System.IO.File.Exists(filePath));
        }
    }
}
