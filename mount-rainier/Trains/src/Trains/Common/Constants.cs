using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common.Map;

namespace Trains.Common
{
    /// <summary>
    /// <see langword="static"/> class for Trains.com constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Represents the string representation of the Hold-10 Strategy for dynamic loading.
        /// </summary>
        public static readonly string hold10Strategy = "Hold10Strategy";

        /// <summary>
        /// Represents the string representation of the Buy-Now Strategy for dynamic loading.
        /// </summary>
        public static readonly string buyNowStrategy = "BuyNowStrategy";

        /// <summary>
        /// Represents the string representation of the Cheat Strategy for dynamic loading.
        /// </summary>
        public static readonly string cheatStrategy = "CheatStrategy";

        /// <summary>
        /// Represents the maximum (inclusive) number of players needed for a Train.com game.
        /// </summary>
        public static readonly int maxPlayers = 8;

        /// <summary>
        /// Represents the minimum (inclusive) number of players needed for a Train.com game.
        /// </summary>
        public static readonly int minPlayers = 2;

        /// <summary>
        /// Represents the pts earned/loss for connecting/not connecting a Destination.
        /// </summary>
        public static readonly int destinationBonusPenalty = 10;
        /// <summary>
        /// Represents the name space required for a dynamically
        /// </summary>
        public static readonly string strategyQualifiedName = "Trains.Player.Strategy";
        /// <summary>
        /// Represents the longest time to wait before timing out in ms.
        /// </summary>
        public static readonly int maxResponseWait = 1000;

        /// <summary>
        /// How many Destination an IPlayer has during a Trains.com game.
        /// </summary>
        public static readonly int destinationsPerPlayer = 2;

        /// <summary>
        /// How many Destination an IPlayer gets to choose from during setup.
        /// </summary>
        public static readonly int destinationsToChooseFrom = 5;

        /// <summary>
        /// Buffer size in the x direction of the map visualizer.
        /// </summary>
        public static readonly int bufferX = 100;

        /// <summary>
        /// Buffer size in the y direction of the map visualizer.
        /// </summary>
        public static readonly int bufferY = 75;

        /// <summary>
        /// How many cards each IPlayer gets to during setup.
        /// </summary>
        public static readonly uint setupPlayerCardsCount = 4;

        /// <summary>
        /// How many cards each IPlayer gets to during setup.
        /// </summary>
        public static readonly uint setupPlayerRailsCount = 45;

        /// <summary>
        /// The default deck for a Trains.com game.
        /// </summary>
        public static readonly Dictionary<Color, uint> defaultDeck = new Dictionary<Color, uint>()
            {
                { Color.Blue, 62 },
                { Color.Green, 62 },
                { Color.White, 62 },
                { Color.Red, 64 }
            };

        /// <summary>
        /// The default TrainsMap for a Trains.com tournament to use.
        /// </summary>
        public static readonly TrainsMap defaultMap = DefaultMap();

        private static TrainsMap DefaultMap()
        {
            TrainsMap.TrainsMapBuilder trainsMapBuilder = new();

            trainsMapBuilder.AddLocation("Seattle", 0.1f, 0.1f);
            trainsMapBuilder.AddLocation("San Francisco", 0.2f, 0.7f);
            trainsMapBuilder.AddLocation("Chicago", 0.5f, 0.5f);
            trainsMapBuilder.AddLocation("Portland", 0.9f, 0.1f);
            trainsMapBuilder.AddLocation("Boston", 0.8f, 0.4f);
            trainsMapBuilder.AddLocation("Washington D.C.", 0.8f, 0.6f);
            trainsMapBuilder.AddLocation("Orlando", 0.9f, 0.9f);
            trainsMapBuilder.AddLocation("Anchorage", 0f, 0f);
            trainsMapBuilder.AddLocation("Honolulu", 0f, 0.9f);

            trainsMapBuilder.AddConnection("Seattle", "San Francisco", 4, Color.Red);
            trainsMapBuilder.AddConnection("Seattle", "Chicago", 5, Color.Blue);
            trainsMapBuilder.AddConnection("San Francisco", "Chicago", 5, Color.Red);
            trainsMapBuilder.AddConnection("Chicago", "Portland", 5, Color.White);
            trainsMapBuilder.AddConnection("Chicago", "Boston", 4, Color.Green);
            trainsMapBuilder.AddConnection("Chicago", "Washington D.C.", 4, Color.Green);
            trainsMapBuilder.AddConnection("Chicago", "Orlando", 3, Color.Blue);
            trainsMapBuilder.AddConnection("Portland", "Boston", 3, Color.Blue);
            trainsMapBuilder.AddConnection("Boston", "Washington D.C.", 3, Color.White);
            trainsMapBuilder.AddConnection("Washington D.C.", "Orlando", 3, Color.Blue);
            trainsMapBuilder.AddConnection("Anchorage", "Honolulu", 5, Color.Blue);

            return trainsMapBuilder.BuildMap();
        }
    }
}
