using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Trains.Common;
using Trains.Common.GameState;
using Trains.Common.GameState.Json;
using Trains.Common.Map;
using Trains.Common.Map.Json;
using Trains.Remote.Function;
using Trains.Remote.Function.Json;

namespace Trains.Tests.Remote.Function.Json;

[TestClass]
public class RemoteFunctionJsonTest
{
    [TestMethod]
    public void RemoteFunctionJsonTestStartSerialization()
    {
        var startFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Start,
            Arguments = new List<object> { true }
        };

        Assert.AreEqual("[\"start\",[true]]", JsonConvert.SerializeObject(startFunction));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestStartDeserialization()
    {
        var startFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Start,
            Arguments = new List<object> { true }
        };

        Assert.AreEqual(startFunction, JsonConvert.DeserializeObject<RemoteFunction>("[\"start\",[true]]"));
    }
    
    [TestMethod]
    public void RemoteFunctionJsonTestSetupSerialization()
    {
        var mapJson =
            "{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}}";
        var map = JsonConvert.DeserializeObject<TrainsMap>(mapJson)!;
        var rails = Constants.setupPlayerRailsCount;
        var cards = new List<Color>() { Color.Blue, Color.Blue, Color.Blue };

        var setupFunction = new RemoteFunction()
        {
            FunctionName = RemoteFunctionName.Setup,
            Arguments = new List<object> { map, rails, cards }
        };
        
        var functionJson = "[\"setup\",[{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}},45,[\"blue\",\"blue\",\"blue\"]]]";
        TrainsMapJsonConverter.ContextWidth = 500;
        TrainsMapJsonConverter.ContextHeight = 250;
        var serializedFunction = JsonConvert.SerializeObject(setupFunction);
        Assert.AreEqual(functionJson, serializedFunction);
    }

    [TestMethod]
    public void RemoteFunctionJsonTestSetupDeserialization()
    {
        var mapJson =
            "{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}}";
        var map = JsonConvert.DeserializeObject<TrainsMap>(mapJson)!;
        var rails = Constants.setupPlayerRailsCount;
        var cards = new List<Color> { Color.Blue, Color.Blue, Color.Blue };

        var setupFunction = new RemoteFunction()
        {
            FunctionName = RemoteFunctionName.Setup,
            Arguments = new List<object> { map, rails, cards }
        };
        var functionJson = "[\"setup\",[{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}},45,[\"blue\",\"blue\",\"blue\"]]]";

        var deserializedFunction = JsonConvert.DeserializeObject<RemoteFunction>(functionJson)!;
        Assert.AreEqual(setupFunction.FunctionName, deserializedFunction.FunctionName);
        Assert.AreEqual(setupFunction.Arguments[0], deserializedFunction.Arguments[0]);
        Assert.AreEqual(setupFunction.Arguments[1], deserializedFunction.Arguments[1]);
        Assert.IsTrue((setupFunction.Arguments[2] as List<Color>)!.SequenceEqual((deserializedFunction.Arguments[2] as List<Color>)!));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestPickReserialization()
    {
        var pickFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Pick,
            Arguments = new List<object>
            {
                new HashSet<Destination>
                {
                    new("city1", "city2"), 
                    new("city3", "city4"), 
                    new("city5", "city6"), 
                    new("city7", "city8"), 
                    new("city9", "city10")
                }.ToImmutableHashSet()
            }
        };

        var pickJson =
            "[\"pick\",[[\"city3\",\"city4\"],][\"city7\",\"city8\"],[\"city5\",\"city6\"],[\"city1\",\"city2\"],[\"city10\", \"city9\"]]";
        var serializedJson = JsonConvert.SerializeObject(pickFunction)!;
        var deserializedFunction = JsonConvert.DeserializeObject<RemoteFunction>(serializedJson)!;
        Assert.IsTrue((pickFunction.Arguments[0] as IImmutableSet<Destination>)!.SetEquals((deserializedFunction.Arguments[0] as IImmutableSet<Destination>)!));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestPlaySerialization()
    {
        var mapJson =
            "{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}}";
        var map = JsonConvert.DeserializeObject<TrainsMap>(mapJson);
        var pgsJson =
            "{\"this\":{\"destination1\":[\"Atlanta\",\"Boston\"],\"destination2\":[\"Boston\",\"Chicago\"],\"rails\":10,\"cards\":{\"white\":10},\"acquired\":[[\"Atlanta\",\"Boston\",\"red\",3],[\"Boston\",\"Chicago\",\"red\",4]]},\"acquired\":[[[\"Atlanta\",\"LosAngeles\",\"white\",5]]]}";
        PlayerGameStateJsonConverter.ContextMap = map;
        var pgs =
            JsonConvert.DeserializeObject(pgsJson);

        var playFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Play,
            Arguments = new List<object> { pgs }
        };

        Console.WriteLine(JsonConvert.SerializeObject(playFunction));
        var winJson =
            "[\"play\",[{\"this\":{\"destination1\":[\"Atlanta\",\"Boston\"],\"destination2\":[\"Boston\",\"Chicago\"],\"rails\":10,\"cards\":{\"white\":10},\"acquired\":[[\"Atlanta\",\"Boston\",\"red\",3],[\"Boston\",\"Chicago\",\"red\",4]]},\"acquired\":[[[\"Atlanta\",\"LosAngeles\",\"white\",5]]]}]]";
        Assert.AreEqual(winJson, JsonConvert.SerializeObject(playFunction));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestPlayDeserialization()
    {
        var mapJson =
            "{\"width\":500,\"height\":250,\"cities\":[[\"Atlanta\",[300,225]],[\"Boston\",[475,100]],[\"Chicago\",[25,125]],[\"LosAngeles\",[100,125]]],\"connections\":{\"Atlanta\":{\"Boston\":{\"red\":3},\"LosAngeles\":{\"white\":5}},\"Boston\":{\"Chicago\":{\"red\":4}}}}";
        var map = JsonConvert.DeserializeObject<TrainsMap>(mapJson);
        var pgsJson =
            "{\"this\":{\"destination1\":[\"Atlanta\",\"Boston\"],\"destination2\":[\"Boston\",\"Chicago\"],\"rails\":10,\"cards\":{\"white\":10},\"acquired\":[[\"Atlanta\",\"Boston\",\"red\",3],[\"Boston\",\"Chicago\",\"red\",4]]},\"acquired\":[[[\"Atlanta\",\"LosAngeles\",\"white\",5]]]}";
        PlayerGameStateJsonConverter.ContextMap = map;
        var pgs =
            JsonConvert.DeserializeObject<PlayerGameState>(pgsJson);

        var playFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Play,
            Arguments = new List<object> { pgs }
        };

        var winJson =
            "[\"play\",[{\"this\":{\"destination1\":[\"Atlanta\",\"Boston\"],\"destination2\":[\"Boston\",\"Chicago\"],\"rails\":10,\"cards\":{\"white\":10},\"acquired\":[[\"Atlanta\",\"Boston\",\"red\",3],[\"Boston\",\"Chicago\",\"red\",4]]},\"acquired\":[[[\"Atlanta\",\"LosAngeles\",\"white\",5]]]}]]";
        RemoteFunctionJsonConverter.ContextMap = map;
        var actual = JsonConvert.DeserializeObject<RemoteFunction>(winJson);
        Assert.AreEqual(playFunction, actual);
    }

    [TestMethod]
    public void RemoteFunctionJsonTestMoreSerialization()
    {
        var moreFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.More,
            Arguments = new List<object> { new List<Color> { Color.Blue, Color.Blue, Color.Blue } }
        };
        
        var expectedMoreJson = "[\"more\",[[\"blue\",\"blue\",\"blue\"]]]";
        var actual = JsonConvert.SerializeObject(moreFunction);
        Assert.AreEqual(expectedMoreJson, actual);
    }

    [TestMethod]
    public void RemoteFunctionJsonTestMoreDeserialization()
    {
        var moreJson = "[\"more\",[[\"blue\",\"blue\",\"blue\"]]]";
        var expectedMoreFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.More,
            Arguments = new List<object> { new List<Color> { Color.Blue, Color.Blue, Color.Blue } }
        };
        var actual = JsonConvert.DeserializeObject<RemoteFunction>(moreJson)!;
        Assert.AreEqual(expectedMoreFunction.FunctionName, actual.FunctionName);
        Assert.IsTrue((expectedMoreFunction.Arguments[0] as List<Color>)!.SequenceEqual((actual.Arguments[0] as List<Color>)!));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestWinSerialization()
    {
        var winFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Win,
            Arguments = new List<object> { true }
        };

        Assert.AreEqual("[\"win\",[true]]", JsonConvert.SerializeObject(winFunction));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestWinDeserialization()
    {
        var winFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Win,
            Arguments = new List<object> { true }
        };

        Assert.AreEqual(winFunction, JsonConvert.DeserializeObject<RemoteFunction>("[\"win\",[true]]"));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestEndSerialization()
    {
        // Construct the function
        var endFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.End,
            Arguments = new List<object> { true }
        };

        Assert.AreEqual("[\"end\",[true]]", JsonConvert.SerializeObject(endFunction));
    }

    [TestMethod]
    public void RemoteFunctionJsonTestEndDeserialization()
    {
        // Construct the function
        var endFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.End,
            Arguments = new List<object> { true }
        };

        Assert.AreEqual(endFunction, JsonConvert.DeserializeObject<RemoteFunction>("[\"end\",[true]]"));
    }
}