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
            //Assert.IsTrue(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<MapJson>(json)) == json);
        }
    }
}