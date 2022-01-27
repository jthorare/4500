using System.Collections.Generic;
using Trains.Common.Map;
#if DEBUG
#else
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
#endif

namespace xtests.xvisualize.ViewModels
{
    /// <summary>
    /// Represents data the Main window requires to function. Contains a Map 
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="TrainsMap"/>
        /// </summary>
        public TrainsMap? Map { get; set; }
        public IEnumerable<Location>? Locations => Map?.Locations.Values;
        public IEnumerable<Connection>? Connections => Map?.Connections;

        public MainWindowViewModel()
        {
#if DEBUG
            TrainsMap.TrainsMapBuilder trainsMapBuilder = new();

            trainsMapBuilder.AddLocation("Seattle", 00f, 0f);
            trainsMapBuilder.AddLocation("San Francisco", 0.2f, 0.7f);
            trainsMapBuilder.AddLocation("Chicago", 0.6f, 0.4f);
            trainsMapBuilder.AddLocation("Portland", 0.9f, 0.1f);
            trainsMapBuilder.AddLocation("Boston", 0.8f, 0.4f);
            trainsMapBuilder.AddLocation("Washington D.C.", 0.8f, 0.6f);
            trainsMapBuilder.AddLocation("Orlando", 0.9f, 0.9f);
            trainsMapBuilder.AddLocation("Anchorage", 0.1f, 0.1f);
            trainsMapBuilder.AddLocation("Honolulu", 0.1f, 0.9f);

            trainsMapBuilder.AddConnection("Seattle", "San Francisco", 4, Color.Red);
            trainsMapBuilder.AddConnection("Seattle", "San Francisco", 3, Color.Blue);
            trainsMapBuilder.AddConnection("Seattle", "San Francisco", 3, Color.Green);
            trainsMapBuilder.AddConnection("Seattle", "San Francisco", 5, Color.Red);
            trainsMapBuilder.AddConnection("Seattle", "San Francisco", 4, Color.Blue);
            trainsMapBuilder.AddConnection("Seattle", "San Francisco", 4, Color.White);
            trainsMapBuilder.AddConnection("Seattle", "Chicago", 5, Color.Blue);
            trainsMapBuilder.AddConnection("San Francisco", "Chicago", 5, Color.Red);
            trainsMapBuilder.AddConnection("Chicago", "Portland", 5, Color.White);
            trainsMapBuilder.AddConnection("Chicago", "Boston", 4, Color.Green);
            trainsMapBuilder.AddConnection("Chicago", "Washington D.C.", 4, Color.Green);
            trainsMapBuilder.AddConnection("Chicago", "Orlando", 3, Color.Blue);
            trainsMapBuilder.AddConnection("Portland", "Boston", 3, Color.Blue);
            trainsMapBuilder.AddConnection("Boston", "Washington D.C.", 3, Color.White);
            trainsMapBuilder.AddConnection("Washington D.C.", "Orlando", 3, Color.Blue);
            trainsMapBuilder.AddConnection("Anchorage", "Honolulu", 5, Color.Blue);

            Map = trainsMapBuilder.BuildMap();
#else
            // Read from stdin json and deserialize to Map.
            string json;
            StringBuilder stringBuilder = new();
            StringWriter stringWriter = new(stringBuilder);
            
            string? line;
            while ((line = Console.ReadLine()) != null)
            {
                stringWriter.WriteLine(line);
            }
            json = stringBuilder.ToString();
            var stream = new StringReader(json);
            var reader = new JsonTextReader(stream);
            reader.SupportMultipleContent = true;
            var serializer = new JsonSerializer();

            if (reader.Read())
            {
                Map = serializer.Deserialize<TrainsMap>(reader)!;
            }
#endif
        }
        
    }
}