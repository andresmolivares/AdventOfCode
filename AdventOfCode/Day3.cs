namespace AdventOfCode
{
    class Day3
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day3");
            var resourceName = "AdventOfCode.Resources.2022.Day3.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);

            var rucksackMarker = $"{Environment.NewLine}";
            var rucksacks = fileContents.Split(rucksackMarker);
            Console.WriteLine($"There are {rucksacks.Length} rucksacks.");

            var prioritySum = rucksacks.Sum(p => new Rucksack(p).Priority);
            Console.WriteLine($"The total sum of the priorities is {prioritySum}.");

            var rucksackGroups = rucksacks.Chunk(RucksackGroup.Capacity);
            Console.WriteLine($"The total of {rucksackGroups.Count()} rucksack groups.");
            
            var badgePrioritySum = rucksackGroups.Select(g => new RucksackGroup(g).BadgePriority).Sum();
            Console.WriteLine($"The total sum of the priorities is {badgePrioritySum}.");
        }
    }


    class Rucksack
    {
        public Rucksack(string data)
        {
            Data = data;
            var length = (data.Length / 2);
            Compartment1 = data.Substring(0, length);
            Compartment2 = data.Substring(length, length);
            var priorityChar = FindPriority();
            Priority = ParsePriority(priorityChar); 
        }

        public string? Data { get; set; }

        private string Compartment1 { get; set; }

        private string Compartment2 { get; set; }

        public static int ParsePriority(char priorityChar)
        {
            var lowerPriority = char.ToUpper(priorityChar) - 64;
            var isUpperCase = char.IsUpper(priorityChar);
            return isUpperCase ? lowerPriority + 26 : lowerPriority; 
        }
        
        private char FindPriority()
        {
            var priorityList = Compartment1.ToList().Intersect(Compartment2.ToList());
            return priorityList.FirstOrDefault();
        }

        public int Priority { get; }
    }


    class RucksackGroup
    {
        public const int Capacity = 3;

        public RucksackGroup(IEnumerable<string> data)
        {
            var intersection = data.ToArray().IntersectMany();
            BadgePriority = Rucksack.ParsePriority(intersection.First());
        }

        public int BadgePriority { get; set; }
    }
}
