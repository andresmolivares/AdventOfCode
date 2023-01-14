using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    class Day4
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day4");

            var resourceName = "AdventOfCode.Resources.2022.Day4.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);

            var elvesPairMarker = $"{Environment.NewLine}";
            var elvesPairs = fileContents.Split(elvesPairMarker);
            Console.WriteLine($"There are {elvesPairs.Length} rucksacks.");

            var fullOverlapPairsCount = elvesPairs
                .Select(p => new ElvesPair(p))
                .Where(p => p.HasFullOverlap())
                .Count();
            Console.WriteLine($"The total sum elves pairs that have full overlap is {fullOverlapPairsCount}.");

            var intersectionPairsCount = elvesPairs
                .Select(p => new ElvesPair(p))
                .Where(p => p.HasIntersections())
                .Count();
            Console.WriteLine($"The total sum elves pairs that intersect is {intersectionPairsCount}.");
        }
    }


    class ElvesPair
    {
        public ElvesPair(string data)
        { 
            var ranges = data.Split(",");
            var range1= ranges[0].Split("-");
            var range2 = ranges[1].Split("-");
            ElfRange1 = int.Parse(range1[0])..int.Parse(range1[1]);
            ElfRange2 = int.Parse(range2[0])..int.Parse(range2[1]);
        }

        public Range ElfRange1 { get; set; } = new();

        public Range ElfRange2 { get; set; } = new();

        public bool HasIntersections()
        { 
            var result = BuildRange(ElfRange1).Intersect(BuildRange(ElfRange2)).Any();
            return result;
        }

        public bool HasFullOverlap()
        {
            return (ElfRange1.Start.Value >= ElfRange2.Start.Value 
                && ElfRange1.End.Value <= ElfRange2.End.Value)
                ||
                (ElfRange2.Start.Value >= ElfRange1.Start.Value 
                && ElfRange2.End.Value <= ElfRange1.End.Value)
                ;
        }

        private static IEnumerable<int> BuildRange(Range range)
        {
            for (var i = range.Start.Value; i <= range.End.Value; i++)
            { 
                yield return i;
            }
        }
    }
}
