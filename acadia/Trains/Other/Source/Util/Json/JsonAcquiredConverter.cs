using Newtonsoft.Json;
using System;

namespace Trains.Util.Json
{
    /// <summary>
    /// Class for converting a https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29 into a JsonAcquired object.
    /// </summary>
    public class JsonAcquiredConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            int arrayDepth = 0;
            int processedVals = 0;
            object[] acquiredJson = new object[Constants.jsonAcquiredSize];
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
                if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer && processedVals < Constants.jsonAcquiredSize)
                {
                    acquiredJson[processedVals] = reader.Value;
                    processedVals++;
                }
            }
            return new JsonAcquired((string)acquiredJson[0], (string)acquiredJson[1], (string)acquiredJson[2], Convert.ToInt32(acquiredJson[3]));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
