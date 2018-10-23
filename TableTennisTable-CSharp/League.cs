using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TableTennisTable_CSharp
{
    public class League
    {
        private List<LeagueRow> _rows;

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
            throw new NotImplementedException();
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
        }

        public string GetWinner()
        {
            if (_rows.Count > 0)
            {
                return _rows.First().GetPlayers().First();
            }

            return null;
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
