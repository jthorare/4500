using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Common.Map;

namespace Trains.Tests.Common.Map
{
    [TestClass]
    [SuppressMessage("ReSharper", "RedundantAssignment")]
    public class LocationTest
    {
        /// <summary>
        /// Test valid creations of Location.
        /// </summary>
        [TestMethod]
        public void TestLocationCreations()
        {
            var _ = new Location("Seattle", 0.1f, 0.1f);
            _ = new Location("San Francisco", 0.2f, 0.7f);
            _ = new Location("Chicago", 0.5f, 0.5f);
            _ = new Location("Portland", 0.9f, 0.1f);
            _ = new Location("Boston", 0.8f, 0.4f);
            _ = new Location("Washington D.C.", 0.8f, 0.6f);
            _ = new Location("Orlando", 0.9f, 0.9f);
            _ = new Location("Anchorage", 0f, 0f);
            _ = new Location("Honolulu", 0f, 0.9f); 
            _ = new Location("City Name", 0f, 0f);
            _ = new Location("City Name", 1f, 1f);
        }

        // Test that creations of Location fail on an invalid name.
        [TestMethod]
        public void TestLocationInvalidName()
        {
            // Invalid due to length
            Assert.ThrowsException<ArgumentException>(() => new Location("1234567890123456789012345", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("12345678901234567890123456", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("12345678901234567890123457", 0.1f, 0.1f));
       
            // Some but not all illegal characters
            Assert.ThrowsException<ArgumentException>(() => new Location("!", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("@", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("#", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("$", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("%", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("^", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("&", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("*", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("(", 0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location(")", 0.1f, 0.1f));
        }

        /// <summary>
        /// Test that creations of Location fail on invalid coordinates.
        /// </summary>
        [TestMethod]
        public void TestLocationInvalidCoordinates()
        {
            Assert.ThrowsException<ArgumentException>(() => new Location("City Name", 0.1f, 1.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("City Name", 1.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("City Name", -0.1f, 0.1f));
            Assert.ThrowsException<ArgumentException>(() => new Location("City Name", 0.1f, -0.1f));
        }
        
        /// <summary>
        /// Test that Location getters returns expected values.
        /// </summary>
        [TestMethod]
        public void TestLocationGetters()
        {
            var location = new Location("City Name", 0f, 0f);
            Assert.AreEqual("City Name", location.Name);
            Assert.AreEqual(0f, location.X);
            Assert.AreEqual(0f, location.Y);
        }

        /// <summary>
        /// Test overridden Location equality.
        /// </summary>
        [TestMethod]
        public void TestLocationEquals()
        {
            Location location = new Location("City Name", 0f, 0f);
            Assert.AreEqual(new Location("City Name", 0f, 0f), location);
            Assert.IsFalse(location.Equals(null));
        }
    }
}