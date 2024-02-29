namespace ChampionshipChallenge;

public class Program
{
    public static void Main()
    {
        var rows = File.ReadAllLines("Matches.txt");
        var results = new Dictionary<string, int>();
        var matchResults = ParseRow(rows);
        foreach (var (team, score) in matchResults.SelectMany(x => x.ToResults))
        {
            if (!results.ContainsKey(team))
            {
                results.Add(team, score);
            }
            else
            {
                results[team] += score;
            }
        }

        foreach (var result in results.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{result.Key} - {result.Value} points");
        }
    }

    private static IEnumerable<MatchResult> ParseRow(string[] rows)
    {
        foreach (var row in rows)
        {
            var split = row.Split(',', ' ');
            if (split.Length != 4)
            {
                throw new ArgumentException(nameof(rows));
            }

            yield return new MatchResult(split[0], int.Parse(split[1]),
                split[2], int.Parse(split[3]));
        }
    }

    private class MatchResult
    {
        public MatchResult(string firstTeam, int firstTeamPoints, string secondTeam, int secondTeamPoints)
        {
            FirstTeam = firstTeam;
            FirstTeamPoints = firstTeamPoints;
            SecondTeam = secondTeam;
            SecondTeamPoints = secondTeamPoints;
        }

        public string FirstTeam { get; }
        public int FirstTeamPoints { get; }

        public int FirstTeamScore
        {
            get
            {
                if (FirstTeamPoints > SecondTeamPoints)
                {
                    return 3;
                }

                if (FirstTeamPoints == SecondTeamPoints)
                {
                    return 1;
                }

                return 0;
            }
        }

        public string SecondTeam { get; }
        public int SecondTeamPoints { get; }

        public int SecondTeamScore
        {
            get
            {
                if (SecondTeamPoints > FirstTeamPoints)
                {
                    return 3;
                }

                if (FirstTeamPoints == SecondTeamPoints)
                {
                    return 1;
                }

                return 0;
            }
        }

        public (string, int)[] ToResults => new[] { (FirstTeam, FirstTeamScore), (SecondTeam, SecondTeamScore) };
    }
}
