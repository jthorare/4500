using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace xgui.Models
{ 
    /// <summary>
    /// A Json converter that converts between JSON and Posn. A Posn is represented in JSON as the array of numbers [X, Y] where
    /// X is the x-coordinate and Y is the y-coordinate of the point.
    /// </summary>
    internal sealed class PosnJsonConverter : JsonConverter<Posn>
    {
        public override Posn Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Malformed Json Posn.");
            
            var posn = new Posn();
            reader.Read();
            posn.X = reader.GetUInt32();
            reader.Read();
            posn.Y = reader.GetUInt32();

            if (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException("Json Posn has too many elements.");
            
            return posn;
        }

        public override void Write(Utf8JsonWriter writer, Posn value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
    }
}