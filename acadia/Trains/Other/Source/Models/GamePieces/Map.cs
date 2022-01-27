using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Util;

namespace Trains.Models.GamePieces
{
    /// <summary>
    /// This class represents the Map used in Trains.com.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// An immutable ICollection of all Connection on the Map that does not contain any two equal Connection. We use ICollection because it
        /// is provided common operations in System.Linq and it allows for HashSet's internally that have O(1) operation to determine if an object is present in the collection.
        /// Accessible using MapObject.Connections.
        /// </summary>
        public ICollection<Connection> Connections { get; }

        /// <summary>
        /// An immutable ICollection of all City on the Map that does not contain any duplicate City.
        /// We use ICollection because it is provided common operations in System.Linq and it allows for HashSet's internally that have O(1) operation
        /// to determine if an object is present in the collection.
        /// Accessible using MapObject.Cities.
        /// </summary>
        public ICollection<City> Cities { get; }

        /// <summary>
        /// An immutable non-negative int representing the Width of this Map in pixels for visualization.
        /// Accessible using MapObject.Width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// An immutable non-negative int representing the Height of this Map in pixels for visualization.
        /// Accessible using MapObject.Height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Constructor used to create a Map.
        /// </summary>
        /// <param name="cities">A IEnumerable of City to add to this Map (possibly contains duplicates)</param>
        /// <param name="connections">A IEnumerable of Connection to add to this Map (possible contains duplicates)</param>
        /// <param name="width">An int representing the width of this Map (must be non-negative)</param>
        /// <param name="height">An int representing the height of this Map (must be non-negative)</param>
        public Map(ICollection<City> mapCities, ICollection<Connection> mapConnections, int mapWidth, int mapHeight)
        {
            if ((mapWidth < 10) || (mapWidth > 800))
            {
                throw new ArgumentException("Width must be within [10,800].");
            }
            else
            {
                Width = mapWidth;
            }
            if ((mapHeight < 10) || (mapHeight > 800))
            {
                throw new ArgumentException("Height must be within [10,800].");
            }
            else
            {
                Height = mapHeight;
            }
            if (mapCities == null)
            {
                throw new ArgumentNullException("A Map cannot have a null reference for a ICollection of City.");
            }
            Cities = new HashSet<City>();
            AddNonDuplicates(mapCities, Cities);
            AllCityBoundsValid();
            if (mapConnections == null)
            {
                throw new ArgumentNullException("A Map cannot have a null reference for a ICollection of Connection.");
            }
            Connections = new HashSet<Connection>();
            AddNonDuplicates(mapConnections, Connections);

        }

        /// <summary>
        /// Yields the names of all cities on this Map.
        /// </summary>
        /// <returns>A Collection of strings where each element is a name of a City on this Map</returns>
        public ICollection<string> AllCityNames()
        {
            ICollection<string> cityNames = new HashSet<string>();
            foreach (City city in Cities)
            {
                cityNames.Add(city.CityName);
            }
            return cityNames;
        }

        /// <summary>
        /// Calculates all possible destinations for this Map. A destination is an ICollection of 2 City,
        /// represented as a HashSet, that are connected to each other by a number >= 1 of Connection.
        /// </summary>
        /// <returns>Returns the ICollection of all possible destinations on this Map.</returns>
        public ICollection<Destination> AllFeasibleDestinations()
        {
            ICollection<Destination> destinations = new HashSet<Destination>();
            for (int src = 0; src < Cities.Count; src++) // for every City in this Map
            {
                City start = Cities.ElementAt(src);
                for (int dest = src + 1; dest < Cities.Count; dest++) // Check for every other City if there exists a path between it and the src City
                {
                    City end = Cities.ElementAt(dest);
                    bool destinationExists = CheckDestinationExists(start, end, destinations);
                    ICollection<City> visitedCities = new HashSet<City>() { start };
                    bool pathExists = Utilities.PathExistsAmongConnections(this.Connections, start, end, visitedCities, this, destinations);
                    if (!destinationExists && pathExists)
                    {
                        destinations.Add(new Destination(this, start, end));
                    }
                }
            }
            return destinations;
        }

        /// <summary>
        /// Determines whether a destination consisting of City start and City end exists in the given collection of destinations destinations.
        /// </summary>
        /// <param name="start">One endpoint of the destination being considered.</param>
        /// <param name="end">The other endpoint of the destination being considered.</param>
        /// <param name="destinations">The set of destinations </param>
        /// <returns></returns>
        public bool CheckDestinationExists(City start, City end, ICollection<Destination> destinations)
        {
            bool rv = false;
            foreach (Destination destination in destinations)
            {
                if ((destination.CityOne.Equals(start) || destination.CityTwo.Equals(start)) &&
                    (destination.CityOne.Equals(end) || destination.CityTwo.Equals(end)))
                {
                    rv = true;
                }
            }
            return rv;
        }

        /// <summary>
        /// Verifies that all City in this Map's Cities is within this Map's bounds.
        /// </summary>
        private void AllCityBoundsValid()
        {
            foreach (City city in Cities)
            {
                if (city.XPosition > Width)
                {
                    throw new ArgumentException($"City {city} has a X value, {city.XPosition}, that is greater this Map's width, {Width}.");
                }
                if (city.YPosition > Height)
                {
                    throw new ArgumentException($"City {city} has a Y value, {city.YPosition}, that is out of this Map's height, {Height}.");
                }
            }
        }
        /// <summary>
        /// Add all of the non-duplicate possible Object to the current ICollection of Object.
        /// Throws ArgumentException if a duplicate is detected.
        /// </summary>
        /// <param name="possible">The ICollection of Objects that is being checked for duplicates.</param>
        /// <param name="current">The ICollection of Objects that is having all non-duplicate Objects from possible added to it.</param>
        private void AddNonDuplicates<T>(ICollection<T> possible, ICollection<T> current)
        {
            foreach (T toAdd in possible) // iterate over all potential objects to add
            {
                bool duplicate = false; // represents if this toAdd is a duplicate
                foreach (T added in current) // iterate over all existing non-duplicate objects
                {
                    if (toAdd.Equals(added)) // if the two objects are equal according to Equals()
                    {
                        duplicate = true; // set duplicate flag as true
                    }
                }
                if (!duplicate) // if the toAdd object is not equal to (i.e. a duplicate of) any object current has
                {
                    current.Add(toAdd); // add it to this Map's ICollection of Connection
                }
                else
                {
                    throw new ArgumentException($"Duplicate detected {toAdd}");
                }
            }
        }

        /// <summary>
        /// Determines equality between this Map and another Object. We define two Map to be equal if they have the same
        /// collection of cities, the same collection of connections, the same width, and the same height.
        /// </summary>
        /// <param name="obj">The Object to determine equality with.</param>
        /// <returns>Whether this Map is equal to the given Object.</returns>
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Map m = (Map)obj;
                bool equal = ((this.Width == m.Width) && (this.Height == m.Height));

                foreach (City c in m.Cities)
                {
                    equal = equal && (c.Equals(this.Cities.FirstOrDefault(city => city.CityName == c.CityName || (city.XPosition == c.XPosition && city.YPosition == c.YPosition))));
                }
                foreach (Connection conn in m.Connections)
                {
                    equal = equal &&
                        (conn.Equals(Connections.FirstOrDefault(connection => connection.Color == conn.Color &&
                                    (conn.City1.Equals(connection.City1) || conn.City1.Equals(connection.City2)) && (conn.City2.Equals(connection.City1) || conn.City2.Equals(connection.City2)))));
                }
                return equal;
            }
        }

        /// <summary>
        /// Determines the hash code for this Map.
        /// </summary>
        /// <returns>An int representing the hash code of this Map.</returns>
        public override int GetHashCode()
        {
            int rv = HashCode.Combine(this.Height, this.Width);
            foreach (City c in this.Cities)
            {
                rv = HashCode.Combine(c, rv);
            }
            foreach (Connection c in this.Connections)
            {
                rv = HashCode.Combine(c, rv);
            }
            return rv;
        }
    }
}
