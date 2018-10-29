using System;

namespace TableTennisTable_CSharp
{
    public class App
    {
        private ILeague _league;
        private ILeagueRenderer _leagueRenderer;
        private IFileService _fileService;
        private string _autosaveName;

        public App(ILeague initialLeague, ILeagueRenderer leagueRenderer, IFileService fileService)
        {
            _league = initialLeague;
            _leagueRenderer = leagueRenderer;
            _fileService = fileService;
            _autosaveName = GenerateAutoSaveName();
        }

        public string SendCommand(string command)
        {
            try
            {
                if (command.StartsWith("add player"))
                {
                    string player = command.Substring(11);
                    _league.AddPlayer(player);
                    AutoSave();
                    return $"Added player {player}";
                }

                if (command.StartsWith("record win"))
                {
                    string playersString = command.Substring(11);
                    var players = playersString.Split(' ');
                    string winner = players[0];
                    string loser = players[1];
                    _league.RecordWin(winner, loser);
                    AutoSave();
                    return $"Recorded {winner} win against {loser}";
                }

                if (command == "print")
                {
                    return _leagueRenderer.Render(_league);
                }

                if (command == "winner")
                {
                    return _league.GetWinner();
                }

                if (command.StartsWith("save"))
                {
                    var name = command.Substring(5);
                    _fileService.Save(name, _league);
                    return $"Saved {name}";
                }

                if (command.StartsWith("load"))
                {
                    var name = command.Substring(5);
                    _league = _fileService.Load(name);
                    if(name.StartsWith("saved_games/autosave"))
                    {
                        _autosaveName = name;
                    }
                    else
                    {
                        _autosaveName = GenerateAutoSaveName();
                    }
                    return $"Loaded {name}";
                }

                if (command.StartsWith("forfeit"))
                {
                    string playersString = command.Substring("forfeit ".Length);
                    string[] players = playersString.Split(' ');
                    string challenger = players[0];
                    string challengee = players[1];
                    _league.Forfeit(challengee, challenger);
                    AutoSave();
                    return $"Player {challengee} forfeited to {challenger}";
                }

                return $"Unknown command: {command}";
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
        }

        private void AutoSave()
        {
            _fileService.Save(_autosaveName, _league);
        }

        public string GenerateAutoSaveName()
        {
            string filename = string.Format("saved_games/autosave_{0:ddMMyy_HHmmss}", DateTime.Now);
            filename = IncrementFileNameIfTaken(filename);
            return filename;
        }

        public string IncrementFileNameIfTaken(string filename)
        {
            // We will keep incrementing the suffix of the filename until there are no existing files with the same name.
            try
            {
                _fileService.Load(filename);
                filename = IncrementFileNameSuffix(filename);
                return IncrementFileNameIfTaken(filename);
            }
            catch(ArgumentException)
            {
                return filename;
            }
        }

        private string IncrementFileNameSuffix(string filename)
        {
            int suffix = 1;
            string[] splitFilename = filename.Split('$');
            if (splitFilename.Length > 1)
            {
                suffix = Convert.ToInt32(splitFilename[1]) + 1;
            }
            return string.Format("{0}${1}", splitFilename[0], suffix);
        }
    }
}
