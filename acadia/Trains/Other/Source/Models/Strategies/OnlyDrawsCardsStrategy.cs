using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.TurnTypes;

namespace Trains.Models.Strategies
{
    public class OnlyDrawsCardsStrategy : Strategy
    {
        public override ICollection<Destination> ChooseDestinations(Map map, ICollection<Destination> destinations)
        {
            return destinations.Take(3).ToList();
        }

        public override ITurn ConductTurn(PlayerGameState playerGameState)
        {
            return new DrawCardsTurn();
        }
    }
}
