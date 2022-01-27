using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Trains.Common;
using Trains.Common.GameState;
using Trains.Common.Map;

namespace Trains.Player.Strategy
{
    public class CheatStrategy : AbstractStrategy
    {
        public override IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations)
        {
            // takes from back
            List<Destination> orderedDests = OrderedDestinations(destinations);
            return orderedDests.Take(destinations.Count - Convert.ToInt32(Constants.destinationsPerPlayer)).ToImmutableHashSet();
        }

        public override PlayerResponse PlayTurn(PlayerGameState gameState)
        {
            Connection cheat;
            while (true)
            {
                Location from = new Location(Utilities.GetRandomString(), 0.1f, 0.1f);
                Location to = new Location(Utilities.GetRandomString(), 0.5f, 0.6f);
                cheat = new Connection(from, to, 4, Color.Red);
                if (!gameState.AvailableConnections.Contains(cheat)) { break; } // ensure that the cheat connection DNE
            }
            return PlayerResponse.ClaimConnection(cheat);
        }
    }
}