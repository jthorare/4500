using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common;
using Trains.Common.Map;

namespace Trains.Admin
{
    public class StackedDeck : Deck
    {
        List<Color> Cards { get; }
        int CardIndex { get; set; }
        public StackedDeck(List<Color> stackedCards, Dictionary<Color, uint> coloredCards, HashSet<Destination> destinations, Random random) : base(coloredCards, destinations, random)
        {
            Cards = stackedCards;
            CardIndex = 0;
        }

        public override HashSet<Destination> GetDestinations()
        {
            HashSet<Destination> returnDestinations = new HashSet<Destination>();
            if (Destinations.Count < Constants.destinationsToChooseFrom)
                throw new ApplicationException($"Deck does not contain {Constants.destinationsToChooseFrom} destinations.");
            for (int i = 0; i < Constants.destinationsToChooseFrom; i++)
            {
                returnDestinations.Add(Destinations.ElementAt(i));
            }
            return returnDestinations;
        }

        public override Dictionary<Color, uint> DrawCards(uint numOfCards)
        {
            Dictionary<Color, uint> drawn = new Dictionary<Color, uint>();
            List<Color> stackedDeal = new List<Color>();
            if (numOfCards > Cards.Count) { 
                numOfCards = (uint)Cards.Count;
            }
            stackedDeal.AddRange(Cards.GetRange(0, Convert.ToInt32(numOfCards)));
            Cards.RemoveRange(0, (int)numOfCards);
            foreach (Color color in stackedDeal.Distinct())
            {
                drawn.Add(color, (uint)stackedDeal.Where(x => x == color).Count());
            }
            return drawn;
        }

        public override object Clone() {
            return new StackedDeck(new List<Color>(Cards), new Dictionary<Color, uint>(ColoredCards), new HashSet<Destination>(Destinations), Random);
        }
    }
}
