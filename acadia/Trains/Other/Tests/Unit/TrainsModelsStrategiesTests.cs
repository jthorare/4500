using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Trains.Models.GamePieces;

namespace Tests
{
    [TestFixture]
    public class TrainsModelsStrategiesTests
    {
        /// <summary>
        /// Test for BuyNowStrategy.ChooseDestinations()
        /// </summary>
        [Test]
        public void TestBuyNowChooseDestinations()
        {
            Assert.That(TestVariables.buyNowStrategy.ChooseDestinations(TestVariables.valid, TestVariables.valid_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_bosNyc, TestVariables.dest_bosSea }));
            Assert.That(TestVariables.buyNowStrategy.ChooseDestinations(TestVariables.valid_discon, TestVariables.valid_discon_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_valid_discon_montgomery_pton }));
        }

        /// <summary>
        /// Test for BuyNowStrategy.ConductTurn()
        /// </summary>
        [Test]
        public void TestBuyNowConductTurn()
        {
            Assert.AreEqual(TestVariables.buyNowStrategy.ConductTurn(TestVariables.pgs_allAvail), TestVariables.acquireConnTurn);
            Assert.AreEqual(TestVariables.buyNowStrategy.ConductTurn(TestVariables.pgs_allOwned), TestVariables.drawCardsTurn);
        }

        /// <summary>
        /// Test for Hold10Strategy.ChooseDestinations()
        /// </summary>
        [Test]
        public void TestHold10ChooseDestinations()
        {
            Assert.That(TestVariables.hold10Strategy.ChooseDestinations(TestVariables.valid, TestVariables.valid_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_houSea, TestVariables.dest_laSea }));
            Assert.That(TestVariables.hold10Strategy.ChooseDestinations(TestVariables.valid_discon, TestVariables.valid_discon_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_valid_discon_pton_watchung }));
        }

        /// <summary>
        /// Test for Hold10Strategy.ChooseDestinations()
        /// </summary>
        [Test]
        public void TestDestinations()
        {
            Assert.That(TestVariables.buyConnectionLength3.ChooseDestinations(TestVariables.valid, TestVariables.valid_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_bosSea, TestVariables.dest_houSea }));
            Assert.That(TestVariables.onlyDrawCards.ChooseDestinations(TestVariables.valid, TestVariables.valid_dests), Is.EquivalentTo(new HashSet<Destination>() {
                TestVariables.dest_bosSea, TestVariables.dest_houSea }));

        }

        /// <summary>
        /// Test for Hold10Strategy.ConductTurn()
        /// </summary>
        [Test]
        public void TestHold10ConductTurn()
        {
            Assert.AreEqual(TestVariables.hold10Strategy.ConductTurn(TestVariables.pgs_allAvail), TestVariables.acquireConnTurn);
            Assert.AreEqual(TestVariables.hold10Strategy.ConductTurn(TestVariables.pgs_allOwned), TestVariables.drawCardsTurn);
        }

        /// <summary>
        /// Test for Hold10Strategy.ConductTurn()
        /// </summary>
        [Test]
        public void TestSetLengthConductTurn()
        {
            Assert.AreEqual(TestVariables.acquireConn3Turn, TestVariables.buyConnectionLength3.ConductTurn(TestVariables.pgs_allSmall2AllAvailable));
            Assert.AreEqual(TestVariables.acquireConn4Turn, TestVariables.buyConnectionLength4.ConductTurn(TestVariables.pgs_allSmall2AllAvailable));
            Assert.AreEqual(TestVariables.acquireConn5Turn, TestVariables.buyConnectionLength5.ConductTurn(TestVariables.pgs_allSmall2AllAvailable));
        }

        /// <summary>
        /// Test for Hold10Strategy.ConductTurn()
        /// </summary>
        [Test]
        public void TestOnlyDrawCardsConductTurn()
        {
            Assert.AreEqual(TestVariables.onlyDrawCards.ConductTurn(TestVariables.pgs_allAvail), TestVariables.drawCardsTurn);
        }
    }
}
