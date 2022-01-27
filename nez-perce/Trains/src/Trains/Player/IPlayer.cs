using System.Collections.Generic;
using System.Collections.Immutable;
using Trains.Common.GameState;
using Trains.Common.Map;

namespace Trains.Player
{
    /// <summary>
    /// Represents a unique player playing a game of Trains.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Represents the name for the IPlayer.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Sets up the player with starting items.
        /// </summary>
        /// <param name="map">The map of the game.</param>
        /// <param name="rails">The number of rails the player starts with.</param>
        /// <param name="cards">The starting hand for the player. Keys are color for the card, values are the number of cards of that color.</param>
        public void Setup(TrainsMap map, uint rails, Cards cards);

        /// <summary>
        /// Receives the given cards to its hand.
        /// </summary>
        /// <param name="cards">The cards to add to its hand.</param>
        public void MoreCards(Cards cards);

        /// <summary>
        /// Tell the player that they should pick destinations from the supplied set of destinations.
        /// </summary>
        /// <param name="map">The map of the game.</param>
        /// <param name="destinations">The set of <see cref="Destination"/> the player should choose from.</param>
        /// <returns>The <see cref="Destination"/> the player would not like to keep.</returns>
        public IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations);

        /// <summary>
        /// Tell the player it is their turn and they should make a move based on the supplied game state.
        /// </summary>
        /// <param name="gameState">The current state of the game.</param>
        /// <returns>The <see cref="PlayerResponse"/> indicating what the player would like to do.</returns>
        public PlayerResponse PlayTurn(PlayerGameState gameState);

        /// <summary>
        /// Tell the player whether they won the game.
        /// </summary>
        /// <param name="isWinner">Whether the player won the game.</param>
        public void IsWinner(bool isWinner);

        /// <summary>
        /// Is this player participating in the tournament that is about to start up?
        /// </summary>
        /// <param name="started">true if the tournament is about to start up</param>
        /// <returns>The IPlayer's submission for the TrainsMap of the tournament</returns>
        public TrainsMap Start(bool started);

        /// <summary>
        /// Did this player win the tournament?
        /// </summary>
        /// <param name="isWinner">true if the player won, false if they lost.</param>
        public void End(bool isWinner);
    }
}