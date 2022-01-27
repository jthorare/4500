using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Trains.Common.Map.Json;

namespace Trains.Tests.Common.Map.Json
{
    [TestClass]
    public class CityJsonConverterTest
    {
        [TestMethod]
        public void CityJsonConverterTestSerialize()
        {
            CityJson city = new()
            {
                Name = "City Name",
                X = 0,
                Y = 0
            };
            
            Assert.IsTrue(JsonConvert.SerializeObject(city) == "[\"City Name\",[0,0]]");
        }
             
        [TestMethod]
        public void CityJsonConverterTestDeserialize()
        {
            CityJson city = JsonConvert.DeserializeObject<CityJson>("[\"City Name\",[0,0]]")!;
            
            Assert.IsTrue(city.Name == "City Name");
            Assert.IsTrue(city.X == 0f);
            Assert.IsTrue(city.Y == 0f);
        }
        
        [TestMethod]
        public void CityJsonConverterTestReserialize()
        {
            CityJson city = JsonConvert.DeserializeObject<CityJson>("[\"City Name\",[0,0]]", new CityJsonConverter())!;

            Assert.IsTrue(JsonConvert.SerializeObject(city, new CityJsonConverter()) == "[\"City Name\",[0,0]]");
        }
        
        [TestMethod]
        public void CityJsonConverterTestBadJson()
        {
            Assert.ThrowsException<JsonException>(() =>
                JsonConvert.DeserializeObject<CityJson>("\"City Name\",0,0]]"));
            Assert.ThrowsException<JsonException>(() =>
                JsonConvert.DeserializeObject<CityJson>("[\"City Name\",0,0]]"));
            Assert.ThrowsException<JsonException>(() =>
                JsonConvert.DeserializeObject<CityJson>("[\"City Name\",[0,0]"));
            Assert.ThrowsException<JsonException>(() =>
                JsonConvert.DeserializeObject<CityJson>("[\"City Name\",[0,0,0]]"));
            Assert.ThrowsException<JsonException>(() =>
                JsonConvert.DeserializeObject<CityJson>("[\"City Name\",[0,0],0]"));
        }
        
        [TestMethod]
        public void TrainsMapJsonConverterTestCanConvert()
        {
            Assert.IsTrue(new CityJsonConverter().CanConvert(typeof(CityJson)));
        }
    }
}