using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.Strategies;
using Trains.Models.TurnTypes;

namespace Trains.Models.GameEntities
{
    /// <summary>
    /// Class that implements the Player interface described in ~/Trains/Planning/player-interface.md
    /// </summary>
    public class Player : IPlayer
    {
        /// <summary>
        /// The IStrategy this Player uses for deciding behavior in a Trains.com game.
        /// </summary>
        private IStrategy Strategy { get; }

        /// <summary>
        /// The GameMap this Player's playing on.
        /// </summary>
        private Map GameMap { get; set; }

        /// <summary>
        /// The number of rail pieces this Player has left in a Trains.com game.
        /// </summary>
        private int Rails { get; set; }

        /// <summary>
        /// The IDictionary containing this Player's Trains.com cards. A Key is the ColoredCard and the Value is how many of that ColoredCard this Player has left.
        /// </summary>
        private IList<ColoredCard> Cards { get; set; } = new List<ColoredCard>();

        /// <summary>
        /// The Destination this Player is trying to connect in this Trains.com game.
        /// </summary>
        private ICollection<Destination> Destinations { get; set; } = new HashSet<Destination>();

        /// <summary>
        /// Constructor for a Player object
        /// </summary>
        /// <param name="strategyPath">The file path to a strategy class</param>
        public Player(string strategyPath)
        {
            Strategy = LoadStrategy(strategyPath);
        }

        /// <summary>
        /// Constructor for use in testing.
        /// </summary>
        /// <param name="strat">The Strategy object for the Player to use.</param>
        public Player(IStrategy strat)
        {
            Strategy = strat;
        }

        public void Setup(Map map, int rails, IList<ColoredCard> cards)
        {
            this.GameMap = map;
            this.Rails = rails;
            MoreCards(cards);
        }

        public void MoreCards(IList<ColoredCard> cards)
        {
            foreach (ColoredCard card in cards)
            {
                Cards.Add(card);
            }
        }

        public ICollection<Destination> ChooseDestinations(ICollection<Destination> destinations)
        {
            Destinations = Strategy.ChooseDestinations(this.GameMap, destinations);
            return Destinations;
        }

        public ITurn PlayTurn(PlayerGameState pgs)
        {
            return Strategy.ConductTurn(pgs);
        }

        public void GameOver(bool won) { }

        /// <summary>
        /// Loads the IStrategy from the given file path. Assumes the file path points to a .dll file which is a compiled Dynamic Linked Library for C# that allows
        /// dynamic loading.
        /// </summary>
        /// <param name="strategyPath">The file path to an IStrategy implementation Dynamic Linked Library file.</param>
        /// <returns>An instance of the IStrategy loaded from the given file path</returns>
        private IStrategy LoadStrategy(string strategyPath)
        {
            string assemblyPath = Path.GetFullPath(strategyPath);
            Assembly stratAssembly = Assembly.LoadFrom(assemblyPath);
            Type type = stratAssembly.GetTypes().FirstOrDefault();
            IStrategy strategy;
            if (type.GetInterfaces().Contains(typeof(IStrategy)))
            {
                strategy = (IStrategy)Activator.CreateInstance(type);
            } else
            {
                throw new Exception("Loaded assembly does not implement IStrategy.");
            }
            return strategy;
        }
    }
}