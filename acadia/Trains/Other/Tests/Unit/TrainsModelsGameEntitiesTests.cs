using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Trains.Models.GameEntities;
using Trains.Models.GamePieces;
using Trains.Models.Strategies;

namespace Tests
{
    /// <summary>
    /// Class that holds all tests involving all GamePieces for Trains.com.
    /// </summary>
    [TestFixture]
    public class TrainsModelsGameEntitiesTests
    {
        /// <summary>
        /// Test for Player.ChooseDestinations()
        /// </summary>
        [Test]
        public void TestPlayerChooseDestinations()
        {
            Assert.That(TestVariables.player.ChooseDestinations(TestVariables.valid_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_houSea, TestVariables.dest_laSea }));
            Assert.That(TestVariables.player.ChooseDestinations(TestVariables.valid_discon_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_valid_discon_pton_watchung }));
        }

        /// <summary>
        /// Test for Player.PlayTurn()
        /// </summary>
        [Test]
        public void TestPlayerPlayTurn()
        {
            Assert.AreEqual(TestVariables.player.PlayTurn(TestVariables.pgs_allAvail), TestVariables.acquireConnTurn);
            Assert.AreEqual(TestVariables.player.PlayTurn(TestVariables.pgs_allOwned), TestVariables.drawCardsTurn);
        }

        /// <summary>
        /// Test for Referee constructor
        /// </summary>
        [Test]
        public void TestRefereeConstructor()
        {
            Assert.Throws<ArgumentException>(() => new Referee(TestVariables.deck251, 1));
        }

        /// <summary>
        /// Test for Referee.ShuffleCards
        /// </summary>
        [Test]
        public void TestRefereeShuffleCardsEqual()
        {
            Assert.AreEqual(TestVariables.referee1.ShuffleCards(TestVariables.deck250), TestVariables.referee2.ShuffleCards(TestVariables.deck250));
            Assert.AreEqual(TestVariables.referee1.ShuffleCards(TestVariables.deck251), TestVariables.referee2.ShuffleCards(TestVariables.deck251));
        }

        /// <summary>
        /// Test for Referee.ShuffleCards
        /// </summary>
        [Test]
        public void TestRefereeShuffleCardsNotEqual()
        {
            Assert.AreNotEqual(TestVariables.deck250, TestVariables.referee3.ShuffleCards(TestVariables.deck250));
            Assert.AreNotEqual(TestVariables.referee1.ShuffleCards(TestVariables.deck251), TestVariables.referee3.ShuffleCards(TestVariables.deck251));
        }

        /// <summary>
        /// Test for Referee.ShuffleCards
        /// </summary>
        [Test]
        public void TestRefereeSelectDestinations()
        {
            CollectionAssert.AreEquivalent(TestVariables.referee1.SelectDestinations(TestVariables.valid_allDests), TestVariables.referee2.SelectDestinations(TestVariables.valid_allDests));
            CollectionAssert.AreNotEquivalent(TestVariables.referee1.SelectDestinations(TestVariables.valid_allDests), TestVariables.referee3.SelectDestinations(TestVariables.valid_allDests));
        }

        /// <summary>
        /// Test for Referee.ShuffleCards
        /// </summary>
        [Test]
        public void TestRefereePlayGame()
        {
            //IDictionary<IPlayer, int> result = TestVariables.referee1.PlayGame(TestVariables.valid, TestVariables.drawCardPlayers);
            //Assert.AreEqual(result, TestVariables.drawCardsPlayersRankings)
            //;
            IDictionary<IPlayer, int> rank = TestVariables.referee1.PlayGame(TestVariables.valid, TestVariables.buyPlayers);

            Assert.AreEqual(rank, TestVariables.buyPlayersRankings);
            //Assert.AreEqual(TestVariables.referee1.PlayGame(TestVariables.valid, TestVariables.holdNowBuy10Players), TestVariables.hold10BuyNowPlayersRankings);
        }
    }
}
