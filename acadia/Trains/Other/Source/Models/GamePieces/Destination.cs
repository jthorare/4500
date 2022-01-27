using System;
using System.Linq;
using System.Collections.Generic;
using Trains.Util;
using Trains.Util.Comparers;

namespace Trains.Models.GamePieces
{
    /// <summary>
    /// A representation for a destination in Trains.com; a pair of Cities with a path between them on a Map.
    /// </summary>
    public class Destination
    {
        /// <summary>
        ///  A City on one end of this Destination.
        /// </summary>
        public City CityOne { get; }

        /// <summary>
        ///  The City on the other end of this Destination.
        /// </summary>
        public City CityTwo { get; }

        /// <summary>
        /// Constructor for a Destination. Requires that the two given Cities are part of the given Map.
        /// </summary>
        /// <param name="gameMap">The Map that the destination being constructed is a part of.</param>
        /// <param name="A">A City on one end of the Destination being constructed. Must be a City on gameMap.</param>
        /// <param name="B">The City on the other end of the Destination being constructed. Must be a City on gameMap.</param>
        public Destination(Map gameMap, City A, City B)
        {
            bool citiesExistInMap = gameMap.Cities.Contains(A) && gameMap.Cities.Contains(B);
            // Mutated destinations collection is irrelevant in this use case
            bool pathExistsBetweenCities = Utilities.PathExistsAmongConnections(
                gameMap.Connections, A, B, new HashSet<City>() { A }, null, null);
            if (citiesExistInMap && pathExistsBetweenCities)
            {
                ICollection<City> orderedCities = Utilities.OrderByComparer(new List<City> { A, B }, new LexicoCityComparer());
                CityOne = orderedCities.ElementAt(0);
                CityTwo = orderedCities.ElementAt(1);
            }
            else
            {
                throw new ArgumentException("A Destination can't be made with a city that's not on the map or with cities that are not connected by a route on the map.");
            }
        }

        /// <summary>
        /// Determines equality between this Destination and another Object. Within the scope of Trains.com,
        /// we define two Destination to be equal if they share the same two City and Map.
        /// </summary>
        /// <param name="obj">The Object to determine equality with.</param>
        /// <returns>Whether this Connection is equal to the given Object.</returns>
        public override bool Equals(Object obj)
        {
            // Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Destination d = (Destination)obj;
                return (CityOne.Equals(d.CityOne) || CityOne.Equals(d.CityTwo)) &&
                    (CityTwo.Equals(d.CityOne) || CityTwo.Equals(d.CityTwo));
            }
        }

        /// <summary>
        /// Determines the hash code for a Destination.
        /// </summary>
        /// <returns>An int representing the hash code of this Connection.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.CityOne, this.CityTwo);
        }

        public override string ToString()
        {
            return $"City 1: {CityOne.CityName} City 2: {CityTwo.CityName}";
        }
    }
}
