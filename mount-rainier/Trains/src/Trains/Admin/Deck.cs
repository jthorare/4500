using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Common;
using Trains.Common.Map;

namespace Trains.Admin
{
    /// <summary>
    /// Represents the deck of cards in a game of Trains
    /// </summary>
    public class Deck : ICloneable
    {
        /// <summary>
        /// The Random object used for randomness.
        /// </summary>
        protected Random Random { get; }

        ///<summary>
        /// The number of red cards in the <see cref="Deck"/>.
        /// </summary> 
        public Dictionary<Color, uint> ColoredCards { get; private set; }

        /// <summary>
        /// Set of <see cref="Destination"/> contained in this <see cref="Deck"/>.
        /// </summary>
        public HashSet<Destination> Destinations { get; private set; }

        /// <summary>
        /// Constructor for making a Deck object.
        /// </summary>
        /// <param name="startingCards">Dictionary of colors to number of that colored cards in the deck</param>
        /// <param name="destinations"></param>
        /// <param name="random"></param>
        public Deck(Dictionary<Color, uint> coloredCards, HashSet<Destination> destinations, Random random)
        {
            ColoredCards = coloredCards;
            Destinations = destinations;
            Random = random;
        }

        /// <summary>
        /// Construct a <see cref="Deck"/> with cards and destinations.
        /// </summary>
        /// <param name="coloredCards"></param>
        /// <param name="destinations">The set of <see cref="Destination"/> that can be handed to players.</param>
        public Deck(Dictionary<Color, uint> coloredCards, HashSet<Destination> destinations) : this(coloredCards, destinations, new Random())
        { }

        /// <summary>
        /// Draws a random card from this Deck
        /// </summary>
        /// <returns>The color representing a single colored card</returns>
        private Color? DrawRandomCard()
        {
            var availableCards = ColoredCards.Where(c => c.Value > 0).ToList();
            if (availableCards.Count <= 0) return null;

            var i = Random.Next(0, availableCards.Count);
            ColoredCards[availableCards[i].Key]--;
            return availableCards[i].Key;
        }

        /// <summary>
        /// Draws the given number of cards from this Deck.
        /// </summary>
        /// <param name="numOfCards">How many cards to draw</param>
        /// <returns>A Dictionary where the sum of all Values is equal to the given number. Each KeyValuePair represents a color for a card
        /// and </returns>
        public virtual Dictionary<Color, uint> DrawCards(uint numOfCards)
        {
            var returnCards = new Dictionary<Color, uint>();
            for (var i = 0; i < numOfCards; i++)
            {
                var drawnCard = DrawRandomCard();

                // No more cards to draw
                if (drawnCard == null) return returnCards;

                // Update the cards to return
                if (returnCards.Keys.Contains(drawnCard.Value))
                    returnCards[drawnCard.Value]++;
                else returnCards.Add(drawnCard.Value, 1);
            }
            return returnCards;
        }

        /// <summary>
        /// Gets a set of Destination for the IPlayer to choose from.
        /// </summary>
        /// <returns>The Destination for an IPlayer choose from</returns>
        public virtual HashSet<Destination> GetDestinations()
        {
            HashSet<Destination> returnDestinations = new HashSet<Destination>();
            if (Destinations.Count < Constants.destinationsToChooseFrom)
                throw new ApplicationException($"Deck does not contain {Constants.destinationsToChooseFrom} destinations.");
            while (returnDestinations.Count < Constants.destinationsToChooseFrom)
            {
                returnDestinations.Add(Destinations.ElementAt(Random.Next(Destinations.Count)));
            }
            return returnDestinations;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Deck other)
            {
                return Equals(other);
            }

            return false;
        }

        private bool Equals(Deck other)
        {
            return ColoredCards.SequenceEqual(other.ColoredCards) && Destinations.SetEquals(other.Destinations);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ColoredCards, Destinations);
        }

        public virtual object Clone()
        {
            return new Deck(new Dictionary<Color, uint>(ColoredCards), new HashSet<Destination>(Destinations));
        }
    }
}
