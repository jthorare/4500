using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Trains.Common.Map
{
    public sealed partial class TrainsMap
    {
        /// <summary>
        /// A Builder class for making TrainMaps ensuring that all invariants about the construction of a Map are held.
        /// </summary>
        public sealed class TrainsMapBuilder
        {
            /// <summary>
            /// A dictionary of Locations with their names as keys. Passed along to construct a Map.
            /// </summary>
            private Dictionary<string, Location> Locations { get; }

            /// <summary>
            /// A List of Connections to be passed along to construct a Map.
            /// </summary>
            private List<Connection> Connections { get; }

            public TrainsMapBuilder()
            {
                Connections = new List<Connection>();
                Locations = new Dictionary<string, Location>();
            }

            /// <summary>
            /// Specify a Location to be added to the map.
            /// </summary>
            /// <param name="locationName">The name of the location.</param>
            /// <param name="x">The normalized x coordinate of the location.</param>
            /// <param name="y">The normalized y coordinate of the location.</param>
            /// <exception cref="ArgumentException">If the specified location is already added.</exception>
            public void AddLocation(string locationName, float x, float y)
            {
                if (Locations.Keys.Contains(locationName))
                {
                    throw new ArgumentException("Cannot add location as location already exists.");
                }

                Locations.Add(locationName, new Location(locationName, x, y));
            }

            /// <summary>
            /// Specify a Connection to be added to the map.
            /// <remarks>Referenced locations must first be added using <c>AddLocation()</c></remarks>
            /// </summary>
            /// <param name="locationName1">The name of the first Location</param>
            /// <param name="locationName2">The name of the second Location</param>
            /// <param name="segments">The number of segments </param>
            /// <param name="color"></param>
            /// <exception cref="ArgumentException"></exception>
            public void AddConnection(string locationName1, string locationName2, uint segments,
                Color color)
            {
                if (!Locations.Keys.Contains(locationName1) || !Locations.Keys.Contains(locationName2))
                {
                    throw new ArgumentException("Cannot add connection as location does not exist.");
                }

                Connection connection = new(Locations[locationName1], Locations[locationName2],
                    segments, color);

                if (Connections.Contains(connection))
                {
                    // Do nothing as the connection already exists.
                    return;
                }

                Connections.Add(connection);
                Locations[locationName1].AddConnection(connection);
                Locations[locationName2].AddConnection(connection);
            }

            /// <summary>
            /// Construct a Map from the supplied Locations and Connections.
            /// </summary>
            /// <returns>The Map representing the supplied Locations and Connections</returns>
            public TrainsMap BuildMap()
            {
                List<Connection> connections = Connections.ToList();

                Dictionary<string, Location> locations = new();
                foreach (var (name, location) in Locations)
                {
                    locations.Add(name, location);
                }

                return new TrainsMap(locations, connections);
            }
        }
    }
}