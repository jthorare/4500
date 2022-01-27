using System;
using System.Collections.Generic;
using System.Text;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.TurnTypes;

namespace Trains.Models.Strategies
{
    /// <summary>
    /// Interface representing the Strategy a Player object can have. All Strategy implementations should implement this interface.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Chooses the Destinations for the Player according to this IStrategy's rules.
        /// </summary>
        /// <param name="destinations">The collection of Destination objects from which this IStrategy chooses.</param>
        /// <param name="map">The Map containing the City objects that make up the Destination objects from which this IStrategy chooses.</param>
        /// <returns>An ICollection of 3 Destination that this Strategy has not chosen from the given ICollection of Destination</returns>
        public ICollection<Destination> ChooseDestinations(Map map, ICollection<Destination> destinations);

        /// <summary>
        /// Conducts a turn using the IStrategy.
        /// </summary>
        /// <param name="playerGameState">The PlayerGameState of the Player using this IStrategy</param>
        /// <returns>The ITurn representing the type of Trains.Com Turn the Player should conduct</returns>
        public ITurn ConductTurn(PlayerGameState playerGameState);
    }
}
