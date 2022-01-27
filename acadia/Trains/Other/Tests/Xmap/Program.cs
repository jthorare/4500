using Newtonsoft.Json;
using System;
using System.IO;
using Trains.Util;
using Trains.Util.Json;

namespace Xmap
{
    /// <summary>
    /// Test harness for milestone 3 that consumes on STDIN a JSON input containing a Map and two City names (https://www.ccs.neu.edu/home/matthias/4500-f21/3.html)
    /// and writes to STDOUT whether there is a connection between the two cities indicated by the passed city names.
    /// Run xmap by redirecting JSON input into a `./xmap` call from the acadia/3/ directory. ex: `./xmap<Tests/1-in.json` 
    /// should write to STDOUT the value that can be found in 1-out.json**
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string input = Utilities.ReadInput();
            if (input != null && input != "")
            {
                string[] inputs = Utilities.ExtractObject(input); // first string is the JsonMap, second is the remaining input that holds the city names
                string[] cityNames = ExtractCities(inputs[1]); // two city names
                JsonMap jsonMap = JsonConvert.DeserializeObject<JsonMap>(inputs[0]);
                Console.WriteLine(jsonMap.IsDestination(cityNames).ToString().ToLower()); // write whether the two city names form a destsination in our deserialized map.
            }
            else
            {
                throw new IOException("Input JSON was null.");
            }
        }

        /// <summary>
        /// Parse the input string and return its two component JSON strings in an array.
        /// </summary>
        /// <param name="input">A string holding two JSON string values representing two City names.</param>
        /// <returns>A new string array of size 2 where each string is a component JSON string representing a city name.</returns>
        private static string[] ExtractCities(string input)
        {
            // each flag corresponds to whether the start or end of a city's name has been found
            bool startCityOneFound = false;
            bool endCityOneFound = false;
            bool startCityTwoFound = false;

            // each variable corresponds to the start or end index of a city's name excluding quotation marks
            // (for use in substring to extract it)
            int startCityOne = 0;
            int startCityTwo = 0;
            int endCityOne = 0;
            int endCityTwo = 0;
            for (int ii = 0; ii < input.Length; ++ii)
            {
                char curr = input[ii];

                if (curr.Equals('\"'))
                {
                    if (!startCityOneFound)
                    {
                        startCityOneFound = true;
                        startCityOne = ii + 1;
                        continue;
                    }
                    else if (startCityOneFound && !endCityOneFound)
                    {
                        endCityOne = ii - 1;
                        endCityOneFound = true;
                        continue;
                    }
                    else if (startCityOneFound && endCityOneFound && !startCityTwoFound)
                    {
                        startCityTwoFound = true;
                        startCityTwo = ii + 1;
                        continue;
                    }
                    else
                    {
                        endCityTwo = ii - 1;
                        break;
                    }
                }
            }
            return new string[2] {
                input.Substring(startCityOne, endCityOne - startCityOne + 1),
                input.Substring(startCityTwo, endCityTwo - startCityTwo + 1) };
        }
    }
}
