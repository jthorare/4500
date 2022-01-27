using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common.GameState;
using Trains.Common.Map;
using Trains.Player.Strategy;
using Trains.Player;

namespace Trains.Common
{
    public static class Utilities
    {
        public static bool EnoughDestinations(int playerCount, int destinationCount)
        {
            return destinationCount >=
                playerCount * Constants.destinationsPerPlayer + (Constants.destinationsToChooseFrom - Constants.destinationsPerPlayer);
        }

        public static string GetRandomString()
        {
            var random = new Random();
            var name = "";
            for (int i = 0; i < random.Next(5, 25); i++)
            {
                name += Char.ConvertFromUtf32(65 + random.Next(26));
            }
            return name;
        }
        public static IStrategy GetRandomStrategy(int seed)
        {
            Random rand = new Random(seed);
            int strat = rand.Next(3);
            switch (strat)
            {
                case 0:
                    return new Hold10Strategy();
                case 1:
                    return new BuyNowStrategy();
                default:
                    return new CheatStrategy();
            }
        }

        /// <summary>
        /// Generates random players for Trains.com.
        /// </summary>
        /// <param name="numPlayers">The number of players to generate.</param>
        /// <param name="name">Optional name parameter that is only used for generating a single random player</param>
        /// <returns>A list of numPlayers count players.</returns>
        public static List<IPlayer> GenerateRandomPlayers(int numPlayers, string? name = null)
        {
            if (numPlayers > 1) return GenerateNPlayers(numPlayers);
            return new List<IPlayer>() { new StrategyPlayer(name!, Utilities.GetRandomStrategy(new Random().Next())) };
        }

        /// <summary>
        /// Generates the given number of random players.
        /// </summary>
        /// <param name="numPlayers">The number of players to generate.</param>
        /// <returns>A list of numPlayers count players.</returns>
        public static List<IPlayer> GenerateNPlayers(int numPlayers)
        {
            List<IPlayer> players = new();
            for (int ii = 0; ii < numPlayers; ii++)
            {
                IPlayer player = new StrategyPlayer(Utilities.GetRandomString(), Utilities.GetRandomStrategy(ii));
                players.Add(player);
            }
            return players;
        }
    }
}
