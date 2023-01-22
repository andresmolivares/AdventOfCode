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
}
