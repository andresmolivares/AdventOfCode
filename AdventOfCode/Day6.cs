namespace AdventOfCode
{
    class Day6
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("2022.Day6");

            var resourceName = "AdventOfCode.Resources.2022.Day6.txt";
            var fileContents = DayServices.ReadResourceAsString(resourceName).AsSpan();

            var packetLength = 4;
            var uniquePacketStart = fileContents.UniqueItemsInLength(packetLength);
            if (uniquePacketStart.Index > 0)
            {
                Console.WriteLine($"The first start-of-packet marker where the packet length is {packetLength} has been detected after character {uniquePacketStart.Index} with the string value {uniquePacketStart.Content}.");
            }
            else
            {
                Console.WriteLine("No first start-of-packet marker was detected.");
            }

            var messageLength = 14;
            var uniqueMessageStart = fileContents.UniqueItemsInLength(messageLength);
            if (uniqueMessageStart.Index > 0)
            {
                Console.WriteLine($"The first start-of-packet marker where the packet length is {messageLength} has been detected after character {uniqueMessageStart.Index} with the string value {uniqueMessageStart.Content}.");
            }
            else
            {
                Console.WriteLine("No first start-of-packet marker was detected.");
            }
        }
    }


    static class ReadOnlySpanExtenstion
    {
        public static (int Index, string Content) UniqueItemsInLength<T>(this ReadOnlySpan<T> owner, int chunkLength)
        {
            for (int start = 0; start + 1 + chunkLength <= owner.Length; start++)
            {
                var slice = owner.Slice(start, chunkLength);
                if (!slice
                    .ToArray()
                    .GroupBy(a => a)
                    .SelectMany(ab => ab.Skip(1).Take(1))
                    .ToList()
                    .Any())
                {
                    return (start + chunkLength, slice.ToString());
                }
            }
            return (-1, string.Empty);
        }
    }
}
