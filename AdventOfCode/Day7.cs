namespace AdventOfCode
{
    class Day7
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day7");

            const int TotalDiskSpace = 70000000;
            const int RequiredSpaceForUpdate = 30000000;

            var resourceName = "AdventOfCode.Resources.2022.Day7.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName);
            var instructionMarker = $"{Environment.NewLine}";
            var instructions = fileContents.Split(instructionMarker);
            // process each command
            // group by first cd to first cd after lst command
            // build instructions
            // build tree of dirs and files
            // calculate all dirs
            instructions.ToList().ForEach(i => FileProcessor.ProcessInstructions(i));

            // find all dirs under 100K
            var results = FileProcessor.Root.Folders
                .Flatten(f => f.Folders)
                .Where(f => f.GetTraversedFolderSize() < 100000)
                .OrderBy(f => f.GetTraversedFolderSize())
                .ToList()
                ;
            ConsoleExtension.WriteNewLine();
            Console.WriteLine($"The count of directories with a total size of at most 100000 is {results.Count}.");
            ConsoleExtension.WriteNewLine();
            results.ForEach(f => Console.WriteLine($"{f.Name}: {f.GetTraversedFolderSize()}"));
            ConsoleExtension.WriteNewLine();
            Console.WriteLine($"The sum of the total sizes of those directories is {results.Sum(f => f.GetTraversedFolderSize())}.");
            ConsoleExtension.WriteNewLine();

            // Process disk space for update
            var totalUsedSpace = FileProcessor.Root.GetTraversedFolderSize();
            Console.WriteLine($"Total space used {totalUsedSpace}.");
            var remainingSpace = TotalDiskSpace - totalUsedSpace;
            Console.WriteLine($"Remaining space is {remainingSpace}.");
            var remainingSpaceForUpdate = RequiredSpaceForUpdate - remainingSpace;
            Console.WriteLine($"Please free up {remainingSpaceForUpdate} on your disk to install the update.");

            // Find the deleteable folder candidates
            ConsoleExtension.WriteNewLine();
            var deleteCandidates = FileProcessor.Root.Folders
                .Flatten(f => f.Folders)
                .Where(f => f.GetTraversedFolderSize() > remainingSpaceForUpdate)
                .OrderByDescending(f => f.GetTraversedFolderSize())
                .ToList();
            deleteCandidates.ForEach(f => Console.WriteLine($"{f.Name}: {f.GetTraversedFolderSize()}"));

            // Output the smallest deletable folder
            var smallestDeleteCandidate = deleteCandidates
                .MinBy(f => f.GetTraversedFolderSize());
            ConsoleExtension.WriteNewLine();
            Console.WriteLine($"The best candidate to remove for an update is {smallestDeleteCandidate!.Name} with a total file size of {smallestDeleteCandidate.GetTraversedFolderSize()}.");
        }
    }

    public static class FileProcessor
    {
        public static Folder Root = new();
        private static Folder? CurrentFolder = null;
        private static int LineNumber = 0;

        public static void ProcessInstructions(string instruction, bool outputProcessing = false)
        {
            const string CdToken = "$ cd ";
            const string LsToken = "$ ls";
            const string DirToken = "dir ";
            const string OutToken = "..";
            LineNumber++;
            // Handle change directory
            if (instruction.StartsWith(CdToken))
            {
                var currentFolderName = CurrentFolder?.Name ?? "null";
                if(outputProcessing)
                    Console.WriteLine($"Current folder is {currentFolderName} folder.");
                // get change directory token name
                var cdName = instruction.Replace(CdToken, string.Empty);
                // move up
                if (cdName == OutToken)
                {
                    CurrentFolder = CurrentFolder!.Parent;
                    if (CurrentFolder is not null)
                    {
                        if (outputProcessing)
                            Console.WriteLine($"LN: {LineNumber} - move up to {CurrentFolder!.Name} folder from {currentFolderName} folder.");
                    }
                    return;
                }
                // find current folder
                var current = CurrentFolder?.Folders.FirstOrDefault(f => f.Name == cdName);
                // add new folder
                if (current is null)
                {
                    if (LineNumber > 1)
                    {
                        throw new InvalidOperationException($"File system only supports a single root. Error processing: {instruction}");
                    }
                    current = new() { Name = instruction.Replace(CdToken, string.Empty), Parent = CurrentFolder };
                    Root = current;
                    if (outputProcessing)
                        Console.WriteLine($"added root folder {current.Name}");
                }
                // set current folder
                CurrentFolder = current;
                if (outputProcessing)
                    Console.WriteLine($"LN: {LineNumber} - move to {CurrentFolder!.Name} folder from {currentFolderName} folder.");
            }
            // Handle adding directory
            else if (instruction.StartsWith(DirToken))
            {
                Folder folder = new() { Name = instruction.Replace(DirToken, string.Empty), Parent = CurrentFolder };
                CurrentFolder!.Folders.Add(folder);
                if (outputProcessing)
                    Console.WriteLine($"LN: {LineNumber} - {instruction} was added to {CurrentFolder!.Name} folder. Current folder size: {CurrentFolder!.TotalFileSize}");
            }
            // Handle adding files
            else if (!instruction.StartsWith(LsToken))
            {
                var attributes = instruction.Split(" ");
                File file = new() { Size = int.Parse(attributes[0]), Name = attributes[1], };
                CurrentFolder!.Add(file);
                if (outputProcessing)
                    Console.WriteLine($"LN: {LineNumber} - {instruction} was added to {CurrentFolder!.Name} folder.");
            }
            else
            {
                if (outputProcessing)
                    Console.WriteLine($"LN: {LineNumber} - Instruction {instruction} ignored.");
            }
        }
    }

    public class Folder : List<File>
    {
        public string? Name { get; set; }

        public Folder? Parent { get; set; }

        public List<Folder> Folders { get; set; } = new();

        public int TotalFileSize => this.Sum(f => f.Size);

        // Get Total Traversed Folder File Size
        public int GetTraversedFolderSize() => Folders.Flatten(f => f.Folders).Sum(f => f.TotalFileSize) + TotalFileSize;
    }

    public class File
    {
        public string? Name { get; set; }

        public int Size { get; set; }
    }
}
