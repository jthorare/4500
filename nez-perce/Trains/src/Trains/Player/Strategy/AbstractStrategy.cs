using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Trains.Common.GameState;
using Trains.Common.Map;

namespace Trains.Player.Strategy
{
    public abstract class AbstractStrategy : IStrategy
    {
        protected static List<Destination> OrderedDestinations(IImmutableSet<Destination> destinations)
        {
            List<Destination> destinationList = destinations.ToList();
            destinationList.Sort();
            return destinationList;
        }

        protected static List<Connection> OrderedConnections(PlayerGameState gameState)
        {
            List<Connection> availableConnectionsList = gameState.AvailableConnections.ToList();
            availableConnectionsList.Sort();
            return availableConnectionsList;
        }

        public abstract IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations);

        public abstract PlayerResponse PlayTurn(PlayerGameState gameState);
    }
}