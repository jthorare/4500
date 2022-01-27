using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Admin
{
    public class PlayerNameException : Exception
    {
        public PlayerNameException(string msg) : base(msg) { }
    }
}
