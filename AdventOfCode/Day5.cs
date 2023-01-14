namespace AdventOfCode
{
    class Day5
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day5");

            var resourceName = "AdventOfCode.Resources.2022.Day5.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);

            new CrateMover("CrateMover 9000").ProcessData(fileContents);

            new CrateMover9001().ProcessData(fileContents);

        }
    }


    public class CrateMover
    {
        public CrateMover(string model)
        {
            Model = model;
        }

        public string Model { get; set; }

        public void ProcessData(string fileContents)
        {
            var stackMarker = $"{Environment.NewLine}";
            var stackData = fileContents.Split(stackMarker).Take(9);
            var stacks = new List<List<string>>();
            
            BuildStacks(stacks, stackData);
            var data = fileContents.Split(stackMarker).Skip(10).ToArray();
            
            MoveCrates(stacks, data);
            var output = string.Empty;
            for (int x = 0; x < stacks.Count; x++)
            {
                output += stacks[x].First();
            }

            Console.WriteLine($"Top of the crate stack with the {Model} reads as {output}.");
        }

        protected void BuildStacks(List<List<string>> stacks, IEnumerable<string> stackData)
        {
            var fixedLengths = new List<Range>
            {
                1..4, 5..8, 9..12, 13..16, 17..20, 21..24, 25..28, 29..32, 33..35
            };
            for (var i = 0; i < 9; i++)
            {
                stacks.Add(new());
            }
            for (int i = stackData.Take(8).Count() - 1, j = 0; i >= 0; i--, j++)
            {
                var stackRow = stackData.ToArray()[j];
                for (int x = 0; x < fixedLengths.Count; x++)
                {
                    var startValue = fixedLengths[x].Start.Value;
                    var lengthValue = fixedLengths[x].End.Value - fixedLengths[x].Start.Value;
                    var crate = stackRow.Substring(startValue, lengthValue)
                        .Replace("[", string.Empty)
                        .Replace("]", string.Empty)
                        .Trim();
                    if (!string.IsNullOrWhiteSpace(crate))
                    {
                        stacks[x].Add(crate);
                    }
                }
            }
        }

        protected void MoveCrates(List<List<string>> stacks, string[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                // initialize data indexes
                var moveEndIndex = data[i].IndexOf("move") + 4;
                var fromIndex = data[i].IndexOf("from");
                var fromEndIndex = fromIndex + 4;
                var toIndex = data[i].IndexOf("to");
                var toEndIndex = toIndex + 2;
                // extract data
                var moveValue = data[i][moveEndIndex..fromIndex];
                var fromValue = data[i][fromEndIndex..toIndex];
                var toValue = data[i][toEndIndex..];
                // convert into values
                var moveId = int.Parse(moveValue.Trim());
                var fromId = int.Parse(fromValue.Trim()) - 1;
                var toId = int.Parse(toValue.Trim()) - 1;
                // arrange crates
                Arrange(stacks, fromId, toId, moveId);
            }
        }

        protected virtual void Arrange(List<List<string>> stacks, int fromId, int toId, int moveId)
        {
            var targetCrates = stacks[fromId].Take(moveId).Reverse();
            stacks[toId].InsertRange(0, targetCrates);
            stacks[fromId].RemoveRange(0, moveId);
        }
    }


    public class CrateMover9001 : CrateMover
    {
        public CrateMover9001() : base("CrateMover 9001")
        { }

        protected override void Arrange(List<List<string>> stacks, int fromId, int toId, int moveId)
        {
            var targetCrates = stacks[fromId].Take(moveId);
            stacks[toId].InsertRange(0, targetCrates);
            stacks[fromId].RemoveRange(0, moveId);
        }
    }
}
