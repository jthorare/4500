using NUnit.Framework;
using System;
using System.Collections.Generic;
using Trains.Models.GamePieces;

namespace Tests
{
    /// <summary>
    /// Class that holds all tests involving all GamePieces for Trains.com.
    /// </summary>
    [TestFixture]
    public class TrainsModelsGamePiecesTests
    {
        /// <summary>
        /// Test for City X property accessors
        /// </summary>
        [Test]
        public void TestCityGetXProperty()
        {
            Assert.AreEqual(TestVariables.boston.XPosition, 255);
            Assert.AreEqual(TestVariables.miami.XPosition, 255);
        }

        /// <summary>
        /// Test for City Y property accessors
        /// </summary>
        [Test]
        public void TestCityGetYProperty()
        {
            Assert.AreEqual(TestVariables.boston.YPosition, 30);
            Assert.AreEqual(TestVariables.miami.YPosition, 255);
        }

        /// <summary>
        /// Test for City Name property accessors
        /// </summary>
        [Test]
        public void TestCityGetNameProperty()
        {
            Assert.AreEqual(TestVariables.boston.CityName, "Boston");
            Assert.AreEqual(TestVariables.miami.CityName, "Miami");
        }

        /// <summary>
        /// Test for all Connection constructor exceptions.
        /// </summary>
        [Test]
        public void TestCityConstructorExceptions()
        {
            Assert.Throws<ArgumentException>(() => new City(-1, -1, null));
            Assert.Throws<ArgumentException>(() => new City(-1, -1, ""));
            Assert.Throws<ArgumentException>(() => new City(-1, -1, "Boston"));
            Assert.Throws<ArgumentException>(() => new City(1, -1, "Boston"));
            Assert.Throws<ArgumentException>(() => new City(-1, 1, "Boston"));
            Assert.Throws<ArgumentException>(() => new City(1, 1, ""));
            Assert.Throws<ArgumentException>(() => new City(1, 1, null));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston?"));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston!"));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston\\"));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston\t"));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Bost\non"));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston\r"));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston["));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston["));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Bostonbostonbostonbostonboston"));
            Assert.Throws<ArgumentException>(() => new City(0, 0, "Boston["));
        }

        /// <summary>
        /// Test for City.GetHashCode().
        /// </summary>
        [Test]
        public void TestCityGetHashCode()
        {
            Assert.AreEqual(HashCode.Combine("Seattle", 30, 0), TestVariables.seattle.GetHashCode());
            Assert.AreNotEqual(HashCode.Combine("Seattle", 30, 10), TestVariables.seattle.GetHashCode());
            Assert.AreNotEqual(HashCode.Combine("Seattle", 30, 0), new City(800, 800, "stuff").GetHashCode());
            Assert.AreNotEqual(HashCode.Combine("stuff", 800, 0), new City(800, 800, "stuff").GetHashCode());
            Assert.AreNotEqual(HashCode.Combine("stuff2", 800, 800), new City(800, 800, "stuff").GetHashCode());
            Assert.AreNotEqual(HashCode.Combine("stuff", 0, 800), new City(800, 800, "stuff").GetHashCode());
            Assert.AreNotEqual(HashCode.Combine(30, 0, "Seattle"), TestVariables.seattle.GetHashCode());
        }

        /// <summary>
        /// Test for City.Equals().
        /// </summary>
        [Test]
        public void TestCityEquals()
        {
            Assert.True(TestVariables.boston.Equals(TestVariables.boston));
            Assert.True(TestVariables.boston.Equals(new City(255, 30, "Boston")));
            Assert.True(TestVariables.boston.Equals(new City(255, 31, "Boston")));
            Assert.True(TestVariables.boston.Equals(new City(254, 30, "Boston")));
            Assert.True(TestVariables.boston.Equals(new City(255, 30, "Bostom")));
            Assert.False(TestVariables.boston.Equals(new City(254, 31, "Bostom")));
            Assert.False(TestVariables.boston.Equals(TestVariables.seattle));
            Assert.True(new City(255, 30, "Boston").Equals(TestVariables.boston));
        }

        /// <summary>
        /// Test for Connection City1 property accessors
        /// </summary>
        [Test]
        public void TestConnectionGetCity1Property()
        {
            Assert.AreEqual(TestVariables.bosSea.City1, new Connection(TestVariables.boston, TestVariables.seattle, GamePieceColor.Red, Connection.Length.Three).City1);
            Assert.AreEqual(TestVariables.nycMia.City1, new Connection(TestVariables.nyc, TestVariables.miami, GamePieceColor.Red, Connection.Length.Three).City1);
        }

        /// <summary>
        /// Test for Connection City2s property accessors
        /// </summary>
        [Test]
        public void TestConnectionGetCity2Property()
        {
            Assert.AreEqual(TestVariables.bosSea.City2, new Connection(TestVariables.boston, TestVariables.seattle, GamePieceColor.Red, Connection.Length.Three).City2);
            Assert.AreEqual(TestVariables.nycMia.City2, new Connection(TestVariables.nyc, TestVariables.miami, GamePieceColor.Red, Connection.Length.Three).City2);
        }

        /// <summary>
        /// Test for Connection Color property accessors
        /// </summary>
        [Test]
        public void TestConnectionGetColorProperty()
        {
            Assert.AreEqual(TestVariables.bosSea.Color, GamePieceColor.Blue);
            Assert.AreEqual(TestVariables.nycMia.Color, GamePieceColor.White);
        }

        /// <summary>
        /// Test for Connection Segments property accessors
        /// </summary>
        [Test]
        public void TestConnectionGetSegmentsProperty()
        {
            Assert.AreEqual(TestVariables.bosSea.NumSegments, Connection.Length.Five);
            Assert.AreEqual(TestVariables.nycMia.NumSegments, Connection.Length.Four);
        }

        /// <summary>
        /// Test for Connection.GetHashCode().
        /// </summary>
        [Test]
        public void TestConnectionGetHashCode()
        {
            Assert.AreEqual(HashCode.Combine(TestVariables.boston.GetHashCode() + TestVariables.seattle.GetHashCode(), (int)GamePieceColor.Blue), TestVariables.bosSea.GetHashCode());
            Assert.AreEqual(TestVariables.seaBos.GetHashCode(), TestVariables.bosSea.GetHashCode());
            Assert.AreNotEqual(HashCode.Combine(TestVariables.boston.GetHashCode() + TestVariables.seattle.GetHashCode(), (int)GamePieceColor.Green), TestVariables.bosSea.GetHashCode());
            Assert.AreNotEqual(HashCode.Combine(TestVariables.nyc.GetHashCode() + TestVariables.seattle.GetHashCode(), (int)GamePieceColor.Green), TestVariables.bosSea.GetHashCode());
            Assert.AreNotEqual(HashCode.Combine(TestVariables.boston.GetHashCode() + TestVariables.houston.GetHashCode(), (int)GamePieceColor.Green), TestVariables.bosSea.GetHashCode());
        }

        /// <summary>
        /// Test for all Connection.Equals().
        /// </summary>
        [Test]
        public void TestConnectionEquals()
        {
            Assert.AreEqual(TestVariables.bosSea, new Connection(TestVariables.boston, TestVariables.seattle, GamePieceColor.Blue, Connection.Length.Five));
            Assert.AreEqual(TestVariables.bosSea, TestVariables.bosSea);
            Assert.AreNotEqual(TestVariables.bosSea, TestVariables.houSea);
            Assert.AreNotEqual(TestVariables.laSea, TestVariables.laSea2); // different colors
            Assert.AreEqual(TestVariables.houSea, TestVariables.houSeaEqual); // different lengths
            Assert.AreEqual(TestVariables.bosSea, TestVariables.bosSeaEqual); // different Cities
        }

        /// <summary>
        /// Test for Map Cities property accessors
        /// </summary>
        [Test]
        public void TestMapGetCitiesProperty()
        {
            Assert.AreEqual(TestVariables.emptyMap.Cities, new HashSet<City>());
            Assert.AreEqual(TestVariables.valid_discon.Cities, new HashSet<City>() { TestVariables.anchorage, TestVariables.montgomery, TestVariables.princeton, TestVariables.watchung });
            Assert.AreEqual(TestVariables.valid.Cities, new HashSet<City>() { TestVariables.boston, TestVariables.seattle, TestVariables.houston, TestVariables.nyc, TestVariables.la, TestVariables.miami });
        }

        /// <summary>
        /// Test for Map Connections property accessors
        /// </summary>
        [Test]
        public void TestMapGetConnectionsProperty()
        {
            Assert.AreEqual(TestVariables.emptyMap.Connections, new List<Connection>());
            Assert.AreEqual(TestVariables.valid_discon.Connections, new List<Connection>() { TestVariables.montPrin, TestVariables.prinWatch, TestVariables.watchMontRed, TestVariables.watchMontBlue });
            Assert.AreEqual(TestVariables.valid.Connections, new List<Connection>() { TestVariables.bosSea, TestVariables.bosNyc, TestVariables.nycMia, TestVariables.nycHou, TestVariables.miaHou, TestVariables.houLa, TestVariables.houSea, TestVariables.laSea });
        }

        /// <summary>
        /// Test for Map Width property accessors
        /// </summary>
        [Test]
        public void TestMapGetWidthProperty()
        {
            Assert.AreEqual(TestVariables.emptyMap.Width, 500);
            Assert.AreEqual(TestVariables.valid_discon.Width, 500);
            Assert.AreEqual(TestVariables.valid.Width, 300);
        }

        /// <summary>
        /// Test for Map Height property accessors
        /// </summary>
        [Test]
        public void TestMapGetHeightProperty()
        {
            Assert.AreEqual(TestVariables.emptyMap.Height, 500);
            Assert.AreEqual(TestVariables.valid_discon.Height, 500);
            Assert.AreEqual(TestVariables.valid.Height, 300);
        }

        /// <summary>
        /// Test for all Map constructor exceptions.
        /// </summary>
        [Test]
        public void TestMapConstructorExceptions()
        {
            Assert.Throws<ArgumentException>(() => new Map(null, null, -1, -1));
            Assert.Throws<ArgumentNullException>(() => new Map(null, null, 10, 10));
            Assert.Throws<ArgumentNullException>(() => new Map(null, TestVariables.connections, 10, 10));
            Assert.Throws<ArgumentNullException>(() => new Map(TestVariables.cities, null, 300, 300));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, TestVariables.connections, 10, -1));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, TestVariables.connections, -1, 10));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, TestVariables.connections, -1, -1));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, TestVariables.connections, 10, 9));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, TestVariables.connections, 9, 10));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, TestVariables.connections, 10, 801));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, TestVariables.connections, 801, 10));
        }

        /// <summary>
        /// Test for duplicate handling in the List of City by the Map constructor.
        /// </summary>
        [Test]
        public void TestMapConstructorCityDuplicates()
        {
            Assert.AreEqual(new List<City>() { TestVariables.boston, TestVariables.seattle }, new Map(new List<City>() { TestVariables.boston, TestVariables.seattle }, TestVariables.connections, 300, 300).Cities);
            Assert.Throws<ArgumentException>(() => new Map(new List<City>() { TestVariables.boston, TestVariables.boston, TestVariables.boston, TestVariables.seattle, TestVariables.seattle }, TestVariables.connections, 300, 300));
            Assert.Throws<ArgumentException>(() => new Map(new List<City>() { TestVariables.boston, TestVariables.bostonEqual, TestVariables.boston, TestVariables.seattle, TestVariables.seattleEqual }, TestVariables.connections, 300, 300));
            Assert.Throws<ArgumentException>(() => new Map(new List<City>() { TestVariables.boston, TestVariables.boston2, TestVariables.boston, TestVariables.seattle, TestVariables.seattle2 }, TestVariables.connections, 300, 300));
        }

        /// <summary>
        /// Test for duplicate handling in the List of Connection by the Map constructor.
        /// </summary>
        [Test]
        public void TestMapConstructorConnectionDuplicates()
        {
            Assert.AreEqual(new List<Connection>() { TestVariables.houSea, TestVariables.laSea }, new Map(TestVariables.cities, new List<Connection>() { TestVariables.houSea, TestVariables.laSea }, 300, 300).Connections);
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, new List<Connection>() { TestVariables.houSea, TestVariables.laSea, TestVariables.houSea, TestVariables.laSea, TestVariables.houSea, TestVariables.laSea }, 300, 300));
            Assert.Throws<ArgumentException>(() => new Map(TestVariables.cities, new List<Connection>() { TestVariables.houSea, TestVariables.laSea, TestVariables.houSeaEqual, TestVariables.laSea2 }, 300, 300));
        }

        /// <summary>
        /// Test for Map AllCityNames function
        /// </summary>
        [Test]
        public void TestAllCityNames()
        {
            Assert.AreEqual(TestVariables.emptyMap.AllCityNames(), new HashSet<string>());
            ICollection<string> validCities = TestVariables.valid.AllCityNames();
            Assert.AreEqual(TestVariables.valid.AllCityNames(), new HashSet<string>() { "Boston", "Seattle", "Houston", "New York City", "Los Angeles", "Miami" });
            Assert.AreEqual(TestVariables.valid_discon.AllCityNames(), new HashSet<string>() { "Anchorage", "Montgomery", "Princeton", "Watchung Hills" });
        }

        /// <summary>
        /// Test for Map.AllFeasibleDestinations().
        /// </summary>
        [Test]
        public void TestAllFeasibleDestinations()
        {
            Assert.AreEqual(new HashSet<Destination>(), new Map(new HashSet<City>(), new HashSet<Connection>(), 400, 400).AllFeasibleDestinations());
            ICollection<Destination> destinations = TestVariables.valid.AllFeasibleDestinations();
            Assert.That(TestVariables.valid.AllFeasibleDestinations(), Is.EquivalentTo(new HashSet<Destination>() {
                new Destination(TestVariables.valid, TestVariables.boston, TestVariables.houston),
                new Destination(TestVariables.valid,TestVariables.boston,TestVariables. la),
                new Destination(TestVariables.valid,TestVariables. boston, TestVariables.miami),
                new Destination(TestVariables.valid, TestVariables.boston, TestVariables.nyc),
                new Destination(TestVariables.valid, TestVariables.boston, TestVariables.seattle),
                new Destination(TestVariables.valid, TestVariables.houston, TestVariables.la),
                new Destination(TestVariables.valid, TestVariables.houston, TestVariables.miami),
                new Destination(TestVariables.valid, TestVariables.houston, TestVariables.nyc),
                new Destination(TestVariables.valid, TestVariables.houston, TestVariables.seattle),
                new Destination(TestVariables.valid, TestVariables.la, TestVariables.miami),
                new Destination(TestVariables.valid, TestVariables.nyc, TestVariables.la),
                new Destination(TestVariables.valid, TestVariables.la, TestVariables.seattle),
                new Destination(TestVariables.valid, TestVariables.nyc, TestVariables.miami),
                new Destination(TestVariables.valid,TestVariables. seattle, TestVariables.miami),
                new Destination(TestVariables.valid, TestVariables.seattle, TestVariables.nyc)}));

            Assert.That(TestVariables.valid_discon.AllFeasibleDestinations(), Is.EquivalentTo(new HashSet<Destination>() {
                new Destination(TestVariables.valid_discon, TestVariables.montgomery, TestVariables.princeton),
                new Destination(TestVariables.valid_discon, TestVariables.montgomery, TestVariables.watchung),
                new Destination(TestVariables.valid_discon, TestVariables.princeton, TestVariables.watchung)}));
        }

        /// <summary>
        /// Test for Map.GetHashCode().
        /// </summary>
        [Test]
        public void TestMapGetHashCode()
        {
            int rv = HashCode.Combine(300, 300);
            foreach (City c in TestVariables.valid.Cities)
            {
                rv = HashCode.Combine(c, rv);
            }
            foreach (Connection c in TestVariables.valid.Connections)
            {
                rv = HashCode.Combine(c, rv);
            }
            Assert.AreEqual(TestVariables.valid.GetHashCode(), rv);
            Assert.AreNotEqual(TestVariables.valid_discon.GetHashCode(), rv + 2 ^ TestVariables.valid.Width.GetHashCode());
        }

        /// <summary>
        /// Test for Map.Equals().
        /// </summary>
        [Test]
        public void TestMapEquals()
        {
            Assert.AreEqual(TestVariables.valid, new Map(TestVariables.cities, TestVariables.connections, 300, 300));
            Assert.AreEqual(TestVariables.valid_discon, new Map(TestVariables.cities_discon, TestVariables.connections_discon, 500, 500));
            Assert.AreEqual(TestVariables.valid, TestVariables.valid2);
            Assert.AreNotEqual(TestVariables.valid, TestVariables.valid_discon);
        }

        /// <summary>
        /// Test for Destination CityOne property accessors
        /// </summary>
        [Test]
        public void TestDestinationGetCityOneProperty()
        {
            Assert.AreEqual(TestVariables.dest_valid_discon_montgomery_watchung.CityOne, TestVariables.montgomery);
            Assert.AreEqual(TestVariables.dest_valid_houston_seattle.CityOne, TestVariables.houston);
        }

        /// <summary>
        /// Test for Destination CityTwo property accessors
        /// </summary>
        [Test]
        public void TestDestinationGetCityTwoProperty()
        {
            Assert.AreEqual(TestVariables.dest_valid_discon_montgomery_watchung.CityTwo, TestVariables.watchung);
            Assert.AreEqual(TestVariables.dest_valid_houston_seattle.CityTwo, TestVariables.seattle);
        }

        /// <summary>
        /// Test for all Destination constructor exceptions.
        /// </summary>
        [Test]
        public void TestDestinationConstructorExceptions()
        {
            Assert.Throws<ArgumentException>(() => new Destination(TestVariables.emptyMap, TestVariables.montgomery, TestVariables.watchung));
            Assert.Throws<ArgumentException>(() => new Destination(TestVariables.valid_discon, TestVariables.anchorage, TestVariables.princeton));
            Assert.Throws<ArgumentException>(() => new Destination(TestVariables.valid_discon, TestVariables.seattle, TestVariables.princeton));
        }

        /// <summary>
        /// Test for Destination.GetHashCode().
        /// </summary>
        [Test]
        public void TestDestinationGetHashCode()
        {
            Assert.AreEqual(HashCode.Combine(TestVariables.houston.GetHashCode(), TestVariables.seattle.GetHashCode()), TestVariables.dest_valid_houston_seattle.GetHashCode());
            Assert.AreNotEqual(HashCode.Combine(TestVariables.watchung.GetHashCode(), TestVariables.montgomery.GetHashCode()), TestVariables.dest_valid_discon_montgomery_watchung.GetHashCode());
        }

        /// <summary>
        /// Test for Destination.Equals().
        /// </summary>
        [Test]
        public void TestDestinationEquals()
        {
            Assert.True(TestVariables.dest_valid_houston_seattle.Equals(TestVariables.dest_valid_houston_seattle2));
            Assert.False(TestVariables.dest_valid_discon_montgomery_watchung.Equals(TestVariables.dest_valid_houston_seattle));
            Assert.True(TestVariables.dest_valid_houston_seattle.Equals(new Destination(TestVariables.valid, TestVariables.seattle, TestVariables.houston)));
            Assert.True(TestVariables.dest_valid_houston_seattle.Equals(TestVariables.dest_valid_houston_seattle));
        }
        /// <summary>
        /// Test for ColoredCard.Equals().
        /// </summary>
        [Test]
        public void TestColoredCardEquals()
        {
            Assert.True(TestVariables.greenCard.Equals(TestVariables.greenCard));
            Assert.True(TestVariables.greenCard.Equals(new ColoredCard(GamePieceColor.Green)));
            Assert.False(TestVariables.greenCard.Equals(new ColoredCard(GamePieceColor.Red)));
            Assert.True(TestVariables.redCard.Equals(new ColoredCard(GamePieceColor.Red)));
            Assert.True(TestVariables.redCard.Equals(TestVariables.redCard));
        }
    }
}