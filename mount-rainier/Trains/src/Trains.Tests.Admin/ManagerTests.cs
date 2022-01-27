using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Strategy;
using Trains.Common;
using System.Collections.Immutable;

namespace Trains.Tests.Admin
{
    [TestClass()]
    public class ManagerTests
    {
        TrainsMap map;
        List<IPlayer> players;

        Manager defaultManager()
        {
            var _map = Constants.defaultMap;
            var _players = Utilities.GenerateRandomPlayers(500);
            var _deck = new Deck(Constants.defaultDeck, _map.GetAllFeasibleDestinations().ToHashSet());
            map = _map;
            players = _players;
            return new Manager(_players, _deck);
        }

        [TestMethod()]
        public void AlertPlayersTest()
        {
            Manager manager = defaultManager();
            Assert.AreEqual(500, manager.AlertPlayers().Count);
        }

        [TestMethod()]
        public void RunTournamentTest()
        {
            var manager = defaultManager();
            Tuple<List<IPlayer>, List<IPlayer>> result = manager.RunTournament(manager.AlertPlayers());
            Assert.IsTrue(result.Item1.Count() != 0 && result.Item2.Count() != 0);
        }
    }
}