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
    public class LexicoDestinationComparer : IComparer<Destination>
    {
        /// <summary>
        /// Compares two Destination objects using the Lexicographic ordering of Destination in Milestone 5.
        /// </summary>
        /// <param name="x">The Destination dubbed p1 in the Milestone 5 definition</param>
        /// <param name="y">The Destination dubbed p2 in the Milestone 5 definition</param>
        /// <returns>Returns less than 0 if x comes before y, 0 if they're lexicographically equivalent, or greater than 0 if y comes before x.</returns>
        public int Compare([AllowNull] Destination x, [AllowNull] Destination y)
        {
            int rv = String.Compare(x.CityOne.CityName, y.CityOne.CityName);
            if (rv == 0)
            {
                rv = String.Compare(x.CityTwo.CityName, y.CityTwo.CityName);
                if (rv == 0) // p1 == p2 in which case we still swap to p2,p1
                {
                    rv = 1;
                }
            }
            return rv;
        }
    }
}
