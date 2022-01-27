using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Trains.Common;
using Trains.Common.GameState;
using Trains.Common.Map;
using Trains.Player;
namespace Trains.Admin
{
    /// <summary>
    /// Represent a Referee that can manage a game of Trains. 
    /// </summary>
    /// <remarks>
    /// Referee is in charge of checking that moves are valid. I.e. that a player does not cheat by performing an
    /// illegal action or raising an exception. Referee does not take into account issues such as long response time or
    /// other networking issues that may arise as a result of players not being a direct part of the running process.
    /// Such issues should be handled by the networking component.
    /// </remarks>
    public class Referee
    {
        private readonly RefereeGameState _gameState;
        private readonly TrainsMap _map;
        readonly uint _railsPerPlayer;
        readonly uint _cardsPerPlayer;

        /// <summary>
        /// Constructor for making a Referee
        /// </summary>
        /// <param name="startingCards">Dictionary of colors to number of that colored cards in the deck</param>
        /// <param name="railsPerPlayer">The starting number of rails for each IPlayer</param>
        /// <param name="cardsPerPlayer">The starting number of cards for each IPlayer</param>
        /// <param name="map">The TrainsMap that this Referee will run a game using</param>
        /// <param name="players">The ordered list of IPlayer playing the game. Index 0 is the oldest player and index count - 1 is the youngest.</param>
        public Referee(Dictionary<Color, uint> startingCards, uint railsPerPlayer, uint cardsPerPlayer, TrainsMap map, List<IPlayer> players, Deck? deck = null)
        {
            _map = map;
            _railsPerPlayer = railsPerPlayer;
            _cardsPerPlayer = cardsPerPlayer;
            if (deck is null)
            {
                deck = new Deck(startingCards, map.GetAllFeasibleDestinations().ToList());
            }
            _gameState = new RefereeGameState((uint)players.Count(), players, map.Connections.ToHashSet(), deck);
            foreach (IPlayer player in players)
            {
                SignIn(player);
            }
        }

        /// <summary>
        /// Constructor for making a Referee for testing purposes only
        /// </summary>
        /// <param name="startingCards">Dictionary of colors to number of that colored cards in the deck</param>
        /// <param name="railsPerPlayer">The starting number of rails for each IPlayer</param>
        /// <param name="cardsPerPlayer">The starting number of cards for each IPlayer</param>
        /// <param name="map">The TrainsMap that this Referee will run a game using</param>
        /// <param name="players">The ordered list of IPlayer playing the game. Index 0 is the oldest player and index count - 1 is the youngest.</param>
        /// <param name="random">The random object to pass to the Deck for ensuring randomness is repeatable and testable</param>
        public Referee(Dictionary<Color, uint> startingCards, uint railsPerPlayer, uint cardsPerPlayer, TrainsMap map, List<IPlayer> players, Random random) :
            this(startingCards, Constants.setupPlayerRailsCount, Constants.setupPlayerCardsCount, map, players, new Deck(startingCards, map.GetAllFeasibleDestinations().ToList(), random))
        { }

        /// <summary>
        /// Constructor for making a Referee with a default even deck
        /// </summary>
        /// <param name="map"></param>
        /// <param name="players"></param>
        public Referee(TrainsMap map, List<IPlayer> players) :
            this(Constants.defaultDeck, Constants.setupPlayerRailsCount, Constants.setupPlayerCardsCount, map, players)
        { }

        /// <summary>
        /// Adds an IPlayer to the game."
        /// </summary>
        /// <param name="player">The new IPlayer to the game</param>
        void SignIn(IPlayer player)
        {
            _gameState.PlayerInventories[player] = new PlayerInventory(_gameState.Cards.DrawCards(_cardsPerPlayer),
                _railsPerPlayer, new HashSet<Destination>());
        }

        /// <summary>
        /// Getter for retrieving the game's TrainsMap.
        /// </summary>
        /// <returns>The TrainsMap this game is being played with</returns>
        public TrainsMap RequestMap()
        {
            return _map;
        }

        /// <summary>
        /// Removes the given IPlayer from the game. Removing the IPlayer makes the Connection
        /// it has acquired available again.
        /// </summary>
        /// <param name="player">The IPlayer to remove</param>
        void KickPlayer(IPlayer player)
        {
            foreach (Connection con in _gameState.ClaimedConnections[player])
            {
                _gameState.AvailableConnections.Add(con);
            }
            _gameState.PlayerInventories.Remove(player);
            _gameState.Players.Remove(player);
            _gameState.KickedPlayers.Add(player);
        }

        /// <summary>
        /// Getter for the RefereeGameState.
        /// </summary>
        /// <returns>This Referee's RefereeGameState</returns>
        public RefereeGameState GetRefereeGameState()
        {
            return _gameState;
        }

        /// <summary>
        /// Runs a Trains.com game. Returning the ranking
        /// </summary>
        public Dictionary<IPlayer, int?> RunGame()
        {
            Dictionary<IPlayer, RefereeGameState> previousGameState = new();
            while (_gameState.Phase != GamePhase.Finished)
            {
                object preTurnGameState = _gameState.Clone();
                switch (_gameState.Phase)
                {
                    case GamePhase.SetUp:
                        SetUpPlayers();
                        preTurnGameState = SetUpGameState(previousGameState);
                        break;
                    case GamePhase.InProgress:
                        if (_gameState.Players.Count == 0)
                        {
                            _gameState.Phase = GamePhase.Finished;
                            break;
                        }
                        HandleRound(previousGameState);
                        break;
                    case GamePhase.FinalRound:
                        LastRound();
                        break;
                }
            }
            Dictionary<IPlayer, int?> ranking = _gameState.GetRankings();
            NotifyPlayers(ranking, ranking.Values.Max());
            return ranking;
        }

        /// <summary>
        /// Handles the last round of the game.
        /// </summary>
        private void LastRound()
        {
            int indexOutOfCard = _gameState.Players.FindIndex((p) => _gameState.IsGameOverByPlayerOutOfCards(p));
            if (_gameState.Players.Count != 1)
            {
                for (int i = (indexOutOfCard + 1); i < _gameState.Players.Count - 1 + indexOutOfCard; i++)
                {
                    PlayerTurn(_gameState.Players[i % _gameState.Players.Count]);
                }
            }
            _gameState.Phase = GamePhase.Finished;
        }

        /// <summary>
        /// Notifies players of their win/loss status.
        /// </summary>
        /// <param name="ranking">The rankings of players.</param>
        /// <param name="maxScore">The highest score in the game.</param>
        private void NotifyPlayers(Dictionary<IPlayer, int?> ranking, int? maxScore)
        {
            foreach (var player in new List<IPlayer>(_gameState.Players))
            {
                WrapPlayerCall(() => player.IsWinner(ranking[player] == maxScore), player);
            }
        }

        /// <summary>
        /// Handles the current round of a Trains.com game.
        /// </summary>
        /// <param name="previousGameState">A mapping of players to the game state after their last turn.</param>
        private void HandleRound(Dictionary<IPlayer, RefereeGameState> previousGameState)
        {
            foreach (var player in new List<IPlayer>(_gameState.Players))
            {
                PlayerTurn(player);
                if (!_gameState.Players.Contains(player))
                {
                    continue;
                }
                if (_gameState.IsGameOverByPlayerOutOfCards(player))
                {
                    _gameState.Phase = GamePhase.FinalRound;
                    break;
                }
                if (previousGameState[player].Equals(_gameState))
                {
                    _gameState.Phase = GamePhase.Finished;
                    break;
                }
                else
                {
                    previousGameState[player] = (RefereeGameState)_gameState.Clone();
                }
            }
        }

        /// <summary>
        /// Sets up the Referee's gamestate.
        /// </summary>
        object SetUpGameState(Dictionary<IPlayer, RefereeGameState> previousGameState)
        {
            object preTurnGameState = _gameState.Clone();
            foreach (var player in _gameState.Players)
            {
                previousGameState[player] = (RefereeGameState)preTurnGameState;
            }
            _gameState.Phase = GamePhase.InProgress;
            return preTurnGameState;
        }
        /// <summary>
        /// Sets up the Trains.com game.
        /// </summary>
        void SetUpPlayers()
        {
            foreach (var player in new List<IPlayer>(_gameState.Players))
            {
                IImmutableSet<Destination> givenDests = _gameState.Cards.GetDestinations().ToImmutableHashSet();
                IImmutableSet<Destination> unchosenDests = new HashSet<Destination>().ToImmutableHashSet(); // make empty for if check
                if (!WrapPlayerCall(() => player.Setup(_map, Constants.setupPlayerRailsCount, _gameState.Cards.DrawCards(Constants.setupPlayerCardsCount)), player)
                    || !WrapPlayerCall(() => unchosenDests = player.ChooseDestinations(givenDests), player)
                    || unchosenDests.Count != Constants.destinationsToChooseFrom - Constants.destinationsPerPlayer)
                {
                    continue;
                }
                // check the response is valid by looping to make it size agnostic, we checked validity of size above
                foreach (Destination dest in unchosenDests)
                {
                    if (!givenDests.Contains(dest))
                    {
                        KickPlayer(player);
                        continue;
                    }
                }
                _gameState.Cards.Destinations.RemoveAll(d => unchosenDests.Contains(d));
                _gameState.PlayerInventories[player].Destinations = new HashSet<Destination>(unchosenDests);
            }
        }

        /// <summary>
        /// Allows the given IPlayer to play a turn.
        /// </summary>
        /// <param name="player">The IPlayer whose turn it is</param>
        void PlayerTurn(IPlayer player)
        {
            PlayerResponse? response = null;

            if (WrapPlayerCall(() => response = player.PlayTurn(_gameState.GetPlayerGameState(player)), player))
            {
                HandleTurn(response!, player);
            }
        }

        /// <summary>
        /// Handles the turn from the IPlayer.
        /// </summary>
        /// <param name="response">The PlayerResponse for what to do for its turn</param>
        /// <param name="player">The IPlayer whose turn it is</param>
        void HandleTurn(PlayerResponse response, IPlayer player)
        {
            if (response.ResponseType == ResponseType.DrawCards)
            {
                var dealtCards = _gameState.Cards.DrawCards(Constants.dealMoreCards);
                _gameState.PlayerInventories[player].UpdateCard(dealtCards);
                WrapPlayerCall(() => player.MoreCards(dealtCards), player);
                return;
            }
            else if (response.ResponseType == ResponseType.ClaimConnection)
            {
                Connection claimedCon = response.RequestedConnectionClaim!;
                if (_gameState.CanClaim(player, claimedCon)) // If the Player can claim the given connection
                {
                    _gameState.ClaimConnection(player, claimedCon);
                    return;
                }
            }
            // the only time this gets reached is if the response.ReponseType given by the Player is an invalid option (reflection)
            KickPlayer(player);
        }


        /// <summary>
        /// Wraps every Player call with timeout and error handling
        /// </summary>
        /// <param name="func">The player function wrapped in an Action</param>
        /// <param name="player">The IPlayer whose call is getting wrapped</param>
        /// <returns>Whether the Player misbehaved.</returns>
        bool WrapPlayerCall(Action func, IPlayer player)
        {
            try
            {
                Task task = new Task(() => func.Invoke());
                task.RunSynchronously();
                if (!task.Wait(TimeSpan.FromMilliseconds(Constants.maxResponseWait)))
                {
                    throw new TimeoutException("Player took too long");
                }
                return true;
            }
            catch (Exception e)
            {
                KickPlayer(player);
                return false;
            }
        }
    }
}