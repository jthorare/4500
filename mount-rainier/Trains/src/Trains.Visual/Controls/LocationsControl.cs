using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Trains.Common;
using Location = Trains.Common.Map.Location;

namespace Trains.Visual.Controls
{
    /// <summary>
    /// A Control that displays Locations on the map.
    /// </summary>
    public class LocationsControl : Control
    {
        // Variables defining the look of a location
        /// <summary>
        /// The <see cref="IBrush"/> used to draw the text of a <see cref="TrainsMap.Location"/>.
        /// </summary>
        private static IBrush TextBrush => Brushes.White;

        /// <summary>
        /// The <see cref="IPen"/> used to draw the point of a <see cref="TrainsMap.Location"/>.
        /// </summary>
        private static IPen PointPen => new Pen(Brushes.Red, 1, lineCap: PenLineCap.Round);

        /// <summary>
        /// The <see cref="IBrush"/> used to draw the point of a <see cref="TrainsMap.Location"/>.
        /// </summary>
        private static IBrush PointBrush => Brushes.Red;

        /// <summary>
        /// Point size (radius) is a fraction of the diagonal of the containing boundaries.
        /// </summary>
        private double PointSize => Math.Sqrt(Math.Pow(Bounds.Width, 2) + Math.Pow(Bounds.Height, 2)) / 50;

        /// <summary>
        /// Text Width is 25 characters long where one character is as wide as the size (radius) of the point.
        /// </summary>
        private double TextWidth => PointSize * 25;

        /// <summary>
        /// The collection of <see cref="TrainsMap.Location"/> that need to be drawn.
        /// </summary>
        public IEnumerable<Location> Locations
        {
            set => SetValue(LocationsProperty, value);
            get => GetValue(LocationsProperty);
        }

        public static readonly StyledProperty<IEnumerable<Location>> LocationsProperty =
            AvaloniaProperty.Register<LocationsControl, IEnumerable<Location>>(nameof(IEnumerable<Location>));

        /// <inheritdoc/>
        /// <summary>
        /// Render all <see cref="TrainsMap.Location"/> to a <see cref="DrawingContext"/>.
        /// </summary>
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            foreach (var location in Locations)
            {
                // Denormalize the coordinates
                var x = (location.X * (Bounds.Width - (2 * Constants.bufferX))) + Constants.bufferX;
                var y = (location.Y * (Bounds.Height - (2 * Constants.bufferY))) + Constants.bufferY;
                var size = new Size(PointSize, PointSize);

                // Draw the point with the coordinates as the center of the point.
                context.DrawGeometry(PointBrush, PointPen,
                    new EllipseGeometry(new Rect(new Point(x - PointSize / 2, y - PointSize / 2), size)));

                // Draw the name
                context.DrawText(TextBrush, new Point(x - TextWidth / 2, y - PointSize / 2),
                    new FormattedText(location.Name, Typeface.Default, PointSize, TextAlignment.Center,
                        TextWrapping.NoWrap,
                        new Size(TextWidth, PointSize)));
            }
        }
    }
}