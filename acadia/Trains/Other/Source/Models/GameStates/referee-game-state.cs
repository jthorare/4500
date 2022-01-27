using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Util;

namespace Trains.Models.GameStates
{
    /// <summary>
    /// A representation of a Trains.com Referee's game state.
    /// Contains all data needed for a Referee to oversee the game.
    /// </summary>
    public class RefereeGameState
    {
        /// <summary>
        /// An immutable copy of the game Map.
        /// </summary>
        public Map GameMap { get; }

        /// <summary>
        /// A queue of the PlayerGameStates that this RefereeGameState is overseeing, in order of the non-active players' turns.
        /// </summary>
        public IList<PlayerGameState> PlayerGameStates { get; }

        /// <summary>
        /// Int that must be kept between 0 and PlayerGameStates.Count representing the index of the current Player
        /// </summary>
        public int CurrentPlayerIndex { get; }

        /// <summary>
        /// Constructor for making a RefereeGameState.
        /// </summary>
        /// <param name="currPlayer">The index of the PlayerGameState of the Player whose turn it is.</param>
        /// <param name="startingMap">The Trains.Models.GamePieces.Map that this RefereeGameState is referring</param>
        /// <param name="startingPlayerGameStates">The Collection of Trains.Models.GameStates.PlayerGameState that this RefereeGameState is maintaining</param>
        public RefereeGameState(int currPlayer, Map startingMap, IList<PlayerGameState> startingPlayerGameStates)
        {
            CurrentPlayerIndex = currPlayer;
            GameMap = startingMap;
            PlayerGameStates = startingPlayerGameStates;
            VerifyIndex();
        }

        /// <summary>
        /// Copy Constructor for removing the given RefereeGameState's CurrentPlayerIndex Player
        /// </summary>
        /// <param name="rgs"></param>
        public RefereeGameState(RefereeGameState rgs)
        {
            IList<PlayerGameState> playerGameStates = RemoveCurrentPlayer(rgs.PlayerGameStates, rgs.CurrentPlayerIndex);
            int currPlayer = rgs.CurrentPlayerIndex < playerGameStates.Count ? rgs.CurrentPlayerIndex : 0;
            CurrentPlayerIndex = currPlayer;
            GameMap = rgs.GameMap;
            PlayerGameStates = playerGameStates;
            VerifyIndex();
        }

        /// <summary>
        /// Copy Constructor for copying a RefereeGameState where the given RefereeGameState's CurrentPlayerIndex Player acquires the given Connection.
        /// </summary>
        /// <param name="rgs">The RefereeGameState to copy from</param>
        /// <param name="toAcquire">The Connection the CurrentPlayerIndex Player is acquiring</param>
        public RefereeGameState(RefereeGameState rgs, Connection toAcquire, int nextIndex)
        {
            IList<PlayerGameState> playerGameStates = AcquireConnection(rgs, toAcquire);
            CurrentPlayerIndex = nextIndex;
            GameMap = rgs.GameMap;
            PlayerGameStates = playerGameStates;
        }

        /// <summary>
        /// Throws an exception if this RefereeGameState was made with an invalid CurrentPlayerIndex
        /// </summary>
        private void VerifyIndex()
        {
            if (CurrentPlayerIndex > PlayerGameStates.Count || CurrentPlayerIndex < 0)
            {
                throw new ArgumentException($"The current player index must be within 0 and {PlayerGameStates.Count}, inclusive.");
            }
        }

        /// <summary>
        /// Removes the given index PlayerGameState from the given list.
        /// </summary>
        /// <param name="playerGameStates">The PlayerGameState list to remove from</param>
        /// <param name="currentPlayerIndex">The index to remove at</param>
        /// <returns>An updated PlayerGameState list</returns>
        private IList<PlayerGameState> RemoveCurrentPlayer(IList<PlayerGameState> playerGameStates, int currentPlayerIndex)
        {
            IList<PlayerGameState> newPlayerGameStates = playerGameStates.ToList();
            PlayerGameState removed = newPlayerGameStates[currentPlayerIndex];
            ICollection<Connection> removedPlayersConnections = removed.OwnedConnections; // get the removed Player's Connections
            newPlayerGameStates.RemoveAt(currentPlayerIndex); // remove them
            foreach (PlayerGameState pgs in newPlayerGameStates) // operate on only the remaining players
            {
                foreach (Connection nowAvailable in removedPlayersConnections)
                {
                    pgs.AvailableConnections.Add(nowAvailable); // add the newly available connections to the available connections
                }
            }
            return newPlayerGameStates;
        }

        /// <summary>
        /// Adds the Connection to the CurrentPlayerIndex Player's owned Connection Collection.
        /// </summary>
        /// <param name="rgs">The RefereeGameState whose current Player is acquiring a Connection</param>
        /// <param name="toAcquire">The Connection being acquired</param>
        /// <returns>A List of updated PlayerGameState objects where order is maintained from the given RefereeGameState.PlayerGameStates List</returns>
        private static IList<PlayerGameState> AcquireConnection(RefereeGameState rgs, Connection toAcquire)
        {
            IList<PlayerGameState> playerGameStates = rgs.PlayerGameStates;
            for (int index = 0; index < playerGameStates.Count; index++)
            {
                PlayerGameState pgs = playerGameStates[index];
                if (index == rgs.CurrentPlayerIndex)
                {
                    playerGameStates[index] = new PlayerGameState(pgs.GameMap, pgs.Rails - (int)toAcquire.NumSegments,
                        RemoveCards(pgs.Cards, toAcquire), pgs.Destinations, pgs.OwnedConnections.Append(toAcquire).ToList(), pgs.AvailableConnections.Where(conn => conn != toAcquire).ToList());
                }
                if (index != rgs.CurrentPlayerIndex)
                {
                    playerGameStates[index] = new PlayerGameState(pgs.GameMap, pgs.Rails, pgs.Cards, pgs.Destinations,
                        pgs.OwnedConnections, pgs.AvailableConnections.Where(conn => conn != toAcquire).ToList());
                }

            }
            return playerGameStates;
        }

        /// <summary>
        /// Removes cards from the hand.
        /// </summary>
        /// <param name="hand">The hand that is spending cards to acquire a Connection</param>
        /// <param name="toAcquire">The Connection being acquired</param>
        /// <returns>Returns a hand with toAcquire NumSegment number of toAcquire.Color cards removed.</returns>
        private static IList<ColoredCard> RemoveCards(IList<ColoredCard> hand, Connection toAcquire)
        {
            IList<ColoredCard> newHand = new List<ColoredCard>();
            int removed = 0;
            int toRemove = (int)toAcquire.NumSegments;
            foreach (ColoredCard card in hand)
            {
                if (removed == toRemove)
                {
                    break;
                }
                if (card.Color != toAcquire.Color)
                {
                    newHand.Add(new ColoredCard(card.Color));
                }
                else
                {
                    removed++;
                    continue;
                }
            }
            return newHand;
        }

        /// <summary>
        /// Determines whether a Connection object can be acquired by the currently active player.
        /// </summary>
        /// <param name="requestedConn">The Connection that is being considered.</param>
        /// <returns>True if the current player's game state reflects that the Connection can be acquired, false otherwise.</returns>
        public bool CanCurrentPlayerAcquireConnection(Connection requestedConn)
        {
            PlayerGameState currentPlayerGameState = PlayerGameStates[CurrentPlayerIndex];
            bool enoughResources =
                (Utilities.GetCardsOfColor(currentPlayerGameState.Cards, requestedConn.Color) >= (int)requestedConn.NumSegments) &&
                (currentPlayerGameState.Rails >= (int)requestedConn.NumSegments);
            return (enoughResources && currentPlayerGameState.AvailableConnections.Contains(requestedConn));
        }

        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                RefereeGameState other = (RefereeGameState)obj;
                bool equals =
                    this.GameMap.Equals(other.GameMap) && this.PlayerGameStates.Count == other.PlayerGameStates.Count &&
                    this.CurrentPlayerIndex == other.CurrentPlayerIndex;
                foreach (PlayerGameState pgs in this.PlayerGameStates)
                {
                    equals = equals && other.PlayerGameStates.Contains(pgs);
                }
                return equals;
            }
        }

        public override int GetHashCode()
        {
            int rv = HashCode.Combine(this.CurrentPlayerIndex, this.GameMap);
            foreach (PlayerGameState pgs in this.PlayerGameStates)
            {
                rv = HashCode.Combine(rv, pgs);
            }
            return rv;
        }
    }
}