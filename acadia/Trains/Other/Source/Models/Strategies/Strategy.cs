using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.TurnTypes;
using Trains.Util.Comparers;
using Trains.Util;

namespace Trains.Models.Strategies
{
    public abstract class Strategy : IStrategy
    {

        /// <summary>
        /// Chooses the Destinations for the Player according to this IStrategy's rules.
        /// </summary>
        /// <param name="destinations">The collection of Destination objects from which this IStrategy chooses.</param>
        /// <param name="func">A function used to select desired Destination from destinations.</param>
        /// <param name="takeFromFront">A bool representing whether to take Destination from the front or back of the lexicographically sorted Collection of destination</param>
        /// <returns>An ICollection of Destination that this Strategy has chosen from the given ICollection of Destination</returns>
        protected ICollection<Destination> ChooseDestinations(ICollection<Destination> destinations, Func<ICollection<Destination>, IEnumerable<Destination>> func)
        {
            List<Destination> chosen = new List<Destination>();
            chosen.AddRange(func(Utilities.OrderByComparer(destinations, new LexicoDestinationComparer())));
            return destinations.Where(dest => !chosen.Contains(dest)).ToHashSet();
        }

        /// <summary>
        /// Takes a pre-defined number (Constants.setupDestinationsCount) of elements from the front
        /// of the given collection of Destination.
        /// </summary>
        /// <param name="dests">The Collection of Destination to take from.</param>
        /// <returns>A HashSet containing the chosen Destination.</returns>
        protected static IEnumerable<Destination> TakeElementsFront(ICollection<Destination> dests)
        {
            return dests.Take(Constants.setupDestinationsCount).ToHashSet();
        }

        /// <summary>
        /// Takes a pre-defined number (Constants.setupDestinationsCount) of elements from the back
        /// of the given collection of Destination.
        /// </summary>
        /// <param name="dests">The Collection of Destination to take from.</param>
        /// <returns>A HashSet containing the chosen Destination.</returns>
        protected static IEnumerable<Destination> TakeElementsBack(ICollection<Destination> dests)
        {
            return dests.TakeLast(Constants.setupDestinationsCount).ToHashSet();
        }

        /// <summary>
        /// Tries to create an AcquireConnectionTurn from the given PlayerGameState.AvailableConnections Collection. If unsuccessful, returns a DrawCardsTurn object.
        /// </summary>
        /// <param name="playerGameState">The PlayerGameState to use for trying to determine what type of ITurn to create</param>
        /// <returns>An ITurn representing the turn for this Strategy object</returns>
        protected ITurn TryAcquireConnectionTurn(PlayerGameState playerGameState)
        {
            ICollection<Connection> lexicoConnections = Utilities.OrderByComparer(playerGameState.AvailableConnections, new LexicoConnectionComparer());
            foreach (Connection conn in lexicoConnections)
            {
                if (Utilities.GetCardsOfColor(playerGameState.Cards, conn.Color) >= (int)conn.NumSegments && playerGameState.Rails >= (int)conn.NumSegments)
                {
                    return new AcquireConnectionTurn(conn);
                }
            }
            return new DrawCardsTurn();
        }

        public abstract ICollection<Destination> ChooseDestinations(Map map, ICollection<Destination> destinations);

        public abstract ITurn ConductTurn(PlayerGameState playerGameState);

    }
}
