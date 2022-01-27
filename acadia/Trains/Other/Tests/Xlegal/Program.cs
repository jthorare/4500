using Newtonsoft.Json;
using System;
using System.IO;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Util;
using Trains.Util.Json;

namespace xlegal
{
    /// <summary>
    /// Test harness for milestone 5 that takes in a Map (https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29),
    /// PlayerState (https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._playerstate%29), and Acquired
    /// (https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29) object JSON inputs and writes to STDOUT
    /// whether the requested action is legal according to the rules with respect to the given map and state.
    /// Run xlegal by redirecting JSON input into a `./xlegal` call from the acadia/5/ directory.
    /// ex: `./xlegal<Tests/1-in.json` should write to STDOUT the value that can be found in 1-out.json.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string input = Utilities.ReadInput();
            if (input != null && input != "")
            {
                string[] mapEtAl = Utilities.ExtractObject(input); // first string is the JsonMap, second is the remaining input that holds the PlayerState and Acquired
                string[] playerStateEtAl = Utilities.ExtractObject(mapEtAl[1]); // first string is the PlayerState, second is the remaining input that holds an Acquired
                Map map = JsonConvert.DeserializeObject<JsonMap>(mapEtAl[0]).ToMap();
                Connection conn = JsonConvert.DeserializeObject<JsonAcquired>(playerStateEtAl[1]).ToConnection(map);
                PlayerGameState playerGameState = JsonConvert.DeserializeObject<JsonPlayerState>(playerStateEtAl[0]).ToPlayerGameState(map);
                // write whether it's legal to acquire the desired Connection
                Console.WriteLine(Utilities.IsLegalMove(playerGameState, conn).ToString().ToLower());
            }
            else
            {
                throw new IOException("Input JSON was null.");
            }
        }
    }
}
