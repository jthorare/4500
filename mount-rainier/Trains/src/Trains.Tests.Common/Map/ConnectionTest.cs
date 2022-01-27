using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Common.Map;

namespace Trains.Tests.Common.Map
{
    [TestClass]
    public class ConnectionTest
    {
        private TrainsMap _map = null!;

        /// <summary>
        /// Initialize map with values to test connections on.
        /// </summary>
        [TestInitialize]
        public void ConnectionTestInitialize()
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

        /// <summary>
        /// Test that a connection with a different order of locations are equal.
        /// </summary>
        [TestMethod]
        public void TestConnectionEquality()
        {
            Location boston = _map.Locations["Boston"];
            Location chicago = _map.Locations["Chicago"];
            Assert.IsTrue(_map.Connections.Contains(new Connection(chicago, boston, 4, Color.Green)));
            Assert.IsTrue(_map.Connections.Contains(new Connection(boston, chicago, 4, Color.Green)));
            Assert.IsFalse(new Connection(boston, chicago, 4, Color.Green).Equals(null));
        }

        /// <summary>
        /// Segment length should only be in [3, 5]
        /// </summary>
        [TestMethod]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public void TestConnectionSegmentLength()
        {
            Location boston = _map.Locations["Boston"];
            Location chicago = _map.Locations["Chicago"];
            var _ = new Connection(chicago, boston, 3, Color.Green);
            _ = new Connection(chicago, boston, 4, Color.Green);
            _ = new Connection(chicago, boston, 5, Color.Green);
            foreach (var i in Enumerable.Range(0, 100))
            {
                var size = (uint)i;
                if (size is not (3 or 4 or 5))
                    Assert.ThrowsException<ArgumentException>(() =>
                        new Connection(chicago, boston, size, Color.Green));
            }
        }
    }
}