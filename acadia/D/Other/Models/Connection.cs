using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgui.Models
{
    public class Connection
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public override string ToString()
        {
            return $"{Start} {End}\n";
        }
    }
}
