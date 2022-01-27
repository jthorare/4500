using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common.Map;

namespace Trains.Common.GameState.Json
{
    public sealed class Card : Dictionary<Color, uint> {}

    public sealed class ThisPlayerJson
    {
        [JsonProperty("destination1")] public Destination Destination1 { get; set; }
        [JsonProperty("destination2")] public Destination Destination2 { get; set; }
        [JsonProperty("rails")] public uint Rails { get; set; }
        [JsonProperty("cards")] public Card Cards { get; set; }
        [JsonProperty("acquired")] public PlayerJson Acquired { get; set; }
    }
}
