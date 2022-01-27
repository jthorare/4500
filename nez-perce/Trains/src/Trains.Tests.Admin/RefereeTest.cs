using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Admin;
using Trains.Common;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Strategy;
using System.Linq;

namespace Trains.Tests.Admin
{
    [TestClass]
    public class RefereeTest
    {
        private TrainsMap _map;
        private Referee _referee;
        private IPlayer _player1;
        private IPlayer _player2;
        private IPlayer _player3;

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

            _map = trainsMapBuilder.BuildMap();
            _player1 = new StrategyPlayer("Player 1", new Hold10Strategy());
            _player2 = new StrategyPlayer("Player 2", new Hold10Strategy());
            _player3 = new StrategyPlayer("Player 3", new BuyNowStrategy());
            List<IPlayer> players = new() { _player1, _player2, _player3 };
            Random random = new Random(3241343);
            _referee = new Referee(Constants.defaultDeck, Constants.setupPlayerRailsCount, Constants.setupPlayerCardsCount, _map, players, random);
        }


        [TestMethod]
        public void RefereeTestCloneEquality()
        {

            RefereeGameState actualGameState = _referee.GetRefereeGameState();
            RefereeGameState preTurnGameState = (RefereeGameState)actualGameState.Clone();
            Assert.IsTrue(actualGameState.Equals(preTurnGameState));
        }

        [TestMethod]
        public void RefereeTestCloneEqualityFalse()
        {

            RefereeGameState actualGameState = _referee.GetRefereeGameState();
            RefereeGameState preTurnGameState = (RefereeGameState)actualGameState.Clone();
            actualGameState.PlayerInventories[actualGameState.Players[0]].UpdateCard(new Dictionary<Color, uint>() { { Color.Green, 50 } });
            Assert.IsFalse(actualGameState.Equals(preTurnGameState));
        }

        [TestMethod]
        public void RefreeTestRun()
        {
            _referee.RunGame();
            Assert.IsTrue(_referee.GetRefereeGameState().Phase == Common.GameState.GamePhase.Finished);
        }

        [TestMethod]
        public void RefereeTestPostGameRankings()
        {
            Dictionary<IPlayer, int> expectedRankings = new Dictionary<IPlayer, int>() { { _player1, 17 }, { _player2, 10 }, { _player3, 37 } };
            var actualRanking = _referee.RunGame();
            Assert.IsTrue(expectedRankings.Count == actualRanking.Count && expectedRankings.All((p) => expectedRankings[p.Key] == actualRanking[p.Key]));
        }

        [TestMethod]
        public void RefereeGameStateTestGetScore()
        {
            _referee.RunGame();
            Assert.AreEqual(17, _referee.GetRefereeGameState().GetScore(_player1));
            Assert.AreEqual(10, _referee.GetRefereeGameState().GetScore(_player2));
            Assert.AreEqual(17, _referee.GetRefereeGameState().GetScore(_player3));
        }


        [TestMethod]
        public void RefereeGameStateLongestPath()
        {
            _referee.RunGame();
            Assert.AreEqual(9, _referee.GetRefereeGameState().LongestPath(_player1));
            Assert.AreEqual(6, _referee.GetRefereeGameState().LongestPath(_player2));
            Assert.AreEqual(10, _referee.GetRefereeGameState().LongestPath(_player3));
        }
    }
}