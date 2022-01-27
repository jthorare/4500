using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Common.GameState.Json
{
    public class PlayerJson : List<AcquiredJson>, IComparable<PlayerJson>
    {
        public PlayerJson(IEnumerable<AcquiredJson> collection) : base(collection)
        {
        }

        public int CompareTo(PlayerJson? other)
        {
            throw new NotImplementedException();
        }
    }
}
