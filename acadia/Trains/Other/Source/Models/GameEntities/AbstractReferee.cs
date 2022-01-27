using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Util;

namespace Trains.Models.GameEntities
{
    public abstract class AbstractReferee : IReferee
    {
        // Random number generator defined once for use by the Referee in different operations with increased entropy compared to a new Random for every randomized operation.
        protected Random Rng { get; set; }

        /// <summary>
        /// Initialize the AbstractReferee's properties.
        /// </summary>
        /// <param name="seed">An optional argument of a seed to initialize the Random object with.
        /// Seed is for use in testing.</param>
        public AbstractReferee(int seed = 0)
        {
            if (seed == 0)
            {
                Rng = new Random();
            }
            else
            {
                Rng = new Random(seed);
            }
        }

        /// <summary>
        /// Randomly selects from the given Destination Collection.
        /// </summary>
        /// <param name="availableDestinations">The Destination Collection to choose from</param>
        /// <returns>A Random number of Destination from the given collection</returns>
        public virtual ICollection<Destination> SelectDestinations(ICollection<Destination> availableDestinations)
        {
            HashSet<int> unusedIndices = new HashSet<int>();
            for (int ii = 0; ii < availableDestinations.Count; ii++)
            {
                unusedIndices.Add(ii);
            }

            ICollection<Destination> playerSelectionPool = new HashSet<Destination>();
            for (int ii = 0; ii < Constants.destinationSelectionPoolSize; ii++)
            {
                int randDestIdx = -1;
                while (true)
                {
                    randDestIdx = Rng.Next(0, availableDestinations.Count);
                    if (unusedIndices.Contains(randDestIdx)) { break; }
                }
                Destination toAdd = availableDestinations.ElementAt(randDestIdx);
                playerSelectionPool.Add(toAdd);
                unusedIndices.Remove(randDestIdx);
            }
            return playerSelectionPool;
        }

        /// <summary>
        /// Shuffles the given deck
        /// </summary>
        /// <param name="deck">The List ColoredCard to shuffle</param>
        /// <returns>The shuffled deck</returns>
        public IList<ColoredCard> ShuffleCards(IList<ColoredCard> deck)
        {
            IList<ColoredCard> shuffledDeck = deck.Select(card => new ColoredCard(card.Color)).ToList();
            Shuffle(shuffledDeck);
            return shuffledDeck;
        }

        /// <summary>
        /// Shuffles the elements within an IList.
        /// from https://stackoverflow.com/questions/273313/randomize-a-listt.
        /// </summary>
        /// <typeparam name="T">Generic typing T.</typeparam>
        /// <param name="list">The IList of elements to shuffle.</param>
        private void Shuffle<T>(IList<T> list)
        {
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int randListIdx = Rng.Next(count + 1);
                T value = list[randListIdx];
                list[randListIdx] = list[count];
                list[count] = value;
            }
        }

        public abstract IDictionary<IPlayer, int> PlayGame(Map map, IList<IPlayer> players);
    }
}
