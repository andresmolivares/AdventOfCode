namespace AdventOfCode
{
    class Day25
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day25");

            var resourceName = "AdventOfCode.Resources.2022.Day25.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);
            var digitsMarker = $"{Environment.NewLine}";
            var digitLines = fileContents.Split(digitsMarker);

            //Test();

            long results = 0;
            var calc = new SnafuCalculator();
            foreach (var value in digitLines)
            {
                var result = calc.ProcessDigits(value);
                Console.WriteLine($"SNAFU value {value} equates to {result}.");
                results += result;
            }
            Console.WriteLine($"Total results are {results}.");

            var snafuTranslation = "2-=102--02--=1-12=22";
            var snafuValue = calc.ProcessDigits(snafuTranslation);
            Console.WriteLine($"The snafu number {snafuTranslation} outputs the following value {snafuValue}.");
        }

        public static string[] Test()
        {
            return new[] {
                "1=-0-2",
                "12111",
                "2=0=",
                "21",
                "2=01",
                "111",
                "20012",
                "112",
                "1=-1=",
                "1-12",
                "12",
                "1=",
                "122"
            };
        }
    }

    public class SnafuCalculator
    {
        #region Legend
        readonly Func<long, long, long> calculate = (n, m) => n * (m == 1 ? 1 : (long)Math.Pow(5, m - 1));
        const int minus = -1;
        const int dbleMinus = -2;
        #endregion Legend

        public long ProcessDigits(string digits)
        {
            long result = 0;
            var length = digits.Length;
            for (var i = digits.Length - 1; i >= 0; i--)
            {
                if (!long.TryParse(digits[i].ToString(), out long number))
                { 
                    number = digits[i] == '-' ? minus : dbleMinus;
                }
                result += calculate(number, length - i);
            }
            return result;
        }
    }
}
