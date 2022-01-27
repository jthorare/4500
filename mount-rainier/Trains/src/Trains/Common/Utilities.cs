using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common.Map;
using Trains.Player.Strategy;
using Trains.Player;

namespace Trains.Common
{
    public static class Utilities
    {
        public static bool EnoughCards(int playerCount, int destinationCount)
        {
            for (int i = 0; i < playerCount; i++)
            {
                if (destinationCount < Constants.destinationsToChooseFrom) { return false; }
                else { destinationCount = destinationCount - Constants.destinationsPerPlayer; }
            }
            return true;
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

        public static List<IPlayer> GenerateRandomPlayers(int numPlayers)
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
