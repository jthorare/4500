using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Trains.Common.Map;

namespace Trains.Common.GameState.Json
{
    public class PlayerStateJson
    {
        [JsonProperty("this")] public ThisPlayerJson This { get; set; }

        [JsonProperty("acquired")] public List<PlayerJson> Acquired { get; set; }

        public static PlayerStateJson ConvertFromPlayerGameState(PlayerGameState pgs)
        {
            var acquireds = pgs.PlayerClaimedConnections.Select(AcquiredJson.ConvertFromConnection).ToList();
            var thisAcquired = new PlayerJson(acquireds.Count > 0 ? acquireds : new());
            thisAcquired.Sort();

            var thisPlayerJson = new ThisPlayerJson
            {
                Destination1 = pgs.PlayerInventory.Destinations.First(),
                Destination2 = pgs.PlayerInventory.Destinations.Last(),
                Acquired = thisAcquired,
                Cards = new Cards(pgs.PlayerInventory.ColoredCards.Where((_, count) => count != 0)),
                Rails = pgs.PlayerInventory.RailCards
            };

            List<PlayerJson> acquired = pgs.ClaimedConnections.Select(playerClaims =>
                new PlayerJson(playerClaims.Select(AcquiredJson.ConvertFromConnection))).ToList();

            return new PlayerStateJson
            {
                This = thisPlayerJson,
                Acquired = acquired
            };
        }

        public PlayerGameState ConvertToPlayerGameState(TrainsMap map)
        {
            List<ImmutableHashSet<Connection>> claimedConnections = new List<ImmutableHashSet<Connection>>();
            HashSet<Connection> allClaimedConnection = new HashSet<Connection>();
            foreach (var player in Acquired)
            {
                HashSet<Connection> connections = new HashSet<Connection>();
                foreach (var con in player)
                {
                    Location loc1 = map.Locations[con.City1];
                    Location loc2 = map.Locations[con.City2];

                    Connection connection = new Connection(loc1, loc2, con.Length, con.Color);
                    connections.Add(connection);
                    allClaimedConnection.Add(connection);
                }

                claimedConnections.Add(connections.ToImmutableHashSet());
            }

            HashSet<Destination> playerDests = new HashSet<Destination>();
            playerDests.Add(This.Destination1);
            playerDests.Add(This.Destination2);

            HashSet<Connection> playerConnections = new HashSet<Connection>();
            foreach (var con in This.Acquired)
            {
                Location loc1 = map.Locations[con.City1];
                Location loc2 = map.Locations[con.City2];
                Connection connection = new Connection(loc1, loc2, con.Length, con.Color);
                playerConnections.Add(connection);
                allClaimedConnection.Add(connection);
            }

            PlayerInventory playerInv =
                new PlayerInventory(This.Cards, This.Rails, playerDests);

            HashSet<Connection> availableConnections =
                new HashSet<Connection>(map.Connections.Except(allClaimedConnection));

            PlayerGameState playerGameState = new PlayerGameState(GamePhase.InProgress, 1000,
                playerConnections.ToImmutableHashSet(),
                claimedConnections.ToImmutableList(), availableConnections.ToImmutableHashSet(), playerInv);

            return playerGameState;
        }
    }
}