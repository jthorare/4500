using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Trains.Common.GameState;
using Trains.Common.GameState.Json;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Strategy;

namespace xtests.xstrategy
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //Read lines from STDIN and save them.
            StringBuilder stringBuilder = new();
            StringWriter stringWriter = new(stringBuilder);

            string? line;
            while ((line = Console.ReadLine()) != null)
            {
                stringWriter.WriteLine(line);
            }
            var json = stringBuilder.ToString();

            // Create a JsonReader to read the json
            var stream = new StringReader(json);
            var reader = new JsonTextReader(stream);
            reader.SupportMultipleContent = true;
            var serializer = new JsonSerializer();


            // Get the map
            reader.Read();
            TrainsMap map = serializer.Deserialize<TrainsMap>(reader)!;
            // Get the Player Game State
            reader.Read();
            PlayerStateJson psj = serializer.Deserialize<PlayerStateJson>(reader);

            List<ImmutableHashSet<Connection>> claimedConnections = new List<ImmutableHashSet<Connection>>();
            HashSet<Connection> allClaimedConnection = new HashSet<Connection>();
            foreach (var player in psj.Acquired)
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
            playerDests.Add(psj.This.Destination1);
            playerDests.Add(psj.This.Destination2);

            HashSet<Connection> playerConnections = new HashSet<Connection>();
            foreach (var con in psj.This.Acquired)
            {
                Location loc1 = map.Locations[con.City1];
                Location loc2 = map.Locations[con.City2];
                Connection connection = new Connection(loc1, loc2, con.Length, con.Color);
                playerConnections.Add(connection);
                allClaimedConnection.Add(connection);
            }
            PlayerInventory playerInv = new PlayerInventory(psj.This.Cards, psj.This.Rails, playerDests);

            HashSet<Connection> availableConnections = new HashSet<Connection>(map.Connections.Except(allClaimedConnection));

            PlayerGameState playerGameState = new PlayerGameState(GamePhase.InProgress, 1000, playerConnections.ToImmutableHashSet(),
                claimedConnections.ToImmutableList(), availableConnections.ToImmutableHashSet(), playerInv);

            IPlayer p1 = new StrategyPlayer("p1", new Hold10Strategy());
            PlayerResponse playerResponse = p1.PlayTurn(playerGameState);
            switch (playerResponse.ResponseType)
            {
                case ResponseType.DrawCards:
                    Console.WriteLine(JsonConvert.SerializeObject("more cards"));
                    break;
                case ResponseType.ClaimConnection:
                    AcquiredJson acquired = new AcquiredJson()
                    {
                        City1 = playerResponse.RequestedConnectionClaim!.Locations.ElementAt(0).Name,
                        City2 = playerResponse.RequestedConnectionClaim!.Locations.ElementAt(1).Name,
                        Color = playerResponse.RequestedConnectionClaim!.Color,
                        Length = playerResponse.RequestedConnectionClaim!.Segments
                    };
                    Console.WriteLine(JsonConvert.SerializeObject(acquired));
                    break;
            }
        }
    }
}
