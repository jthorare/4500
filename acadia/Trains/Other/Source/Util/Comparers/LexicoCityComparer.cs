using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Trains.Models.GamePieces;

namespace Trains.Util.Comparers
{
    public class LexicoCityComparer : IComparer<City>
    {
        public int Compare([AllowNull] City x, [AllowNull] City y)
        {
            return x.CityName.CompareTo(y.CityName);
        }
    }
}
