using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Common.Map;

namespace Trains.Tests.Common.Map
{
    [TestClass]
    public class MapBuilderTest
    {
        private TrainsMap.TrainsMapBuilder _trainsMapBuilder = null!;

        /// <summary>
        /// Test various cases where AddLocations succeeds and situations where it should fail.
        /// </summary>
        [TestMethod]
        public void TestMapBuilderAddLocations()
        {
            _trainsMapBuilder = new TrainsMap.TrainsMapBuilder();

            _trainsMapBuilder.AddLocation("Seattle", 0.1f, 0.1f);
            _trainsMapBuilder.AddLocation("San Francisco", 0.2f, 0.7f);
            _trainsMapBuilder.AddLocation("Chicago", 0.5f, 0.5f);
            _trainsMapBuilder.AddLocation("Portland", 0.9f, 0.1f);
            _trainsMapBuilder.AddLocation("Boston", 0.8f, 0.4f);
            _trainsMapBuilder.AddLocation("Washington D.C.", 0.8f, 0.6f);
            _trainsMapBuilder.AddLocation("Orlando", 0.9f, 0.9f);
            _trainsMapBuilder.AddLocation("Anchorage", 0f, 0f);
            _trainsMapBuilder.AddLocation("Honolulu", 0f, 0.9f);

            Assert.ThrowsException<ArgumentException>(() =>
                _trainsMapBuilder.AddLocation("Honolulu", 0f, 0.9f));
        }
        /// <summary>
        /// Test various cases where AddConnections succeeds and situations where it should fail.
        /// </summary>
        [TestMethod]
        public void TestMapBuilderAddConnections()
        {
            _trainsMapBuilder = new TrainsMap.TrainsMapBuilder();

            _trainsMapBuilder.AddLocation("Seattle", 0.1f, 0.1f);
            _trainsMapBuilder.AddLocation("San Francisco", 0.2f, 0.7f);
            _trainsMapBuilder.AddLocation("Chicago", 0.5f, 0.5f);
            _trainsMapBuilder.AddLocation("Portland", 0.9f, 0.1f);
            _trainsMapBuilder.AddLocation("Boston", 0.8f, 0.4f);
            _trainsMapBuilder.AddLocation("Washington D.C.", 0.8f, 0.6f);
            _trainsMapBuilder.AddLocation("Orlando", 0.9f, 0.9f);
            _trainsMapBuilder.AddLocation("Anchorage", 0f, 0f);
            _trainsMapBuilder.AddLocation("Honolulu", 0f, 0.9f);

            _trainsMapBuilder.AddConnection("Seattle", "San Francisco", 4, Color.Red);
            _trainsMapBuilder.AddConnection("Seattle", "Chicago", 5, Color.Blue);
            _trainsMapBuilder.AddConnection("San Francisco", "Chicago", 5, Color.Red);
            _trainsMapBuilder.AddConnection("Chicago", "Portland", 5, Color.White);
            _trainsMapBuilder.AddConnection("Chicago", "Boston", 4, Color.Green);
            _trainsMapBuilder.AddConnection("Chicago", "Washington D.C.", 4, Color.Green);
            _trainsMapBuilder.AddConnection("Chicago", "Orlando", 3, Color.Blue);
            _trainsMapBuilder.AddConnection("Portland", "Boston", 3, Color.Blue);
            _trainsMapBuilder.AddConnection("Boston", "Washington D.C.", 3, Color.White);
            _trainsMapBuilder.AddConnection("Washington D.C.", "Orlando", 3, Color.Blue);
            _trainsMapBuilder.AddConnection("Anchorage", "Honolulu", 5, Color.Blue);

            // Do nothing if Connection already exists
            _trainsMapBuilder.AddConnection("Anchorage", "Honolulu", 5, Color.Blue);
            
            // Location does not exist
            Assert.ThrowsException<ArgumentException>(() =>
                _trainsMapBuilder.AddConnection("asdf", "Honolulu", 5, Color.Blue));
            Assert.ThrowsException<ArgumentException>(() =>
                _trainsMapBuilder.AddConnection("Anchorage", "asdf", 5, Color.Blue));
            Assert.ThrowsException<ArgumentException>(() =>
                _trainsMapBuilder.AddConnection("asdf", "jkl;", 5, Color.Blue));

        }

        /// <summary>
        /// Test that a MapBuilder does not fail on building a Map.
        /// </summary>
        [TestMethod]
        public void TestMapBuilderBuildMap()
        {
            _trainsMapBuilder = new TrainsMap.TrainsMapBuilder();

            _trainsMapBuilder.AddLocation("Seattle", 0.1f, 0.1f);
            _trainsMapBuilder.AddLocation("San Francisco", 0.2f, 0.7f);
            _trainsMapBuilder.AddLocation("Chicago", 0.5f, 0.5f);
            _trainsMapBuilder.AddLocation("Portland", 0.9f, 0.1f);
            _trainsMapBuilder.AddLocation("Boston", 0.8f, 0.4f);
            _trainsMapBuilder.AddLocation("Washington D.C.", 0.8f, 0.6f);
            _trainsMapBuilder.AddLocation("Orlando", 0.9f, 0.9f);
            _trainsMapBuilder.AddLocation("Anchorage", 0f, 0f);
            _trainsMapBuilder.AddLocation("Honolulu", 0f, 0.9f);

            _trainsMapBuilder.AddConnection("Seattle", "San Francisco", 4, Color.Red);
            _trainsMapBuilder.AddConnection("Seattle", "Chicago", 5, Color.Blue);
            _trainsMapBuilder.AddConnection("San Francisco", "Chicago", 5, Color.Red);
            _trainsMapBuilder.AddConnection("Chicago", "Portland", 5, Color.White);
            _trainsMapBuilder.AddConnection("Chicago", "Boston", 4, Color.Green);
            _trainsMapBuilder.AddConnection("Chicago", "Washington D.C.", 4, Color.Green);
            _trainsMapBuilder.AddConnection("Chicago", "Orlando", 3, Color.Blue);
            _trainsMapBuilder.AddConnection("Portland", "Boston", 3, Color.Blue);
            _trainsMapBuilder.AddConnection("Boston", "Washington D.C.", 3, Color.White);
            _trainsMapBuilder.AddConnection("Washington D.C.", "Orlando", 3, Color.Blue);
            _trainsMapBuilder.AddConnection("Anchorage", "Honolulu", 5, Color.Blue);

            TrainsMap _ = _trainsMapBuilder.BuildMap();
        }
    }
}