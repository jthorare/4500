using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using xgui.Models;

namespace xgui.Controls
{
    public class GraphCanvas : Canvas
    {
        public Graph Graph
        {
            set => SetValue(GraphProperty, value);
            get => GetValue(GraphProperty);
        }

        public static StyledProperty<Graph> GraphProperty =
            AvaloniaProperty.Register<GraphCanvas, Graph>(nameof(Graph));

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            var pen = new Pen(Brushes.Red, 2, lineCap: PenLineCap.Round);
            for (int i = 0; i < Graph.Nodes.Count; i++)
            {
                for (int j = i + 1; j < Graph.Nodes.Count; j++)
                {
                    context.DrawLine(pen, Graph.Nodes[i], Graph.Nodes[j]);
                }
            }

            pen = new Pen(Brushes.Black, 1, lineCap: PenLineCap.Round);
            var brush = Brushes.Black;
            for (int i = 0; i < Graph.Nodes.Count; i++)
            {
                var rect = new Rect(new Point(Graph.Nodes[i].X-6, Graph.Nodes[i].Y-6), new Size(11, 11));
                context.DrawGeometry(brush, pen, new EllipseGeometry(rect));
            }
        }
    }
}