using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Trains.Admin;
using Trains.Admin.Json;
using Trains.Common;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Json;
using Trains.Remote;

namespace xtest.xserver
{
    /// <summary>
    /// Test harness for XServer.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) { throw new ArgumentException("Please supply a port number"); }
            int port = Int32.Parse(args[0]);
            // inputs are Map, Array of PlayerInstances, Deck
            string json = JsonUtilities.ReadStdin();
            // Create a JsonReader to read the json
            var stream = new StringReader(json);
            JsonTextReader reader = new JsonTextReader(stream);
            reader.SupportMultipleContent = true;
            JsonSerializer serializer = new JsonSerializer();

            reader.Read(); // skip curly brace
            TrainsMap map = JsonUtilities.ReadMap(serializer, reader);
            reader.Read(); // skip curly brace;
            object players = serializer.Deserialize<object>(reader);
            reader.Read(); // skip curly brace;
            Deck deck = JsonUtilities.ReadDeck(serializer, reader, map);

            Server server = new Server(port, deck);

            try
            {
                Tuple<List<IPlayer>, List<IPlayer>> ranking = server.RunServer();
                var output = JsonUtilities.RankingsToJson(ranking) + "\n";
                Console.WriteLine(output);
            }
            catch (MapException notEnoughDests)
            {
                Console.WriteLine(notEnoughDests.Message);
            }
        }
    }
}
