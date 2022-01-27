using System;
using System.Collections.Generic;
using System.Text;

namespace Trains.Models.TurnTypes
{
    /// <summary>
    /// Class representing a Turn where the Player should draw cards.
    /// </summary>
    public class DrawCardsTurn : ITurn
    {
        /// <summary>
        /// Determines equality between this DrawCardsTurn and another Object. We define two City to be equal if they have the same Name,
        /// or if they have the same [x, y] position.
        /// </summary>
        /// <param name="obj">The Object to determine equality with.</param>
        /// <returns>Whether this DrawCardsTurn is equal to the given Object.</returns>
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            return !((obj == null) || !this.GetType().Equals(obj.GetType()));
        }

        /// <summary>
        /// Determines the hash code for this DrawCardsTurn.
        /// </summary>
        /// <returns>An int representing the hash code of this DrawCardsTurn</returns>
        public override int GetHashCode()
        {
            return 0;
        }
    }
}
