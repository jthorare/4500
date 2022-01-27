using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.TurnTypes;
using Trains.Util;

namespace Trains.Models.Strategies
{
    /// <summary>
    /// Class representing the Buy-Now Strategy from Trains.Com Milestone 5
    /// </summary>
    public class BuyNowStrategy : Strategy
    {
        public override ICollection<Destination> ChooseDestinations(Map map, ICollection<Destination> destinations)
        {
            return base.ChooseDestinations(destinations, Strategy.TakeElementsBack);
        }

        public override ITurn ConductTurn(PlayerGameState playerGameState)
        {
            return base.TryAcquireConnectionTurn(playerGameState);
        }
    }
}
