using System.Runtime.CompilerServices;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("Trains.Tests.Common")]

namespace Trains.Common.Map.Json
{
    /// <summary>
    /// Represents the Json representation of a city.
    /// </summary>
    [JsonConverter(typeof(CityJsonConverter))]
    internal sealed class CityJson
    {
        public string? Name { get; set; }
        public uint X { get; set; }
        public uint Y { get; set; }
    }
}