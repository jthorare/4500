using System;
using System.Text.RegularExpressions;
using Trains.Util;

namespace Trains.Models.GamePieces
{
    /// <summary>
    /// Class representing a City used in a Map.
    /// </summary>
    public class City
    {
        /// <summary>
        /// The immutable X-position in pixels of a City on a Map where the origin (0,0) is in the top-left and right is the positive x-axis.
        /// The X-position must be non-negative.
        /// Accessible using CityObject.X.
        /// </summary>
        public int XPosition { get; }

        /// <summary>
        /// The immutable Y-position in pixels of a City on a Map where the origin (0,0) is in the top-left and down is the positive y-axis.
        /// The Y-position must be non-negative.
        /// Accessible using CityObject.Y.
        /// </summary>
        public int YPosition { get; }

        /// <summary>
        /// The immutable name of a City on a Map.
        /// Accessible using CityObject.Name.
        /// </summary>
        public string CityName { get; }

        /// <summary>
        /// Constructor used to make a City. Ensures that 
        /// </summary>
        /// <param name="x">The X-Position in pixels of the City represented as a non-negative integer.</param>
        /// <param name="y">The Y-Position in pixels of the City represented as a non-negative integer.</param>
        /// <param name="name">The name of the City represented as a string.</param>
        public City(int x, int y, string name)
        {
            if (x < 0 || x > 800) // ensure non-negative invariant
            {
                throw new ArgumentException(String.Format("The x-coordinate {0} must be a natural number less than or equal to 800.", x));
            }
            else
            {
                XPosition = x;
            }
            if (y < 0 || y > 800) // ensure non-negative invariant
            {
                throw new ArgumentException(String.Format("The y-coordinate {0} must be a natural number less than or equal to 800.", y));
            }
            else
            {
                YPosition = y;
            }
            const string regex = @"^[a-zA-Z0-9,. ]+$";
            // A City's Name cannot be an empty-string, null, or only whitespace, and it must consist only of the letters of the English alphabet plus space, dot, and comma and have at most 25 ASCII characters.
            if (String.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, regex) ||
                name.Length > Constants.maximumCityNameLength)
            {
                throw new ArgumentException(String.Format("Name {0} must be non-null, non-empty, consist only of the letter of the English alphabet, space, dot, or comma, and have at most 25 ASCII characters.", name));
            }
            else
            {
                CityName = name;
            }
        }

        /// <summary>
        /// Overridden ToString() that provides information about the values of this City.
        /// </summary>
        /// <returns>A string representing this City.</returns>
        public override string ToString()
        {
            return String.Format("City: {0}, {1}, {2}", CityName, XPosition, YPosition);
        }

        /// <summary>
        /// Determines equality between this City and another Object. We define two City to be equal if they have the same Name,
        /// or if they have the same [x, y] position.
        /// </summary>
        /// <param name="obj">The Object to determine equality with.</param>
        /// <returns>Whether this City is equal to the given Object.</returns>
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                City c = (City)obj;
                return c.CityName == this.CityName || (c.XPosition == this.XPosition && c.YPosition == this.YPosition);
            }
        }

        /// <summary>
        /// Determines the hash code for this City.
        /// </summary>
        /// <returns>An int representing the hash code of this City</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.CityName, this.YPosition, this.XPosition);
        }
    }
}
