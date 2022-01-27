using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Util;

namespace Trains.Models.GameStates
{
    /// <summary>
    /// A immutable representation of a Trains.com player's game state, given by the Referee.
    /// Contains all data needed for a player to play its turn.
    /// </summary>
    public class PlayerGameState
    {
        /// <summary>
        /// An immutable copy of the game Map.
        /// </summary>
        public Map GameMap { get; }

        /// <summary>
        /// The immutable integer amount of rails that this player has remaining. Must be >= 0.
        /// </summary>
        public int Rails { get; }

        /// <summary>
        ///  The game cards that this  PlayerGameState has in its hand
        /// </summary>
        public IList<ColoredCard> Cards { get; }

        /// <summary>
        /// The immutable set of destinations that this player is aiming to complete, contained within a set.
        /// </summary>
        public ICollection<Destination> Destinations { get; }

        /// <summary>
        ///  The immutable set of Connections that this player owns, contained within a set.
        /// </summary>
        public ICollection<Connection> OwnedConnections { get; }

        /// <summary>
        ///  The immutable set of unowned, available Connections, contained within a set.
        /// </summary>
        public ICollection<Connection> AvailableConnections { get; }

        /// <summary>
        /// Constructor for making a PlayerGameState.
        /// </summary>
        /// <param name="refereeMap">The Map the game is being played with</param>
        /// <param name="currentNumberOfRails">The number of rails the Player has left</param>
        /// <param name="currentCards">The hand the Player currently has</param>
        /// <param name="playerDestinations">The Destination objects the Player chose at setup</param>
        /// <param name="gameOwnedConnections">The Connection the Player owns currently</param>
        /// <param name="gameAvailableConnections">The Connection currently available in the game</param>
        public PlayerGameState(Map refereeMap, int currentNumberOfRails, IList<ColoredCard> currentCards, ICollection<Destination> playerDestinations,
            ICollection<Connection> gameOwnedConnections, ICollection<Connection> gameAvailableConnections)
        {
            GameMap = refereeMap;
            if (Rails > Constants.setupNumberRails)
            {
                throw new ArgumentException($"A Player can only have a maximum number of {Constants.setupNumberRails} rails");
            }
            else
            {
                Rails = currentNumberOfRails;
            }
            Cards = currentCards.ToList();
            Destinations = playerDestinations.ToList();
            OwnedConnections = gameOwnedConnections.ToList();
            AvailableConnections = gameAvailableConnections.ToList();
        }

        /// <summary>
        /// Copy constructor for handing a Player an immutable PlayerGameState
        /// </summary>
        /// <param name="pgs">The PlayerGameState to make immutable</param>
        public PlayerGameState(PlayerGameState pgs)
        {
            GameMap = pgs.GameMap;
            if (Rails > Constants.setupNumberRails)
            {
                throw new ArgumentException($"A Player can only have a maximum number of {Constants.setupNumberRails} rails");
            }
            else
            {
                Rails = pgs.Rails;
            }
            Cards = new ReadOnlyCollection<ColoredCard>(pgs.Cards.ToList());
            Destinations = new ReadOnlyCollection<Destination>(pgs.Destinations.ToList());
            OwnedConnections = new ReadOnlyCollection<Connection>(pgs.OwnedConnections.ToList());
            AvailableConnections = new ReadOnlyCollection<Connection>(pgs.AvailableConnections.ToList());
        }

        /// <summary>
        /// Determines whether this player has a path between the two given Cities using its owned Connections.
        /// </summary>
        /// <param name="cityOne">A City on one end of the path.</param>
        /// <param name="cityTwo">The City on the other end of the path.</param>
        /// <returns>True if a path between the two Cities exist using the player's owned Connections, false otherwise.</returns>
        public bool HasRouteBetween(City cityOne, City cityTwo)
        {
            bool cityOneExists = false;
            bool cityTwoExists = false;
            foreach (Connection conn in OwnedConnections)
            {
                if (conn.City1.Equals(cityOne) || conn.City2.Equals(cityOne))
                {
                    cityOneExists = true;
                }
                if (conn.City1.Equals(cityTwo) || conn.City2.Equals(cityTwo))
                {
                    cityTwoExists = true;
                }
            }
            return (cityOneExists && cityTwoExists && Utilities.PathExistsAmongConnections(OwnedConnections, cityOne, cityTwo, new HashSet<City>() { cityOne }, null, null));
        }

        public override bool Equals(object? obj)
        {
            if (obj.GetType() != typeof(PlayerGameState))
            {
                return false;
            }
            else
            {
                PlayerGameState other = (PlayerGameState)obj;
                bool equals = other.GameMap.Equals(this.GameMap)
                              && other.Rails == this.Rails
                              && other.AvailableConnections.Count == this.AvailableConnections.Count
                              && other.OwnedConnections.Count == this.OwnedConnections.Count
                              && other.Cards.Count == this.Cards.Count
                              && other.Destinations.Count == this.Destinations.Count;

                foreach (Connection conn in this.AvailableConnections)
                {
                    equals = equals && other.AvailableConnections.Contains(conn);
                }
                foreach (Connection conn in this.OwnedConnections)
                {
                    equals = equals && other.OwnedConnections.Contains(conn);
                }
                ICollection<GamePieceColor> colors = new HashSet<GamePieceColor>() { GamePieceColor.Red, GamePieceColor.Blue, GamePieceColor.Green, GamePieceColor.White };
                foreach (GamePieceColor cardColor in colors)
                {
                    equals = equals && other.Cards.Where(card => card.Color == cardColor).Count() == this.Cards.Where(card => card.Color == cardColor).Count();
                }
                foreach (Destination dest in this.Destinations)
                {
                    equals = equals && other.Destinations.Contains(dest);
                }
                return equals;
            }
        }

        public override int GetHashCode()
        {
            int rv = HashCode.Combine(Rails, GameMap, Destinations.ElementAt(0), Destinations.ElementAt(1));
            rv = HashCode.Combine(AvailableConnections.Count, OwnedConnections.Count, Cards.Count);
            foreach (Connection conn in this.AvailableConnections)
            {
                rv = HashCode.Combine(rv, conn);
            }
            foreach (Connection conn in this.OwnedConnections)
            {
                rv = HashCode.Combine(rv, conn);
            }
            foreach (ColoredCard cards in this.Cards)
            {
                rv = HashCode.Combine(rv, cards);
            }
            foreach (Destination dest in this.Destinations)
            {
                rv = HashCode.Combine(rv, dest);
            }
            return rv;
        }
    }
}

