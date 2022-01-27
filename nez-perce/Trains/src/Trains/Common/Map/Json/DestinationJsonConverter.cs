using System;
using System.Linq;
using MoreLinq.Extensions;
using Newtonsoft.Json;

namespace Trains.Common.Map.Json
{
    internal class DestinationJsonConverter : JsonConverter<Destination>
    {
        public override Destination? ReadJson(JsonReader reader, Type objectType, Destination? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Malformed Destination Json.");

            string city1 = reader.ReadAsString()!;
            string city2 = reader.ReadAsString()!;

            if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
                throw new JsonException("Acquire Json has too many elements.");

            return new Destination(city1, city2);
        }

        public override void WriteJson(JsonWriter writer, Destination? value, JsonSerializer serializer)
        {
            var sorted = value!.ToList();
            sorted.Sort();
            serializer.Serialize(writer, sorted);
        }
    }
}
