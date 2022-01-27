using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Trains.Util.Json;

namespace Trains.Models.TurnTypes
{

    /// <summary>
    /// Interface representing the type of turn a Player should take. 
    /// </summary>
    [JsonConverter(typeof(JsonActionConverter))]
    public interface ITurn
    {
    }
}
