using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;

namespace Trains.Util.Json
{
    /// <summary>
    /// An object representation of a ThisPlayer JSON object as defined in milestone 5.
    /// https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._thisplayer%29
    /// </summary>
    public class JsonThisPlayer
    {
        /// <summary>
        /// One of this player's destination cards.
        /// </summary>
        [JsonProperty("destination1")]
        public ICollection<string> Destination1 { get; }

        /// <summary>
        /// Another of this player's destination cards.
        /// </summary>
        [JsonProperty("destination2")]
        public ICollection<string> Destination2 { get; }

        /// <summary>
        /// The number of rails that this player has, represented as an int. Must be a natural number.
        /// </summary>
        [JsonProperty("rails")]
        public int Rails { get; }

        /// <summary>
        /// This player's cards, represented as a Dictionary where the domain elements are strings representing a valid game color,
        /// and the range elements are integers that must be natural numbers representing the quantity of that color card that is owned.
        /// 
        /// A color that doesn't show up in the domain is == to having a quantity of 0.
        /// </summary>
        [JsonProperty("cards")]
        public IDictionary<string, int> Cards { get; }

        /// <summary>
        /// This player's list of owned connections.
        /// </summary>
        [JsonProperty("acquired")]
        public ICollection<JsonAcquired> AcquiredConnections { get; }

        // JsonConstructor tag indicates the constructor to use in deserialization.
        [JsonConstructor]
        public JsonThisPlayer(ICollection<string> destination1, ICollection<string> destination2, int rails, IDictionary<string, int> cards, ICollection<JsonAcquired> acquired)
        {
            Destination1 = destination1;
            Destination2 = destination2;
            Rails = rails;
            Cards = cards;
            AcquiredConnections = acquired;
        }

        // constructor used to make JsonThisPlayer objects from PlayerGameState objects for use in testing.
        public JsonThisPlayer(PlayerGameState playerGameState)
        {
            Destination1 = new HashSet<string>() { playerGameState.Destinations.ElementAt(0).CityOne.CityName, playerGameState.Destinations.ElementAt(0).CityTwo.CityName };
            Destination2 = new HashSet<string>() { playerGameState.Destinations.ElementAt(1).CityOne.CityName, playerGameState.Destinations.ElementAt(1).CityTwo.CityName };
            Rails = playerGameState.Rails;
            Cards = CardsToDictionary(playerGameState.Cards);
            AcquiredConnections = ConnectionsToAcquireds(playerGameState.OwnedConnections);
        }

        /// <summary>
        /// Convert this player's JSON representation of its game cards into an ordered list of ColoredCard objects to their count.
        /// </summary>
        /// <returns>A HashSet of the ColoredCards that this player has.</returns>
        public IList<ColoredCard> GetColoredCards()
        {
            IList<ColoredCard> cards = new List<ColoredCard>();
            foreach (KeyValuePair<string, int> pair in this.Cards)
            {
                for(int count = 0; count < pair.Value; count++)
                {
                    cards.Add(new ColoredCard(Utilities.ToColor(pair.Key)));
                }
            }
            return cards;
        }

        /// <summary>
        /// Converts a Collection of ColoredCard to a Dictionary<string, int> where the key is the Color in english as a string and the value is how many of that ColoredCard exists in the Collection.
        /// </summary>
        /// <param name="cards">The Collection of ColoredCard to convert to a Dictionary</param>
        /// <returns>Converts a Collection of ColoredCard to a Dictionary<string, int> where the key is the Color in english as a string and the value is how many of that ColoredCard exists in the Collection</returns>
        private IDictionary<string, int> CardsToDictionary(IList<ColoredCard> cards)
        {
            IDictionary<string, int> cardsDictionary = new Dictionary<string, int>();
            foreach (ColoredCard card in cards)
            {
                string color = card.Color.ToString().ToLower();
                if (cardsDictionary.ContainsKey(color))
                {
                    cardsDictionary[color]++;
                }
                else
                {
                    cardsDictionary.Add(color, 1);
                }
            }
            return cardsDictionary;
        }

        /// <summary>
        /// Converts a Collection of Connections to a Collection of JsonAcquired.
        /// </summary>
        /// <param name="connections">The Collection of Connections to convert</param>
        /// <returns>The Collection of JsonAcquired where each element has an equivalent Connection in the given argument</returns>
        private ICollection<JsonAcquired> ConnectionsToAcquireds(ICollection<Connection> connections)
        {
            ICollection<JsonAcquired> jsonAcquireds = new HashSet<JsonAcquired>();
            foreach (Connection conn in connections)
            {
                jsonAcquireds.Add(new JsonAcquired(conn.City1.CityName, conn.City2.CityName, conn.Color.ToString().ToLower(), (int)conn.NumSegments));
            }
            return jsonAcquireds;
        }
    }
}
