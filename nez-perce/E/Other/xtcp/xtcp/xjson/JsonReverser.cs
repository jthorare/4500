using System;
using System.Linq;
using System.Text.Json;

namespace xtcp.xjson
{
    public static class JsonReverser
    {

        public static void Reverse(Utf8JsonWriter writer, JsonProperty element)
        {
            switch (element.Value.ValueKind)
            {
                case JsonValueKind.Undefined:
                    break;
                case JsonValueKind.Object:
                    writer.WriteStartObject(element.Name);
                    foreach (var e in element.Value.EnumerateObject()) { 
                        Reverse(writer, e);
                    }
                    writer.WriteEndObject();
                    break;
                case JsonValueKind.Array:
                    writer.WriteStartArray(element.Name);
                    foreach (var e in element.Value.EnumerateArray().Reverse())
                    {
                        ReverseElement(writer, e);
                    }
                    writer.WriteEndArray();
                    break;
                case JsonValueKind.String:
                    writer.WriteString(element.Name, ReverseString(element.Value.GetString()!));
                    break;
                case JsonValueKind.Number:
                    writer.WriteNumber(element.Name, ReverseFloat(element.Value.GetDouble()!));
                    break;
                case JsonValueKind.True:
                    writer.WriteBoolean(element.Name, false);
                    break;
                case JsonValueKind.False:
                    writer.WriteBoolean(element.Name, true);
                    break;
                case JsonValueKind.Null:
                    writer.WriteNull(element.Name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void ReverseElement(Utf8JsonWriter writer, JsonElement element) {
            switch (element.ValueKind)
            {
                case JsonValueKind.Undefined:
                    break;
                case JsonValueKind.Object:
                    writer.WriteStartObject();
                    foreach (var e in element.EnumerateObject())
                    {
                        Reverse(writer, e);
                    }
                    writer.WriteEndObject();
                    break;
                case JsonValueKind.Array:
                    writer.WriteStartArray();
                    foreach (var e in element.EnumerateArray().Reverse())
                    {
                        ReverseElement(writer, e);
                    }
                    writer.WriteEndArray();
                    break;
                case JsonValueKind.String:
                    writer.WriteStringValue(ReverseString(element.GetString()!));
                    break;
                case JsonValueKind.Number:
                    writer.WriteNumberValue(ReverseFloat(element.GetDouble()!));
                    break;
                case JsonValueKind.True:
                    writer.WriteBooleanValue(false);
                    break;
                case JsonValueKind.False:
                    writer.WriteBooleanValue(true);
                    break;
                case JsonValueKind.Null:
                    writer.WriteNullValue();
                    break;
            }
        }

        public static string ReverseString(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        public static long ReverseInt(long number)
        {
            return -number;
        }

        public static double ReverseFloat(double number)
        {
            return -number;
        }
    }
}