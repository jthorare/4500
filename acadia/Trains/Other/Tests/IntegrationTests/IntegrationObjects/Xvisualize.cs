using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Trains;
using Trains.Util;
using Trains.Util.Json;

namespace IntegrationTests.IntegrationObjects
{
    class Xvisualize : IIntegration
    {
        public void Run()
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
