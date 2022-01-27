using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
    }
}