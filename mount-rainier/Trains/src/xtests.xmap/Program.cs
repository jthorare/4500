using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Trains.Common.Map;

namespace xtests.xmap
{
    /// <summary>
    /// Takes in 3 Json from STDIN city1, city2, and map. Returns output as Boolean json to STDOUT. Result is whether
    /// city1 and city2 are connected to each other through the map (answers the question: Are these two cities a valid
    /// Destination?)
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // Read lines from STDIN and save them.
            StringBuilder stringBuilder = new();
            StringWriter stringWriter = new(stringBuilder);
            
            string? line;
            while ((line = Console.ReadLine()) != null)
            {
                stringWriter.WriteLine(line);
            }
            var json = stringBuilder.ToString();
            
            // Create a JsonReader to read the json
            var stream = new StringReader(json);
            var reader = new JsonTextReader(stream);
            reader.SupportMultipleContent = true;
            var serializer = new JsonSerializer();


            // Get the first city name
            reader.Read();
            string city1 = serializer.Deserialize<string>(reader)!;
            // Get the second city name
            reader.Read();
            string city2 = serializer.Deserialize<string>(reader)!;
            // Get the map object
            reader.Read();
            TrainsMap map = serializer.Deserialize<TrainsMap>(reader)!;
            
            // Make json boolean output of whether the two cities are a destination in the map.
            Console.WriteLine(JsonConvert.SerializeObject(map.IsValidDestination(city1,city2)));
        }
    }
}