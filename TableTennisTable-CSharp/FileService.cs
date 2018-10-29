using System;
using System.IO;
using System.Linq;

namespace TableTennisTable_CSharp
{
    public interface IFileService
    {
        void Save(string path, ILeague league);
        League Load(string path);
    }

    public class FileService : IFileService
    {
        public League Load(string path)
        {
            try
            {
                return new League(File.ReadLines(path).Select(DeserialiseRow).ToList());
            }
            catch (IOException e)
            {
                throw new ArgumentException($"Could not load league {path}", e);
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException($"League {path} was not valid");
            }
        }

        private LeagueRow DeserialiseRow(string line, int index)
        {
            LeagueRow row = new LeagueRow(index + 1);
            foreach (var player in line.Split(','))
            {
                row.Add(player);
            }
            return row;
        }

        public void Save(string path, ILeague league)
        {
            try
            {
                File.WriteAllLines(path, league.GetRows().Select(SerialiseRow));
            }
            catch (IOException e)
            {
                throw new ArgumentException($"Could not save league {path}", e);
            }
        }

        private string SerialiseRow(LeagueRow row)
        {
            return string.Join(",", row.GetPlayers());
        }
    }
}
