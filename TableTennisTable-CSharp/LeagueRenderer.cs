using System.Linq;

namespace TableTennisTable_CSharp
{
    public interface ILeagueRenderer
    {
        string Render(ILeague league);
    }

    public class LeagueRenderer : ILeagueRenderer
    {
        private const int MaxNameLength = 17;
        private const int BoxWidth = MaxNameLength + 2;
        private static readonly string Boundary = new string('-', BoxWidth);
        private static readonly string EmptyName = $"|{new string(' ', MaxNameLength)}|";

        public string Render(ILeague league)
        {
            var rows = league.GetRows();

            if (rows.Count == 0)
            {
                return "No players yet";
            }

            var renderedRows = rows.Select((row, index) => RenderRow(row, index, rows.Count));

            return string.Join("\r\n", renderedRows);
        }

        private string RenderRow(LeagueRow row, int rowIndex, int totalRows)
        {
            string rowBoundary = string.Join(" ", Enumerable.Repeat(Boundary, row.GetMaxSize()));
            var formattedNames = row.GetPlayers().Select(name => $"|{FormatName(name)}|").ToList();
            int rowsRemaining = totalRows - rowIndex;
            int paddingLength = (BoxWidth + 1) / 2 * rowsRemaining;
            string padding = new string(' ', paddingLength);
            var emptyNames = Enumerable.Repeat(EmptyName, row.GetMaxSize() - formattedNames.Count);
            var allNames = string.Join(" ", formattedNames.Concat(emptyNames));

            return $"{padding}{rowBoundary}\r\n{padding}{allNames}\r\n{padding}{rowBoundary}";
        }

        private string FormatName(string name)
        {
            if (name.Length > MaxNameLength)
            {
                return name.Take(MaxNameLength - 3) + "...";
            }

            int leftPad = (MaxNameLength - name.Length) / 2;
            return name.PadLeft(leftPad + name.Length).PadRight(MaxNameLength);
        }
    }
}
