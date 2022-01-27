using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.TurnTypes;
using Trains.Util;

namespace Trains.Models.Strategies
{
    public class SetLengthStrategy : Strategy
    {
        int Length { get; }
        public SetLengthStrategy(int lengthConnection)
        {
            Length = lengthConnection;
        }
        public override ICollection<Destination> ChooseDestinations(Map map, ICollection<Destination> destinations)
        {
            return base.ChooseDestinations(destinations, Strategy.TakeElementsBack);
        }

        public override ITurn ConductTurn(PlayerGameState playerGameState)
        {
            if (playerGameState.OwnedConnections.Count == 0 && playerGameState.AvailableConnections.Where(conn => (int)conn.NumSegments == Length).Count() > 0)
            {
                return TryAcquireConnectionTurn(playerGameState);
            }
            else
            {
                return new DrawCardsTurn();
            }
        }
    }
}
