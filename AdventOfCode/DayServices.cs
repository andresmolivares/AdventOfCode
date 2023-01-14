using System.Reflection;

namespace AdventOfCode
{
    public static class DayServices
    {
        public static string ReadResourceAsString(string resourceName)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            var tr = new StreamReader(stream!);
            return tr.ReadToEnd();
        }
    }
}
