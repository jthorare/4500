using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Util
{
    /// <summary>
    /// Static class that is used for constants across various files.
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// Represents the amount of a Connection's length that should be used for the length of the gap between each segment.
        /// </summary>
        public static readonly double connectionGapSize = 0.05;
        /// <summary>
        /// Represents the maximum number of characters that a City name can be.
        /// </summary>
        public static readonly int maximumCityNameLength = 25;
        /// <summary>
        /// Represents the number of destinations a Player chooses when setting up a Trains.Com game.
        /// </summary>
        public static readonly int setupDestinationsCount = 2;
        /// <summary>
        /// Represents the number of rails a Player receives when setting up a Trains.Com game.
        /// </summary>
        public static readonly int setupNumberRails = 45;
        /// <summary>
        /// Represents the number of cards in a deck of ColoredCard when setting up a Trains.Com game.
        /// </summary>
        public static readonly int startingDeckCardsCount = 250;
        /// <summary>
        /// Represents the number of cards a Player receives when setting up a Trains.Com game.
        /// </summary>
        public static readonly int setupDealtPlayerCards = 4;
        /// <summary>
        /// Represents the number of cards a Player receives when drawing cards for its turn in a Trains.Com game.
        /// </summary>
        public static readonly int drawCardsCount = 2;
        /// <summary>
        /// Represents the number of values in a JsonAcquired.
        /// </summary>
        public static readonly int jsonAcquiredSize = 4;
        /// <summary>
        /// Represents the number of Destination in the collection that the Referee offers to a Player to select from.
        /// </summary>
        public static readonly int destinationSelectionPoolSize = 5;
        /// <summary>
        /// Represents the minimum number of rails a Player can have before causing the last round sequence.
        /// </summary>
        public static readonly int minimumPlayerRails = 3;
        /// <summary>
        /// Represents the point bonus a Player receives for having the longest, continuous, simple, acyclic path in the game.
        /// </summary>
        public static readonly int longestPathBonus = 20;
        /// <summary>
        /// Represents the point bonus a Player receives for connecting one of its Destination.
        /// </summary>
        public static readonly int connectedDestinationBonus = 10;
        /// <summary>
        /// Represents the minimum number of players needed for one game of Trains.
        /// </summary>
        public static readonly int minGamePlayerCount = 2;
        /// <summary>
        /// Represents the maximum number of players needed for one game of Trains.
        /// </summary>
        public static readonly int maxGamePlayerCount = 8;
        /// <summary>
        /// Represents the number of milliseconds that a Player object has to return from a function call in a Trains game.
        /// </summary>
        public static readonly int playerTimeAllotted = 1000;
        /// <summary>
        /// Represents the number of milliseconds that a Trains game can take.
        /// </summary>
        public static readonly int maxGameDuration = 60000;
    }
}
