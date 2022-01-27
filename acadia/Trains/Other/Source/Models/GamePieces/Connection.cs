using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Util;
using Trains.Util.Comparers;

namespace Trains.Models.GamePieces
{

    /// <summary>
    /// Class representing a direct Connection between two Cities on a Map.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// A single City representing one end of this Connection.
        /// Accessible using ConnectionObject.City1.
        /// </summary>
        public City City1 { get; }

        /// <summary>
        /// A single City representing one end of this Connection.
        /// Accessible using ConnectionObject.City2.
        /// </summary>
        public City City2 { get; }

        /// <summary>
        /// The color of cards required for this Connection's segments represented as an enumeration of the valid colors for Trains.com.
        /// Accessible using ConnectionObject.Color.
        /// </summary>
        public GamePieceColor Color { get; }

        /// <summary>
        /// The number of segments in this Connection represented as an enumeration of the acceptable lengths.
        /// Accessible using ConnectionObject.Segments.
        /// </summary>
        public Length NumSegments { get; }

        /// <summary>
        /// Constructs a Connection object. A Connection can only be constructed via a IEnumerable of City of size 2, a SegmentColor for the segments of this Connection,
        /// and a Length representing an acceptable number of segments in this Connection.
        /// </summary>
        /// <param name="cityPair">An ICollection of size 2 that makes up the endpoints of this Connection.</param>
        /// <param name="color">A SegmentColor indicating the color of this Connection's segments.</param>
        /// <param name="length">A Length indicating the number of segments in this Connection.</param>
        public Connection(City one, City two, GamePieceColor connectionColor, Length connectionLength)
        {
            ICollection<City> orderedCities = Utilities.OrderByComparer(new List<City> { one, two }, new LexicoCityComparer());
            City1 = orderedCities.ElementAt(0);
            City2 = orderedCities.ElementAt(1);
            Color = connectionColor;
            NumSegments = connectionLength;
        }


        /// <summary>
        /// Determines equality between this Connection and another Object. Within the scope of Trains.com,
        /// we define two Connection to be equal if they share the same two City and SegmentColor.
        /// </summary>
        /// <param name="obj">The Object to determine equality with.</param>
        /// <returns>Whether this Connection is equal to the given Object.</returns>
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Connection c = (Connection)obj;
                bool equals = this.Color == c.Color;

                equals = equals && (this.City1.Equals(c.City1) || this.City1.Equals(c.City2)) && (this.City2.Equals(c.City1) || this.City2.Equals(c.City2));
                return equals;
            }
        }

        /// <summary>
        /// Determines the hash code for a Connection.
        /// </summary>
        /// <returns>An int representing the hash code of this Connection.</returns>
        public override int GetHashCode()
        {
            int rv = this.City1.GetHashCode() + this.City2.GetHashCode();
            return HashCode.Combine(rv, (int)this.Color);
        }

        /// <summary>
        /// Enum representing the acceptable lengths of a Connection for Trains.Com.
        /// </summary>
        public enum Length
        {
            /// <summary>
            /// A length of 3 segments in a Connection.
            /// </summary>
            Three = 3,

            /// <summary>
            /// A length of 4 segments in a Connection.
            /// </summary>
            Four = 4,

            /// <summary>
            /// A length of 5 segments in a Connection.
            /// </summary>
            Five = 5
        }
    }
}