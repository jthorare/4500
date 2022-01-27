using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Trains.Common.Map
{
    /// <summary>
    /// Represents a connection between two Locations. A Connection has a color and a number of rail segments that
    /// connect the two locations. A Connection can optionally be owned by a player.
    /// </summary>
    public sealed class Connection : IComparable
    {
        /// <summary>
        /// The two locations this Connection connects as a set.
        /// </summary>
        private readonly HashSet<Location> _locations;

        /// <summary>
        /// The number of segments in this Connection.
        /// </summary>
        public ImmutableHashSet<Location> Locations => _locations.ToImmutableHashSet();
        
        /// <summary>
        /// The number of segments in this Connection.
        /// </summary>
        public uint Segments { get; }

        /// <summary>
        /// The color of the segments in this Connection.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// Construct a Connection between two points with a number of segments and a Color.
        /// </summary>
        /// <remarks>
        /// The order of locations does not matter as the connection is undirected.
        /// Segments must be in [3,5] and Color must be one of Color.{Red,Blue,Green,White}.
        /// </remarks>
        /// <param name="location1">The first location of the connection</param>
        /// <param name="location2">The second location of the connection.</param>
        /// <param name="segments">The number of segments in the connection.</param>
        /// <param name="color">The color of the connection.</param>
        /// <exception cref="ArgumentException"></exception>
        public Connection(Location location1, Location location2, uint segments, Color color)
        {
            if (segments is not (3 or 4 or 5))
            {
                throw new ArgumentException("The number of segments must be between [3,5]");
            }
            _locations = new HashSet<Location> { location1, location2 };
            Segments = segments;
            Color = color;
        }


        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not Connection other)
            {
                return false;
            }

            return _locations.SetEquals(other._locations) &&
                   Segments == other.Segments && Color == other.Color;
        }

        public static bool operator ==(Connection obj1, Connection obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Connection obj1, Connection obj2)
        {
            return !obj1.Equals(obj2);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Segments, Color);
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is not Connection other)
            {
                throw new ArgumentException("Cannot compare Connection to non-Connection");
            }

            // Order locations for both connections.
            List<Location> thisSortedLocations = Locations.ToList();
            thisSortedLocations.Sort();
            List<Location> otherSortedLocations = other.Locations.ToList();
            otherSortedLocations.Sort();

            // Compare alphabetically first location
            var returnValue = thisSortedLocations[0].CompareTo(otherSortedLocations[0]);
            
            // Break tie with second location
            if (returnValue == 0) returnValue = thisSortedLocations[1].CompareTo(otherSortedLocations[1]);
            
            // Break tie with Segments
            if (returnValue == 0) returnValue = Segments.CompareTo(other.Segments);
            
            // Break tie with Color name
            if (returnValue == 0) returnValue = string.CompareOrdinal(Color.ToString(), other.Color.ToString());
            return returnValue;
        }

        public override string ToString()
        {
            return $"{Locations.ElementAt(0)} --> {Locations.ElementAt(1)}";
        }
    }
}