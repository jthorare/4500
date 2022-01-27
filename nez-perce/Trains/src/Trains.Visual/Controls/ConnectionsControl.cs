using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Trains.Common;
using Trains.Common.Map;

namespace Trains.Visual.Controls
{
    /// <summary>
    /// Represents a <see cref="Control"/> that 
    /// </summary>
    public class ConnectionsControl : Control
    {
        // Variables defining the look of a location
        /// <summary>
        /// The Thickness of a line as a fraction of the diagonal of the bounds
        /// </summary>
        private double LineThickness => Math.Sqrt(Math.Pow(Bounds.Width, 2) + Math.Pow(Bounds.Height, 2)) / 500;

        /// <summary>
        /// The line offset
        /// </summary>
        private const double LineOffset = 0;

        /// <summary>
        /// The percentage of a line that is the segment.
        /// </summary>
        private const double SegmentPercent = 0.9;

        /// <summary>
        /// The percentage of a line that is space.
        /// </summary>
        private const double SpacePercent = 1 - SegmentPercent;

        /// <summary>
        /// The collection of <see cref="Connection"/> to render.
        /// </summary>
        public IEnumerable<Connection> Connections
        {
            set => SetValue(ConnectionsProperty, value);
            get => GetValue(ConnectionsProperty);
        }

        public static readonly StyledProperty<IEnumerable<Connection>> ConnectionsProperty =
            AvaloniaProperty.Register<LocationsControl, IEnumerable<Connection>>(nameof(IEnumerable<Connection>));

        ///<inheritdoc/>
        /// <summary>
        /// Render all <see cref="Trains.Map.Location"/> to a <see cref="DrawingContext"/>.
        /// </summary>
        public override void Render(DrawingContext context)
        {
            // TODO multiple connections between two locations
            base.Render(context);
            foreach (var connection in Connections)
            {
                // Place normalized coords on graph
                var location1 = connection.Locations.ElementAt(0);
                var location2 = connection.Locations.ElementAt(1);
                int index = Connections.Where((c) => c.Locations.Contains(location1) && c.Locations.Contains(location2)).ToList().IndexOf(connection);
                var point1 = new Point((location1.X * (Bounds.Width - (2*Constants.bufferX))) + Constants.bufferX, 
                    (location1.Y * (Bounds.Height - (2 * Constants.bufferY))) + Constants.bufferY);
                var point2 = new Point((location2.X * (Bounds.Width - (2 * Constants.bufferX))) + Constants.bufferX, 
                    (location2.Y * (Bounds.Height - (2 * Constants.bufferY))) + Constants.bufferY);

                // Set color
                var color = connection.Color;
                var brush = ColorToBrush(color);

                // Math for calculating dashes
                var distance = Math.Sqrt(Math.Pow(point2.X - point1.X, 2) +
                                         Math.Pow(point2.Y - point1.Y, 2));
                // Length is based on Line Thickness, remove it from the equation
                var segmentAndSpaceLength = distance / connection.Segments / LineThickness;
                // 10% space, 90% line
                var segmentSpacing = segmentAndSpaceLength * SpacePercent;
                var segmentLength = segmentAndSpaceLength * SegmentPercent + segmentSpacing / connection.Segments;
                var dashStyle = new DashStyle(new[] { segmentLength, segmentSpacing }, LineOffset);

                // Draw the line
                var pen = new Pen(brush, LineThickness, dashStyle);
                if (index != 0) {
                    // Find the offset needed for this line
                    double offset = calculateIndex(index) * 5d;
                    // Translate the points by this offset in the normal direction
                    var points = getPrepPoints(point1, point2, offset);
                    point1 = points.Item1;
                    point2 = points.Item2;
                }
                // Draw the line
                context.DrawLine(pen, point1, point2);
            }
        }

        /// <summary>
        /// Finds the correct offset direction for a given index in the following pattern
        /// 0, 1, -1, 2, -2, 3, -3, 4, -4, etc
        /// </summary>
        /// <param name="index"> The index of this line in the collection</param>
        /// <returns></returns>
        public static int calculateIndex(int index) {
            int div2 = index / 2;
            int mod2 = index % 2;
            if (mod2 == 0)
            {
                return -div2;
            }
            else
            {
                return div2 + 1;
            }
        }


        /// <summary>
        /// Calculates two new points offset by the given offset in the normal direction
        /// </summary>
        /// <param name="p1"> Point 1</param>
        /// <param name="p2"> Point 2</param>
        /// <param name="offset"> The required offset</param>
        /// <returns></returns>
        Tuple<Point, Point> getPrepPoints(Point p1, Point p2, double offset) {
            Point p = p1 - p2;
            Point n = new Point(-p.Y, p.X);
            double normalLength = Math.Sqrt((n.X*n.X) + (n.Y *n.Y));
            n = new Point(n.X / normalLength, n.Y / normalLength);
            return new Tuple<Point, Point> (p1 + (offset * n), p2 + (offset * n));
        }

        /// <summary>
        /// Converts a Common.Map.Color to the appropriate Avalonia.Media.Brush
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <returns>A Brush with the correct color</returns>
        Brush ColorToBrush(Common.Map.Color color)
        {
            Brush brush;
            switch (color)
            {
                case Common.Map.Color.Red:
                    brush = new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 255, 0, 0));
                    break;
                case Common.Map.Color.Blue:
                    brush = new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 0, 0, 255));
                    break;
                case Common.Map.Color.Green:
                    brush = new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 0, 255, 0));
                    break;
                case Common.Map.Color.White:
                    brush = new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 255, 255, 255));
                    break;
                default:
                    throw new ArgumentException("Invalid color");
            }
            return brush;
        }
    }
}