using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Trains.Common.Map
{
    /// <summary>
    /// Represents a named location on an Map. An Location may be connected to other locations via Connections. A
    /// location also has a coordinate on the map.
    /// </summary>
    public sealed class Location : IComparable
    {
        /// <summary>
        /// The internal set of connections this location is a part of.
        /// </summary>
        private readonly HashSet<Connection> _connections;

        /// <summary>
        /// Construct a <see cref="Location"/> with required information.
        /// </summary>
        /// <param name="name">The name of the location.</param>
        /// <param name="x">The normalized X-coordinate of the location.</param>
        /// <param name="y">The normalized Y-coordinate of the location.</param>
        /// <exception cref="ArgumentException">
        /// If the coordinates are not normalized or supplied name is not valid.
        /// </exception>
        public Location(string name, float x, float y)
        {
            // Length should be 25 characters or less
            if (name.Length >= 25)
            {
                throw new ArgumentException("The length of the location name cannot exceed 25 characters");
            }

            // Names should match the supplied regex
            if (!Regex.Match(name, "^[a-zA-Z0-9\\ \\.\\,]+$").Success)
            {
                throw new ArgumentException("Illegal character in location name.");
            }

            // Coordinates should be normalized between 0 and 1
            if (x is < 0f or > 1f || y is < 0f or > 1f)
            {
                throw new ArgumentException("The supplied coordinate is not normalized.");
            }

            _connections = new HashSet<Connection>();
            Name = name;
            X = x;
            Y = y;
        }

        /// <summary>
        /// The unique name of this Location
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The set of Connections that this Location is connected by.
        /// </summary>
        public ImmutableHashSet<Connection> Connections => _connections.ToImmutableHashSet();

        /// <summary>
        /// The normalized X coordinate of this Location on the map. 0 is at the origin and 1 is at the right edge of
        /// the screen.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// The normalized Y coordinate of this Location on the map. 0 is at the origin and 1 is at the bottom edge of
        /// the screen.
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Add a Connection to the location.
        /// </summary>
        /// <remarks>
        /// This is an internal method that will NEVER be called outside the Trains.Common.Map scope. As a result it does
        /// not validate that the connection is valid for the location. It is expected that this will be validated when
        /// creating a Map.
        /// </remarks>
        /// <param name="connection">The connection </param>
        internal void AddConnection(Connection connection)
        {
            _connections.Add(connection);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not Location other)
            {
                return false;
            }

            return Name == other.Name &&
                   Math.Abs(X - other.X) < 0.0001f && Math.Abs(Y - other.Y) < 0.0001f;
        }

        public static bool operator ==(Location obj1, Location obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Location obj1, Location obj2)
        {
            return !obj1.Equals(obj2);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is not Location other)
            {
                throw new ArgumentException("Cannot compare Location to non-Location");
            }

            // Compare using Name
            return string.CompareOrdinal(Name, other.Name);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name;
        }
    }
}