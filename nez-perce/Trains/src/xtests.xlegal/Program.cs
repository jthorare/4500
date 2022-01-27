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

namespace xtests.xlegal
{
    internal static class Program
    {
        internal static void Main(string[] args)
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
            // Get the map object
            reader.Read();
            AcquiredJson wantedAcquired = serializer.Deserialize<AcquiredJson>(reader)!;

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

            Location location1 = map.Locations[wantedAcquired.City1];
            Location location2 = map.Locations[wantedAcquired.City2];
            Connection wantedConnection = new Connection(location1, location2, wantedAcquired.Length, wantedAcquired.Color);

            Console.WriteLine(JsonConvert.SerializeObject(playerGameState.CanClaim(wantedConnection)));
        }
    }
}
