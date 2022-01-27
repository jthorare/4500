using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Trains.Common;
using Trains.Common.GameState;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Strategy;

namespace Trains.Tests.Player.Strategy
{
    [TestClass]
    public class StrategyTest
    {
        private readonly IStrategy _hold10 = new Hold10Strategy();
        private readonly IStrategy _buyNow = new BuyNowStrategy();
        private IImmutableSet<Destination> _chooseFrom;
        private PlayerInventory _noCardsMaxRails;
        private PlayerInventory _maxCardsMaxRails;
        private PlayerGameState _pgs_noCards;
        private PlayerGameState _pgs_MaxCards;
        private PlayerGameState _pgs_NoConnections;
        private IList<Connection> _connections;
        [TestInitialize]
        public void Initialize()
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

            var map = trainsMapBuilder.BuildMap();
            _connections = map.Connections;
            List<Destination> destinationList = map.GetAllFeasibleDestinations().ToList();
            destinationList.Sort();
            _chooseFrom = destinationList.Take(Convert.ToInt32(Constants.destinationsToChooseFrom)).ToImmutableHashSet();
            _noCardsMaxRails = new PlayerInventory(new Dictionary<Color, uint>(), Constants.setupPlayerRailsCount, new HashSet<Destination>());
            _maxCardsMaxRails = new PlayerInventory(Constants.defaultDeck, Constants.setupPlayerRailsCount, new HashSet<Destination>());
            _pgs_noCards = new PlayerGameState(GamePhase.InProgress, int.MaxValue, new List<ImmutableHashSet<Connection>>() { new HashSet<Connection>().ToImmutableHashSet() }.ToImmutableList(),
               new HashSet<Connection>().ToImmutableHashSet(), _noCardsMaxRails);
            _pgs_MaxCards = new PlayerGameState(GamePhase.InProgress, int.MaxValue, new List<ImmutableHashSet<Connection>>() { new HashSet<Connection>().ToImmutableHashSet() }.ToImmutableList(),
                map.Connections.ToImmutableHashSet(), _maxCardsMaxRails);
            _pgs_NoConnections = new PlayerGameState(GamePhase.InProgress, int.MaxValue, new List<ImmutableHashSet<Connection>>() { new HashSet<Connection>().ToImmutableHashSet() }.ToImmutableList(),
                 new HashSet<Connection>().ToImmutableHashSet(), _maxCardsMaxRails);
        }

        [TestMethod]
        public void ChooseDestinationsTest()
        {
            var expected = _chooseFrom.TakeLast(Convert.ToInt32(Constants.destinationsToChooseFrom - Constants.destinationsPerPlayer)).ToImmutableHashSet();
            var actual = _hold10.ChooseDestinations(_chooseFrom);
            Assert.IsTrue(expected.SetEquals(actual));
            expected = _chooseFrom.Take(Convert.ToInt32(Constants.destinationsToChooseFrom - Constants.destinationsPerPlayer)).ToImmutableHashSet();
            actual = _buyNow.ChooseDestinations(_chooseFrom);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod]
        public void PlayTurnTest()
        {
            PlayerResponse hold10 = _hold10.PlayTurn(_pgs_noCards);
            Assert.IsTrue(hold10.Equals(PlayerResponse.DrawCard()));
            Assert.IsTrue(_buyNow.PlayTurn(_pgs_noCards).Equals(PlayerResponse.DrawCard()));
            var actual = _hold10.PlayTurn(_pgs_MaxCards);
            var expected = PlayerResponse.ClaimConnection(_connections.Where(x => x.Locations.ElementAt(0).Name == "Anchorage").FirstOrDefault());
            Assert.IsTrue(actual.Equals(expected));
            actual = _buyNow.PlayTurn(_pgs_MaxCards);
            Assert.IsTrue(actual.Equals(expected));
        }
    }
}