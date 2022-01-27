using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia;

namespace xgui.Models
{
    /// <summary>
    /// Represents a point on a graph
    /// </summary>
    [JsonConverter(typeof(PosnJsonConverter))]
    public class Posn
    {
        /// <summary>
        /// The X coordinate on the graph.
        /// </summary>
        public uint X { get; set; }

        /// <summary>
        /// The Y coordinate on the graph.
        /// </summary>
        public uint Y { get; set; }

        public static implicit operator Point(Posn p) => new(p.X, p.Y);
    }
}