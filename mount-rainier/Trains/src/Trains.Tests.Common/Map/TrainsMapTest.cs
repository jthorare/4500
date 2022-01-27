using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Common.Map;

namespace Trains.Tests.Common.Map
{
    [TestClass]
    public class TrainsMapTest
    {
        private TrainsMap _map = null!;

        /// <summary>
        /// Initialize map with values
        /// </summary>
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
        }

        // /// <summary>
        // /// Test situations where constructing a map explicitly should fail.
        // /// </summary>
        // [TestMethod]
        // public void TrainsMapTestInvalidConstruction()
        // {
        //     // The connections should have locations that are in the supplied list.
        //     Location seattle = new("Seattle", 0.1f, 0.1f);
        //     Location sanFrancisco = new("San Francisco", 0.2f, 0.7f);
        //     Location chicago = new("Chicago", 0.5f, 0.5f);
        //     Dictionary<string, Location> locations = new()
        //     {
        //         { "Seattle", seattle },
        //         { "San Francisco", sanFrancisco }
        //     };
        //     Connection connection = new (seattle, chicago, 5, Color.Blue);
        //     List<Connection> connections = new()
        //     {
        //         connection
        //     };
        //     Assert.ThrowsException<ArgumentException>(() => new TrainsMap(locations, connections));
        //     
        //     // Make setup valid.
        //     seattle.AddConnection(connection);
        //     chicago.AddConnection(connection);
        //     locations.Add("Chicago", chicago);
        //     var _ = new TrainsMap(locations, connections);
        //     
        //     // All location connections should be in the supplied list.
        //     Connection connection2 = new (seattle, sanFrancisco, 5, Color.Blue);
        //     seattle.AddConnection(connection2);
        //     sanFrancisco.AddConnection(connection2);
        //     Assert.ThrowsException<ArgumentException>(() => new TrainsMap(locations, connections));
        //     
        // }
        
        /// <summary>
        /// Validate that the CityNames property returns the expected set of city names.
        /// </summary>
        [TestMethod]
        public void TrainsMapTestCityNames()
        {
            Assert.IsTrue(_map.CityNames.SetEquals(
                new HashSet<string>
                {
                    "Seattle", "San Francisco", "Chicago", "Portland", "Boston", "Washington D.C.", "Orlando",
                    "Anchorage", "Honolulu"
                }
            ));
        }

        /// <summary>
        /// Validate that the Destinations property returns the expected sets of locations.
        /// </summary>
        [TestMethod]
        public void TrainsMapDestinations()
        {
            var expectedDestinations = new HashSet<HashSet<string>>
            {
                new() { "Seattle", "San Francisco", "Chicago", "Portland", "Boston", "Washington D.C.", "Orlando" },
                new() { "Honolulu", "Anchorage" }
            };
            var destinations = _map.ConnectedLocations;

            foreach (var destinationSet in _map.ConnectedLocations)
                Assert.IsTrue(expectedDestinations.Any(it => destinationSet.SetEquals(it)));
        }

        /// <summary>
        /// Validate that the GetConnectionsFor method returns the expected set of locations.
        /// </summary>
        [TestMethod]
        public void TrainsMapTestConnectionsFor()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                _map.GetConnectionsFor("City Name"));

            foreach (var (locaionName, location) in _map.Locations)
            {
                var connectionList = location.Connections;
                Assert.IsTrue(_map.GetConnectionsFor(locaionName).All(it => connectionList.Contains(it)));
            }
        }

        /// <summary>
        /// Validate that the GetDestinationsFor method returns the expected set of locations.
        /// </summary>
        [TestMethod]
        public void TrainsMapTestDestinationsFor()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                _map.GetConnectedLocationsFor("City Name"));

            var expectedDestinations = new HashSet<string>
                { "Seattle", "San Francisco", "Chicago", "Portland", "Boston", "Washington D.C.", "Orlando" };

            foreach (var destination in expectedDestinations)
                Assert.IsTrue(_map.GetConnectedLocationsFor(destination).SetEquals(expectedDestinations));

            expectedDestinations = new HashSet<string>
                { "Anchorage", "Honolulu" };

            foreach (var destination in expectedDestinations)
                Assert.IsTrue(_map.GetConnectedLocationsFor(destination).SetEquals(expectedDestinations));
        }
        
        /// <summary>
        /// Validate that the IsValidDestination method returns the expected results for destinations.
        /// </summary>
        [TestMethod]
        public void TrainsMapTestIsValidDestination()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                _map.IsValidDestination("City Name", "Boston"));
            Assert.ThrowsException<ArgumentException>(() =>
                _map.IsValidDestination("Boston", "City Name"));
            Assert.ThrowsException<ArgumentException>(() =>
                _map.IsValidDestination("City Name", "City Name"));

            var expectedDestinations = new HashSet<string>
                { "Seattle", "San Francisco", "Chicago", "Portland", "Boston", "Washington D.C.", "Orlando" };

            foreach (var destination1 in expectedDestinations)
                foreach (var destination2 in expectedDestinations)
                    Assert.IsTrue(_map.IsValidDestination(destination1, destination2));

            expectedDestinations = new HashSet<string>
                { "Anchorage", "Honolulu" };

            foreach (var destination1 in expectedDestinations)
                foreach (var destination2 in expectedDestinations)
                    Assert.IsTrue(_map.IsValidDestination(destination1, destination2));
        }
    }
}