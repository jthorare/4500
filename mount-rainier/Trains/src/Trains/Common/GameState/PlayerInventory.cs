using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Common.Map;

namespace Trains.Common.GameState
{
    /// Represents the inventory of a player.
    public sealed class PlayerInventory
    {
        /// <summary>
        /// A Dictionary containing the hand of cards for the Player.
        /// </summary>
        public Dictionary<Color, uint> ColoredCards { get; }

        ///<summary>
        /// The number of rail cards in the <see cref="PlayerInventory"/>.
        /// </summary> 
        public uint RailCards { get; set; }

        /// <summary>
        /// Set of <see cref="IDestination"/> contained in this <see cref="PlayerInventory"/>.
        /// </summary>
        public HashSet<Destination> Destinations { get; set; }

        /// <summary>
        /// Construct a <see cref="PlayerInventory"/> with cards and destinations.
        /// </summary>
        /// <param name="redCards">The number of red cards.</param>
        /// <param name="blueCards">The number of blue cards.</param>
        /// <param name="greenCards">The number of green cards.</param>
        /// <param name="whiteCards">The number of white cards.</param>
        /// <param name="railCards">The number of rail cards.</param>
        /// <param name="destinations">The set of <see cref="IDestination"/> that can be handed to players.</param>
        public PlayerInventory(Dictionary<Color, uint> coloredCards, uint railCards, HashSet<Destination> destinations)
        {
            ColoredCards = coloredCards;
            RailCards = railCards;
            Destinations = destinations;
            foreach (Color c in Enum.GetValues(typeof(Color)))
            {
                if (!ColoredCards.ContainsKey(c))
                {
                    coloredCards[c] = 0;
                }
            }
        }

        /// <summary>
        /// Determines the number of cards left in the hand.
        /// </summary>
        /// <returns>Natural number of how many cards are left in the hand</returns>
        public uint CardLeft()
        {
            uint numCard = 0;
            foreach (var c in ColoredCards)
            {
                numCard += c.Value;
            }
            return numCard;
        }

        /// <summary>
        /// Adds the given cards to the hand
        /// </summary>
        /// <param name="newCards">The cards to add to the hand. Key is a color for a card and the value is how many of that color are being received.</param>
        public void UpdateCard(Dictionary<Color, uint> newCards)
        {
            foreach (Color c in newCards.Keys)
            {
                if (ColoredCards.ContainsKey(c)) { ColoredCards[c] += newCards[c]; }
                else { ColoredCards[c] = newCards[c]; }
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is PlayerInventory other)
            {
                return Equals(other);
            }

            return false;
        }

        private bool Equals(PlayerInventory other)
        {
            return ColoredCards.SequenceEqual(other.ColoredCards) && RailCards == other.RailCards && Destinations.SetEquals(other.Destinations);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ColoredCards, RailCards, Destinations);
        }
    }
}
