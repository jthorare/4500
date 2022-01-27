using System;
using System.Collections.Immutable;
using Trains.Common.GameState;
using Trains.Common.Map;

namespace Trains.Player.Strategy
{
    /// <summary>
    /// A strategy that a player can employ to make decisions in the game.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Given a set of <see cref="Destination"/> return the selection of two that the player should keep.
        /// </summary>
        /// <param name="destinations">The selection of <see cref="Destination"/>.</param>
        /// <returns>The <see cref="Destination"/> the player did not choose.</returns>
        public IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations);

        /// <summary>
        /// Given the current game state return the <see cref="PlayerResponse"/> representing what the player should do.
        /// </summary>
        /// <param name="gameState">The current state of the game.</param>
        /// <returns>The <see cref="PlayerResponse"/> of the player.</returns>
        public PlayerResponse PlayTurn(PlayerGameState gameState);
    }
}