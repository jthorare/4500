using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Trains.Models.GamePieces;

namespace Trains.Util.AvaloniaConverters
{
    /// <summary>
    /// Converter for converting a City into an Avalonia.Point
    /// </summary>
    public class CityPointConverter : IValueConverter
    {
        /// <summary>
        /// Converts a given object if and only if it is a Trains.Models.GamePieces.City to an Avalonia.Point. If it is any other type,
        /// Throws a NotImplementedException.
        /// </summary>
        /// <param name="value">The object to Convert into an Avalonia.Point (should be a Trains.Models.GamePieces.City)</param>
        /// <param name="targetType">The targetType to convert the given object into (should be an Avalonia.Point)</param>
        /// <param name="parameter">The parameter given to this converter</param>
        /// <param name="culture">Object that is not used for this converter</param>
        /// <returns>An Avalonia.Point for use within a View</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is City && targetType == typeof(Point)) // if we are converting the right type of object and converting into the right type of object
            {
                City city = value as City;
                return new Point(city.XPosition, city.YPosition);
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
