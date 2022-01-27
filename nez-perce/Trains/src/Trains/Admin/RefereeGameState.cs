using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Trains.Common.GameState;
using Trains.Common.Map;
using Trains.Player;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Trains.Tests.Admin")]

// TODO - Organize using partial

namespace Trains.Admin
{
    /// <summary>
    /// Represents the state of the game visible to the referee.
    /// </summary>
    public sealed class RefereeGameState : AbstractGameState, ICloneable
    {
        public List<IPlayer> Players { get; }

        /// <summary>
        /// Map of <see cref="IPlayer"/> to the <see cref="PlayerInventory"/> representing the cards and destinations they
        /// have.
        /// </summary>
        public Dictionary<IPlayer, PlayerInventory> PlayerInventories { get; }

        /// <summary>
        /// The deck of cards that a referee has access to and can draw from.
        /// </summary>
        public Deck Cards { get; }

        /// <summary>
        /// Map of <see cref="IPlayer"/> to the set of <see cref="Connection"/> that they have claimed.
        /// </summary>
        public Dictionary<IPlayer, HashSet<Connection>> ClaimedConnections { get; }

        /// <summary>
        /// The still available Connection on the TrainsMap for the game.
        /// </summary>
        public HashSet<Connection> AvailableConnections { get; set; }

        /// <summary>
        /// All IPlayer that have been kicked for misbehavior.
        /// </summary>
        public HashSet<IPlayer> KickedPlayers { get; }


        /// <summary>
        /// Construct a <see cref="RefereeGameState"/> with the phase of the game, number of players, currently active
        /// player, claimed connections, player inventories and deck of cards.
        /// </summary>
        /// <param name="gamePhase">The current phase of the game.</param>
        /// <param name="numberOfPlayers">The number of players in the game.</param>
        /// <param name="players">The list of all players in the game.</param>
        /// <param name="claimedConnections">The claimed connections that players own.</param>
        /// <param name="availableConnections">The set of connections available to be claimed.</param>
        /// <param name="playerInventories">The inventories of all players.</param>
        /// <param name="cards">The deck of cards the referee has access to hand out.</param>
        public RefereeGameState(GamePhase gamePhase, uint numberOfPlayers, List<IPlayer> players,
            Dictionary<IPlayer, HashSet<Connection>> claimedConnections, HashSet<Connection> availableConnections,
            Dictionary<IPlayer, PlayerInventory> playerInventories, Deck cards, HashSet<IPlayer>? kickedPlayers = null) : base(gamePhase, numberOfPlayers)
        {
            PlayerInventories = playerInventories;
            Cards = cards;
            Players = players;
            ClaimedConnections = claimedConnections;
            AvailableConnections = availableConnections;
            KickedPlayers = kickedPlayers == null ? new HashSet<IPlayer>() : kickedPlayers;
            foreach (var player in players)
            {
                if (!ClaimedConnections.ContainsKey(player))
                {
                    ClaimedConnections[player] = new();
                }
            }
        }

        /// <summary>
        /// Constructor for making a RefereeGameState where each Player has 0 owned Connection.
        /// </summary>
        /// <param name="numberOfPlayers">How many players are in the game</param>
        /// <param name="players">The ordered list of IPlayer playing the game. Index 0 is the oldest player and index count - 1 is the youngest.</param>
        /// <param name="availableConnections">The Connection that have not been acquired</param>
        /// <param name="deck">The Deck for this game</param>
        public RefereeGameState(uint numberOfPlayers, List<IPlayer> players, HashSet<Connection> availableConnections, Deck deck) :
            this(GamePhase.SetUp, numberOfPlayers, players, new(), availableConnections, new(), deck)
        {
            foreach (var player in players)
            {
                ClaimedConnections[player] = new();
            }
        }


        /// <summary>
        /// Gets the up-to-date PlayerGameState for the given IPlayer.
        /// </summary>
        /// <param name="player">The IPlayer whose PlayerGameState we want</param>
        /// <returns>A new copy of the PlayerGameState for the given IPlayer with up-to-date information.</returns>
        public PlayerGameState GetPlayerGameState(IPlayer player)
        {
            var claimedConnections = new List<ImmutableHashSet<Connection>>();
            var playerClaimedConnections = new HashSet<Connection>();
            foreach (var (p, connections) in ClaimedConnections)
            {
                if (player != p)
                    claimedConnections.Add(connections.ToImmutableHashSet());

            }

            return new PlayerGameState(Phase, NumberOfPlayers,
                ClaimedConnections[player].ToImmutableHashSet(),
                claimedConnections.ToImmutableList(),
                AvailableConnections.ToImmutableHashSet(),
                PlayerInventories[player]);
        }

        /// <summary>
        /// Determines if the given IPlayer can acquire the given Connection.
        /// </summary>
        /// <param name="player">The IPlayer who wants to acquire the Connection</param>
        /// <param name="connection">The Connection in question</param>
        /// <returns>True if the Player can acquire the Connection, false if otherwise</returns>
        public bool CanClaim(IPlayer player, Connection connection)
        {
            return AvailableConnections.Contains(connection) &&
                   connection.Segments <= PlayerInventories[player].RailCards &&
                   connection.Segments <= PlayerInventories[player].ColoredCards[connection.Color];
        }

        /// <summary>
        /// The given IPlayer acquires the given Connection.
        /// </summary>
        /// <param name="player">The IPlayer acquiring the Connection</param>
        /// <param name="connection">The Connection in question</param>
        /// <returns>True if the IPlayer acquires the Connection, false otherwise</returns>
        public bool ClaimConnection(IPlayer player, Connection connection)
        {
            if (!CanClaim(player, connection)) return false;

            PlayerInventories[player].RailCards -= connection.Segments;
            PlayerInventories[player].ColoredCards[connection.Color] -= connection.Segments;
            AvailableConnections.Remove(connection);
            ClaimedConnections[player].Add(connection);
            return true;
        }

        /// <summary>
        /// Does the given IPlayer have enough cards to continue playing.
        /// </summary>
        /// <param name="player">The IPlayer whose hand to check</param>
        /// <returns>True if the Player has less than or equal to 3 cards, false otherwise</returns>
        public bool IsGameOverByPlayerOutOfCards(IPlayer player)
        {
            return PlayerInventories[player].CardLeft() <= 3;
        }

        /// <summary>
        /// Finds the longest path in the given set of Connection that starts at the given Location.
        /// </summary>
        /// <param name="connections">The set of Connection the path will be made from</param>
        /// <param name="starting">The starting Location for the path</param>
        /// <returns>An int representing the length of the longest path starting from the given Location</returns>
        private static int LongestPath(ISet<Connection> connections, Location starting)
        {
            // using a modified A* algorithm to find LONGEST rather than shortest path
            Queue<Location> locationsToSee = new(new HashSet<Location> { starting });
            HashSet<Location> visited = new();
            Dictionary<Location, int> gScore = new() { { starting, 0 } };

            while (locationsToSee.TryDequeue(out var current))
            {
                visited.Add(current);

                foreach (var connection in new List<Connection>(connections))
                {
                    var loc1 = connection.Locations.ElementAt(0);
                    var loc2 = connection.Locations.ElementAt(1);
                    if (loc1 == current || loc2 == current)
                    {
                        connections.Remove(connection);
                        var newLoc = loc1 == current ? loc2 : loc1;
                        var newScore = gScore[current] + (int)connection.Segments;
                        if (!gScore.ContainsKey(newLoc) || gScore[newLoc] < newScore) // TODO verify changing || to && makes sense
                        {
                            gScore[newLoc] = newScore;
                            if (!visited.Contains(newLoc))
                            {
                                locationsToSee.Enqueue(newLoc);
                            }
                        }
                    }
                }
            }

            return gScore.Values.Max();
        }

        /// <summary>
        /// Finds the length of the longest path that the given IPlayer connects.
        /// </summary>
        /// <param name="player">The IPlayer whose longest path's length is desired</param>
        /// <returns>The length of the longest path</returns>
        public int LongestPath(IPlayer player)
        {
            var playerConnections = ClaimedConnections[player];
            HashSet<Location> playerLocations = new();
            foreach (var connection in playerConnections)
            {
                playerLocations.Add(connection.Locations.ElementAt(0));
                playerLocations.Add(connection.Locations.ElementAt(1));
            }

            int longest = 0;
            foreach (var location in playerLocations)
            {
                int pathFromLocation = LongestPath(new HashSet<Connection>(playerConnections), location);
                longest = Math.Max(longest, pathFromLocation);
            }

            return longest;
        }

        /// <summary>
        /// Finds the IPlayer with the longest length path.
        /// </summary>
        /// <returns>The IPlayer with the longest length path</returns>
        private List<IPlayer> PlayersWithLongestPath()
        {
            List<IPlayer> longestPlayers = new();
            var longestPath = 0;
            for (var index = 0; index < Players.Count; index++)
            {
                var player = Players[index];
                var playerPathLength = LongestPath(player);
                if (longestPath < playerPathLength)
                {
                    longestPlayers.Clear();
                    longestPath = playerPathLength;
                    longestPlayers.Add(player);
                }
                if (longestPath == playerPathLength)
                {
                    longestPlayers.Add(player);
                }
            }
            return longestPlayers;
        }

        /// <summary>
        /// Gets the score for the given IPlayer not including longest path
        /// </summary>
        /// <param name="player">The IPlayer whose score to find</param>
        /// <returns>The score as an int where pts are described on The Game: Trains</returns>
        public int GetScore(IPlayer player)
        {
            var segmentPoints = (int)ClaimedConnections[player].Sum(connection => connection.Segments);
            var destinationPoints = PlayerInventories[player].Destinations.Sum(destination =>
                destination.FulfilledBy(ClaimedConnections[player]) ? 10 : -10);

            return segmentPoints + destinationPoints;
        }

        /// <summary>
        /// Gets the rankings for the game.
        /// </summary>
        /// <returns>A Dictionary where the key is an IPlayer and the value is their score</returns>
        public Dictionary<IPlayer, int?> GetRankings()
        {
            Dictionary<IPlayer, int?> rankings = new();
            List<IPlayer> playersWithLongestPath = PlayersWithLongestPath();

            foreach (var player in Players)
            {
                rankings.Add(player, GetScore(player) + (playersWithLongestPath.Contains(player) ? 20 : 0));
            }

            foreach (var player in KickedPlayers)
            {
                rankings.Add(player, null);
            }
            return rankings;
        }


        /// <summary>
        /// Clones this into another RefereeGameState.
        /// </summary>
        /// <returns>A RefereeGameState that is a clone of this</returns>
        public object Clone()
        {
            Dictionary<IPlayer, PlayerInventory> playersInvs = new();
            foreach (var entry in PlayerInventories)
            {
                playersInvs.Add(entry.Key, new PlayerInventory(new Cards(entry.Value.ColoredCards), entry.Value.RailCards,
                    new HashSet<Destination>(entry.Value.Destinations)));
            }
            return new RefereeGameState(Phase, NumberOfPlayers, new List<IPlayer>(Players),
                new Dictionary<IPlayer, HashSet<Connection>>(ClaimedConnections), new HashSet<Connection>(AvailableConnections), playersInvs,
                new Deck(new Dictionary<Color, uint>(Cards.ColoredCards), new List<Destination>(Cards.Destinations)), KickedPlayers);
        }
        ///<inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is RefereeGameState other)
            {
                return Equals(other);
            }
            return false;
        }

        ///<inheritdoc/>
        private bool Equals(RefereeGameState other)
        {
            return Phase.Equals(other.Phase) && NumberOfPlayers.Equals(other.NumberOfPlayers) &&
                   Players.SequenceEqual(other.Players) && PlayerInventories.Keys.SequenceEqual(other.PlayerInventories.Keys) && PlayerInventories.Values.SequenceEqual(other.PlayerInventories.Values) &&
                   Cards.Equals(other.Cards) && ClaimedConnections.SequenceEqual(other.ClaimedConnections) &&
                   AvailableConnections.SetEquals(other.AvailableConnections) && KickedPlayers.SetEquals(other.KickedPlayers);
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(NumberOfPlayers);
        }
    }
}