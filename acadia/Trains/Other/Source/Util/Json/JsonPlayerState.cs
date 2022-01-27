using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;

namespace Trains.Util.Json
{
    /// <summary>
    /// Represents the state of the active player requesting an action, and as much as it can know about the remaining players,
    /// as specified by https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._playerstate%29.
    /// </summary>
    public class JsonPlayerState
    {
        [JsonProperty("this")]
        public JsonThisPlayer ThisPlayer { get; set; }

        /// <summary>
        /// The other player's owned connections represented as a 2D array of JsonAcquired.
        /// The outer array's entries each represents a player as specified by https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._player%29.
        /// The inner array consists of said player's acquired connections as specified by https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29.
        /// </summary>
        [JsonProperty("acquired")]
        public ICollection<HashSet<JsonAcquired>> OtherPlayers { get; set; }

        // JsonConstructor tag indicates the constructor to use in deserialization.
        [JsonConstructor]
        // empty constructor used because can't use 'this' JsonProperty name as constructor parameter name,
        // which Newtonsoft requires. The object gets created and then its properties mutated.
        public JsonPlayerState() { }

        // constructor used to make JsonPlayerState objects from PlayerGameState objects for use in testing.
        public JsonPlayerState(PlayerGameState playerGameState)
        {
            ThisPlayer = new JsonThisPlayer(playerGameState);
            ICollection<Connection> otherPlayerOwnedConnections = playerGameState.GameMap.Connections.Where(conn => !playerGameState.AvailableConnections.Contains(conn) && !playerGameState.OwnedConnections.Contains(conn)
            ).ToList();
            OtherPlayers = new HashSet<HashSet<JsonAcquired>>() { ConnectionsToAcquireds(otherPlayerOwnedConnections).ToHashSet() };
        }

        /// <summary>
        /// Convert this JSON representation of a player's game state into a PlayerGameState object.
        /// </summary>
        /// <returns>Return a PlayerGameState from this JSON representation of a player state.</returns>
        public PlayerGameState ToPlayerGameState(Map map)
        {
            IList<ColoredCard> cards = this.ThisPlayer.GetColoredCards();
            ICollection<Destination> destinations = new HashSet<Destination>()
            {
                Utilities.ToDestination(this.ThisPlayer.Destination1, map),
                Utilities.ToDestination(this.ThisPlayer.Destination2, map)
            };
            ICollection<Connection> ownedConnections = new HashSet<Connection>();
            foreach (JsonAcquired conn in this.ThisPlayer.AcquiredConnections)
            {
                ownedConnections.Add(conn.ToConnection(map));
            }
            ICollection<Connection> otherPlayers = new HashSet<Connection>();
            foreach (ICollection<JsonAcquired> playerConnections in this.OtherPlayers)
            {
                foreach (JsonAcquired conn in playerConnections)
                {
                    otherPlayers.Add(conn.ToConnection(map));
                }
            }
            ICollection<Connection> availConnections = map.Connections.Where(conn => !otherPlayers.Contains(conn) && !ownedConnections.Contains(conn)).ToHashSet();
            return new PlayerGameState(map, this.ThisPlayer.Rails, cards, destinations, ownedConnections, availConnections);
        }

        /// <summary>
        /// Converts a Collection of Connections to a Collection of JsonAcquired.
        /// </summary>
        /// <param name="connections">The Collection of Connections to convert</param>
        /// <returns>The Collection of JsonAcquired where each element has an equivalent Connection in the given argument</returns>
        private ICollection<JsonAcquired> ConnectionsToAcquireds(ICollection<Connection> connections)
        {
            ICollection<JsonAcquired> jsonAcquireds = new HashSet<JsonAcquired>();
            foreach (Connection conn in connections)
            {
                jsonAcquireds.Add(new JsonAcquired(conn.City1.CityName, conn.City2.CityName, conn.Color.ToString().ToLower(), (int)conn.NumSegments));
            }
            return jsonAcquireds;
        }
    }
}
