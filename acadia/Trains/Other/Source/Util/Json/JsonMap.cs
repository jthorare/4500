using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;

namespace Trains.Util.Json
{
    /// <summary>
    /// Class representing a Map object as specified by the specifications at
    /// https://www.ccs.neu.edu/home/matthias/4500-f21/3.html
    /// </summary>
    public class JsonMap
    {
        // JsonProperty(<prop_name>)] is a specifier for the Newtonsoft.Json Library that
        // indicates deserializing and serializing should use this property
        // when it encounters a property name that matches the <prop_name>

        /// <summary>
        /// Represents the Width of a Trains.Models.GamePieces.Map.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; }

        /// <summary>
        /// Represents the Height of a Trains.Models.GamePieces.Map.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; }

        /// <summary>
        /// Represents the Cities of a Trains.Models.GamePieces.Map.
        /// </summary>
        [JsonProperty("cities")]
        [JsonConverter(typeof(XmapCityConverter))]
        public ICollection<City> Cities { get; }

        /// <summary>
        /// The "connections" property of an Xmap Json is an Object composed of an undefined number of string : Target[] pairs.
        /// The Target array is represented as a Collection of Target.
        /// 
        /// A Target is represented by a Dictionary<string, Dictionary<string,int>> where the key is a string
        /// representing a Trains.Models.GamePieces.City's name and the value is a Segment.
        /// 
        /// A Segment is represented by a Dictionary<string,int> where the key is the string representing
        /// the Segment's Color and the value is the Segment's length as an int
        /// </summary>
        [JsonProperty("connections")]
        public IDictionary<string, Dictionary<string, Dictionary<string, int>>> ConnectionsDictionary { get; }

        /// <summary>
        /// Constructor for a JsonMap.
        /// </summary>
        /// <param name="width">The Width of the Trains.Models.GamePieces.Map this JsonMap represents</param>
        /// <param name="height">The Height of the Trains.Models.GamePieces.Map this JsonMap represents</param>
        /// <param name="cities">The Collection of the Trains.Models.GamePieces.City for the Trains.Models.GamePieces.Map this JsonMap represents</param>
        /// <param name="connectionsDictionary">A Dictionary representing the Collection of Trains.Models.GamePieces.Connection for the Trains.Models.GamePieces.Map this JsonMap represents</param>
        public JsonMap(int width, int height, ICollection<City> cities, IDictionary<string, Dictionary<string, Dictionary<string, int>>> connectionsDictionary)
        {
            Width = width;
            Height = height;
            Cities = cities;
            ConnectionsDictionary = connectionsDictionary;
        }

        /// <summary>
        /// Creates a Trains.Models.GamePieces.Map that this JsonMap represents.
        /// </summary>
        /// <returns>A Trains.Models.GamePieces.Map that this JsonMap represents</returns>
        public Map ToMap()
        {
            Map map = new Map(Cities, this.ToTrainsConnections(), Width, Height);
            return map;
        }

        /// <summary>
        /// Converts this JsonMap's IDictionary<string,Target> ConnectionsDictionary into a Collection of Trains.Models.GamePieces.Connection.
        /// </summary>
        /// <returns>A Collection of Trains.Models.GamePieces.Connection that this JsonMap's IDictionary<string,Target> ConnectionsDictionary represents</returns>
        public ICollection<Connection> ToTrainsConnections()
        {
            ICollection<Connection> connections = new HashSet<Connection>();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, int>>> jsonConnection in ConnectionsDictionary)
            {
                City city1 = Cities.Where(x => x.CityName == jsonConnection.Key).FirstOrDefault(); // City name's are unique and a JsonConnection's City name must exist in the JsonMap's list of Cities
                foreach (KeyValuePair<string, Dictionary<string, int>> target in jsonConnection.Value) // iterate through the list of Target's
                {
                    City city2 = Cities.Where(x => x.CityName == target.Key).FirstOrDefault(); // get the City that the Target refers to
                    foreach (KeyValuePair<string, int> segment in target.Value) // iterate over the possible Segment objects that connect city1 and city2
                    {
                        Connection connection = new Connection(city1, city2 , Utilities.ToColor(segment.Key), Utilities.ToLength(segment.Value));
                        connections.Add(connection);
                    }
                }
            }
            return connections;
        }

        /// <summary>
        /// Determines if the two City's referred to by the City1 and City2 strings are a valid destination in a Trains.Models.GamePieces.Map constructed from this JsonMap.
        /// Two City are a destination if there exists a route in a Trains.Models.GamePieces.Map connecting the two cities.
        /// </summary>
        /// <returns>A bool representing if this JsonInput represents a Trains.Models.GamePieces.Map where the two named City by City1 and City2 are connected.</returns>
        public bool IsDestination(string[] cityNames)
        {
            Map map = this.ToMap();
            City city1 = map.Cities.Where(city => city.CityName == cityNames[0]).FirstOrDefault(); // Get the City named by cityNames[0]
            City city2 = map.Cities.Where(city => city.CityName == cityNames[1]).FirstOrDefault(); // Get the City named by cityNames[1]
            if (city1 == null || city2 == null) // check that City objects were retrieved
            {
                throw new ArgumentException($"One of the Cities is invalid:\nCity 1 {city1}\n City 2 {city2}");
            }

            ICollection<Destination> destinations = map.AllFeasibleDestinations(); // Determine all feasible destinations of the Map
            return destinations.Where(dest =>
            (dest.CityOne.Equals(city1) || dest.CityOne.Equals(city2)) && (dest.CityTwo.Equals(city1) || dest.CityTwo.Equals(city2)))
                .Count() > 0; // Return whether there exists a destination pair that includes the two City named in this JsonInput
        }

        
    }
}
