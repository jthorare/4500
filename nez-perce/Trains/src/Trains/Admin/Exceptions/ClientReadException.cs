using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Admin.Exceptions
{
    public class ClientReadException : Exception
    {
        public ClientReadException(string msg) : base(msg) { }
    }
}
