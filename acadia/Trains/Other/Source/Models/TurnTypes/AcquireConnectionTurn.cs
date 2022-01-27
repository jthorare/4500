using System;
using System.Collections.Generic;
using System.Text;
using Trains.Models.GamePieces;

namespace Trains.Models.TurnTypes
{
    /// <summary>
    /// Class representing a Trains.Com turn where the action of a Player is to acquire a Connection.
    /// </summary>
    public class AcquireConnectionTurn : ITurn
    {
        /// <summary>
        /// A reference to the Connection to acquire
        /// </summary>
        public Connection ToAcquire { get; }

        /// <summary>
        /// Constructs a new AcquireConnection with the Connection to acquire.
        /// </summary>
        /// <param name="connection">A reference to the Connection to acquire</param>
        public AcquireConnectionTurn(Connection connection)
        {
            ToAcquire = connection;
        }

        /// <summary>
        /// Determines equality between this AcquireConnectionTurn and another Object.
        /// We define two AcquireConnectionTurn to be equal if they have equal ToAcquire properties.
        /// </summary>
        /// <param name="obj">The Object to determine equality with.</param>
        /// <returns>Whether this AcquireConnectionTurn is equal to the given Object.</returns>
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AcquireConnectionTurn t = (AcquireConnectionTurn)obj;
                return this.ToAcquire.Equals(t.ToAcquire);
            }
        }

        /// <summary>
        /// Determines the hash code for this AcquireConnectionTurn.
        /// </summary>
        /// <returns>An int representing the hash code of this AcquireConnectionTurn</returns>
        public override int GetHashCode()
        {
            return this.ToAcquire.GetHashCode();
        }
    }
}
