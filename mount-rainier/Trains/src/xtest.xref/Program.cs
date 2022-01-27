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

namespace xtest.xref
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = ReadStdin();
            // Create a JsonReader to read the json
            var stream = new StringReader(json);
            JsonTextReader reader = new JsonTextReader(stream);
            reader.SupportMultipleContent = true;
            JsonSerializer serializer = new JsonSerializer();

            reader.Read(); // skip curly brace
            TrainsMap map = ReadMap(serializer, reader);
            List<IPlayer> players = ReadPlayers(serializer, reader);
            if (!Utilities.EnoughCards(players.Count, map.GetAllFeasibleDestinations().Count))
            {
                Console.WriteLine("\"error: not enough destinations\"\n");
                return;
            }
            reader.Read(); // skip curly brace;
            Deck deck = ReadDeck(serializer, reader, map);
            Referee referee = new Referee(null, Constants.setupPlayerRailsCount, Constants.setupPlayerCardsCount, map, players, deck);
            Dictionary<IPlayer, int?> ranking = referee.RunGame();
            var output = RankingsToJson(ranking) + "\n";
            Console.WriteLine(output);
        }

        /// <summary>
        /// Converts all ranks to the ranking string.
        /// </summary>
        /// <param name="ranking">The ranking from the game</param>
        /// <returns>A string representing the ranking and rank of a games outcome</returns>
        static string RankingsToJson(Dictionary<IPlayer, int?> ranking)
        {
            string rankingJson = "[[";

            List<int?> distinctScores = ranking.Values.Distinct().OrderBy(x => x).ToList();
            for (int i = 0; i < distinctScores.Count; i++)
            {
                var players = ranking.Keys.Where(x => distinctScores[i] != null && ranking[x] == distinctScores[i]).ToList();
                if (players.Count == 0) continue;
                rankingJson += ToRank(players) + (i == distinctScores.Count - 1 ? "" : ",");
            }
            rankingJson += "],";
            rankingJson += ToRank(ranking.Keys.Where(x => ranking[x] == null).ToList(), true);
            rankingJson += "]";
            return rankingJson;
        }

        /// <summary>
        /// Converts a list of players to a Rank.
        /// </summary>
        /// <param name="players">The players of the rank</param>
        /// <returns>A string representing a JSON rank</returns>
        static string ToRank(List<IPlayer> players, bool keepEmptyRank = false)
        {
            var pl = players.Select(players => "\"" + players.Name + "\"").ToList();
            pl.Sort();
            if (pl.Count == 0 && !keepEmptyRank) return "";
            string rankJson = "[";
            rankJson += String.Join(',', pl); // comma delimit each name
            rankJson += "]";
            return rankJson;
        }

        static string ReadStdin()
        {
            //Read lines from STDIN and save them.
            StringBuilder stringBuilder = new();
            StringWriter stringWriter = new(stringBuilder);

            string? line;
            while ((line = Console.ReadLine()) != null)
            {
                stringWriter.WriteLine(line);
            }
            return stringBuilder.ToString();
        }
        static TrainsMap ReadMap(JsonSerializer serializer, JsonTextReader reader)
        {

            // Get the map
            TrainsMap map = serializer.Deserialize<TrainsMap>(reader)!;
            return map;
        }

        static List<IPlayer> ReadPlayers(JsonSerializer serializer, JsonTextReader reader)
        {
            JsonPlayerInstanceConverter converter = new JsonPlayerInstanceConverter();
            List<IPlayer> players = (List<IPlayer>)converter.ReadJson(reader, null, null, serializer);
            return players;
        }

        static Deck ReadDeck(JsonSerializer serializer, JsonTextReader reader, TrainsMap map)
        {
            JsonCardConverter converter = new JsonCardConverter();
            List<Color> cards = (List<Color>)converter.ReadJson(reader, null, null, serializer);
            return new StackedDeck(cards, new(), map.GetAllFeasibleDestinations().ToHashSet(), new Random(1));
        }
    }
}
