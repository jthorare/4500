using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Common.Map
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Color {
        [EnumMember(Value = "red")] Red,
        [EnumMember(Value = "blue")] Blue,
        [EnumMember(Value = "green")] Green,
        [EnumMember(Value = "white")] White
    }
}
