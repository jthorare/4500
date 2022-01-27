using System.Collections.Generic;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.TurnTypes;

namespace Trains.Models.GameEntities
{
    // The specifications that a player in the Trains.com game must follow to play a Trains.com game.
    public interface IPlayer
    {
        /// <summary>
        /// Sets up this Player for a Trains.com game.
        /// </summary>
        /// <param name="map">The Game map for the Trains.com game</param>
        /// <param name="rails">The number of rails this Player starts with</param>
        /// <param name="cards">The initial cards dealt to this Player</param>
        public void Setup(Map map, int rails, IList<ColoredCard> cards);

        /// <summary>
        /// Adds the given cards to this Player's hand.
        /// </summary>
        /// <param name="cards">The Dictionary containing cards dealt to this Player</param>
        public void MoreCards(IList<ColoredCard> cards);

        /// <summary>
        /// Chooses the Destination this Player needs to complete for a Trains.com game.
        /// </summary>
        /// <param name="destinations">The ICollection of Destination for this Player to choose from.</param>
        /// <returns>An ICollection of Destination consisting of this Player's chosen Destination elements.</returns>
        public ICollection<Destination> ChooseDestinations(ICollection<Destination> destinations);

        /// <summary>
        /// Plays the next turn for this Player according to the given PlayerGameState representing this Player's state in a Trains.com game.
        /// </summary>
        /// <param name="pgs">This Player's current state in a Trains.com game</param>
        /// <returns>An ITurn representing this Player's next turn's behavior</returns>
        public ITurn PlayTurn(PlayerGameState pgs);

        /// <summary>
        /// Calling this signifies to this IPlayer that the Trains.com game is over.
        /// </summary>
        /// <param name="won">A bool representing if this Player won (true) the Trains.com game.</param>
        public void GameOver(bool won);
    }
}
