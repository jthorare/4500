using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Trains.Admin;
using Trains.Admin.Json;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Json;

namespace Trains.Common
{
    public static class JsonUtilities
    {

        /// <summary>
        /// Converts all ranks to the ranking string.
        /// </summary>
        /// <param name="ranking">The ranking from the game</param>
        /// <returns>A string representing the ranking and rank of a games outcome</returns>
        public static string RankingsToJson(Tuple<List<IPlayer>, List<IPlayer>> ranking)
        {
            string rankingJson = "[[";
            rankingJson += ToRank(ranking.Item1);
            rankingJson += "],";
            rankingJson += ToRank(ranking.Item2, true);
            rankingJson += "]";
            return rankingJson;
        }

        /// <summary>
        /// Converts a list of players to a Rank.
        /// </summary>
        /// <param name="players">The players of the rank</param>
        /// <returns>A string representing a JSON rank</returns>
        public static string ToRank(List<IPlayer> players, bool keepEmptyRank = false)
        {
            var pl = players.Select(players => "\"" + players.Name + "\"").ToList();
            pl.Sort();
            if (pl.Count == 0 && !keepEmptyRank) return "";
            string rankJson = "[";
            rankJson += String.Join(',', pl); // comma delimit each name
            rankJson += "]";
            return rankJson;
        }

        public static string ReadStdin()
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
        public static TrainsMap ReadMap(JsonSerializer serializer, JsonTextReader reader)
        {

            // Get the map
            TrainsMap map = serializer.Deserialize<TrainsMap>(reader)!;
            return map;
        }

        public static List<IPlayer> ReadPlayers(JsonSerializer serializer, JsonTextReader reader, TrainsMap map)
        {
            JsonPlayerInstanceConverter converter = new JsonPlayerInstanceConverter(map);
            List<IPlayer> players = (List<IPlayer>)converter.ReadJson(reader, null, null, serializer);
            return players;
        }

        public static Deck ReadDeck(JsonSerializer serializer, JsonTextReader reader, TrainsMap map)
        {
            JsonCardConverter converter = new JsonCardConverter();
            List<Color> cards = (List<Color>)converter.ReadJson(reader, null, null, serializer);
            return new StackedDeck(cards, new(), map.GetAllFeasibleDestinations().ToList(), new Random(1));
        }
    }
}
