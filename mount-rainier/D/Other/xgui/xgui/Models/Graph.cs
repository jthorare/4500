using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Text.Json.Serialization;

namespace xgui.Models
{
    /// <summary>
    /// Represents a graph made of points. a graph has a Size between [100, 800] and Nodes, a list of points, that fit
    /// on a Size x Size grid. Points are ordered and connected by lines.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// The Size of the graph.
        /// </summary>
        [JsonPropertyName("size")] 
        public uint Size { get; }
        
        /// <summary>
        /// A list of points that fit on a Size x Size grid. Point order matters and each point is connected to the next
        /// by a line.
        /// </summary>
        [JsonPropertyName("nodes")]
        public ImmutableList<Posn> Nodes { get; }
        
        /// <summary>
        /// Construct a Graph with a size and nodes.
        /// </summary>
        /// <param name="nodes">The list of ordered points on the graph.</param>
        /// <param name="size">The size of the graph.</param>
        /// <exception cref="ArgumentException">
        /// If size is not between [100, 800] or any point is not smaller than size.
        /// </exception>
        [JsonConstructor]
        public Graph(List<Posn> nodes, uint size)
        {
            if (size is < 100 or > 800) 
                throw new ConstraintException("Graph size must be between [100, 800]");
            
            foreach (Posn posn in nodes)
            {
                if (posn.X > size || posn.X > size)
                    throw new ArgumentException("Posn must be smaller than graph size.");
            }

            Nodes = nodes.ToImmutableList();
            Size = size;
        }
    }
}