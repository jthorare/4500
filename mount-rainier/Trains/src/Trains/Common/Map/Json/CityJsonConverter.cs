using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("Trains.Tests.Common")]

namespace Trains.Common.Map.Json
{
    /// <summary>
    /// Converter to assist in seralizing/deserializing a CityJson.
    /// </summary>
    internal sealed class CityJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            CityJson cityJson = (value as CityJson)!;
            writer.WriteStartArray();
            writer.WriteValue(cityJson.Name);
            writer.WriteStartArray();
            writer.WriteValue(cityJson.X);
            writer.WriteValue(cityJson.Y);
            writer.WriteEndArray();
            writer.WriteEndArray();
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Malformed Location Json.");

            var name = reader.ReadAsString()!;
            if (reader.Read() && reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Malformed Location Json.");
            var x = (int)reader.ReadAsInt32()!;
            var y = (int)reader.ReadAsInt32()!;

            if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
                throw new JsonException("Location Json has too many position elements.");

            if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
                throw new JsonException("Location Json has too many elements.");

            return new CityJson
            {
                Name = name,
                X = (uint)x,
                Y = (uint)y
            };
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CityJson);
        }
    }
}