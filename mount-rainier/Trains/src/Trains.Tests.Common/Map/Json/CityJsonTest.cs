using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Common.Map.Json;

namespace Trains.Tests.Common.Map.Json
{
    [TestClass]
    public class CityJsonTest
    {
        [TestMethod]
        public void LocationJsonTestProperties()
        {
            CityJson location = new()
            {
                Name = "City Name",
                X = 0,
                Y = 0
            };
            
            Assert.IsTrue(location.Name == "City Name");
            Assert.IsTrue(location.X == 0);
            Assert.IsTrue(location.Y == 0);
        }
    }
}