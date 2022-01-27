using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;

namespace Tests
{
    /// <summary>
    /// Class for Unit Testing Trains.Util.Json
    /// </summary>
    public class TrainsUtilJsonTests
    {
        // Tests for JsonMap
        /// <summary>
        /// Test to verify that an Trains.Util.Json.JsonMap.ToMap() resulting Map is equivalent to the Map it SHOULD represent
        /// </summary>
        [Test]
        public void TestJsonMapToMap()
        {
            Assert.AreEqual(TestVariables.validJson.ToMap(), TestVariables.valid_discon);
        }

        /// <summary>
        /// Test to verify that an Trains.Util.Json.JsonMap.ToTrainsConnections() resulting Collection of Connection is equivalent to the Collection it SHOULD represent
        /// </summary>
        [Test]
        public void TestToTrainsConnections()
        {
            HashSet<Connection> conns = TestVariables.validJson.ToTrainsConnections().ToHashSet();
            Assert.IsTrue(conns.SetEquals(TestVariables.connections_discon));
        }

        /// <summary>
        /// Test to verify that Trains.Util.Json.JsonMap.IsDestination() returns the correct bool value for two City names.
        /// </summary>
        [Test]
        public void TestIsDestination()
        {
            Assert.IsTrue(TestVariables.validJson.IsDestination(new string[2] { "Montgomery", "Watchung Hills" }));
            Assert.IsTrue(TestVariables.validJson.IsDestination(new string[2] { "Watchung Hills", "Montgomery" }));
            Assert.IsFalse(TestVariables.validJson.IsDestination(new string[2] { "Anchorage", "Montgomery" }));
            Assert.IsFalse(TestVariables.validJson.IsDestination(new string[2] { "Montgomery", "Anchorage" }));
        }

        // Test for JsonAcquired
        /// <summary>
        /// Test to verify that Trains.Util.Json.JsonAcquired.ToConnection() returns the correct Connection value.
        /// </summary>
        [Test]
        public void TestJsonAcquiredToConnection()
        {
            Assert.AreEqual(TestVariables.bosSea, TestVariables.acquired.ToConnection(TestVariables.valid));
            Assert.AreEqual(TestVariables.houLa, TestVariables.acquired2.ToConnection(TestVariables.valid));
        }

        // Test for JsonPlayerState
        /// <summary>
        /// Test to verify that Trains.Util.Json.JsonPlayerState.ToPlayerGameState() returns the correct PlayerGameState value.
        /// </summary>
        [Test]
        public void TestJsonPlayerStateToPlayerGameState()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0, TestVariables.jsonPlayerState.ToPlayerGameState(TestVariables.pgs_allSmall0.GameMap));
            Assert.AreEqual(TestVariables.pgs_allOwned, TestVariables.jsonPlayerState2.ToPlayerGameState(TestVariables.pgs_allOwned.GameMap));
        }

        // Test for JsonThisPlayer
        /// <summary>
        /// Test to verify that Trains.Util.Json.JsonThisPlayer.GetColoredCards() returns the correct Collection of ColoredCard value.
        /// </summary>
        [Test]
        public void TestJsonThisPlayerGetColoredCards()
        {
            Assert.AreEqual(TestVariables.pgs_allSmall0.Cards, TestVariables.jsonThisPlayer.GetColoredCards());
            Assert.AreEqual(TestVariables.pgs_allSmall1.Cards, TestVariables.jsonThisPlayer1.GetColoredCards());
        }
    }
}
