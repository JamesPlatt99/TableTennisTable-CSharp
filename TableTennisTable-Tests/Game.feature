Feature: Game

	Scenario: Empty League
		Given the league has no players
		When I print the league
		Then I should see "No players yet"

	Scenario: Adding a player
		Given the league has no players
		When I add the player "Barry"
		Then I should see "Added player Barry"

	Scenario: PlayerWinsGame
		Given the league has the players "Barry, Steve"
		When I record "Steve" won against "Barry"
		Then Then the winner should be "Steve"

	Scenario: PlayerForfeitsGameOnce
		Given the league has the players "Barry, Steve"
		When Player "Barry" forfeits to "Steve" "1" times
		Then Then the winner should be "Barry"

	Scenario: PlayerForfeitsGameThreeTimes
		Given the league has the players "Barry, Steve"
		When Player "Barry" forfeits to "Steve" "3" times
		Then Then the winner should be "Steve"

	Scenario: SaveGame
		Given the league has the players "Carlos, Pete, Manuel"
		When The game is saved with the name "TestSaveGame123"
		Then Then the file "TestSaveGame123" should exist

	Scenario: LoadGame
		Given There is a save game named "TestSaveGame1234"
		When The game is loaded with the name "TestSaveGame1234"
		Then I should see "Loaded TestSaveGame1234"