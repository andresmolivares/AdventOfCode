namespace AdventOfCode
{
    class Day2
	{
		public static void Main(string[] args)
		{
            Console.WriteLine("2022.Day2");
            var resourceName = "AdventOfCode.Resources.2022.Day2.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);

            var playMarker = $"{Environment.NewLine}";
            var plays = fileContents.Split(playMarker);
            Console.WriteLine($"There are {plays.Length} games played.");

            var playerMarker = " ";
            var playResults = plays.Select(p => {
				var opp = p.Split(playerMarker).First();
                var player = p.Split(playerMarker).Last();
				return GameEngine.Play(player, opp);
            });
			var totalScore = playResults.Sum();
            Console.WriteLine($"The total score after all games played is {totalScore}.");

            var ultraPlayResults = plays.Select(p => {
                var opp = p.Split(playerMarker).First();
                var player = p.Split(playerMarker).Last();
                return GameEngine.UltraPlay(player, opp);
            });
            var ultraTotalScore = ultraPlayResults.Sum();
            Console.WriteLine($"The total score after all games played in ultra mode is {ultraTotalScore}.");
        }
	}


	static class GameEngine
	{
        const int WinnerScore = 6;
        const int DrawScore = 3;
        const int LoserScore = 0;

        private static readonly Dictionary<SelectionEnum, int> SelectionPointMap = new()
        {
            [SelectionEnum.Rock] = 1,
            [SelectionEnum.Paper] = 2,
            [SelectionEnum.Scissors] = 3
        };

        public static int Play(string player, string opp)
		{
			var play = TranslatePlay(player);
            var score = ProcessScore(play, TranslatePlay(opp));
			var points = SelectionPointMap[play];
			return score + points;
		}

		private static SelectionEnum TranslatePlay(string input)
		{
            return input switch
            {
                var s when new[] { "A", "X" }.Contains(s) => SelectionEnum.Rock,
                var s when new[] { "B", "Y" }.Contains(s) => SelectionEnum.Paper,
                _ => SelectionEnum.Scissors,
            };
		}

        private static int ProcessScore(SelectionEnum playerPlay, SelectionEnum oppPlay)
		{
            if (playerPlay == oppPlay)
				return DrawScore;
			if ((playerPlay == SelectionEnum.Scissors && oppPlay == SelectionEnum.Paper)
				||
				(playerPlay == SelectionEnum.Paper && oppPlay == SelectionEnum.Rock)
				||
				(playerPlay == SelectionEnum.Rock && oppPlay == SelectionEnum.Scissors))
				return WinnerScore;
			return LoserScore;
        }

        #region Ultra Play

        public static int UltraPlay(string player, string opp)
        {
            var play = UltraTranslatePlay(player, opp);
            var score = ProcessScore(play, TranslatePlay(opp));
            var points = SelectionPointMap[play];
            return score + points;
        }

        private static SelectionEnum UltraTranslatePlay(string input, string opp)
        {
            var ultraInput = string.Empty;

            switch (input.ToUpper())
            {
                case "X":
                    ultraInput = GetLoser(opp);
                    break;
                case "Z":
                    ultraInput = GetWinner(opp);
                    break;
                default:
                    ultraInput = opp;
                    break;
            }
            return TranslatePlay(ultraInput);
        }

        private static string GetWinner(string input)
        {
            switch (input.ToUpper())
            {
                case "A": return "B";
                case "B": return "C";
                default: return "A";
            }
        }

        private static string GetLoser(string input)
        {
            switch (input.ToUpper())
            {
                case "A": return "C";
                case "B": return "A";
                default: return "B";
            }
        }

        #endregion Ultra Play
    }


    enum SelectionEnum
	{ 
		Rock,
		Paper,
		Scissors
	}
}
