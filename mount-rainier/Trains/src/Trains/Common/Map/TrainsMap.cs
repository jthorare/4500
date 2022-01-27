using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Trains.Common.Map.Json;

[assembly: InternalsVisibleTo("Trains.Tests.Common")]

namespace Trains.Common.Map
{
    /// <summary>
    /// Represents the unchanging map of a game of Trains. It keeps track of locations and connections between the
    /// locations as well as exposes queryable information about the map such as destinations between locations.
    /// The size of the map is normalized to allow for resizing of the window that keeps the relative distance between locations the same.
    /// </summary>
    [JsonConverter(typeof(TrainsMapJsonConverter))]
    public sealed partial class TrainsMap
    {
        /// <summary>
        /// Internal structure holding all connections in a List for quick lookup by index.
        /// </summary>
        private readonly List<Connection> _connections;

        /// <summary>
        /// Internal structure holding all possible destinations as sets of locations that can access each other.
        /// </summary>
        private readonly HashSet<ImmutableHashSet<string>> _destinations;

        /// <summary>
        /// Internal structure holding all locations in a Dictionary for quick lookup based on location name.
        /// </summary>
        private readonly Dictionary<string, Location> _locations;

        /// <summary>
        /// Construct a Map given a list of all Connections and Locations.
        /// </summary>
        /// <param name="locations">Locations to include in the Map</param>
        /// <param name="connections">Connections to include in the Map</param>
        /// <exception cref="ArgumentException">
        /// If any Connection or Location has references to other Locations or Connections not in the supplied lists.
        /// </exception>
        /// <remarks>
        /// Internal constructor used by a MapBuilder. Only MapBuilder should be used to create Maps.
        /// </remarks>
        private TrainsMap(Dictionary<string, Location> locations, List<Connection> connections)
        {
            // The connections should have locations that are in the supplied list
            if (connections.Any(connection => !connection.Locations.All(it => locations.Values.Contains(it))))
            {
                throw new ArgumentException($"Connection  has location not in supplied list of locations.");
            }

            // All location connections should be in the supplied list.
            foreach (var (name, location) in locations)
            {
                if (location.Connections.Any(it => !connections.Contains(it)))
                {
                    throw new ArgumentException($"Location {name} has connection not in supplied list of connections:");
                }
            }

            _connections = connections;
            _locations = locations;

            // Calculate destination sets
            _destinations = new HashSet<ImmutableHashSet<string>>();
            foreach (Location location in _locations.Values)
                // If a location has not been seen in a set of destinations
                if (!_destinations.Any(it => it.Contains(location.Name)))
                {
                    // Begin a new destination set.
                    HashSet<Location> destinations = new();
                    // Keep track of locations yet to visit in a queue starting with the first location.
                    Queue<Location> locationsToVisit = new();
                    locationsToVisit.Enqueue(location);
                    while (locationsToVisit.TryDequeue(out var currentLocation))
                    {
                        // skip the location if it has already been visited
                        if (destinations.Contains(currentLocation)) continue;

                        // Add each neighbor to the queue if it has not been seen.
                        destinations.Add(currentLocation);
                        foreach (var connection in currentLocation.Connections)
                            foreach (var neighbor in connection.Locations)
                                if (!destinations.Contains(neighbor))
                                    locationsToVisit.Enqueue(neighbor);
                    }

                    _destinations.Add(destinations.Select(it => it.Name).ToImmutableHashSet());
                }
        }

        /// <summary>
        /// All the <see cref="Location"/> on the Map stored with their names as their key for quick lookup. 
        /// </summary>
        public ImmutableDictionary<string, Location> Locations => _locations.ToImmutableDictionary();

        /// <summary>
        /// All the <see cref="Connection"/> on the map.
        /// </summary>
        public ImmutableList<Connection> Connections => _connections.ToImmutableList();

        /// <summary>
        /// All city names from the map.
        /// </summary>
        public ImmutableHashSet<string> CityNames => Locations.Keys.ToImmutableHashSet();

        /// <summary>
        /// All feasible <see cref="Destination"/> from the map.
        /// </summary>
        public ImmutableHashSet<ImmutableHashSet<string>> ConnectedLocations => _destinations.ToImmutableHashSet();

        /// <summary>
        /// Get all the Connections that link to the given Location.
        /// </summary>
        /// <param name="locationName">The unique name of the location from which to get Connections.</param>
        /// <returns>The collection of all connections that come from the Location.</returns>
        public ImmutableList<Connection> GetConnectionsFor(string locationName)
        {
            if (!_locations.Keys.Contains(locationName))
            {
                throw new ArgumentException("Location name does not exist on the map.");
            }

            return _locations[locationName].Connections.ToImmutableList();
        }

        /// <summary>
        /// Get all feasible destination names for a given location.
        /// </summary>
        /// <param name="locationName">The name of the location from which look for feasible destinations.</param>
        /// <returns>The set of feasible destination names from the given location.</returns>
        public ImmutableHashSet<string> GetConnectedLocationsFor(string locationName)
        {
            foreach (var locationSet in _destinations)
            {
                if (locationSet.Contains(locationName)) return locationSet;
            }

            throw new ArgumentException("Location name does not exist on the map.");
        }

        /// <summary>
        /// Return whether the given destination is valid on the map.
        /// </summary>
        /// <param name="location1">The first location in the destination.</param>
        /// <param name="location2">The second location in the destination.</param>
        /// <returns>Whether the destination is valid.</returns>
        public bool IsValidDestination(string location1, string location2)
        {
            if (!_locations.Keys.Contains(location1) || !_locations.Keys.Contains(location2))
            {
                throw new ArgumentException("Location name does not exist on the map.");
            }

            return GetConnectedLocationsFor(location1).Contains(location2);
        }

        /// <summary>
        /// Determines if the given <see cref="Destination"/> is made of two connected <see cref="Location"/> on this TrainsMap
        /// </summary>
        /// <param name="destination">The Destination to determine validity for</param>
        /// <returns>Whether the two <see cref="Location"/> of the given Destination make a valid Destination</returns>
        public bool IsValidDestination(Destination destination)
        {
            return IsValidDestination(destination.ElementAt(0), destination.ElementAt(1));
        }

        /// <summary>
        /// Finds all pairs of connected <see cref="Location"/> on this TrainsMap.
        /// </summary>
        /// <returns>All possible, valid <see cref="Destination"/> on this TrainsMap</returns>
        public ImmutableHashSet<Destination> GetAllFeasibleDestinations()
        {
            var connectedLocations = ConnectedLocations.ToList();
            HashSet<Destination> returnValue = new HashSet<Destination>();
            foreach (var graph in connectedLocations)
            {
                for (int i = 0; i < graph.Count; i++)
                {
                    for (int j = 0; j < graph.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        string city1 = graph.ElementAt(i);
                        string city2 = graph.ElementAt(j);
                        if (returnValue.Where(dest => dest.TryGetValue(city1, out string city1Name) && dest.TryGetValue(city2, out string city2Name)).ToList().Count == 0)
                        {
                            returnValue.Add(new Destination(graph.ElementAt(i), graph.ElementAt(j)));
                        }
                    }
                }
            }
            return returnValue.ToImmutableHashSet();
        }
    }
}