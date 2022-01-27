using System;
using System.Linq;
using System.Collections.Generic;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Util;
using Trains.Models.TurnTypes;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Trains.Models.GameEntities
{
    /// <summary>
    /// Class representing the rule managing module for a Trains.com game.
    /// 
    /// Abnormal interactions this Referee handles:
    ///     - The player hands back null instead of a list or an ITurn or a null Connection to acquire
    ///     - Check for timeout (Constants.playerTimeAllotted = 1000ms) on all Player function calls.
    ///     - The Player is only given IMMUTABLE shallow copies of the list that the Player is using
    ///     - Validate destinations picked
    ///     - Validate PlayTurn call (one of OUR ITurns, and not null, AcquireConnection has legal Connection)
    ///     - Try catch everything
    /// 
    /// Abnormal interactions this Referee DOES NOT handle:
    /// 
    /// </summary>
    public class Referee : AbstractReferee
    {
        /// <summary>
        /// Represents the Deck of the List where 0 index indicates the next card to be drawn/dealt
        /// </summary>
        private IList<ColoredCard> Deck { get; set; }

        /// <summary>
        /// Constructor for a Trains.com Referee
        /// </summary>
        /// <param name="playingCards">The deck of cards for the Referee's game</param>
        /// <param name="seed">An optional argument for seeding an AbstractReferee's Random</param>
        public Referee(IList<ColoredCard> playingCards, int seed = 0) : base(seed)
        {
            if (playingCards.Count == Constants.startingDeckCardsCount)
            {
                Deck = ShuffleCards(playingCards);
            }
            else
            {
                throw new ArgumentException($"Referee initialized with invalid number of cards: {playingCards.Count}");
            }
        }

        /// <summary>
        /// Plays an entire game of Trains.com from start to end
        /// </summary>
        /// <param name="map">The Map for the game to be played on</param>
        /// <param name="players">The IPlayers of the game given in descending order of Player age.</param>
        /// <returns>The ranking of Players in the game. Any Player with as a score was kicked from the game.</returns>
        public override IDictionary<IPlayer, int> PlayGame(Map map, IList<IPlayer> players)
        {
            IDictionary<IPlayer, int> rankedPlayers = new Dictionary<IPlayer, int>();
            if (players.Count >= Constants.minGamePlayerCount && players.Count <= Constants.maxGamePlayerCount) // only play a game given a valid number of players
            {
                IList<IPlayer> startedPlayers = new List<IPlayer>(players);
                IList<PlayerGameState> playerGameStates = SetupPlayers(map, players);
                SetPlayerDestinations(map, players, playerGameStates);
                RefereeGameState rgs = InitializeRefereeGameState(map, playerGameStates);
                rgs = CheckTurnPhaseLength(TurnPhase, rgs, players);
                // CLOSING PHASE
                rankedPlayers = Score(startedPlayers, players, rgs);
                InformPlayersFinalStatus(rankedPlayers);
            }
            else
            {   // do not play a game
                foreach (IPlayer player in players)
                {
                    rankedPlayers[player] = 0; // all players of an unplayed game receive score 0
                }
            }
            return rankedPlayers;
        }
        private RefereeGameState CheckTurnPhaseLength(Func<RefereeGameState, IList<IPlayer>, RefereeGameState> turnLoop, RefereeGameState startingState, IList<IPlayer> players)
        {
            Task<RefereeGameState> task = Task.Run(() => turnLoop(startingState, players));
            if (task.Wait(TimeSpan.FromMilliseconds(Constants.maxGameDuration)))
            {
                return task.Result;
            }
            else
            {
                return startingState;
            }
        }

        /// <summary>
        /// Informs all non-booted Player objects whether they won or lost.
        /// </summary>
        /// <param name="rankedPlayers">The score mapping from IPlayer to int representing the number of points they have earned.
        /// Points are integers. Ordered in order of highest score to lowest.</param>
        private void InformPlayersFinalStatus(IDictionary<IPlayer, int> rankedPlayers)
        {
            int max = rankedPlayers.Values.Max();
            foreach (KeyValuePair<IPlayer, int> kvp in rankedPlayers)
            {
                if (kvp.Value != -1) // -1 indicates booted player; don't communicate
                {
                    bool won = kvp.Value == max;
                    CheckPlayerGameOverMisbehavior(kvp.Key.GameOver, won);
                }
            }
        }

        /// <summary>
        /// Scores the Player performances (kicked Player receives -1 score).
        /// </summary>
        /// <param name="startedPlayers">The list of IPlayer that start the game</param>
        /// <param name="endingPlayers">All IPlayer that finished the game</param>
        /// <param name="rgs">The ending RefereeGameState for the game</param>
        /// <returns>The score mapping from Player to int representing the number of points they have earned. Points are integers.
        /// Ordered in order of highest score to lowest.</returns>
        public IDictionary<IPlayer, int> Score(IList<IPlayer> startedPlayers, IList<IPlayer> endingPlayers, RefereeGameState rgs)
        {
            Dictionary<IPlayer, int> scores = new Dictionary<IPlayer, int>(endingPlayers.ToDictionary(player => player, player => 0)); // create a dictionary of players where each player starts w 0 points.
            scores = ScoreConnections(endingPlayers, rgs, scores); // add the pts for connections to each player
            scores = AddPointsToPlayers(PlayersThatOwnLongestPaths(endingPlayers, rgs), scores, Constants.longestPathBonus);
            IList<IPlayer> kickedPlayers = startedPlayers.Where(player => !endingPlayers.Contains(player)).ToList();
            scores = AddPointsToPlayers(kickedPlayers, scores, -1);
            scores = AddDestinationBonuses(endingPlayers, rgs, scores);
            // rank players into a list where index 0 = highest score
            return scores.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Adds the supplied number of points to the supplied IPlayer in the score mapping.
        /// </summary>
        /// <param name="playersEarningPts">The IPlayer objects earning pts. Order of the adding does not matter.</param>
        /// <param name="scores">The score mapping from IPlayer to int representing the number of points they have earned. Points are integers.</param>
        /// <param name="pts">The number of points to add to the IPlayer</param>
        /// <returns>The updated mapping of IPlayer to points.</returns>
        private Dictionary<IPlayer, int> AddPointsToPlayers(IList<IPlayer> playersEarningPts, Dictionary<IPlayer, int> scores, int pts)
        {
            foreach (IPlayer player in playersEarningPts)
            {
                if (scores.ContainsKey(player))
                {
                    scores[player] += pts;
                }
                else
                {
                    scores.Add(player, pts); // -1 indicates they were kicked
                }
            }
            return scores;
        }

        /// <summary>
        /// Adds pts to the score mapping for each where Connection.NumSegments : points is a 1 : 1 conversion.
        /// </summary>
        /// <param name="playersEarningPts">The IPlayer that are earning points</param>
        /// <param name="rgs">The ending RefereeGameState of a the game</param>
        /// <param name="scores">The score mapping from IPlayer to int representing the number of points they have earned. Points are integers.</param>
        /// <returns>The updated mapping of IPlayer to points.</returns>
        private Dictionary<IPlayer, int> ScoreConnections(IList<IPlayer> playersEarningPts, RefereeGameState rgs, Dictionary<IPlayer, int> scores)
        {
            for (int index = 0; index < playersEarningPts.Count; index++) // foreach player that played until the end
            {
                PlayerGameState pgs = rgs.PlayerGameStates[index]; // get its end state
                int score = 0;
                foreach (Connection connection in pgs.OwnedConnections) // score all connections
                {
                    score += (int)connection.NumSegments;
                }
                scores[playersEarningPts[index]] = score; // add their score so far
            }
            return scores;
        }

        /// <summary>
        /// Finds all IPlayer objects that own the longest simple, acyclic, continuous path.
        /// </summary>
        /// <param name="endingPlayers">The IPlayer that are possible longest path owners</param>
        /// <param name="rgs">The ending RefereeGameState of a the game</param>
        /// <returns>The List of IPlayer that own the longest path</returns>
        private IList<IPlayer> PlayersThatOwnLongestPaths(IList<IPlayer> endingPlayers, RefereeGameState rgs)
        {
            int longestPathSoFar = -1; // keep track of the longest path so far
            IList<IPlayer> ownLongestPaths = new List<IPlayer>(); // keep track of players that have that length path
            for (int index = 0; index < endingPlayers.Count; index++) // foreach player that played until the end
            {
                PlayerGameState pgs = rgs.PlayerGameStates[index]; // get its end state
                int playersLongestPath = LongestPath(pgs); // find the longest path
                if (playersLongestPath > longestPathSoFar) { ownLongestPaths = new List<IPlayer>() { endingPlayers[index] }; }
                if (playersLongestPath == longestPathSoFar) { ownLongestPaths.Add(endingPlayers[index]); }
            }
            return ownLongestPaths;
        }

        /// <summary>
        /// Adds the appropriate destination connection bonus for each IPlayer in the supplied list
        /// </summary>
        /// <param name="endingPlayers">The IPlayer that are earning the bonus</param>
        /// <param name="rgs">The ending RefereeGameState of a the game</param>
        /// <param name="scores">The score mapping from IPlayer to int representing the number of points they have earned. Points are integers.</param>
        /// <returns>The updated mapping of IPlayer to points.</returns>
        private Dictionary<IPlayer, int> AddDestinationBonuses(IList<IPlayer> endingPlayers, RefereeGameState rgs, Dictionary<IPlayer, int> scores)
        {
            for (int index = 0; index < endingPlayers.Count; index++)
            {
                PlayerGameState pgs = rgs.PlayerGameStates[index];
                int destinationsConnected = CountConnectedDestinations(pgs);
                int destinationBonusPts = DestinationBonusPoints(destinationsConnected);
                scores = AddPointsToPlayers(new List<IPlayer>() { endingPlayers[index] }, scores, destinationBonusPts);
            }
            return scores;
        }

        /// <summary>
        /// Returns how many bonus pts are earned or lost from a number of destinations that were connected.
        /// </summary>
        /// <param name="connectedDestinations">The number of destinations that were connected</param>
        /// <returns>An int representing the number of bonus points earned or lost</returns>
        private int DestinationBonusPoints(int connectedDestinations)
        {
            int notConnectedDestinations = Constants.setupDestinationsCount - connectedDestinations;
            return Constants.connectedDestinationBonus * connectedDestinations - Constants.connectedDestinationBonus * notConnectedDestinations;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pgs"></param>
        /// <returns></returns>
        private int CountConnectedDestinations(PlayerGameState pgs)
        {
            int destinationsConnected = 0;
            foreach (Destination destination in pgs.Destinations)
            {

                if (Utilities.PathExistsAmongConnections(pgs.OwnedConnections, destination.CityOne, destination.CityTwo, new HashSet<City>(), null, null))
                {
                    destinationsConnected++;
                }
            }
            return destinationsConnected;
        }

        /// <summary>
        /// Determines the length of the longest path from the given PlayerGameState.OwnedConnections.
        /// </summary>
        /// <param name="pgs">The PlayerGameState whose OwnedConnections we are finding the longest path of</param>
        /// <returns>The length of the longest path</returns>
        private int LongestPath(PlayerGameState pgs)
        {
            int longestPath = -1; // the longest path this PlayerGameState has
            ICollection<Connection> ownedConnections = pgs.OwnedConnections;
            IList<Connection> checkedDownThisPath = new List<Connection>();
            foreach (Connection connection in pgs.OwnedConnections) // start with each owned Connection as the FIRST CONNECTION
            {
                int longestPathSoFar = (int)connection.NumSegments; // start at the length of the first connection
                longestPathSoFar += ContinueDownPath(longestPathSoFar, connection, pgs.OwnedConnections.Where(conn => !checkedDownThisPath.Contains(conn)).ToList()); // Move down the path
                if (longestPathSoFar > longestPath) { longestPath = longestPathSoFar; } // if the longest path from this Connection starting point is the longest of all
                                                                                        // Players, set that
            }
            return longestPath;
        }

        /// <summary>
        /// Returns whether two Connection have a City in common
        /// </summary>
        /// <param name="conn1">The first Connection to compare</param>
        /// <param name="conn2">The second Connection to compare</param>
        /// <returns>If the Connection have a City in common</returns>
        private bool ShareEnd(Connection conn1, Connection conn2)
        {
            return conn1.City1 == conn2.City1 || conn1.City1 == conn2.City2 || conn1.City2 == conn2.City1 || conn1.City2 == conn2.City2;
        }


        /// <summary>
        /// Continues down the path if possible and returns the longest reached path.
        /// </summary>
        /// <param name="pathLength">The path length so far</param>
        /// <param name="lastAddedConnection">The last added Connection. Its length has already been added</param>
        /// <param name="leftToCheck">The owned Connection left to check for end sharing</param>
        /// <returns>Returns the longest path length possible from the lastAddedConnection</returns>
        private int ContinueDownPath(int pathLength, Connection lastAddedConnection, List<Connection> leftToCheck)
        {
            int longestContinuedPath = pathLength;
            foreach (Connection connection2 in leftToCheck) // see if the connection connects to the last added connection
            {
                int lengthSoFar = pathLength; // reset the length to the point after adding lastAddedConnection
                if (ShareEnd(lastAddedConnection, connection2)) // they share a city
                {
                    lengthSoFar += (int)connection2.NumSegments; // add its length to the path length
                    lengthSoFar = ContinueDownPath(lengthSoFar, connection2, leftToCheck.Where(conn => conn != connection2).ToList());
                    // continue down the path with the newly added connection as the next starting point
                    if (lengthSoFar > longestContinuedPath) // if we found a new longest path
                    {
                        longestContinuedPath = lengthSoFar;
                    }
                }
            }
            return longestContinuedPath;
        }


        /// <summary>
        /// Loops through all Turns of a Trains.com game.
        /// </summary>
        /// <param name="rgs">The current RefereeGameState of the game</param>
        /// <param name="players">The IPlayers of the game where 0 is the oldest IPlayer and players.Count is the youngest. This list
        /// gets updated as Player objects are kicked</param>
        /// <returns></returns>
        private RefereeGameState TurnPhase(RefereeGameState rgs, IList<IPlayer> players)
        {
            IList<RefereeGameState> lastRoundRefStates = new List<RefereeGameState>(); // an array of the state after currPlayerIndex IPlayer's turn a round ago
            RefereeGameState currentTurn = rgs;
            int previousTurnPlayerIndex = -1;
            do
            {
                if (lastRoundRefStates.Count - 1 < currentTurn.CurrentPlayerIndex || !NoMoreChanges(lastRoundRefStates[previousTurnPlayerIndex], currentTurn))
                {
                    previousTurnPlayerIndex = currentTurn.CurrentPlayerIndex;
                    OverWriteLastRoundState(currentTurn.CurrentPlayerIndex, players.Count(), lastRoundRefStates, currentTurn);
                    currentTurn = PlayTurn(currentTurn, players, players[currentTurn.CurrentPlayerIndex], currentTurn.PlayerGameStates[currentTurn.CurrentPlayerIndex]); // play CurrPlayerIndex IPlayer's turn
                }
                else { return currentTurn; }
            } while (!CheckRails(currentTurn) && Deck.Count != 0); // assumes on the first round, a IPlayer HAS to be able to make a move
            return LastRound(currentTurn, players, currentTurn.CurrentPlayerIndex); // Return the RGS after the last round
        }


        /// <summary>
        /// Overwrites, if it exists, the currPlayerIndex IPlayer's post turn state from last round.
        /// </summary>
        /// <param name="currPlayerIndex">The index of the current IPlayer</param>
        /// <param name="playerCount">The number of IPlayers in the game</param>
        /// <param name="lastRoundRefStates">A List of PlayerGameState from each IPlayer's last successful turn. Only includes states for currently active players 
        /// (not those who have been kicked)</param>
        /// <param name="currentTurn">The RefereeGameState after the current IPlayer's turn for this round</param>
        private void OverWriteLastRoundState(int currPlayerIndex, int playerCount, IList<RefereeGameState> lastRoundRefStates, RefereeGameState currentTurn)
        {
            if (currPlayerIndex >= lastRoundRefStates.Count)
            {
                lastRoundRefStates.Add(currentTurn);
            }
            else if (currPlayerIndex != 0)
            {
                lastRoundRefStates[currPlayerIndex - 1] = currentTurn;
            }
        }

        /// <summary>
        /// Determines if the game has reached the end condition of no moves being able to be made.
        /// </summary>
        /// <param name="previous">The last end of the last turn's RefereeGameState</param>
        /// <param name="current">The end of the current turn's RefereeGameState </param>
        /// <returns>Whether the game has reached an end condition as defined in The Game: Trains</returns>
        private bool NoMoreChanges(RefereeGameState previous, RefereeGameState current)
        {
            return previous == current || current.PlayerGameStates[0].AvailableConnections.Count == 0;
        }

        /// <summary>
        /// Returns if any PlayerGameState has fewer than the minimum acceptable rail count.
        /// </summary>
        /// <param name="current">The current RefereeGameState</param>
        /// <returns>If there is a PlayerGameState with too few of rails to occupy another Connection</returns>
        private bool CheckRails(RefereeGameState current)
        {
            foreach (PlayerGameState pgs in current.PlayerGameStates)
            {
                if (pgs.Rails < Constants.minimumPlayerRails) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Increments the index to the next appropriate value.
        /// </summary>
        /// <param name="currPlayerIndex">The current index</param>
        /// <param name="playerCount">The number of players in the game</param>
        /// <returns>The next appropriate index</returns>
        private int IncrementIndex(int currPlayerIndex, int playerCount)
        {
            if (currPlayerIndex + 1 >= playerCount)
            {
                return 0;
            }
            else
            {
                return currPlayerIndex + 1;
            }
        }

        /// <summary>
        /// Plays a turn from a IPlayer and updates the state of the game.
        /// </summary>
        /// <param name="rgs">The current RefereeGameState.</param>
        /// <param name="player">The IPlayer whose turn it is</param>
        /// <param name="pgs">The PlayerGameState of the IPlayer</param>
        /// <returns>The RefereeGameState after executing the IPlayer's turn</returns>
        private RefereeGameState PlayTurn(RefereeGameState rgs, IList<IPlayer> players, IPlayer player, PlayerGameState pgs)
        {
            Tuple<bool, ITurn> output = CheckPlayerPlayTurnMisbehavior(player.PlayTurn, new PlayerGameState(pgs));
            if (!output.Item1)
            {
                return KickCurrentPlayer(players, rgs);
            }
            else
            {
                return ExecuteTurn(rgs, players, player, output.Item2); // execute turn and update rgs
            }
        }

        /// <summary>
        /// Executes a IPlayer. Either a IPlayer makes a legal move of acquiring a Connection or drawing cards, or it was removed from the game
        /// and its acquired Connection collection was made available again.
        /// </summary>
        /// <param name="rgs">The current RefereeGameState</param>
        /// <param name="player">The IPlayer whose turn it is</param>
        /// <param name="turnBehavior">The type of turn the IPlayer is executing</param>
        /// <returns>The RefereeGameState after executing the IPlayer's turn</returns>
        private RefereeGameState ExecuteTurn(RefereeGameState rgs, IList<IPlayer> players, IPlayer player, ITurn turnBehavior)
        {
            if (turnBehavior.GetType() == typeof(DrawCardsTurn))
            {
                IList<ColoredCard> addToHand = DealCards(Deck, Constants.drawCardsCount); // deal cards for its turn
                if (!CheckPlayerMoreCardsTimeout(player.MoreCards, addToHand))
                {
                    return KickCurrentPlayer(players, rgs);
                }
                else
                {
                    return UpdateDrawCards(addToHand, rgs);
                }
            }
            else if (turnBehavior.GetType() == typeof(AcquireConnectionTurn) && ((AcquireConnectionTurn)turnBehavior).ToAcquire != null)
            {
                return UpdateAcquireConnection(players, ((AcquireConnectionTurn)turnBehavior).ToAcquire, rgs);
            }
            else // Player returned some invalid ITurn that is not accepted
            {
                return KickCurrentPlayer(players, rgs);
            }
        }

        /// <summary>
        /// Updates the RefereeGameState to add the given cards to the CurrentPlayerIndex PlayerGameState's hand
        /// </summary>
        /// <param name="addToHand">The cards to add to the PlayerGameState</param>
        /// <param name="rgs">The current RefereeGameState</param>
        /// <returns>The RefereeGameState after executing the IPlayer's DrawCardTurn</returns>
        private RefereeGameState UpdateDrawCards(IList<ColoredCard> addToHand, RefereeGameState rgs)
        {
            PlayerGameState pgs = rgs.PlayerGameStates[rgs.CurrentPlayerIndex]; // get the current IPlayer's PGS
            IList<ColoredCard> currentHand = pgs.Cards.ToList(); // get its current hand
            foreach (ColoredCard addCard in addToHand) // add dealt cards to its hand
            {
                currentHand.Add(addCard);
            }
            PlayerGameState newPgs = new PlayerGameState(pgs.GameMap, pgs.Rails, currentHand, pgs.Destinations, pgs.OwnedConnections, pgs.AvailableConnections);
            IList<PlayerGameState> playerGameStates = rgs.PlayerGameStates.ToList(); // get a changeable list
            playerGameStates[rgs.CurrentPlayerIndex] = newPgs; // update that PlayerGameState while maintaing order
            return new RefereeGameState(IncrementIndex(rgs.CurrentPlayerIndex, rgs.PlayerGameStates.Count), rgs.GameMap, playerGameStates);
        }

        /// <summary>
        /// Updates the current RefereeGameState to reflect the Connection was acquired by the CurrentPlayerIndex IPlayer
        /// </summary>
        /// <param name="toAcquire">The Connection being acquired</param>
        /// <param name="rgs">The current RefereeGameState</param>
        /// <returns>The state of the game after the given Connection is acquired</returns>
        private RefereeGameState UpdateAcquireConnection(IList<IPlayer> players, Connection toAcquire, RefereeGameState rgs)
        {
            if (rgs.CanCurrentPlayerAcquireConnection(toAcquire))
            {
                return new RefereeGameState(rgs, toAcquire, IncrementIndex(rgs.CurrentPlayerIndex + 1, rgs.PlayerGameStates.Count));
            }
            else
            {
                return KickCurrentPlayer(players, rgs);
            }
        }

        /// <summary>
        /// Removes the CurrentPlayerIndex IPlayer from the game
        /// </summary>
        /// <param name="rgs">The current RefereeGameState</param>
        /// <returns>The RefereeGameState after kicking the current player</returns>
        private RefereeGameState KickCurrentPlayer(IList<IPlayer> players, RefereeGameState rgs)
        {
            players.RemoveAt(rgs.CurrentPlayerIndex);
            return new RefereeGameState(rgs);
        }

        /// <summary>
        /// Operates the turns for the last round of a Trains.com game.
        /// </summary>
        /// <param name="rgs">The current RefereeGameState</param>
        /// <param name="players">The IPlayer whose turn it is</param>
        /// <param name="playerGameStates">The PlayerGameState of the IPlayers in the game</param>
        /// <param name="startedLastTurn">The index of the IPlayer that initiated end game conditions</param>
        /// <returns>The RefereeGameState after executing the IPlayer's turn</returns>
        private RefereeGameState LastRound(RefereeGameState rgs, IList<IPlayer> players, int startedLastTurn)
        {
            int playerCount = players.Count();
            int currPlayerIndex = IncrementIndex(startedLastTurn, playerCount);
            while (currPlayerIndex != startedLastTurn) // once we get back to the player who started the Last Round we are done
            {
                rgs = PlayTurn(rgs, players, players[currPlayerIndex], rgs.PlayerGameStates[currPlayerIndex]);
                currPlayerIndex = IncrementIndex(currPlayerIndex, players.Count());
            }
            return rgs;
        }

        /// <summary>
        /// Initially sets up a IPlayer with cards, rails, and the map for a Trains.com game.
        /// </summary>
        /// <param name="map">The Map the game is being played on</param>
        /// <param name="players">The IPlayers playing the game</param>
        /// <returns>A List of PlayerGameState where the IPlayer in the given list of player exists at the same index as its PlayerGameState in the return value."</returns>
        private IList<PlayerGameState> SetupPlayers(Map map, IList<IPlayer> players)
        {
            IList<PlayerGameState> playerGameStates = new List<PlayerGameState>();
            foreach (IPlayer player in players)
            {
                IList<ColoredCard> playerHand = DealCards(Deck, Constants.setupDealtPlayerCards);
                if (!CheckPlayerSetupTimeout(player.Setup, map, Constants.setupNumberRails, playerHand))
                {
                    players.Remove(player); // kick the player (no impact on game at this point)
                }
                else
                {
                    playerGameStates.Add(new PlayerGameState(map, Constants.setupNumberRails, playerHand, new HashSet<Destination>(),
                                                         new HashSet<Connection>(), map.Connections));
                }
            }
            return playerGameStates;
        }

        /// <summary>
        /// Sets the IPlayer's Destination's via IPlayer.ChooseDestination()
        /// </summary>
        /// <param name="map">The Map whose destinations are being chosen from</param>
        /// <param name="players">The IPlayer objects choosing destinations</param>
        /// <param name="playerGameStates">The PlayerGameState objects for the IPlayer at the same index in the players param</param>
        /// <returns>The List containing updated PlayerGameState objects after each IPlayer chooses its destinations</returns>
        private IList<PlayerGameState> SetPlayerDestinations(Map map, IList<IPlayer> players, IList<PlayerGameState> playerGameStates)
        {
            ICollection<Destination> availableDestinations = map.AllFeasibleDestinations();

            for (int index = 0; index < players.Count; index++)
            {
                ICollection<Destination> playerSelected = AssignDestinations(SelectDestinations(availableDestinations), index, players, playerGameStates);
                availableDestinations = UpdateAvailableDestinations(availableDestinations, playerSelected);
            }
            return playerGameStates;
        }

        /// <summary>
        /// Assigns Destination to the given Player. If the Player misbehaves, they are removed from the list of Player.
        /// </summary>
        /// <param name="playerSelectionPool">The Destination for the Player to choose from</param>
        /// <param name="player">The Player that is choosing</param>
        /// <param name="players">The complete list of Player in the game</param>
        /// <param name="pgs">The PlayerGameState of the Player choosing Destination objects</param>
        /// <returns>The Destination objects chosen by the Player or an empty set if the Player got kicked.</returns>
        private ICollection<Destination> AssignDestinations(ICollection<Destination> playerSelectionPool, int index, IList<IPlayer> players, IList<PlayerGameState> playerGameStates)
        {
            IPlayer player = players[index];
            PlayerGameState pgs = playerGameStates[index];
            Tuple<bool, ICollection<Destination>> output = CheckPlayerChooseDestinationsMisbehavior(player.ChooseDestinations, playerSelectionPool);
            if (!output.Item1 || output.Item2.Count != (playerSelectionPool.Count - Constants.setupDestinationsCount))
            {
                players.RemoveAt(index); // kick the player (no impact on game at this point)
                playerGameStates.RemoveAt(index); // kick the player (no impact on game at this point)
                return new HashSet<Destination>(); // assigned 0 Destination to the given Player
            }
            else
            {   // Player didn't error and chose enough Destination
                try // PlayerGameState constructor ensures that Destination chosen are valid given the GameMap 
                {   // and try catch ensure the ICollection contains Destination objects and not null
                    ICollection<Destination> rejectedDestinations = output.Item2; // Player chooses 2 and returns 3 
                    ICollection<Destination> acceptedDestinations = playerSelectionPool.Where(dest => !rejectedDestinations.Contains(dest)).ToHashSet(); // filter to the chosen

                    pgs = new PlayerGameState(pgs.GameMap, pgs.Rails, pgs.Cards, acceptedDestinations, pgs.OwnedConnections, pgs.AvailableConnections);
                    return acceptedDestinations;
                }
                catch
                {
                    players.RemoveAt(index); // kick the player (no impact on game at this point)
                    playerGameStates.RemoveAt(index); // kick the player (no impact on game at this point)
                    return new HashSet<Destination>(); // assigned 0 Destination to the given Player
                }
            }
        }

        /// <summary>
        /// Updates the given available Destination Collection to reflect the given accepted destinations are no longer available.
        /// </summary>
        /// <param name="availableDestinations">Previous collection of available Destination</param>
        /// <param name="acceptedDestinations">Destination that are no longer available</param>
        /// <returns>An updated Collection of available Destination</returns>
        private ICollection<Destination> UpdateAvailableDestinations(ICollection<Destination> availableDestinations, ICollection<Destination> acceptedDestinations)
        {
            foreach (Destination selectedDestination in acceptedDestinations) // remove chosen from available
            {
                availableDestinations.Remove(selectedDestination);
            }
            return availableDestinations;
        }

        /// <summary>
        /// Initializes the RefereeGameState for this Referee's Trains.com game
        /// </summary>
        /// <param name="map">The Map the game is being played with</param>
        /// <param name="playerGameStates">The PlayerGameState's of players playing the game</param>
        /// <returns>The initial RefereeGameState</returns>
        private RefereeGameState InitializeRefereeGameState(Map map, IList<PlayerGameState> playerGameStates)
        {
            RefereeGameState rgs = new RefereeGameState(0, map, playerGameStates);
            return rgs;
        }

        /// <summary>
        /// Deals a given number of cards from the given deck of cards.
        /// </summary>
        /// <param name="deck">The deck of cards with which to deal from</param>
        /// <param name="cardsToDeal">The number of cards to deal</param>
        /// <returns>An IList representing the cards dealt. Removes the dealt cards from the argument deck</returns>
        private IList<ColoredCard> DealCards(IList<ColoredCard> deck, int cardsToDeal)
        {
            IList<ColoredCard> playersHand = new List<ColoredCard>();
            // if we have enough cards to deal as requested, deal them out, or deal all remaining cards if not enough but non-0 cards left
            for (int ii = 0; ii < cardsToDeal; ii++)
            {
                if (deck.Count != 0)
                {
                    playersHand.Add(deck.ElementAt(0));
                    deck.RemoveAt(0);
                }
            }
            return playersHand;
        }

        /// <summary>
        /// Executes Player.Setup and returns whether it was completed within the allotted time span.
        /// Should only be used with Player.Setup as the given Action.
        /// </summary>
        /// <param name="setup">Player.Setup(). No return type.</param>
        /// <param name="map">Player.Setup() map parameter.</param>
        /// <param name="rails">Player.Setup() rails parameter.</param>
        /// <param name="cards">Player.Setup() cards parameter.</param>
        /// <returns>Whether Player.Setup() executed within Constants.playerTimeAllotted ms.</returns>
        private bool CheckPlayerSetupTimeout(Action<Map, int, IList<ColoredCard>> setup, Map map, int rails, IList<ColoredCard> cards)
        {
            try
            {
                Task task = Task.Run(() => setup(map, rails, new ReadOnlyCollection<ColoredCard>(cards)));
                if (task.Wait(TimeSpan.FromMilliseconds(Constants.playerTimeAllotted)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes Player.MoreCards and returns whether it was completed within the allotted time span.
        /// Should only be used with Player.MoreCards as the given Action.
        /// </summary>
        /// <param name="moreCards">Player.MoreCards(). No return type.</param>
        /// <param name="cards">Player.MoreCards() destinations parameter.</param>
        /// <returns>Whether Player.MoreCards() executed within Constants.playerTimeAllotted ms.</returns>
        private bool CheckPlayerMoreCardsTimeout(Action<IList<ColoredCard>> moreCards, IList<ColoredCard> cards)
        {
            try
            {
                Task task = Task.Run(() => moreCards(new ReadOnlyCollection<ColoredCard>(cards)));
                if (task.Wait(TimeSpan.FromMilliseconds(Constants.playerTimeAllotted)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes Player.ChooseDestinations and returns whether it was completed within the allotted time span.
        /// Should only be used with Player.ChooseDestinations as the given Func.
        /// </summary>
        /// <param name="chooseDestinations">Player.ChooseDestinations(). No return type.</param>
        /// <param name="destinations">Player.ChooseDestinations() destinations parameter.</param>
        /// <returns>A tuple containing:
        /// a bool representing whether Player.ChooseDestinations() executed within Constants.playerTimeAllotted ms.
        /// The output value of Player.ChooseDestinations().</returns>
        private Tuple<bool, ICollection<Destination>> CheckPlayerChooseDestinationsMisbehavior(Func<ICollection<Destination>, ICollection<Destination>> chooseDestinations, ICollection<Destination> destinations)
        {
            try
            {
                Task<ICollection<Destination>> task = Task.Run(() => chooseDestinations(new ReadOnlyCollection<Destination>(destinations.ToList())));
                if (task.Wait(TimeSpan.FromMilliseconds(Constants.playerTimeAllotted)))
                {
                    return Tuple.Create(true, task.Result);
                }
                else
                {
                    return Tuple.Create(false, task.Result);
                }
            }
            catch
            {
                return Tuple.Create<bool, ICollection<Destination>>(false, new HashSet<Destination>());
            }
        }

        /// <summary>
        /// Executes Player.PlayTurn and returns whether it was completed within the allotted time span.
        /// Should only be used with Player.PlayTurn as the given Func.
        /// </summary>
        /// <param name="playTurn">Player.PlayTurn(). No return type.</param>
        /// <param name="pgs">Player.PlayTurn() pgs parameter.</param>
        /// <returns>A tuple containing:
        /// a bool representing whether Player.PlayTurn() executed within Constants.playerTimeAllotted ms.
        /// The output value of Player.PlayTurn().</returns>
        private Tuple<bool, ITurn> CheckPlayerPlayTurnMisbehavior(Func<PlayerGameState, ITurn> playTurn, PlayerGameState pgs)
        {
            try
            {
                Task<ITurn> task = Task.Run(() => playTurn(pgs));
                if (task.Wait(TimeSpan.FromMilliseconds(Constants.playerTimeAllotted)))
                {
                    return Tuple.Create(true, task.Result);
                }
                else
                {
                    return Tuple.Create(false, task.Result);
                }
            }
            catch
            {
                return Tuple.Create<bool, ITurn>(false, new DrawCardsTurn());
            }
        }

        /// <summary>
        /// Executes Player.GameOver and returns whether it was completed within the allotted time span.
        /// Should only be used with Player.GameOver as the given Action.
        /// </summary>
        /// <param name="gameOver">Player.GameOver(). No return type.</param>
        /// <param name="won">Player.GameOver() destinations parameter.</param>
        /// <returns>Whether Player.GameOver() executed within Constants.playerTimeAllotted ms.</returns>
        private bool CheckPlayerGameOverMisbehavior(Action<bool> gameOver, bool won)
        {
            Task task = Task.Run(() => gameOver(won));
            if (task.Wait(TimeSpan.FromMilliseconds(Constants.playerTimeAllotted)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
