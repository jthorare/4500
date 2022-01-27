using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[assembly: InternalsVisibleTo("Trains.Tests.Common")]

namespace Trains.Common.Map.Json
{
    /// <summary>
    /// Represents a Segment in a Json representation of TargetJson.
    /// </summary>
    internal sealed class SegmentJson : Dictionary<Color, uint>
    {
    }

    /// <summary>
    /// Represents a Target in a json representation of Connections.
    /// </summary>
    internal sealed class TargetJson : Dictionary<string, SegmentJson>
    {
    }

    /// <summary>
    /// Represents a Map in a Json representation of a TrainsMap.
    /// </summary>
    internal sealed class MapJson
    {
        [JsonProperty("width")] public uint Width { get; set; }

        [JsonProperty("height")] public uint Height { get; set; }

        [JsonProperty("cities")] public ICollection<CityJson>? Cities { get; set; }

        [JsonProperty("connections")] public IDictionary<string, TargetJson>? Connections { get; set; }

        public static MapJson ConvertFromTrainsMap(TrainsMap trainsMap, uint width, uint height)
        {
            var sortedCities = trainsMap.Locations.Values.ToList();
            sortedCities.Sort();
            var seenConnections = new HashSet<Connection>();

            var connections = new Dictionary<string, TargetJson>();
            foreach (var location in sortedCities)
            {
                // connections not yet visited
                var adjacentConnections = trainsMap.Connections.Where(it => it.Locations.Contains(location))
                    .Except(seenConnections).ToList();
                adjacentConnections.Sort();
                // create/update TargetJson
                var targetJson = new TargetJson();
                foreach (var connection in adjacentConnections)
                {
                    var otherCity = connection.Locations.First(it => !it.Equals(location));
                    if (targetJson.ContainsKey(otherCity.Name))
                    {
                        targetJson[otherCity.Name].Add(connection.Color, connection.Segments);
                    }
                    else
                    {
                        targetJson.Add(otherCity.Name, new SegmentJson { { connection.Color, connection.Segments } });
                    }
                    // add connection to seen connections
                    seenConnections.Add(connection);
                }
                // add TargetJson to Connections has valid targets
                if (targetJson.Any())
                {
                    connections.Add(location.Name, targetJson);
                }
            }

            var locations = trainsMap.Locations.Values.ToList();
            locations.Sort();
            
            return new MapJson()
            {
                Width = width,
                Height = height,
                Cities = locations.Select(location => new CityJson
                {
                    Name = location.Name,
                    X = (uint)(location.X * width),
                    Y = (uint)(location.Y * height)
                }).ToList(),
                Connections = connections
            };
        }
    }
}