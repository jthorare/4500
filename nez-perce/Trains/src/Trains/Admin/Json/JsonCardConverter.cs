using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common.Map;

namespace Trains.Admin.Json
{
    public class JsonCardConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            List<Color> cards = new();
            string name = "";
            string filePath = "../Trains/src/Trains/Player/Strategy/";

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                {
                    break;
                }
                if (reader.TokenType == JsonToken.StartArray)
                {
                    continue;
                }
                if (reader.TokenType == JsonToken.String)
                {
                    switch (reader.Value)
                    {
                        case "red":
                            cards.Add(Color.Red);
                            break;
                        case "blue":
                            cards.Add(Color.Blue);
                            break;
                        case "green":
                            cards.Add(Color.Green);
                            break;
                        case "white":
                            cards.Add(Color.White);
                            break;
                    }
                }
            }
            return cards;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
