using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Trains.Common;
using Trains.Common.GameState;
using Trains.Common.Map;
using Trains.Player.Strategy;

namespace Trains.Player
{
    /// <summary>
    /// Implementation of an IPlayer with a Strategy
    /// </summary>
    public class StrategyPlayer : IPlayer
    {
        private TrainsMap? _map;
        private PlayerGameState? _gameState;
        private readonly IStrategy _strategy;

        public string Name { get; }

        /// <summary>
        /// Constructor for making a house Player
        /// </summary>
        /// <param name="strategy"></param>
        public StrategyPlayer(string name, IStrategy strategy)
        {
            Name = name;
            _map = null;
            _gameState = null;
            _strategy = strategy;
        }

        /// <summary>
        /// Constructor for making a Player from a dynamically loaded strategy.
        /// Assumes that the namespace for the dynamically loaded strategy .cs file is "Trains.Player.Strategy"
        /// </summary>
        /// <param name="filepath">The filepath for the strategy to load</param>
        public StrategyPlayer(string name, string filepath)
        {
            Name = name;
            _map = null;
            _gameState = null;
            string[] nameSpace = filepath.Split('/');
            _strategy = new DynamicStrategy(filepath, $"{Constants.strategyQualifiedName}.{nameSpace[nameSpace.Length - 1].Substring(0, nameSpace[nameSpace.Length - 1].Length -3)}");
        }

        /// <inheritdoc />
        public void ReceiveGameState(PlayerGameState gameState)
        {
            _gameState = gameState;
        }

        /// <inheritdoc />
        public IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations)
        {
            return _strategy.ChooseDestinations(destinations);
        }

        /// <inheritdoc />
        public PlayerResponse PlayTurn(PlayerGameState gameState)
        {
            _gameState = gameState;
            return _strategy.PlayTurn(gameState);
        }

        /// <inheritdoc />
        public void IsWinner(bool isWinner)
        {
            // Do nothing as the Robot overlords do not care about winning
        }

        /// <inheritdoc />
        public void Setup(TrainsMap map, uint rails, Dictionary<Color, uint> cards)
        {
            ImmutableList<ImmutableHashSet<Connection>> claimedConnections = new List<ImmutableHashSet<Connection>>() { new HashSet<Connection>().ToImmutableHashSet() }.ToImmutableList();
            _gameState = new PlayerGameState(GamePhase.SetUp, int.MaxValue, claimedConnections, new HashSet<Connection>().ToImmutableHashSet(),
                new PlayerInventory(cards, rails, new HashSet<Destination>()));
            _map = map;
        }

        /// <inheritdoc />
        public void MoreCards(Dictionary<Color, uint> cards)
        {
            foreach (var card in cards)
            {
                _gameState.PlayerInventory.ColoredCards[card.Key] += card.Value;
            }
        }

        // TODO implement
        public TrainsMap Start(bool started)
        {
            return Constants.defaultMap;
        }

        // TODO implement
        public void End(bool win)
        {
        }
    }
}