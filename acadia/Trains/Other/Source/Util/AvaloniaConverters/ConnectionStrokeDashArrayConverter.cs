using Avalonia.Data.Converters;
using System;
using System.Linq;
using Trains.Models.GamePieces;
using System.Globalization;
using Avalonia.Collections;

namespace Trains.Util.AvaloniaConverters
{
    /// <summary>
    /// Converter for converting a Connection to a StrokeDashArray
    /// </summary>
    public class ConnectionStrokeDashArrayConverter : IValueConverter
    {
        /// <summary>
        /// Converts a given object if and only if it is a Trains.Models.GamePieces.Connection to a an Avalonia.Collections.AvaloniaList. If it is any other type,
        /// Throws a NotImplementedException.
        /// </summary>
        /// <param name="value">The object to Convert into a StrokeDashArray (should be a Trains.Models.GamePieces.Connection)</param>
        /// <param name="targetType">The targetType to convert the given object into (should be an Avalonia.Collections.AvaloniaList</param>
        /// <param name="parameter">The parameter given to this converter</param>
        /// <param name="culture">Object that is not used for this converter</param>
        /// <returns>An AvaloniaList<double> for use as a StrokeDashArray within a View</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is Connection && targetType == typeof(AvaloniaList<Double>)) // if we are converting the right type of object and converting into the right type of object
            {
                Connection connection = value as Connection;
                City city1 = connection.City1;
                City city2 = connection.City2;
                double lineLength = Math.Sqrt((Math.Pow(city1.YPosition - city2.YPosition, 2) + Math.Pow(city1.XPosition - city2.XPosition, 2)));
                double gapSize = Constants.connectionGapSize * lineLength; // Make the gap size of the Connection a function of the length of the Connection
                double SegmentLength = (lineLength / (double)connection.NumSegments) - 
                    (gapSize * ((((double)connection.NumSegments) - 1) / (double)connection.NumSegments));
                return new AvaloniaList<double>() { SegmentLength, gapSize };
            }
            throw new NotSupportedException("CityPointConverter only converts a City -> Avalonia.Point.");
        }

        /// <summary>
        /// Not implemented in this class: Throws a NotImplementedException.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Throws a NotImplementedException</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
