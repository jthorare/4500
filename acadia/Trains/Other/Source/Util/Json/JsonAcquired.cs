using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Trains.Models.GamePieces;

namespace Trains.Util.Json
{
    /// <summary>
    /// Represents the connection the active players wishes to acquire, as specified by https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29.
    /// </summary>
    [JsonConverter(typeof(JsonAcquiredConverter))]
    public class JsonAcquired
    {
        /// <summary>
        /// Represents the name of one City endpoint of a Trains.Models.GamePieces.Connection.
        /// </summary>
        public string CityNameOne { get; }

        /// <summary>
        /// Represents the name of the other City endpoint of a Trains.Models.GamePieces.Connection.
        /// </summary>
        public string CityNameTwo { get; }

        /// <summary>
        /// Represents the color of a Trains.Models.GamePieces.Connection.
        /// </summary>
        public string Color { get; }

        /// <summary>
        /// Represents the length of a Trains.Models.GamePieces.Connection.
        /// </summary>
        public int Length { get; }

        public JsonAcquired(string aName, string anotherName, string connColor, int connLength)
        {
            CityNameOne = aName;
            CityNameTwo = anotherName;
            Color = connColor;
            Length = connLength;
        }

        /// <summary>
        /// Convert this JSON representation of a Connection into a Trains.Models.GamePieces.Connection.
        /// </summary>
        /// <returns>Returns this JSON representation of a Connection as a Trains.Models.GamePieces.Connection.</returns>
        public Connection ToConnection(Map map)
        {
            IList<City> connectionCities = Utilities.GetCitiesFromNames(map, this.CityNameOne, this.CityNameTwo);
            City cityOne = connectionCities[0];
            City cityTwo = connectionCities[1];
            return new Connection(cityOne, cityTwo, Utilities.ToColor(this.Color), Utilities.ToLength(this.Length));
        }
    }
}
