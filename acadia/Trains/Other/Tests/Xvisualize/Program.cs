using Newtonsoft.Json;
using System;
using System.IO;
using Trains;
using Trains.Util;
using Trains.Util.Json;

namespace Xvisualize
{
    /// <summary>
    /// Visualization harness for milestone 4 that consumes its Map JSON input (https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29) 
    /// on STDIN, pops up a window that visualizes the given Map, and closes it after 10s.
    /// Run it from acadia/Trains/4/ using `./xvisualize<Vis/1-in.json`, or provide it a different valid, well-formed JSON input.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Represents the length of time that the visualization window should remain open, in seconds.
            int visualizationLength = 10;

            string input = Utilities.ReadInput();
            if (input != null && input != "")
            {
                JsonMap jsonMap = JsonConvert.DeserializeObject<JsonMap>(input);
                MapEditor mapEditor = new MapEditor(jsonMap.ToMap(), visualizationLength);
                mapEditor.Visualize();
            }
            else
            {
                throw new IOException("Input JSON was null.");
            }
        }
    }
}
