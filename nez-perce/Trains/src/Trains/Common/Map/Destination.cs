using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;
using Trains.Common.Map.Json;

namespace Trains.Common.Map
{
    /// <summary>
    /// Represents a set of two locations that are connected.
    /// </summary>
    /// <remarks>
    /// Decorator class for ImmutableHashSet of size 2.
    /// </remarks>
    [JsonConverter(typeof(DestinationJsonConverter))]
    public partial class Destination
    {
        private readonly string _city1;
        private readonly string _city2;

        public Destination(string city1, string city2)
        {
            _city1 = city1;
            _city2 = city2;
            _destinationNames = new HashSet<string> { city1, city2 }.ToImmutableHashSet();
        }

        public bool FulfilledBy(ISet<Connection> connections)
        {
            // Easy exit if both cities do not have an adjacent connection in the supplied set.
            if (connections.All(connection =>
                connection.Locations.All(location => location.Name != _city1 && location.Name != _city2)))
            {
                return false;
            }

            // DFS Traverse from city1 and try to find city2
            // Initialize frontier with connections that have city1 as its location.
            Stack<Connection> frontier =
                new(connections.Where(connection => connection.Locations.Any(location => location.Name == _city1)));
            // Keep track of locations already visited.
            HashSet<Location> visitedLocations = new();
            while (frontier.TryPop(out var checkedConnection))
            {
                var locationsToSearch = checkedConnection.Locations.Except(visitedLocations);

                // If any of the locations match final destination then the destinations are connected.
                if (locationsToSearch.Any(location => location.Name == _city2))
                {
                    return true;
                }

                // Add the checked Locations to the locations already visited
                visitedLocations.UnionWith(checkedConnection.Locations);

                // Add unchecked connections to the stack.
                foreach (var connection in connections.Where(connection =>
                    connection.Locations.Any(location => !visitedLocations.Contains(location))))
                {
                    frontier.Push(connection);
                }
            }

            return false;
        }

        public override string ToString()
        {
            return $"{_city1} <--> {_city2}";
        }
    }


    // The following partial implementation implements IImmutableSet by delegating to _destinationNames.
    public partial class Destination : IImmutableSet<string>
    {
        /// <summary>
        /// Internal container for this decorator class.
        /// </summary>
        private readonly ImmutableHashSet<string> _destinationNames;

        /// <inheritdoc/>
        public int Count => _destinationNames.Count;

        /// <inheritdoc/>
        public IImmutableSet<string> Add(string value)
        {
            return _destinationNames.Add(value);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> Clear()
        {
            return _destinationNames.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(string value)
        {
            return _destinationNames.Contains(value);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> Except(IEnumerable<string> other)
        {
            return _destinationNames.Except(other);
        }

        /// <inheritdoc/>
        public IEnumerator<string> GetEnumerator()
        {
            return _destinationNames.GetEnumerator();
        }

        /// <inheritdoc/>
        public IImmutableSet<string> Intersect(IEnumerable<string> other)
        {
            return _destinationNames.Intersect(other);
        }

        /// <inheritdoc/>
        public bool IsProperSubsetOf(IEnumerable<string> other)
        {
            return _destinationNames.IsProperSubsetOf(other);
        }

        /// <inheritdoc/>
        public bool IsProperSupersetOf(IEnumerable<string> other)
        {
            return _destinationNames.IsProperSupersetOf(other);
        }

        /// <inheritdoc/>
        public bool IsSubsetOf(IEnumerable<string> other)
        {
            return _destinationNames.IsSubsetOf(other);
        }

        /// <inheritdoc/>
        public bool IsSupersetOf(IEnumerable<string> other)
        {
            return _destinationNames.IsSupersetOf(other);
        }

        /// <inheritdoc/>
        public bool Overlaps(IEnumerable<string> other)
        {
            return _destinationNames.Overlaps(other);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> Remove(string value)
        {
            return _destinationNames.Remove(value);
        }

        /// <inheritdoc/>
        public bool SetEquals(IEnumerable<string> other)
        {
            return _destinationNames.SetEquals(other);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> SymmetricExcept(IEnumerable<string> other)
        {
            return _destinationNames.SymmetricExcept(other);
        }

        /// <inheritdoc/>
        public bool TryGetValue(string equalValue, out string actualValue)
        {
            return _destinationNames.TryGetValue(equalValue, out actualValue);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> Union(IEnumerable<string> other)
        {
            return _destinationNames.Union(other);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _destinationNames.GetEnumerator();
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Destination destination && destination._destinationNames.SetEquals(_destinationNames);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(_destinationNames.ElementAt(0).GetHashCode(), _destinationNames.ElementAt(1).GetHashCode());
        }
    }

    // This partial implementation of Destination implements IComparable
    public partial class Destination : IComparable
    {
        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is not Destination other)
            {
                throw new ArgumentException("Cannot compare Destination to non-Destination");
            }

            // Order both Destination's destination names
            List<string> thisSortedDestinationNames = _destinationNames.ToList();
            thisSortedDestinationNames.Sort();
            List<string> otherSortedDestinationNames = other._destinationNames.ToList();
            otherSortedDestinationNames.Sort();

            // Compare alphabetically first destination name
            var returnValue = string.CompareOrdinal(thisSortedDestinationNames[0], otherSortedDestinationNames[0]);

            // Break tie with second location
            if (returnValue == 0)
                returnValue = string.CompareOrdinal(thisSortedDestinationNames[0], otherSortedDestinationNames[0]);

            return returnValue;
        }
    }
}