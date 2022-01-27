using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Trains.Admin;
using Trains.Admin.Json;
using Trains.Common;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Json;
using Trains.Remote;

namespace xtest.xclients
{
    /// <summary>
    /// Test harness for xclients.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2) { throw new ArgumentException("Please supply a port number (and optional IP address)."); }
            int port = Int32.Parse(args[0]);
            IPAddress? ip = null;
            if (args.Length == 2)
            {
                ip = IPAddress.Parse(args[1]);
            }

            // inputs are Map, Array of PlayerInstances, Deck
            string json = JsonUtilities.ReadStdin();

            // Create a JsonReader to read the json
            var stream = new StringReader(json);
            JsonTextReader reader = new JsonTextReader(stream);
            reader.SupportMultipleContent = true;
            JsonSerializer serializer = new JsonSerializer();

            reader.Read(); // skip curly brace
            TrainsMap map = JsonUtilities.ReadMap(serializer, reader);
            List<IPlayer> players = JsonUtilities.ReadPlayers(serializer, reader, map);
            List<Task> clientPool = SpawnClients(ip, port, players);
            Task.WaitAll(clientPool.ToArray());
        }

        private static List<Task> SpawnClients(IPAddress? ip, int port, List<IPlayer> players)
        {
            List<Task> clientPool = new();
            foreach (IPlayer player in players)
            {
                Client client = new Client(player);
                clientPool.Add(Task.Run(() => client.ConnectToTrains(ip, port))); // do not await because it will continue the main method without waiting for 
                // client to connect
            }
            return clientPool;
        }
    }
}
