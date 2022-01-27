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
            Dictionary<IPlayer, TrainsMap> playerSubmissions = new Dictionary<IPlayer, TrainsMap>();
            foreach (IPlayer player in _players)
            {
                TrainsMap submission = null;
                WrapPlayerCall(() => submission = player.Start(true), player); // true means the tournament started
                if (submission != null)
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
                return false;
            }
        }

        /// <summary>
        /// Runs a Trains.com tournament.
        /// </summary>
        /// <param name="playerSubmissions">The player submissions for the map of the tournament</param>
        /// <returns>A Tuple where the first element is the list of winners and the second element is the list of kicked players</returns>
        public Tuple<List<IPlayer>, List<IPlayer>> RunTournament(Dictionary<IPlayer, TrainsMap> playerSubmissions)
        {
            List<IPlayer> advancingPlayers = _players; // all players in _players are to play the first round
            List<IPlayer> winners;
            TrainsMap tournamentMap = ChooseMap(playerSubmissions); // the map
            while (true)
            {
                IEnumerable<IEnumerable<IPlayer>> games = AssignPlayersToGames(advancingPlayers); // reassign players to
                Tuple<List<IPlayer>, List<IPlayer>> result = PlayTournamentRound(tournamentMap, games); // play the round and get the next rounds players
                winners = result.Item1;
                List<IPlayer> kicked = result.Item2;
                /* end conditions:
                   two rounds result in same set of winners
                   there are 1 or 0 winners
                */
                if (winners.Count < 2 || SameSetOfWinners(advancingPlayers, winners))
                {
                    CloseTournament(winners);
                    break;
                }
                else
                {
                    advancingPlayers = winners;
                    _kickedPlayers.AddRange(kicked);
                }
            }
            return Tuple.Create(winners, _kickedPlayers);
        }

        /// <summary>
        /// Closes the Trains.com tournament.
        /// </summary>
        /// <param name="winners">The list of winners</param>
        void CloseTournament(List<IPlayer> winners)
        {
            List<IPlayer> losers = _players.Where(player => !winners.Contains(player)).ToList();
            SendWinLoss(losers, false);
            SendWinLoss(winners, false);
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

        private bool SameSetOfWinners(List<IPlayer> lastRound, List<IPlayer> thisRound)
        {
            foreach (IPlayer player in lastRound) // check last rounds winners to this rounds
            {
                if (!thisRound.Contains(player)) { return false; } // a player did not advance from last round
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
            int chunkSize = Constants.maxPlayers;

            while (players.Count % chunkSize < Constants.minPlayers && players.Count % chunkSize != 0)
            {
                chunkSize--;
            }
            return players.Chunk(chunkSize);
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
                if (submission.Value.GetAllFeasibleDestinations().Count >= playerCount * Constants.destinationsPerPlayer)
                { // we choose the first TrainsMap with at least as many destinations per player as we need
                    return submission.Value;
                }
            }
            return Constants.defaultMap; // default map that works for Constants.maxPlayers
        }
    }
}
