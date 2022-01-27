using System;
using Newtonsoft.Json;

namespace Trains.Common.Map.Json
{
    internal class DestinationJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Destination);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Malformed Destination Json.");

            string city1 = reader.ReadAsString()!;
            string city2 = reader.ReadAsString()!;

            if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
                throw new JsonException("Acquire Json has too many elements.");

            return new Destination(city1, city2);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            Destination destination = (value as Destination)!;
            writer.WriteStartArray();
            foreach (var city in destination)
            {
                writer.WriteValue(city);
            }
            writer.WriteEndArray();
        }
    }
}
