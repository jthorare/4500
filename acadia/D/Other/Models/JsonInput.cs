using System.Collections.Generic;
using Newtonsoft.Json;
namespace xgui.Models
{
    class JsonInput
    {
        [JsonProperty("nodes")]
        public IList<IList<int>> nodes { get; set; }
        [JsonProperty("size")]
        public int size { get; set; }

        public JsonInput(IList<IList<int>> _nodes, int _size)
        {
            nodes = _nodes;
            size = _size;
        }

        public override string ToString()
        {
            string str = $"size: {size}\nnodes: ";
            foreach (List<int> node in nodes)
            {
                str += $"[{node[0]}, {node[1]}] ";
            }
            return str;
        }
    }
}
