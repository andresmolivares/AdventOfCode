namespace AdventOfCode
{
    class Day5
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day5");

            var resourceName = "AdventOfCode.Resources.2022.Day5.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);

            new CrateMover("CrateMover 9000", 8, 10).ProcessData(fileContents);

            new CrateMover9001(8, 10).ProcessData(fileContents);

        }
    }

    public class StackBuilder
    {
        protected virtual List<Range> GetFixedLengths()
        {
            return new List<Range>
            {
                1..4, 5..8, 9..12, 13..16, 17..20, 21..24, 25..28, 29..32, 33..35
            };
        }

        public virtual List<List<string>> BuildStacks(
            string fileContents, 
            int stackContentLineTake)
        {
            var stackMarker = $"{Environment.NewLine}";
            var stackData = fileContents.Split(stackMarker).Take(stackContentLineTake);
            var stacks = new List<List<string>>();

            var fixedLengths = GetFixedLengths();
            for (var i = 0; i < fixedLengths.Count; i++)
            {
                stacks.Add(new());
            }
            for (int i = stackData.Take(stackContentLineTake).Count() - 1, j = 0; i >= 0; i--, j++)
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

            return stacks;  
        }
    }


    public class CrateMover
    {
        protected readonly string MoveToken = "move";
        protected readonly string FromToken = "from";
        protected readonly string ToToken = "to";
        protected readonly StackBuilder builder = new();

        public CrateMover(string model, int contentLineTake, int contentLineSkip)
        {
            Model = model;
            ContentLineTake = contentLineTake;
            ContentLineSkip = contentLineSkip;
        }

        public string Model { get; init; }

        public int ContentLineTake { get; init; }

        public int ContentLineSkip { get; init; }

        public void ProcessData(string fileContents)
        {
            var stackMarker = $"{Environment.NewLine}";
            var stacks = builder.BuildStacks(fileContents, ContentLineTake);
            var data = fileContents.Split(stackMarker).Skip(ContentLineSkip).ToArray();
            
            MoveCrates(stacks, data);
            var output = string.Empty;
            for (int x = 0; x < stacks.Count; x++)
            {
                output += stacks[x].First();
            }

            Console.WriteLine($"Top of the crate stack with the {Model} reads as {output}.");
        }

        protected virtual void MoveCrates(List<List<string>> stacks, string[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                // initialize data indexes
                var moveEndIndex = data[i].IndexOf(MoveToken) + MoveToken.Length;
                var fromIndex = data[i].IndexOf(FromToken);
                var fromEndIndex = fromIndex + FromToken.Length;
                var toIndex = data[i].IndexOf(ToToken);
                var toEndIndex = toIndex + ToToken.Length;
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
        const string ModelName = "CrateMover 9001";

        public CrateMover9001(int contentLineTake, int contentLineSkip) 
            : base(ModelName, contentLineTake, contentLineSkip)
        { }

        protected override void Arrange(List<List<string>> stacks, int fromId, int toId, int moveId)
        {
            var targetCrates = stacks[fromId].Take(moveId);
            stacks[toId].InsertRange(0, targetCrates);
            stacks[fromId].RemoveRange(0, moveId);
        }
    }
}
