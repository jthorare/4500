using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Trains.Common.Map;

namespace Trains.Common.GameState.Json
{
    [JsonConverter(typeof(AcquiredJsonConverter))]
    public sealed class AcquiredJson : IComparable
    {
        public string City1 { get; set; }
        public string City2 { get; set; }
        public Color Color { get; set; }
        public uint Length { get; set; }

        public static AcquiredJson ConvertFromConnection(Connection connection)
        {
            var sorted = new List<Location>(connection.Locations);
            sorted.Sort();
            return new AcquiredJson
            {
                City1 = sorted.First().Name,
                City2 = sorted.Last().Name,
                Color = connection.Color,
                Length = connection.Segments
            };
        }

        public int CompareTo(object? obj)
        {
            if (obj is not AcquiredJson other)
            {
                throw new ArgumentException("Cannot compare AcquireJson to non-AcquiredJson");
            }

            var compare = string.CompareOrdinal(City1, other.City1);
            
            // Break tie
            if (compare == 0)
            {
                compare = string.CompareOrdinal(City2, other.City2);
            }

            return compare;
        }
    }
}
