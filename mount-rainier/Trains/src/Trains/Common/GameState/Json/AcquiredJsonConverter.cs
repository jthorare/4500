using System;
using Newtonsoft.Json;
using Trains.Common.Map;

namespace Trains.Common.GameState.Json
{
    internal class AcquiredJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AcquiredJson);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Malformed Acquired Json.");

            var city1 = reader.ReadAsString()!;
            var city2 = reader.ReadAsString()!;
            reader.Read();
            var color = serializer.Deserialize<Color>(reader);
            var length = (uint)reader.ReadAsInt32()!;

            if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
                throw new JsonException("Acquire Json has too many elements.");

            return new AcquiredJson
            {
                City1 = city1,
                City2 = city2,
                Color = color,
                Length = length
            };
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            AcquiredJson acquiredJson = (value as AcquiredJson)!;
            
            writer.WriteStartArray();
            serializer.Serialize(writer, acquiredJson.City1);
            serializer.Serialize(writer, acquiredJson.City2);
            serializer.Serialize(writer, acquiredJson.Color);
            serializer.Serialize(writer, acquiredJson.Length);
            writer.WriteEndArray();
        }
    }
}
