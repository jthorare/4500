using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Trains.Common.Map;

namespace Trains.Common.GameState.Json
{
    [JsonConverter(typeof(AcquiredJsonConverter))]
    public sealed class AcquiredJson
    {
        public string City1 { get; set; }
        public string City2 { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public Color Color { get; set; }
        public uint Length { get; set; }
    }
}
