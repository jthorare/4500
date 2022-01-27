using NUnit.Framework;
using System;
using System.Collections.Generic;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;

namespace Tests
{
    [TestFixture]
    public class TrainsModelsGameStatesTests
    {

        // Tests for PlayerGameState
        /// <summary>
        /// Test for PlayerGameState GameMap property accessors
        /// </summary>
        [Test]
        public void TestPlayerGameStateGetGameMapProperty()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0.GameMap, TestVariables.valid_discon);
            Assert.AreEqual(TestVariables.pgs_allSmall1.GameMap, TestVariables.valid_discon);
            Assert.AreNotEqual(TestVariables.pgs_allSmall1.GameMap, TestVariables.valid);
        }

        /// <summary>
        /// Test for PlayerGameState Rails property accessors
        /// </summary>
        [Test]
        public void TestPlayerGameStateGetRailsProperty()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0.Rails, 35);
            Assert.AreEqual(TestVariables.pgs_allSmall1.Rails, 45);
            Assert.AreNotEqual(TestVariables.pgs_allSmall1.Rails, 40);
        }

        /// <summary>
        /// Test for PlayerGameState Cards property accessors
        /// </summary>
        [Test]
        public void TestPlayerGameStateGetCardsProperty()
        {

            Assert.AreEqual(TestVariables.pgs_allSmall0.Cards, new List<ColoredCard>() { new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) });
            Assert.AreEqual(TestVariables.pgs_allSmall1.Cards, TestVariables.pgs_allSmall1.Cards);
            Assert.AreNotEqual(TestVariables.pgs_allSmall1.Cards, new List<ColoredCard>() { new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue) });
        }


        /// <summary>
        /// Test for PlayerGameState Destinations property accessors
        /// </summary>
        [Test]
        public void TestPlayerGameStateGetDestinationsProperty()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0.Destinations, new HashSet<Destination>() { TestVariables.dest_valid_discon_montgomery_watchung, TestVariables.dest_valid_discon_pton_watchung });
            Assert.AreNotEqual(TestVariables.pgs_allSmall1.Destinations, new HashSet<Destination>() { TestVariables.dest_bosNyc });
            Assert.AreNotEqual(TestVariables.pgs_allSmall1.Destinations, new HashSet<Destination>());
        }

        /// <summary>
        /// Test for PlayerGameState OwnedConnections property accessors
        /// </summary>
        [Test]
        public void TestPlayerGameStateGetOwnedConnectionsProperty()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0.OwnedConnections, new HashSet<Connection>() { TestVariables.montPrin, TestVariables.prinWatch, TestVariables.watchMontRed, TestVariables.watchMontBlue });
            Assert.AreEqual(TestVariables.pgs_allSmall1.OwnedConnections, new HashSet<Connection>() { TestVariables.montPrin, TestVariables.prinWatch, TestVariables.watchMontRed, TestVariables.watchMontBlue });
            Assert.AreNotEqual(TestVariables.pgs_allSmall1.OwnedConnections, new HashSet<Connection>() { TestVariables.montPrin, TestVariables.prinWatch, TestVariables.watchMontRed });
        }

        /// <summary>
        /// Test for PlayerGameState AvailableConnections property accessors
        /// </summary>
        [Test]
        public void TestPlayerGameStateGetAvailableConnectionsProperty()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0AllAvailable.AvailableConnections, new HashSet<Connection>() { TestVariables.montPrin, TestVariables.prinWatch, TestVariables.watchMontRed, TestVariables.watchMontBlue });
            Assert.AreEqual(TestVariables.pgs_allSmall1AllAvailable.AvailableConnections, new HashSet<Connection>() { TestVariables.montPrin, TestVariables.prinWatch, TestVariables.watchMontRed, TestVariables.watchMontBlue });
            Assert.AreNotEqual(TestVariables.pgs_allSmall1.AvailableConnections, new HashSet<Connection>() { TestVariables.montPrin, TestVariables.prinWatch, TestVariables.watchMontRed });
        }

        /// <summary>
        /// Test for PlayerGameState.HasRouteBetween()
        /// </summary>
        [Test]
        public void TestPlayerGameStateHasRouteBetween()
        {
            Assert.IsTrue(TestVariables.pgs_allSmall0.HasRouteBetween(TestVariables.montgomery, TestVariables.watchung));
            Assert.IsFalse(TestVariables.pgs_allSmall0.HasRouteBetween(TestVariables.montgomery, TestVariables.anchorage));
            Assert.IsTrue(TestVariables.pgs_allSmall1.HasRouteBetween(TestVariables.montgomery, TestVariables.watchung));
            Assert.IsFalse(TestVariables.pgs_allSmall1.HasRouteBetween(TestVariables.anchorage, TestVariables.watchung));
        }

        /// <summary>
        /// Test for PlayerGameState.Equals()
        /// </summary>
        [Test]
        public void TestPlayerGameStateEquals()
        {
            Assert.IsTrue(TestVariables.pgs_allSmall0.Equals(TestVariables.pgs_allSmall0));
            Assert.IsTrue(TestVariables.pgs_allSmall0.Equals(TestVariables.pgs_allSmall0Dup));
            Assert.IsFalse(TestVariables.pgs_allSmall0.Equals(TestVariables.pgs_allSmall1));
            Assert.IsFalse(TestVariables.pgs_allSmall0.Equals(TestVariables.pgs_allSmall0AllAvailable));
        }

        /// <summary>
        /// Test for PlayerGameState.GetHashCode()
        /// </summary>
        [Test]
        public void TestPlayerGameStateGetHashCode()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0.GetHashCode(), TestVariables.pgs_allSmall0.GetHashCode());
            Assert.AreEqual(TestVariables.pgs_allSmall0.GetHashCode(), TestVariables.pgs_allSmall0Dup.GetHashCode());
            Assert.AreNotEqual(TestVariables.pgs_allSmall0.GetHashCode(), TestVariables.pgs_allSmall1.GetHashCode());
            Assert.AreNotEqual(TestVariables.pgs_allSmall0.GetHashCode(), TestVariables.pgs_allSmall0AllAvailable.GetHashCode());
        }

        // Tests for RefereeGameState
        /// <summary>
        /// Test for RefereeGameState.GetGameMap()
        /// </summary>
        [Test]
        public void TestRefereeGameStateGetGameMap()
        {
            Assert.AreEqual(TestVariables.valid, TestVariables.rgs.GameMap);
            Assert.AreNotEqual(TestVariables.validJson, TestVariables.rgs.GameMap);
        }

        /// <summary>
        /// Test for RefereeGameState.GetPlayerGameStates()
        /// </summary>
        [Test]
        public void TestRefereeGameStateGetPlayerGameStates()
        {
            Assert.AreEqual(new HashSet<PlayerGameState>() { TestVariables.pgs_allSmall0, TestVariables.pgs_allSmall1 }, TestVariables.rgs.PlayerGameStates);
            Assert.AreNotEqual(new HashSet<PlayerGameState>() { TestVariables.pgs_allSmall0, TestVariables.pgs_allSmall0 }, TestVariables.rgs.PlayerGameStates);
        }

        /// <summary>
        /// Test for RefereeGameState.CanCurrentPlayerAcquireConnection()
        /// </summary>
        [Test]
        public void TestRefereeGameStateCanCurrentPlayerAcquireConnection()
        {
            Assert.IsTrue(TestVariables.rgs2.CanCurrentPlayerAcquireConnection(TestVariables.miaHou));
            Assert.IsFalse(TestVariables.rgs.CanCurrentPlayerAcquireConnection(TestVariables.bosSea));
        }

        /// <summary>
        /// Test for RefereeGameState.Equals()
        /// </summary>
        [Test]
        public void TestRefereeGameStateEquals()
        {
            Assert.IsTrue(TestVariables.rgs.Equals(TestVariables.rgs));
            Assert.IsTrue(TestVariables.rgs.Equals(TestVariables.rgs_copy));
            Assert.IsFalse(TestVariables.rgs.Equals(TestVariables.rgs2));
        }

        /// <summary>
        /// Test for RefereeGameState.GetHashCode()
        /// </summary>
        [Test]
        public void TestRefereeGameStateGetHashCode()
        {
            Assert.AreEqual(TestVariables.rgs.GetHashCode(), TestVariables.rgs.GetHashCode());
            Assert.AreEqual(TestVariables.rgs_copy.GetHashCode(), TestVariables.rgs_copy.GetHashCode());
            Assert.AreEqual(TestVariables.rgs.GetHashCode(), TestVariables.rgs_copy.GetHashCode());
            Assert.AreNotEqual(TestVariables.rgs2.GetHashCode(), TestVariables.rgs_copy.GetHashCode());
        }
    }
}
