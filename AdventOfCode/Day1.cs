using System.Reflection;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    class Day1
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day1");

            var resourceName = "AdventOfCode.Resources.2022.Day1.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);

            var elfMarker = $"{Environment.NewLine}{Environment.NewLine}";
            var elfCalorieData = fileContents.Split(elfMarker);
            var calorieMarker = $"{Environment.NewLine}";
            var elves = elfCalorieData.Select(d => new ElfCalorie(d.Split(calorieMarker)));

            Console.WriteLine($"There are {elves.Count()} elves.");
            var maxCalorie = elves.Max(e => e.TotalCalories);
            Console.WriteLine($"The elf with the most calories has {maxCalorie} calories.");
            var topThreeCalorieTotal = elves
                .OrderByDescending(e => e.TotalCalories)
                .Take(3)
                .Sum(e => e.TotalCalories);
            Console.WriteLine($"The total of the top 3 calorie max is {topThreeCalorieTotal} calories.");
        }
    }


    class ElfCalorie
    {
        public ElfCalorie(string[] data)
        {
            data
                .ToList()
                .ForEach(d => Calories.Add(int.Parse(d)));
            TotalCalories = Calories.Sum();
        }

        public List<int> Calories { get; set; } = new List<int>();

        public int TotalCalories { get; set; }

        public int MaxCalories => Calories.Max();
    }
}