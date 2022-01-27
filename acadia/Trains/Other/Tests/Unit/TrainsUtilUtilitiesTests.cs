using NUnit.Framework;
using System;
using System.Collections.Generic;
using Trains.Models.GamePieces;
using Trains.Util;

namespace Tests
{
    /// <summary>
    /// Class that holds all tests involving Utilities for Trains.com.
    /// </summary>
    [TestFixture]
    public class TrainsUtilUtilitiesTests
    {
        /// <summary>
        /// Test for Utilities.GetCardsOfColor()
        /// </summary>
        [Test]
        public void TestUtilitiesGetCardsOfColor()
        {
            Assert.AreEqual(Utilities.GetCardsOfColor(TestVariables.deck_3R2G, GamePieceColor.Red), 3);
            Assert.AreEqual(Utilities.GetCardsOfColor(TestVariables.deck_3R2G, GamePieceColor.Blue), 0);
            Assert.AreEqual(Utilities.GetCardsOfColor(TestVariables.deck_3R2G, GamePieceColor.Green), 2);
        }

        [Test]
        public void TestUtilitiesPathExistsAmongConnections()
        {
            Assert.True(Utilities.PathExistsAmongConnections(TestVariables.valid.Connections, TestVariables.seattle, TestVariables.miami, new HashSet<City>() { TestVariables.seattle }, TestVariables.valid, new HashSet<Destination>()));
            ICollection<Destination> destinations = new HashSet<Destination>();
            Utilities.PathExistsAmongConnections(TestVariables.valid.Connections, TestVariables.seattle, TestVariables.miami, new HashSet<City>() { TestVariables.seattle }, TestVariables.valid, destinations);
            Assert.AreEqual(destinations, new HashSet<Destination>() { TestVariables.dest_bosSea, TestVariables.dest_houSea, TestVariables.dest_laSea, TestVariables.dest_bosNyc });

            Assert.True(Utilities.PathExistsAmongConnections(TestVariables.valid_discon.Connections, TestVariables.montgomery, TestVariables.princeton, new HashSet<City>() { TestVariables.montgomery }, TestVariables.valid_discon, new HashSet<Destination>()));
            Assert.False(Utilities.PathExistsAmongConnections(TestVariables.valid_discon.Connections, TestVariables.anchorage, TestVariables.princeton, new HashSet<City>() { TestVariables.anchorage }, TestVariables.valid_discon, null));
            Assert.False(Utilities.PathExistsAmongConnections(TestVariables.valid_discon.Connections, TestVariables.princeton, TestVariables.anchorage, new HashSet<City>() { TestVariables.princeton }, TestVariables.valid_discon, null));
        }

        [Test]
        public void TestUtilitiesGetFirstObjectBounds()
        {
            Assert.AreEqual(Utilities.GetFirstObjectBounds(TestVariables.xlegal_1), new int[2] { 0, 423 });
            Assert.AreEqual(Utilities.GetFirstObjectBounds(TestVariables.xlegal_3), new int[2] { 0, 262 });
        }

        [Test]
        public void TestUtilitiesExtractObject()
        {
            Assert.AreEqual(Utilities.ExtractObject(TestVariables.xlegal_1), new string[2] { 
                TestVariables.xlegal_1_map, 
                TestVariables.xlegal_1_other 
            });
            Assert.AreEqual(Utilities.ExtractObject(TestVariables.xlegal_3), new string[2] { 
                TestVariables.xlegal_3_map, 
                TestVariables.xlegal_3_other 
            });
        }

        /// <summary>
        /// Test for Utilities.OrderByComparer()
        /// </summary>
        [Test]
        public void TestUtilitiesOrderByComparer()
        {
         //   Assert.AreEqual(Utilities.OrderByComparer(TestVariables.valid_dests, TestVariables.destComp), TestVariables.valid_dests_ordered);
            //Assert.AreEqual(Utilities.OrderByComparer(TestVariables.connections3, TestVariables.connComp), TestVariables.connections3_ordered);
           // Assert.AreEqual(Utilities.OrderByComparer(TestVariables.connections3, TestVariables.connComp), TestVariables.connections3_ordered);
            Assert.AreEqual(Utilities.OrderByComparer(TestVariables.cities, TestVariables.cityComp), TestVariables.cities_ordered);
        }

        /// <summary>
        /// Test for Utilities.ToColor()
        /// </summary>
        [Test]
        public void TestUtilitiesToColor()
        {
            Assert.AreEqual(Utilities.ToColor("red"), GamePieceColor.Red);
            Assert.AreEqual(Utilities.ToColor("blue"), GamePieceColor.Blue);
            Assert.AreEqual(Utilities.ToColor("green"), GamePieceColor.Green);
            Assert.AreEqual(Utilities.ToColor("white"), GamePieceColor.White);
            Assert.Throws<ArgumentException>(() => Utilities.ToColor("blah"));
        }

        /// <summary>
        /// Test for Utilities.ToLength()
        /// </summary>
        [Test]
        public void TestUtilitiesToLength()
        {
            Assert.AreEqual(Utilities.ToLength(3), Connection.Length.Three);
            Assert.AreEqual(Utilities.ToLength(4), Connection.Length.Four);
            Assert.AreEqual(Utilities.ToLength(5), Connection.Length.Five);
            Assert.Throws<ArgumentException>(() => Utilities.ToLength(6));
        }

        /// <summary>
        /// Test for Utilities.ToDestination()
        /// </summary>
        [Test]
        public void TestUtilitiesToDestination()
        {
            Assert.AreEqual(Utilities.ToDestination(new HashSet<string>() { "Princeton", "Montgomery" }, TestVariables.valid_discon), TestVariables.dest_valid_discon_montgomery_pton);
            Assert.AreEqual(Utilities.ToDestination(new HashSet<string>() { "Watchung Hills", "Montgomery" }, TestVariables.valid_discon), TestVariables.dest_valid_discon_montgomery_watchung);
        }

        /// <summary>
        /// Test for Utilities.GetCitiesFromNames()
        /// </summary>
        [Test]
        public void TestUtilitiesGetCitiesFromNames()
        {
            Assert.AreEqual(Utilities.GetCitiesFromNames(TestVariables.valid_discon, "Montgomery", "Princeton"), new List<City>() { TestVariables.montgomery, TestVariables.princeton });
            Assert.AreEqual(Utilities.GetCitiesFromNames(TestVariables.valid_discon, "Anchorage", "Watchung Hills"), new List<City>() { TestVariables.anchorage, TestVariables.watchung });
        }

        /// <summary>
        /// Test for Utilities.IsLegalMove()
        /// </summary>
        [Test]
        public void TestUtilitiesIsLegalMove()
        {
            Assert.True(Utilities.IsLegalMove(TestVariables.pgs_allAvail, TestVariables.montPrin));
            Assert.False(Utilities.IsLegalMove(TestVariables.pgs_allOwned, TestVariables.montPrin));
        }
    }
}
