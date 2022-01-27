using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Trains.Common.GameState.Json;

namespace Trains.Tests.Common
{
    [TestClass]
    public class PlayerStateJsonTest
    {
        //TODO Actually test json converters.
        [TestMethod]
        public void PlayerStateJsonTestProperties()
        {
            string json = @"{""this"": { ""destination1"": [""Boston"", ""Atlanta""], ""destination2"": [""Boston"", ""Chicago""], ""rails"": 10, ""cards"": {""red"": 10}, ""acquired"": [[""Boston"", ""Atlanta"", ""red"", 3], [""Boston"", ""Chicago"", ""red"", 4]] }, ""acquired"": []}";

            PlayerStateJson psj = JsonConvert.DeserializeObject<PlayerStateJson>(json);
            Console.WriteLine(JsonConvert.SerializeObject(psj));
        }

    }
}
