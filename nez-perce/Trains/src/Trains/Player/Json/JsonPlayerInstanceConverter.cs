using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common;
using Trains.Common.Map;

namespace Trains.Player.Json
{
    /// <summary>
    /// Class for converting a https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29 into a JsonAcquired object.
    /// </summary>
    public class JsonPlayerInstanceConverter : JsonConverter
    {
        private readonly TrainsMap? _map;
        public JsonPlayerInstanceConverter(TrainsMap? map)
        {
            _map = map;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            int arrayDepth = 0;
            int processedVals = 0;
            List<IPlayer> players = new();
            string filePath = "../Trains/src/Trains/Player/Strategy/";

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                {
                    arrayDepth--;
                    if (arrayDepth <= 0)
                    {
                        break;
                    }
                    continue;
                }
                if (reader.TokenType == JsonToken.StartArray)
                {
                    arrayDepth++;
                    continue;
                }
                if (reader.TokenType == JsonToken.String)
                {
                    var name = (string)reader.Value!;
                    reader.Read();
                    var strat = (string)reader.Value!;
                    switch (strat)
                    {
                        case "Hold-10":
#if DEBUG
                            players.Add(new StrategyPlayer(name, "../../../../Trains/Player/Strategy/Hold10Strategy.cs", _map));
#else
                            players.Add(new StrategyPlayer(name, "../Trains/src/Trains/Player/Strategy/Hold10Strategy.cs", _map));
#endif
                            break;
                        case "Buy-Now":
#if DEBUG
                            players.Add(new StrategyPlayer(name, "../../../../Trains/Player/Strategy/BuyNowStrategy.cs", _map));
#else
                            players.Add(new StrategyPlayer(name, "../Trains/src/Trains/Player/Strategy/BuyNowStrategy.cs", _map));
#endif
                            break;
                        case "Cheat":
#if DEBUG
                            players.Add(new StrategyPlayer(name, "../../../../Trains/Player/Strategy/CheatStrategy.cs", _map));
#else
                            players.Add(new StrategyPlayer(name, "../Trains/src/Trains/Player/Strategy/CheatStrategy.cs", _map));

#endif
                            break;
                        default:
#if DEBUG
                            players.Add(new StrategyPlayer(name, $"../../../../Trains/Player/Strategy/{strat}.cs", _map));
#else
                            players.Add(new StrategyPlayer(name, $"../Trains/src/Trains/Player/Strategy/{strat}.cs", _map));
#endif
                            break;
                    }
                }
            }
            return players;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
