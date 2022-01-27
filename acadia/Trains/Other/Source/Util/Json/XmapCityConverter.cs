using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Trains.Models.GamePieces;

namespace Trains.Util.Json
{
    /// <summary>
    /// A custom JSON converter that converts a city represented by specifications in
    /// https://www.ccs.neu.edu/home/matthias/4500-f21/3.html to a Trains.Models.GamePieces.City.
    /// </summary>
    public class XmapCityConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            ICollection<City> cities = new HashSet<City>();
            int x = 0;
            int y = 0;
            string name = "";
            int prop = 0;
            int array = 1;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                {
                    x = 0;
                    y = 0;
                    name = "";
                    prop = 0;
                    array--;
                    if (array == 0)
                    {
                        break;
                    }
                    continue;
                }
                if (reader.TokenType == JsonToken.StartArray)
                {
                    array++;
                    continue;
                }

                if (prop == 0)
                {
                    name = (string)reader.Value;
                    prop++;
                }
                else if (prop == 1)
                {
                    x = Convert.ToInt32(reader.Value);
                    prop++;
                }
                else if (prop == 2)
                {
                    y = Convert.ToInt32(reader.Value);
                    prop++;
                    cities.Add(new City(x, y, name));
                }
            }
            return cities;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
