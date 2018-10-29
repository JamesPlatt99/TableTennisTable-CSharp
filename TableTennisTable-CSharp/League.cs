using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TableTennisTable_CSharp
{
    public interface ILeague
    {
        void Forfeit(string challengee, string challenger);
        void AddPlayer(string player);
        List<LeagueRow> GetRows();
        void RecordWin(string winner, string loser);
        string GetWinner();
    }

    public class League : ILeague
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

            if (!Player1OneRankBelowPlayer2(challenger, challengee))
            {
                throw new ArgumentException($"Player {challengee} cannot forfeit to player {challenger} as they are lower ranked");
            }
            int forfeitCount = IncrementForfeitCount(challengee);
            if(forfeitCount >= 3)
            {            
                SwapPlayer1AndPlayer2(challenger, challengee);
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
            if (!Player1OneRankBelowPlayer2(winner, loser))
            {
                throw new ArgumentException($"Cannot record match result. Winner {winner} must be one row below loser {loser}");
            }

            SwapPlayer1AndPlayer2(winner, loser);
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

        private bool Player1OneRankBelowPlayer2(string player1, string player2)
        {
            int player1RowIndex = FindPlayerRowIndex(player1);
            int player2RowIndex = FindPlayerRowIndex(player2);
            return (player1RowIndex - player2RowIndex == 1);
        }

        private void SwapPlayer1AndPlayer2(string player1, string player2)
        {
            int player1RowIndex = FindPlayerRowIndex(player1);
            int player2RowIndex = FindPlayerRowIndex(player2);

            _rows[player1RowIndex].Swap(player1, player2);
            _rows[player2RowIndex].Swap(player2, player1);
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
