using System.Collections.Immutable;
using Trains.Common.Map;

namespace Trains.Common.GameState
{
    /// <summary>
    /// Represents a player's view of a snapshot of a game of Trains.
    /// </summary>
    //TODO - Converter for PlayerStateJson
    public sealed class PlayerGameState : AbstractGameState
    {
        /// <summary>
        /// The player's inventory.
        /// </summary>
        public PlayerInventory PlayerInventory { get; }

        /// <summary>
        /// A list of hashsets for each player containing the connections they own.
        /// </summary>
        ImmutableList<ImmutableHashSet<Connection>> ClaimedConnections { get; }

        /// <summary>
        /// A hashset of all the connection available on the map.
        /// </summary>
        public ImmutableHashSet<Connection> AvailableConnections { get; }


        /// <summary>
        /// Construct a <see cref="PlayerGameState"/> with all required information about the game. 
        /// </summary>
        /// <param name="gamePhase">Current phase of the game.</param>
        /// <param name="numberOfPlayers">Total number of players in a game.</param>
        /// <param name="claimedConnections">A list of hashsets for each player containing the connections they own.</param>
        /// <param name="availableConnections">A hashset of all the connection available on the map.</param>
        /// <param name="inventory">A PlayerInventory representing the players inventory</param>
        public PlayerGameState(GamePhase gamePhase, uint numberOfPlayers, 
            ImmutableList<ImmutableHashSet<Connection>> claimedConnections,
            ImmutableHashSet<Connection> availableConnections, PlayerInventory inventory) : base(gamePhase,
            numberOfPlayers)
        {
            PlayerInventory = inventory;
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
    }
}