using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Trains.Common;
using Trains.Common.GameState;
using Trains.Common.Map;

namespace Trains.Player.Strategy
{
    public class BuyNowStrategy : AbstractStrategy
    {
        public override IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations)
        {
            // takes from back
            List<Destination> orderedDests = OrderedDestinations(destinations);
            return orderedDests.Take(destinations.Count - Convert.ToInt32(Constants.destinationsPerPlayer)).ToImmutableHashSet();
        }

        public override PlayerResponse PlayTurn(PlayerGameState gameState)
        {
            List<Connection> orderedChoices = OrderedConnections(gameState);
            List<Connection> canClaim = orderedChoices.Where(gameState.CanClaim).ToList();
            return !canClaim.Any() ? PlayerResponse.DrawCard() : PlayerResponse.ClaimConnection(canClaim[0]);
        }
    }
}