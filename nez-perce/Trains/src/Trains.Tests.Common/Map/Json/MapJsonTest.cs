using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Trains.Common.Map;
using Trains.Common.Map.Json;

namespace Trains.Tests.Common.Map.Json
{
    [TestClass]
    public class MapJsonTest
    {
        [TestMethod]
        public void MapJsonTestSerialize()
        {
            var map = new MapJson
            {
                Width = 100,
                Height = 100,
                Cities = new List<CityJson>
                {
                    new() { Name = "City1", X = 0, Y = 0 },
                    new() { Name = "City2", X = 100, Y = 100 }
                },
                Connections = new Dictionary<string, TargetJson>
                {
                    {
                        "City1", new TargetJson
                        {
                            {
                                "City2", new SegmentJson
                                {
                                    { Color.Red, 3 }
                                }
                            }
                        }
                    }
                }
            };

            Console.WriteLine(JsonConvert.SerializeObject(map));
            Console.WriteLine("{\"width\":100,\"height\":100,\"cities\":[[\"City1\",[0,0]],[\"City2\",[100,100]]],\"connections\":{\"City1\":{\"City2\":{\"red\":3}}}}");
            
            Assert.IsTrue(JsonConvert.SerializeObject(map).Equals("{\"width\":100,\"height\":100,\"cities\":[[\"City1\",[0,0]],[\"City2\",[100,100]]],\"connections\":{\"City1\":{\"City2\":{\"red\":3}}}}"));
        }
        
        [TestMethod]
        public void MapJsonTestReserialize()
        {
            string json =
                "{\"width\":100,\"height\":100,\"cities\":[[\"City1\",[0,0]],[\"City2\",[100,100]]],\"connections\":{\"City1\":{\"City2\":{\"red\":3}}}}";
            Console.WriteLine(JsonConvert.DeserializeObject<MapJson>(json));
            Assert.IsTrue(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<MapJson>(json)) == json);
        }

        [TestMethod]
        public void MapJsonTestConvertFromTrainsMap()
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
            trainsMapSerializer.AddConnection("Chicago", "Washington D.C.", 4, Color.Green);
            trainsMapSerializer.AddConnection("Chicago", "Orlando", 3, Color.Blue);
            trainsMapSerializer.AddConnection("Portland", "Boston", 3, Color.Blue);
            trainsMapSerializer.AddConnection("Boston", "Washington D.C.", 3, Color.White);
            trainsMapSerializer.AddConnection("Boston", "Chicago", 4, Color.Green);
            trainsMapSerializer.AddConnection("Washington D.C.", "Orlando", 3, Color.Blue);
            trainsMapSerializer.AddConnection("Anchorage", "Honolulu", 5, Color.Blue);

            var map = trainsMapSerializer.BuildMap();
            const string expected = "{\"width\":800,\"height\":800,\"cities\":[[\"Anchorage\",[0,0]],[\"Boston\",[640,320]],[\"Chicago\",[400,400]],[\"Honolulu\",[0,720]],[\"Orlando\",[720,720]],[\"Portland\",[720,80]],[\"San Francisco\",[160,560]],[\"Seattle\",[80,80]],[\"Washington D.C.\",[640,480]]],\"connections\":{\"Anchorage\":{\"Honolulu\":{\"blue\":5}},\"Boston\":{\"Chicago\":{\"green\":4},\"Portland\":{\"blue\":3},\"Washington D.C.\":{\"white\":3}},\"Chicago\":{\"Orlando\":{\"blue\":3},\"Portland\":{\"white\":5},\"San Francisco\":{\"red\":5},\"Seattle\":{\"blue\":5},\"Washington D.C.\":{\"green\":4}},\"Orlando\":{\"Washington D.C.\":{\"blue\":3}},\"San Francisco\":{\"Seattle\":{\"red\":4}}}}";
            
            var mapJson = MapJson.ConvertFromTrainsMap(map, 800, 800);
            var actual = JsonConvert.SerializeObject(mapJson);
            Assert.AreEqual(expected, actual);
        }
    }
}