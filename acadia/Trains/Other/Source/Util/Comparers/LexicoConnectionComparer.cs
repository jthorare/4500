using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Trains.Models.GamePieces;

namespace Trains.Util.Comparers
{
    /// <summary>
    /// Class representing
    /// </summary>
    public class LexicoConnectionComparer : IComparer<Connection>
    {
        /// <summary>
        /// Compares two Destination objects using the Lexicographic definition of Connection in Milestone 5.
        /// </summary>
        /// <param name="x">The first Connection to compare with</param>
        /// <param name="y">The second Connection to compare with</param>
        /// <returns>Returns less than 0 if x comes before y, 0 if they're lexicographically equivalent, or greater than 0 if y comes before x.</returns>
        public int Compare([AllowNull] Connection x, [AllowNull] Connection y)
        {
            int rv = String.Compare(x.City1.CityName, y.City1.CityName);
            if (rv == 0)
            {
                rv = String.Compare(x.City2.CityName, y.City2.CityName);
                if (rv == 0) // p1 == p2 in which case we still swap to p2,p1
                {
                    rv = x.NumSegments - y.NumSegments;
                    if (rv == 0)
                    {
                        rv = String.Compare(x.Color.ToString().ToLower(), y.Color.ToString().ToLower());
                    }
                }
            }
            return rv;
        }
    }
}
