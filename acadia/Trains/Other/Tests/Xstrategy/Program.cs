using Newtonsoft.Json;
using System;
using System.IO;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.Strategies;
using Trains.Models.TurnTypes;
using Trains.Util;
using Trains.Util.Json;

namespace Xstrategy
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Utilities.ReadInput();
            if (input != null && input != "")
            {
                string[] mapEtAl = Utilities.ExtractObject(input); // first string is the JsonMap, second is the remaining input that holds the PlayerState
                Map map = JsonConvert.DeserializeObject<JsonMap>(mapEtAl[0]).ToMap();
                PlayerGameState playerGameState = JsonConvert.DeserializeObject<JsonPlayerState>(mapEtAl[1]).ToPlayerGameState(map);
                // write the returning action based on the Hold-10 strategy; one of: "more cards", JsonAcquired.
                IStrategy hold10strategy = new Hold10Strategy();
                ITurn action = hold10strategy.ConductTurn(playerGameState);
                Console.WriteLine(JsonConvert.SerializeObject(action, new JsonActionConverter()));
            }
            else
            {
                throw new IOException("Input JSON was null.");
            }
        }
    }
}
