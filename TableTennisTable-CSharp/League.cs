using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TableTennisTable_CSharp
{
    public class League
    {
        private List<LeagueRow> _rows;
        private Dictionary<string, int> _forfeitCount = new Dictionary<string, int>();

        public League() : this(new List<LeagueRow>())
        {
        }

        public League(List<LeagueRow> rows)
        {
            _rows = rows;
        }

        private Regex _validNameRegex = new Regex("^\\w+$");

        public void Forfeit(string challengee, string challenger)
        {
            CheckPlayerIsInGame(challengee);
            CheckPlayerIsInGame(challenger);

            int chalengeeRowIndex = FindPlayerRowIndex(challengee);
            int challengerRowIndex = FindPlayerRowIndex(challenger);

            if (challengerRowIndex - chalengeeRowIndex != 1)
            {
                throw new ArgumentException($"Player {challengee} cannot forfeit to player {challenger} as they are lower ranked");
            }
            int forfeitCount = IncrementForfeitCount(challengee);
            if(forfeitCount == 3)
            {
                RecordWin(challenger, challengee);
            }
        }

        public void AddPlayer(string player)
        {
            ValidateName(player);
            CheckPlayerIsUnique(player);

            if (AreAllRowsFull())
            {
                AddNewRow();
            }

            BottomRow().Add(player);
        }

        public List<LeagueRow> GetRows()
        {
            return _rows;
        }

        public void RecordWin(string winner, string loser)
        {
            CheckPlayerIsInGame(winner);
            CheckPlayerIsInGame(loser);

            int winnerRowIndex = FindPlayerRowIndex(winner);
            int loserRowIndex = FindPlayerRowIndex(loser);

            if (winnerRowIndex - loserRowIndex != 1)
            {
                throw new ArgumentException($"Cannot record match result. Winner {winner} must be one row below loser {loser}");
            }

            _rows[winnerRowIndex].Swap(winner, loser);
            _rows[loserRowIndex].Swap(loser, winner);
            ResetForfeitCount(loser);
            ResetForfeitCount(winner);
        }

        public string GetWinner()
        {
            if (_rows.Count > 0)
            {
                return _rows.First().GetPlayers().First();
            }

            return null;
        }

        private int IncrementForfeitCount(string player)
        {
            if (!_forfeitCount.ContainsKey(player))
            {
                _forfeitCount.Add(player, 0);
            }
            _forfeitCount[player]++;
            return _forfeitCount[player];
        }

        private void ResetForfeitCount(string player)
        {
            if (!_forfeitCount.ContainsKey(player))
            {
                _forfeitCount.Add(player, 0);
            }
            _forfeitCount[player] = 0;
        }

        private bool Player1OneRankBelowPlayer2(string player1, string player2)
        {
            int player1RowIndex = FindPlayerRowIndex(player1);
            int player2RowIndex = FindPlayerRowIndex(player2);

            return player2RowIndex - player1RowIndex == 1;
        }

        private bool AreAllRowsFull()
        {
            return _rows.Count == 0 || BottomRow().IsFull();
        }

        private void AddNewRow()
        {
            _rows.Add(new LeagueRow(_rows.Count + 1));
        }

        private LeagueRow BottomRow()
        {
            return _rows.Last();
        }

        private void ValidateName(string player)
        {
            if (!_validNameRegex.IsMatch(player))
            {
                throw new ArgumentException($"Player name {player} contains invalid");
            }
        }

        private void CheckPlayerIsInGame(string player)
        {
            if (!IsPlayerInGame(player))
            {
                throw new ArgumentException($"Player {player} is not in the game");
            }
        }

        private void CheckPlayerIsUnique(string player)
        {
            if (IsPlayerInGame(player))
            {
                throw new ArgumentException($"Cannot add player {player} because they are already in the game");
            }
        }

        private bool IsPlayerInGame(string player)
        {
            return FindPlayerRowIndex(player) >= 0;
        }

        private int FindPlayerRowIndex(string player)
        {
            return _rows.FindIndex(row => row.Includes(player));
        }
    }
}
