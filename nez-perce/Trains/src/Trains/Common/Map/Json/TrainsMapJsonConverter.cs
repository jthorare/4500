using System;
using Newtonsoft.Json;

namespace Trains.Common.Map.Json
{
    /// <summary>
    /// Converter to assist in seralizing/deserializing a TrainsMap
    /// </summary>
    public sealed class TrainsMapJsonConverter : JsonConverter<TrainsMap>
    {
        public static uint? ContextHeight { get; set; } = Constants.DefaultMapHeight;
        public static uint? ContextWidth { get; set; } = Constants.DefaultMapHeight;
        public override void WriteJson(JsonWriter writer, TrainsMap? value, JsonSerializer serializer)
        {
            if (ContextWidth == null || ContextHeight == null)
            {
                throw new JsonException("Please set TrainsMapJsonConverter.ContextWidth and TrainsMapJsonConverter.ContextHeight before Serializing a TrainsMap.");
            }
            var mapJson = MapJson.ConvertFromTrainsMap(value!, (uint)ContextWidth, (uint)ContextHeight);

            serializer.Serialize(writer, mapJson);
        }


        public override TrainsMap? ReadJson(JsonReader reader, Type objectType, TrainsMap? existingValue, bool hasExistingValue,
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
    }
}