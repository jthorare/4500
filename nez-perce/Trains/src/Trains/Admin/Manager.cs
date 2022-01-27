using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common;
using Trains.Common.Map;
using Trains.Player;

namespace Trains.Admin
{
    public class Manager
    {
        private List<IPlayer> _players;
        private List<IPlayer> _kickedPlayers = new List<IPlayer>();
        private Deck _deck;

        /// <summary>
        /// Constructor a tournament manager.
        /// </summary>
        /// <param name="activePlayers">List of players in the tournament where index 0 is the oldest and youngest is at the end.</param>
        /// <param name="deck">Single Deck used for each game in the tournament.</param>
        public Manager(List<IPlayer> activePlayers, Deck deck)
        {
            _players = activePlayers;
            _deck = deck;
        }

        /// <summary>
        /// Alerts all the players that the tournament has started.
        /// </summary>
        /// <returns>The mapping of players to their submission for the tournament TrainsMap</returns>
        public Dictionary<IPlayer, TrainsMap> AlertPlayers()
        {
            List<IPlayer> players = new List<IPlayer>(_players); // make a copy so that moving a player from _players -> _kickedPlayers does not throw exeption when iterating
            Dictionary<IPlayer, TrainsMap> playerSubmissions = new Dictionary<IPlayer, TrainsMap>();
            foreach (IPlayer player in players)
            {
                TrainsMap? submission = null;
                if (WrapPlayerCall(() => submission = player.Start(true), player) && submission != null) // true means the tournament started
                {
                    playerSubmissions.Add(player, submission);
                }
            }
            return playerSubmissions;
        }

        /// <summary>
        /// Wraps every Player call with timeout and error handling
        /// </summary>
        /// <param name="func">The player function wrapped in an Action</param>
        /// <param name="player">The IPlayer whose call is getting wrapped</param>
        /// <returns>Whether the Player misbehaved.</returns>
        public bool WrapPlayerCall(Action func, IPlayer player)
        {
            try
            {
                Task task = Task.Run(() => func.Invoke());
                if (!task.Wait(TimeSpan.FromMilliseconds(Constants.maxResponseWait)))
                {
                    throw new TimeoutException("Player took too long");
                }
                return true;
            }
            catch (Exception e)
            {
                _kickedPlayers.Add(player);
                _players.Remove(player);
                return false;
            }
        }

        /// <summary>
        /// Runs a Trains.com tournament.
        /// </summary>
        /// <returns>A Tuple where the first element is the list of winners and the second element is the list of kicked players</returns>
        public Tuple<List<IPlayer>, List<IPlayer>> RunTournament()
        {
            Dictionary<IPlayer, TrainsMap> playerSubmissions = AlertPlayers();
            TrainsMap tournamentMap = ChooseMap(playerSubmissions); // the map
            Tuple<List<IPlayer>, List<IPlayer>> tournamentResults = TournamentLoop(tournamentMap);
            (var winners, var _) = tournamentResults;
            // close tournament 
            CloseTournament(winners);
            return tournamentResults;
        }

        /// <summary>
        /// Runs the loop of all games of a tournament.
        /// </summary>
        /// <param name="tournamentMap">The map for each game of the tournament to be played on</param>
        /// <returns>A Tuple where the first element is the list of winners and the second element </returns>
        private Tuple<List<IPlayer>, List<IPlayer>> TournamentLoop(TrainsMap tournamentMap)
        {
            List<IPlayer> previousRound; // all players in _players are to play the first round
            List<IPlayer> currentRound = _players;

            do
            {
                previousRound = currentRound;
                IEnumerable<IEnumerable<IPlayer>> games = AssignPlayersToGames(currentRound); // reassign players to
                (currentRound, var kicked) = PlayTournamentRound(tournamentMap, games); // play the round and get the next rounds players
                _kickedPlayers.AddRange(kicked);
            } while (!TournamentIsOver(previousRound, currentRound));
            return Tuple.Create(currentRound, _kickedPlayers);
        }

        private bool TournamentIsOver(List<IPlayer> previousRound, List<IPlayer> currentRound)
        {
            return currentRound.Count < 2 || SameSetOfWinners(previousRound, currentRound);
        }

        /// <summary>
        /// Closes the Trains.com tournament.
        /// </summary>
        /// <param name="winners">The list of winners</param>
        void CloseTournament(List<IPlayer> winners)
        {
            List<IPlayer> losers = _players.Where(player => !winners.Contains(player)).ToList();
            SendWinLoss(losers, false);
            SendWinLoss(winners, true);
        }

        /// <summary>
        /// Sends win/loss message to each player.
        /// </summary>
        /// <param name="players">The players to alert</param>
        /// <param name="won">true if the players won, false if they lost</param>
        void SendWinLoss(List<IPlayer> players, bool won)
        {
            foreach (IPlayer player in players)
            {
                WrapPlayerCall(() => player.End(won), player);
            }
        }

        private bool SameSetOfWinners(List<IPlayer> previousRound, List<IPlayer> currentRound)
        {
            foreach (IPlayer player in previousRound) // check last rounds winners to this rounds
            {
                if (!currentRound.Contains(player)) { return false; } // a player did not advance from last round
            }
            return true; // we have the exact same set of winners as last round
        }

        /// <summary>
        /// Chunks the players into games.
        /// </summary>
        /// <param name="players">The players of the game</param>
        /// <returns>A List of Lists where each element represents the list of players in a single game.</returns>
        IEnumerable<IEnumerable<IPlayer>> AssignPlayersToGames(List<IPlayer> players)
        {
            if (players.Count - Constants.trainsGameMaxPlayers < Constants.trainsGameMinPlayers && players.Count >= Constants.trainsGameMaxPlayers) // if we do not have enough to allocate the last game
            {
                IEnumerable<IPlayer> secondToLastGame = players.Take(Constants.trainsGameMaxPlayers - (players.Count - Constants.trainsGameMinPlayers));
                IEnumerable<IPlayer> lastGame = players.Skip(players.Count - (Constants.trainsGameMinPlayers));
                return new List<IEnumerable<IPlayer>>() { secondToLastGame, lastGame };
            }
            List<IEnumerable<IPlayer>> games = new List<IEnumerable<IPlayer>>() { players.Take(Constants.trainsGameMaxPlayers).ToList() };
            if (players.Count - Constants.trainsGameMaxPlayers > 0) // if we still have players to allocate
            { // recur
                games.AddRange(AssignPlayersToGames(players.Skip(Constants.trainsGameMaxPlayers).ToList()));
            }
            return games;
        }


        /// <summary>
        /// Plays a round of a Trains.com tournament
        /// </summary>
        /// <param name="map">The map to play on</param>
        /// <param name="games">A list of games' players</param>
        /// <returns>The winners of the round.</returns>
        Tuple<List<IPlayer>, List<IPlayer>> PlayTournamentRound(TrainsMap map, IEnumerable<IEnumerable<IPlayer>> games)
        {
            List<IPlayer> winners = new();
            List<IPlayer> kicked = new();
            foreach (IEnumerable<IPlayer> players in games)
            {
                Referee gameRef = new Referee(null, Constants.setupPlayerRailsCount, Constants.setupPlayerCardsCount, map, players.ToList(), (Deck?)_deck.Clone());
                Dictionary<IPlayer, int?> rankings = gameRef.RunGame();
                int maxScore = rankings.Values.Max() == null ? 0 : (int)rankings.Values.Max();
                winners.AddRange(rankings.Keys.Where(player => rankings[player] == maxScore).ToList());
                kicked.AddRange(rankings.Keys.Where((player) => rankings[player] == null).ToList());
            }
            return Tuple.Create(winners, kicked);
        }

        /// <summary>
        /// Chooses the TrainsMap for the tournament.
        /// </summary>
        /// <param name="playerSubmissions">All player submissions</param>
        /// <returns></returns>
        private TrainsMap ChooseMap(Dictionary<IPlayer, TrainsMap> playerSubmissions)
        {
            int playerCount = playerSubmissions.Count;
            foreach (KeyValuePair<IPlayer, TrainsMap> submission in playerSubmissions)
            {
                if (submission.Value.GetAllFeasibleDestinations().Count >=
                    (playerCount * Constants.destinationsPerPlayer + (Constants.destinationsToChooseFrom - Constants.destinationsPerPlayer)))
                { // we choose the first TrainsMap with at least as many destinations per player as we need
                    return submission.Value;
                }
            }
            throw new MapException();
        }
    }
}
