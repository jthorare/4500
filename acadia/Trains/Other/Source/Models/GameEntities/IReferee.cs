using System;
using System.Collections.Generic;
using System.Text;
using Trains.Models.GamePieces;

namespace Trains.Models.GameEntities
{
    // the specifications that a referee must follow to govern a Trains.com game.
    interface IReferee
    {
        /// <summary>
        /// Selects (enough) Destination from the given Map for Player objects to pick from.
        /// The amount is defined in Constants.cs as destinationSelectionPoolSize.
        /// </summary>
        /// <param name="availableDestinations">The Destionation objects to select from.</param>
        /// <param name="map">The map that the Destination objects come from.</param>
        /// <returns>A Collection of Destination for Player objects to pick from.</returns>
        public ICollection<Destination> SelectDestinations(ICollection<Destination> availableDestinations);

        /// <summary>
        /// Shuffles the given deck of cards.
        /// </summary>
        /// <param name="deck">The deck of cards to shuffle.</param>
        /// <returns>A collection containing the shuffled cards.</returns>
        public IList<ColoredCard> ShuffleCards(IList<ColoredCard> deck);

        /// <summary>
        /// Completes an iteration of a Trains.com game with the given players, on the given map.
        /// </summary>
        /// <param name="map">The game map being played on.</param>
        /// <param name="players">The players playing the game.</param>
        /// <returns>The ranking of Players in the game. Any Player with as a score was kicked from the game.</returns>
        public IDictionary<IPlayer, int> PlayGame(Map map, IList<IPlayer> players);
    }
}
