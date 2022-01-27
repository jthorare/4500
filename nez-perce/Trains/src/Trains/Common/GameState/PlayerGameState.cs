using System;
using System.Collections;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;
using Trains.Common.GameState.Json;
using Trains.Common.Map;

namespace Trains.Common.GameState
{
    /// <summary>
    /// Represents a player's view of a snapshot of a game of Trains.
    /// </summary>
    [JsonConverter(typeof(PlayerGameStateJsonConverter))]
    public sealed class PlayerGameState : AbstractGameState
    {
        /// <summary>
        /// The player's inventory.
        /// </summary>
        public PlayerInventory PlayerInventory { get; }

        /// <summary>
        /// A list of hashsets for each player containing the connections they own.
        /// </summary>
        public ImmutableHashSet<Connection> PlayerClaimedConnections { get; }

        /// <summary>
        /// A hashset of all the claimed connection on the map.
        /// </summary>
        public ImmutableList<ImmutableHashSet<Connection>> ClaimedConnections { get; }

        /// <summary>
        /// A hashset of all the available connections on the map.
        /// </summary>
        public ImmutableHashSet<Connection> AvailableConnections { get; }


        /// <summary>
        /// Construct a <see cref="PlayerGameState"/> with all required information about the game. 
        /// </summary>
        /// <param name="gamePhase">Current phase of the game.</param>
        /// <param name="numberOfPlayers">Total number of players in a game.</param>
        /// <param name="playerClaimedConnections">A list of hashsets for each player containing the connections they own.</param>
        /// <param name="claimedConnections">A hashset of all the claimed connections on the map.</param>
        /// <param name="availableConnections">A hashset of all available connections on the map.</param>
        /// <param name="inventory">A PlayerInventory representing the players inventory</param>
        public PlayerGameState(GamePhase gamePhase, uint numberOfPlayers,
            ImmutableHashSet<Connection> playerClaimedConnections,
            ImmutableList<ImmutableHashSet<Connection>> claimedConnections,
            ImmutableHashSet<Connection> availableConnections,
            PlayerInventory inventory) : base(gamePhase,
            numberOfPlayers)
        {
            PlayerInventory = inventory;
            PlayerClaimedConnections = playerClaimedConnections;
            ClaimedConnections = claimedConnections;
            AvailableConnections = availableConnections;
        }


        /// <summary>
        /// Return whether the player can aquire a given connection.
        /// </summary>
        /// <param name="connection">A connection a player wishes to aquire.</param>
        /// <returns>Whether the connection can be claimed.</returns>
        public bool CanClaim(Connection connection)
        {
            return AvailableConnections.Contains(connection) &&
                   connection.Segments <= PlayerInventory.RailCards &&
                   connection.Segments <= PlayerInventory.ColoredCards[connection.Color];
        }

        public override bool Equals(object? obj)
        {
            if (obj is not PlayerGameState other)
            {
                return false;
            }

            // Check each player's claimed connection-set are equal since SequenceEqual does not use SetEqual for its
            // members.
            var equalClaimedConnections = true;
            for (int player = 0; player < ClaimedConnections.Count; player++)
            {
                equalClaimedConnections &= ClaimedConnections[player].SetEquals(other.ClaimedConnections[player]);
            }

            return PlayerInventory.Equals(other.PlayerInventory) &&
                   PlayerClaimedConnections.SetEquals(other.PlayerClaimedConnections) &&
                   AvailableConnections.SetEquals(other.AvailableConnections) &&
                   equalClaimedConnections;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerInventory, PlayerClaimedConnections, ClaimedConnections,
                AvailableConnections);
        }
    }
}