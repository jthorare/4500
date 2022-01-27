using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Common.GameState.Json
{
    public class PlayerStateJson
    {
        [JsonProperty("this")] public ThisPlayerJson This { get; set; }

        [JsonProperty("acquired")] public List<PlayerJson> Acquired { get; set; }
    }
}
