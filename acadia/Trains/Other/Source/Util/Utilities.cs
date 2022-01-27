using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;

namespace Trains.Util
{

    /// <summary>
    /// Class representing Utility function with Trains.com
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Validate the Destination given are in the Map fo this game.
        /// </summary>
        /// <param name="map">The map to check the Destination objects against.</param>
        /// <param name="playerDestinations">The Destination objects to validate.</param>
        /// <returns>The given Destination Collection</returns>
        public static ICollection<Destination> ValidateDestinations(Map map, ICollection<Destination> playerDestinations)
        {
            if (playerDestinations.Count != Constants.setupDestinationsCount)
            {
                throw new ArgumentException($"A Player must have {Constants.setupDestinationsCount} destinations");
            }
            else
            {
                foreach (Destination dest in playerDestinations)
                {
                    if (!map.Cities.Contains(dest.CityOne) || !map.Cities.Contains(dest.CityTwo))
                    {
                        throw new ArgumentException($"All destination cities must exist in the map. Destination ({dest.CityOne}, {dest.CityTwo}) is invalid.");
                    }
                }
            }
            return playerDestinations;
        }

        /// <summary>
        /// Return the number of cards of the desired color that this player has remaining, as an integer >= 0.
        /// </summary>
        /// <param name="desiredColor">The color of card to look for.</param>
        /// <returns></returns>
        public static int GetCardsOfColor(IList<ColoredCard> cards, GamePieceColor desiredColor)
        {
            int count = cards.Where(card => card.Color == desiredColor).Count();
            return count;
        }

        /// <summary>
        /// Reads in input from STDIN into a string, one character at a time.
        /// </summary>
        /// <returns>A string contaning the STDIN data.</returns>
        public static string ReadInput()
        {
            StreamReader sr = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(sr);
            string input = ""; // read in the input JSON.
            while (!sr.EndOfStream)
            {
                input += (char)sr.Read();
            }
            return input;
        }

        /// <summary>
        /// Determines whether the requested action is legal according to the rules with respect to this map, and the given state. A move is legal if the PlayerGameState has at least
        /// as many rails as the Connection has segments and the Player has at least as many cards of the same Color as the Connection.Color as the Connection has segments.
        /// </summary>
        /// <param name="state">The state of the active player requesting an action, and as much as it can know about the remaining players.</param>
        /// <param name="desiredConn">The Connection the active players wishes to acquire.</param>
        /// <returns>True if the requested action is legal, false otherwise.</returns>
        public static bool IsLegalMove(PlayerGameState state, Connection desiredConn)
        {
            return Utilities.GetCardsOfColor(state.Cards, desiredConn.Color) >= (int)desiredConn.NumSegments && state.Rails >= (int)desiredConn.NumSegments
                && state.AvailableConnections.Contains(desiredConn);
        }

        /// <summary>
        /// Parse the input string starting with a JSON object and return its component JSON object, as well as the input string without said component JSON object.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <returns>A new string array of size 2 where the first string is the component JSON object and the second string
        /// is the input string with the component JSON object removed.</returns>
        public static string[] ExtractObject(string input)
        {
            int[] objBounds = GetFirstObjectBounds(input);
            int objStart = objBounds[0];
            int objEnd = objBounds[1];
            return SplitStringExcludingIndices(input, objStart, objEnd);
        }

        /// <summary>
        /// Splits the given string into two strings, where the first string starts directly after the start index and ends directly before the end index.
        /// The second string is the remainder of the string.
        /// </summary>
        /// <param name="input">The string to split.</param>
        /// <param name="startIndex">The starting index of the first string..</param>
        /// <param name="endIndex">The ending index of the first string.</param>
        /// <returns></returns>
        private static string[] SplitStringExcludingIndices(string input, int startIndex, int endIndex)
        {
            const int splitSize = 2;
            return new string[splitSize] { input.Substring(startIndex, endIndex - startIndex + 1), input.Remove(startIndex, endIndex - startIndex + 1) };
        }

        /// <summary>
        /// Returns the start and end positions of the first JSON object in the given input string.
        /// </summary>
        /// <param name="input">A string representation of a JSON with at least one object.</param>
        /// <returns>The start and end position of the first JSON object in an array.</returns>
        public static int[] GetFirstObjectBounds(string input)
        {
            // each variable corresponds to the start and end index, respectively,
            // of the JSON map object within input (for use in substring to extract it)
            int objStart = 0;
            int objEnd = 0;

            // JSON object has nested objects, keeps track of when we have exited the main object, rather than a nested one.
            int objDepth = 0;
            for (int ii = 0; ii < input.Length; ++ii)
            {
                char curr = input[ii];
                // increase object depth if we get to the string equivalent of a JSON StartObject
                if (curr.Equals('{'))
                {
                    if (objDepth == 0) // if entering main object, store start index of object
                    {
                        objStart = ii;
                    }
                    objDepth++;
                }
                // decrease object depth if we get to the string equivalent of a JSON EndObject
                if (curr.Equals('}'))
                {
                    objDepth--;
                    if (objDepth == 0) // if exiting main object, store end index of object
                    {
                        objEnd = ii;
                        break;
                    }
                }
            }
            return new int[2] { objStart, objEnd };
        }

        /// <summary>
        /// Determines if a path between City start and end exists in the given Collection of Connection using a Depth First Search approach.
        /// Accumulates Connection with start as destinations for efficiency in accumulating destinations.
        /// </summary>
        /// <param name="connections">A Collection of the Connection to consider in determining path existence.</param>
        /// <param name="start">The City on one end of the path in question.</param>
        /// <param name="end">The City on the other end of the path in question.</param>
        /// <param name="visitedCities">A Collection of the City that have already been considered during DFS.</param>
        /// <param name="map">The map that destinations belong to. Null if not used.</param>
        /// <param name="destinations">A Collection of destinations to add direct start Connection to. Null if not used.</param>
        /// <returns>True if a route exists between the two given City among the given Connection, false otherwise.</returns>
        public static bool PathExistsAmongConnections(ICollection<Connection> connections, City start, City end, ICollection<City> visitedCities, Map map, ICollection<Destination> destinations)
        {
            // Base Case -> There exists a DIRECT Connection that contains both City start AND end.
            foreach (Connection connection in connections)
            {
                if ((connection.City1.Equals(start) || connection.City2.Equals(start)) &&
                    (connection.City1.Equals(end) || connection.City2.Equals(end)))
                {
                    return true;
                }
            }
            // Not Base Case
            // Filter all of the direct Connection from City start into a Collection 
            ICollection<Connection> startConnections = connections.Where(
                connection => connection.City1.Equals(start) || connection.City2.Equals(start)).ToList();
            if (destinations != null && map != null)
            {
                AddDirectConnectionDestinations(map, startConnections, destinations);
            }
            return RecurOnNextCity(startConnections, visitedCities, start, end, connections, map, destinations);
        }

        /// <summary>
        /// Determines whether the end City is reachable from the non-start City in any direct Connection including the start City.
        /// </summary>
        /// <param name="startConnections">Collection of all of this Map's direct Connection from City start; does not include a Connection with City end.</param>
        /// <param name="visitedCities">The Collection of City that have already been visited in this Map.</param>
        /// <param name="start">One endpoint of the destination being considered.</param>
        /// <param name="end">The other endpoint of the destination being considered.</param>
        /// <param name="allConnections">The Collection of Connection that is being considered in trying to find a path.</param>
        /// <param name="map">The Map object that destination belong to. Null if not used.</param>
        /// <param name="destinations">The Collection of already determined to exist destinations for this Map. Null if not used.</param>
        /// <returns>True if the end City is reachable from the non-start City in any direct Connection including the start City.</returns>
        private static bool RecurOnNextCity(ICollection<Connection> startConnections, ICollection<City> visitedCities,
            City start, City end, ICollection<Connection> allConnections, Map map, ICollection<Destination> destinations)
        {
            // iterate over all Connections that include Start as a City
            foreach (Connection startConnection in startConnections)
            {
                // startConnection : Cities = {start, nextCity} where nextCity != end
                City nextCity = startConnection.City1 == start ? startConnection.City2 : startConnection.City1; // get the next (non-start) city
                if (!visitedCities.Contains(nextCity))
                {
                    visitedCities.Add(nextCity);
                    if (PathExistsAmongConnections(allConnections, nextCity, end, visitedCities, map, destinations)) // if there is a path from the next city to the destination end city
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Add all direct connections in startConnections as destinations to the collection of destinations if they haven't been added already.
        /// </summary>
        /// <param name="startConnections">ICollection of direct connections from a starting city.</param>
        /// <param name="destinations">The ICollection of already determined to exist destinations for this Map.</param>
        private static void AddDirectConnectionDestinations(Map map, ICollection<Connection> startConnections, ICollection<Destination> destinations)
        {
            foreach (Connection startConnection in startConnections) // for all direct Connections from City start
            {
                // startConnection : Cities = {start, ??}
                if (!destinations.Any(dest =>
                (dest.CityOne.Equals(startConnection.City1) || dest.CityTwo.Equals(startConnection.City1)) &&
                (dest.CityOne.Equals(startConnection.City2) || dest.CityTwo.Equals(startConnection.City2))))
                {
                    // if we have come to a new destination pair
                    destinations.Add(new Destination(map, startConnection.City1, startConnection.City2));
                }
            }
        }

        /// <summary>
        /// Returns the given Collection sorted by the given Comparer<T> implementation of Compare
        /// </summary>
        /// <typeparam name="T">The type of object to be compared</typeparam>
        /// <param name="collection">The Collection to be ordered</param>
        /// <param name="comparer">The IComparer<T> to use for comparing elements of the given ICollection</param>
        /// <returns>A new ordered ICollection containing the same references as the given ICollection.</returns>
        public static ICollection<T> OrderByComparer<T>(ICollection<T> collection, IComparer<T> comparer)
        {
            return new List<T>(collection.OrderBy(obj => obj, comparer).ToList());
        }

        /// <summary>
        /// Converts the given string into a Trains.Models.GamePieces.GamePieceColor.
        /// Throws an ArgumentException if the string is not a valid color string by the Xmap JSON specification.
        /// </summary>
        /// <param name="color">A string representing a color</param>
        /// <returns>A Trains.Models.GamePieces.GamePieceColor</returns>
        public static GamePieceColor ToColor(string color)
        {
            switch (color.ToLower())
            {
                case "red":
                    return GamePieceColor.Red;
                case "blue":
                    return GamePieceColor.Blue;
                case "green":
                    return GamePieceColor.Green;
                case "white":
                    return GamePieceColor.White;
                default:
                    throw new ArgumentException($"Invalid Color string: {color}");
            }
        }

        /// <summary>
        /// Converts the given int into a Trains.Models.Connection.Length.
        /// Throws an ArgumentException if the int is not a valid option by the Xmap JSON specification.
        /// </summary>
        /// <param name="length">The int to convert</param>
        /// <returns>A Trains.Models.Connection.Length</returns>
        public static Connection.Length ToLength(int length)
        {
            switch (length)
            {
                case 3:
                    return Connection.Length.Three;
                case 4:
                    return Connection.Length.Four;
                case 5:
                    return Connection.Length.Five;
                default:
                    throw new ArgumentException($"Invalid length : {length}");
            }
        }

        /// <summary>
        /// Convert the string JSON representation of a destination into a Destination object.
        /// </summary>
        /// <param name="dest">The two strings corresponding to a destination's two city endpoints' names.</param>
        /// <param name="map">Map to consider for cities.</param>
        /// <returns></returns>
        public static Destination ToDestination(ICollection<string> dest, Map map)
        {
            string jsonCityOne = dest.ElementAt(0);
            string jsonCityTwo = dest.ElementAt(1);
            IList<City> connectionCities = Utilities.GetCitiesFromNames(map, jsonCityOne, jsonCityTwo);
            City cityOne = connectionCities[0];
            City cityTwo = connectionCities[1];
            return new Destination(map, cityOne, cityTwo);
        }

        /// <summary>
        /// Return the corresponding City objects to the given strings indicating city names from the given map.
        /// </summary>
        /// <param name="map">The map to consider for cities.</param>
        /// <param name="cityOneName">The name for one of the city names to convert.</param>
        /// <param name="cityTwoName">The name for the other city name to convert.</param>
        /// <returns>A list holding the corresponding City objects in no particular order.
        /// Throws an exception if no corresponding City exists.
        /// </returns>
        public static IList<City> GetCitiesFromNames(Map map, string cityOneName, string cityTwoName)
        {
            City cityOne = null;
            City cityTwo = null;
            foreach (City c in map.Cities)
            {
                if (c.CityName.Equals(cityOneName))
                {
                    cityOne = c;
                }
                else if (c.CityName.Equals(cityTwoName))
                {
                    cityTwo = c;
                }
            }
            if (cityOne != null && cityTwo != null)
            {
                return new List<City>() { cityOne, cityTwo };
            }
            else
            {
                throw new ArgumentException("Given city names were not present in the given map.");
            }
        }
    }
}
