using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Trains.Common.Map;
using Trains.Common.Map.Json;

namespace Trains.Tests.Common.Map.Json
{
    [TestClass]
    public class TrainsMapJsonConverterTest
    {
        [TestMethod]
        public void TrainsMapJsonConverterTestDeserialize()
        {
            string json =
                "{\"width\":100,\"height\":100,\"cities\":[[\"City1\",[0,0]],[\"City2\",[100,100]]],\"connections\":{\"City1\":{\"City2\":{\"Red\":3}}}}";
            var _ = JsonConvert.DeserializeObject<TrainsMap>(json);
        }

        [TestMethod]
        public void TrainsMapJsonConverterTestSerialize()
        {
            var trainsMapSerializer = new TrainsMap.TrainsMapBuilder();

            trainsMapSerializer.AddLocation("Seattle", 0.1f, 0.1f);
            trainsMapSerializer.AddLocation("San Francisco", 0.2f, 0.7f);
            trainsMapSerializer.AddLocation("Chicago", 0.5f, 0.5f);
            trainsMapSerializer.AddLocation("Portland", 0.9f, 0.1f);
            trainsMapSerializer.AddLocation("Boston", 0.8f, 0.4f);
            trainsMapSerializer.AddLocation("Washington D.C.", 0.8f, 0.6f);
            trainsMapSerializer.AddLocation("Orlando", 0.9f, 0.9f);
            trainsMapSerializer.AddLocation("Anchorage", 0f, 0f);
            trainsMapSerializer.AddLocation("Honolulu", 0f, 0.9f);

            trainsMapSerializer.AddConnection("Seattle", "San Francisco", 4, Color.Red);
            trainsMapSerializer.AddConnection("Seattle", "Chicago", 5, Color.Blue);
            trainsMapSerializer.AddConnection("San Francisco", "Chicago", 5, Color.Red);
            trainsMapSerializer.AddConnection("Chicago", "Portland", 5, Color.White);
            trainsMapSerializer.AddConnection("Chicago", "Boston", 4, Color.Green);
            trainsMapSerializer.AddConnection("Chicago", "Washington D.C.", 4, Color.Green);
            trainsMapSerializer.AddConnection("Chicago", "Orlando", 3, Color.Blue);
            trainsMapSerializer.AddConnection("Portland", "Boston", 3, Color.Blue);
            trainsMapSerializer.AddConnection("Boston", "Washington D.C.", 3, Color.White);
            trainsMapSerializer.AddConnection("Washington D.C.", "Orlando", 3, Color.Blue);
            trainsMapSerializer.AddConnection("Anchorage", "Honolulu", 5, Color.Blue);

            var map = trainsMapSerializer.BuildMap();

            Assert.ThrowsException<NotImplementedException>(() => JsonConvert.SerializeObject(map));
        }
        
        [TestMethod]
        public void TrainsMapJsonConverterTestCanConvert()
        {
            Assert.IsTrue(new TrainsMapJsonConverter().CanConvert(typeof(TrainsMap)));
        }
    }
}