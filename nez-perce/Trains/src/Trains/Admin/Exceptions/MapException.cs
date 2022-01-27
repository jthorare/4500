using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common;

namespace Trains.Admin
{
    public class MapException : Exception
    {
        public MapException() : base(Constants.destinationExceptionMsg) { }
    }
}
