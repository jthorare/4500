using System;
using System.Drawing;
using Newtonsoft.Json;

namespace Trains.Common.Map.Json
{
    /// <summary>
    /// Converter to assist in seralizing/deserializing a TrainsMap
    /// </summary>
    internal sealed class TrainsMapJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            var mapJson = serializer.Deserialize<MapJson>(reader)!;
            var trainsMapBuilder = new TrainsMap.TrainsMapBuilder();
            var width = mapJson.Width;
            var height = mapJson.Height;

            foreach (var city in mapJson.Cities!)
            {
                trainsMapBuilder.AddLocation(city.Name!, (float)city.X / width, (float)city.Y / height);
            }

            foreach (var (location1, targetJson) in mapJson.Connections!)
            {
                foreach (var (location2, segmentJson) in targetJson)
                {
                    foreach (var (color, segments) in segmentJson)
                    {
                        trainsMapBuilder.AddConnection(location1, location2, segments, color);
                    }
                }
            }

            return trainsMapBuilder.BuildMap();
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TrainsMap);
        }
    }
}