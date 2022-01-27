using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Trains.Common.GameState;
using Trains.Common.GameState.Json;
using Trains.Common.Map;

namespace Trains.Tests.Common.GameState.Json;

[TestClass]
public class PlayerGameStateJsonConverterTest
{
    [TestMethod]
    public void PlayerGameStateJsonConverterTestDeserialize()
    {
        var mapJson =
            "{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}}";
        var map = JsonConvert.DeserializeObject<TrainsMap>(mapJson);
        var pgsJson =
            "{\"this\":{\"destination1\":[\"Atlanta\",\"Boston\"],\"destination2\":[\"Boston\",\"Chicago\"],\"rails\":10,\"cards\":{\"white\":10},\"acquired\":[[\"Atlanta\",\"Boston\",\"red\",3],[\"Boston\",\"Chicago\",\"red\",4]]},\"acquired\":[[[\"Atlanta\",\"LosAngeles\",\"white\",5]]]}";

        PlayerGameStateJsonConverter.ContextMap = map;
        var playerGameState =
            JsonConvert.DeserializeObject<PlayerGameState>(pgsJson);
    }

    [TestMethod]
    public void PlayerGameStateJsonConverterTestReserialize()
    {
        var mapJson =
            "{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}}";
        var map = JsonConvert.DeserializeObject<TrainsMap>(mapJson);
        var pgsJson =
            "{\"this\":{\"destination1\":[\"Atlanta\",\"Boston\"],\"destination2\":[\"Boston\",\"Chicago\"],\"rails\":10,\"cards\":{\"white\":10},\"acquired\":[[\"Atlanta\",\"Boston\",\"red\",3],[\"Boston\",\"Chicago\",\"red\",4]]},\"acquired\":[[[\"Atlanta\",\"LosAngeles\",\"white\",5]]]}";
        
        PlayerGameStateJsonConverter.ContextMap = map;
        var playerGameState = JsonConvert.DeserializeObject<PlayerGameState>(pgsJson);
        var serializedJson = JsonConvert.SerializeObject(playerGameState);
        Assert.AreEqual(pgsJson, serializedJson);
    }
}