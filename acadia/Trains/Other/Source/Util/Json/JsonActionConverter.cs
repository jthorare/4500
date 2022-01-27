using Newtonsoft.Json;
using System;
using Trains.Models.TurnTypes;

namespace Trains.Util.Json
{
    public class JsonActionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITurn);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            Type actionType = value.GetType();
            if (actionType == typeof(DrawCardsTurn))
            {
                writer.WriteValue("more cards");
            } else
            {
                AcquireConnectionTurn action = value as AcquireConnectionTurn;
                writer.WriteStartArray();
                writer.WriteValue(action.ToAcquire.City1.CityName);
                writer.WriteValue(action.ToAcquire.City2.CityName);
                writer.WriteValue(action.ToAcquire.Color.ToString().ToLower());
                writer.WriteValue((int) action.ToAcquire.NumSegments);
                writer.WriteEndArray();
            }
        }
    }
}
