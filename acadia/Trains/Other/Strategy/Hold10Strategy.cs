using System.Linq;
using System.Collections.Generic;
using Trains.Models.GamePieces;
using Trains.Models.TurnTypes;
using Trains.Util;
using Trains.Models.GameStates;

namespace Trains.Models.Strategies
{
    /// <summary>
    /// Class implementing the Hold-10 Strategy from Milestone 5.
    /// </summary>
    public class Hold10Strategy : Strategy
    {

        public override ICollection<Destination> ChooseDestinations(Map map, ICollection<Destination> destinations)
        {
            return base.ChooseDestinations(destinations, Strategy.TakeElementsFront);
        }

        public override ITurn ConductTurn(PlayerGameState playerGameState)
        {
            int cardCount = playerGameState.Cards.Count();

            if (cardCount > 10)
            {
                return base.TryAcquireConnectionTurn(playerGameState);
            }

            return new DrawCardsTurn();
        }
    }
}
