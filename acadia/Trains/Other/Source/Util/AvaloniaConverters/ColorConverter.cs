using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Models.GamePieces;

namespace Trains.Util.AvaloniaConverters
{
    /// <summary>
    /// Converter for converting a GamePieceColor to an Avalonia Color.
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a GamePieceColor to the mapped Avalonia.Media.Color.
        /// </summary>
        /// <param name="value">Object to convert (should be a GamePieceColor reference).</param>
        /// <param name="targetType">The type to convert the given 'value' into (should be a Avalonia.Media.Color).</param>
        /// <param name="parameter">The parameter given to this converter</param>
        /// <param name="culture">Object that is not used for this converter</param>
        /// <returns>Returns a SolidColorBrush reference with the matching Avalonia.Media.Color from the GamePieceColor</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return null; }

            if (value is GamePieceColor && targetType == typeof(IBrush)) // if we are converting the right type of object and converting into the right type of object
            {
                switch ((GamePieceColor)value)
                {
                    case GamePieceColor.Red:
                        return new SolidColorBrush(Colors.Red);
                    case GamePieceColor.Green:
                        return new SolidColorBrush(Colors.LightGreen);
                    case GamePieceColor.White:
                        return new SolidColorBrush(Colors.GhostWhite);
                    case GamePieceColor.Blue:
                        return new SolidColorBrush(Colors.DodgerBlue);
                    default:
                        throw new ArgumentException("Invalid Connection color");
                }
            }
            throw new NotSupportedException();
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
