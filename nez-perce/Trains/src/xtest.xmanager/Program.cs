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

namespace xtest.xmanager
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = JsonUtilities.ReadStdin();
            // Create a JsonReader to read the json
            var stream = new StringReader(json);
            JsonTextReader reader = new JsonTextReader(stream);
            reader.SupportMultipleContent = true;
            JsonSerializer serializer = new JsonSerializer();

            reader.Read(); // skip curly brace
            TrainsMap map = JsonUtilities.ReadMap(serializer, reader);
            List<IPlayer> players = JsonUtilities.ReadPlayers(serializer, reader, map);
            if (!Utilities.EnoughDestinations(players.Count, map.GetAllFeasibleDestinations().Count))
            {
                Console.WriteLine("\"error: not enough destinations\"\n");
                return;
            }
            reader.Read(); // skip curly brace;
            Deck deck = JsonUtilities.ReadDeck(serializer, reader, map);

            Manager manager = new Manager(players, deck);
            Tuple<List<IPlayer>, List<IPlayer>> ranking = manager.RunTournament();
            var output = JsonUtilities.RankingsToJson(ranking) + "\n";
            Console.WriteLine(output);
        }

    }
}
