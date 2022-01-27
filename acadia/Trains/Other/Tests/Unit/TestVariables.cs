using System;
using System.Collections.Generic;
using Trains.Models.GameEntities;
using Trains.Models.GamePieces;
using Trains.Models.GameStates;
using Trains.Models.Strategies;
using Trains.Models.TurnTypes;
using Trains.Util;
using Trains.Util.Comparers;
using Trains.Util.Json;

namespace Tests
{
    public static class TestVariables
    {
        // Seattle ---- Boston
        //   |             |
        //   |            NYC
        //   |             |
        //   |___Houston___|
        //   |             |
        //  LA           Miami
        public static City boston = new City(255, 30, "Boston");
        public static City bostonEqual = new City(255, 30, "Boston");
        public static City boston2 = new City(25, 30, "Boston");
        public static City seattle = new City(0, 30, "Seattle");
        public static City seattleEqual = new City(0, 30, "Seattle");
        public static City seattle2 = new City(0, 3, "Seattle");
        public static City houston = new City(125, 255, "Houston");
        public static City nyc = new City(255, 50, "New York City");
        public static City la = new City(0, 255, "Los Angeles");
        public static City miami = new City(255, 255, "Miami");
        public static IList<City> cities = new List<City>() { boston, seattle, houston, nyc, la, miami };
        public static IList<City> cities_ordered = new List<City>() { boston, houston, la, miami, nyc, seattle };
        public static Connection bosSea = new Connection(boston, seattle, GamePieceColor.Blue, Connection.Length.Five);
        public static Connection seaBos = new Connection(seattle, boston, GamePieceColor.Blue, Connection.Length.Five);
        public static Connection bosSeaEqual = new Connection(bostonEqual, seattleEqual, GamePieceColor.Blue, Connection.Length.Five);
        public static Connection bosNyc = new Connection(boston, nyc, GamePieceColor.Red, Connection.Length.Three);
        public static Connection nycMia = new Connection(nyc, miami, GamePieceColor.White, Connection.Length.Four);
        public static Connection nycHou = new Connection(nyc, houston, GamePieceColor.White, Connection.Length.Five);
        public static Connection miaHou = new Connection(miami, houston, GamePieceColor.Green, Connection.Length.Four);
        public static Connection houLa = new Connection(houston, la, GamePieceColor.Red, Connection.Length.Four);
        public static Connection houSea = new Connection(houston, seattle, GamePieceColor.White, Connection.Length.Four);
        public static Connection houSeaEqual = new Connection(houston, seattle, GamePieceColor.White, Connection.Length.Three);
        public static Connection laSea = new Connection(la, seattle, GamePieceColor.Green, Connection.Length.Five);
        public static Connection laSea_compTie = new Connection(la, seattle, GamePieceColor.Red, Connection.Length.Five);
        public static Connection laSea2 = new Connection(la, seattle, GamePieceColor.Red, Connection.Length.Five);
        public static ICollection<Connection> connections = new HashSet<Connection>() { bosSea, bosNyc, nycMia, nycHou, miaHou, houLa, houSea, laSea };
        public static ICollection<Connection> connections2 = new HashSet<Connection>() { nycMia, seaBos, bosNyc, nycHou, miaHou, houLa, houSea, laSea };
        public static ICollection<Connection> connections3 = new HashSet<Connection>() { laSea, bosNyc, houLa, laSea_compTie, houSea, bosSea };
        public static ICollection<Connection> connections3_ordered = new HashSet<Connection>() { bosNyc, bosSea, houLa, houSea, laSea, laSea_compTie };
        public static Map valid = new Map(cities, connections, 300, 300);
        public static Map valid2 = new Map(cities, connections2, 300, 300);
        public static Destination dest_valid_houston_seattle = new Destination(valid, houston, seattle);
        public static Destination dest_valid_houston_seattle2 = new Destination(valid2, houston, seattle);
        public static Destination dest_bosSea = new Destination(valid, boston, seattle);
        public static Destination dest_houSea = new Destination(valid, houston, seattle);
        public static Destination dest_laSea = new Destination(valid, la, seattle);
        public static Destination dest_bosNyc = new Destination(valid, boston, nyc);
        public static ICollection<Destination> valid_dests = new HashSet<Destination>() { dest_bosSea, dest_houSea, dest_laSea, dest_bosNyc };
        public static ICollection<Destination> valid_dests_ordered = new HashSet<Destination>() { dest_bosNyc, dest_bosSea, dest_houSea, dest_laSea };

        // Montgomery - Princeton
        //         \    /                          Anchorage
        //     Watchung Hills
        public static City montgomery = new City(48, 500, "Montgomery");
        public static City princeton = new City(258, 32, "Princeton");
        public static City watchung = new City(0, 66, "Watchung Hills");
        public static City anchorage = new City(43, 104, "Anchorage");
        public static ICollection<City> cities_discon = new HashSet<City>() { anchorage, montgomery, princeton, watchung };
        public static Connection montPrin = new Connection(montgomery, princeton, GamePieceColor.Blue, Connection.Length.Five);
        public static Connection prinWatch = new Connection(princeton, watchung, GamePieceColor.Blue, Connection.Length.Five);
        public static Connection watchMontRed = new Connection(watchung, montgomery, GamePieceColor.Red, Connection.Length.Five);
        public static Connection watchMontBlue = new Connection(watchung, montgomery, GamePieceColor.Blue, Connection.Length.Five);
        public static ICollection<Connection> connections_discon = new HashSet<Connection>() { montPrin, prinWatch, watchMontRed, watchMontBlue };
        public static ICollection<Connection> connections_discon_ownNone = new HashSet<Connection>() { };
        public static Map valid_discon = new Map(cities_discon, connections_discon, 500, 500);
        public static Destination dest_valid_discon_montgomery_watchung = new Destination(valid_discon, montgomery, watchung);
        public static Destination dest_valid_discon_pton_watchung = new Destination(valid_discon, princeton, watchung);
        public static Destination dest_valid_discon_montgomery_pton = new Destination(valid_discon, montgomery, princeton);
        public static ICollection<Destination> valid_discon_dests = new HashSet<Destination>() { dest_valid_discon_montgomery_pton, dest_valid_discon_pton_watchung, dest_valid_discon_montgomery_watchung };

        public static Dictionary<string, int> jsonSegmentsMontyPton = new Dictionary<string, int>()
        {
            { "blue", 5 },
        };
        public static Dictionary<string, int> jsonSegmentsMontyWatchung = new Dictionary<string, int>()
        {
            { "red", 5 },
            { "blue", 5 }
        };
        public static Dictionary<string, int> jsonSegmentsPtonWatchung = new Dictionary<string, int>()
        {
            { "blue", 5 },
        };
        public static Dictionary<string, Dictionary<string, int>> jsonConnectionsMonty = new Dictionary<string, Dictionary<string, int>>()
        {
            { "Princeton", jsonSegmentsMontyPton },
            { "Watchung Hills", jsonSegmentsMontyWatchung }
        };
        public static Dictionary<string, Dictionary<string, int>> jsonConnectionsPton = new Dictionary<string, Dictionary<string, int>>()
        {
            { "Watchung Hills", jsonSegmentsPtonWatchung }
        };
        public static Dictionary<string, Dictionary<string, Dictionary<string, int>>> jsonConnections = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>()
        {
            { "Montgomery", jsonConnectionsMonty },
            { "Princeton", jsonConnectionsPton }
        };
        public static JsonMap validJson = new JsonMap(500, 500, cities_discon, jsonConnections);

        public static Map emptyMap = new Map(new List<City>(), new List<Connection>(), 500, 500);

        public static ColoredCard redCard = new ColoredCard(GamePieceColor.Red);
        public static ColoredCard greenCard = new ColoredCard(GamePieceColor.Green);
        public static ColoredCard blueCard = new ColoredCard(GamePieceColor.Blue);
        public static ColoredCard whiteCard = new ColoredCard(GamePieceColor.White);
        public static PlayerGameState pgs_allSmall0 = new PlayerGameState(valid_discon, 35,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) },
            new HashSet<Destination> { dest_valid_discon_montgomery_watchung, dest_valid_discon_pton_watchung }, valid_discon.Connections, new HashSet<Connection>());
        public static PlayerGameState pgs_allSmall0Dup = new PlayerGameState(valid_discon, 35,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) },
            new HashSet<Destination> { dest_valid_discon_montgomery_watchung, dest_valid_discon_pton_watchung }, valid_discon.Connections, new HashSet<Connection>());
        public static PlayerGameState pgs_allSmall0AllAvailable = new PlayerGameState(valid_discon, 35,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) },
             new HashSet<Destination> { dest_valid_discon_montgomery_watchung, dest_valid_discon_pton_watchung }, new HashSet<Connection>(), valid_discon.Connections);
        public static PlayerGameState pgs_allSmall1 = new PlayerGameState(valid_discon, 45,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red) },
            new HashSet<Destination> { dest_valid_discon_montgomery_watchung, dest_valid_discon_pton_watchung }, valid_discon.Connections, new HashSet<Connection>());
        public static PlayerGameState pgs_allSmall1AllAvailable = new PlayerGameState(valid_discon, 45,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red) },
             new HashSet<Destination> { dest_valid_discon_montgomery_watchung, dest_valid_discon_pton_watchung }, new HashSet<Connection>(), valid_discon.Connections);
        public static PlayerGameState pgs_allSmall2 = new PlayerGameState(valid, 35,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) },
            new HashSet<Destination> { dest_laSea, dest_valid_houston_seattle }, valid.Connections, new HashSet<Connection>());
        public static PlayerGameState pgs_allSmall2AllAvailable = new PlayerGameState(valid, 35,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) },
            new HashSet<Destination> { dest_laSea, dest_valid_houston_seattle }, new HashSet<Connection>(), valid.Connections);
        public static PlayerGameState pgs_allOwned = new PlayerGameState(valid_discon, 35,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) },
            new HashSet<Destination> { dest_valid_discon_montgomery_watchung, dest_valid_discon_pton_watchung }, valid_discon.Connections, new HashSet<Connection>());
        public static PlayerGameState pgs_allAvail = new PlayerGameState(valid_discon, 45,
            new List<ColoredCard>() { new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue), new ColoredCard(GamePieceColor.Blue) },
            new HashSet<Destination> { dest_valid_discon_montgomery_watchung, dest_valid_discon_pton_watchung }, new HashSet<Connection>(), valid_discon.Connections);

        public static IList<ColoredCard> deck_3R2G = new List<ColoredCard> { new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Red), new ColoredCard(GamePieceColor.Green), new ColoredCard(GamePieceColor.Green) };

        public static RefereeGameState rgs = new RefereeGameState(0, valid, new List<PlayerGameState>() { pgs_allSmall0, pgs_allSmall1 });
        public static RefereeGameState rgs_copy = new RefereeGameState(0, valid, new List<PlayerGameState>() { pgs_allSmall0, pgs_allSmall1 });
        public static RefereeGameState rgs2 = new RefereeGameState(0, valid, new List<PlayerGameState>() { pgs_allSmall2AllAvailable });

        public static IStrategy buyNowStrategy = new BuyNowStrategy();
        public static IStrategy hold10Strategy = new Hold10Strategy();
        public static IStrategy buyConnectionLength3 = new SetLengthStrategy(3);
        public static IStrategy buyConnectionLength4 = new SetLengthStrategy(4);
        public static IStrategy buyConnectionLength5 = new SetLengthStrategy(5);
        public static IStrategy onlyDrawCards = new OnlyDrawsCardsStrategy();
        public static ITurn drawCardsTurn = new DrawCardsTurn();
        public static ITurn acquireConnTurn = new AcquireConnectionTurn(montPrin);
        public static ITurn acquireConn3Turn = new AcquireConnectionTurn(bosNyc);
        public static ITurn acquireConn4Turn = new AcquireConnectionTurn(nycMia);
        public static ITurn acquireConn5Turn = new AcquireConnectionTurn(bosSea);

        public static string xlegal_1 = "{\"width\":300,\"height\":300,\"cities\":[[\"Boston\",[255,30]],[\"Houston\",[125,255]],[\"Seattle\",[0,30]],[\"NewYorkCity\"," +
            "[255,50]],[\"LosAngeles\",[0,255]],[\"Miami\",[255,255]]],\"connections\":{\"Boston\":{\"Seattle\":{\"blue\":5,\"green\":4},\"NewYorkCity\":{\"red\":3}},\"Hous" +
            "ton\":{\"NewYorkCity\":{\"white\":5},\"Miami\":{\"green\":4},\"LosAngeles\":{\"red\":4},\"Seattle\":{\"white\":4}},\"Miami\":{\"NewYorkCity\":{\"white\":4}},\"Los" +
            "Angeles\":{\"Seattle\":{\"green\":5}}}}{\"this\":{\"destination1\":[\"Seattle\",\"Miami\"],\"destination2\":[\"LosAngeles\",\"Miami\"],\"rails\":12,\"cards\":{\"re" +
            "d\":4,\"green\":4,\"white\":6},\"acquired\":[[\"Houston\",\"LosAngeles\",\"red\",4],[\"Miami\",\"NewYorkCity\",\"white\",4],[\"LosAngeles\",\"Seattle\",\"green\",5]" +
            "]},\"acquired\":[[[\"Houston\",\"NewYorkCity\",\"white\",5],[\"Miami\",\"NewYorkCity\",\"white\",4]],[[\"Houston\",\"Seattle\",\"white\",4]],[[\"Houston\",\"Miami\",\"g" +
            "reen\",4],[\"Boston\",\"Seattle\",\"blue\",5]]]}[\"Boston\",\"Seattle\",\"green\",4]";
        public static string xlegal_1_map = "{\"width\":300,\"height\":300,\"cities\":[[\"Boston\",[255,30]],[\"Houston\",[125,255]],[\"Seattle\",[0,30]],[\"NewYorkCity\"," +
            "[255,50]],[\"LosAngeles\",[0,255]],[\"Miami\",[255,255]]],\"connections\":{\"Boston\":{\"Seattle\":{\"blue\":5,\"green\":4},\"NewYorkCity\":{\"red\":3}},\"Hous" +
            "ton\":{\"NewYorkCity\":{\"white\":5},\"Miami\":{\"green\":4},\"LosAngeles\":{\"red\":4},\"Seattle\":{\"white\":4}},\"Miami\":{\"NewYorkCity\":{\"white\":4}},\"Los" +
            "Angeles\":{\"Seattle\":{\"green\":5}}}}";
        public static string xlegal_1_other = "{\"this\":{\"destination1\":[\"Seattle\",\"Miami\"],\"destination2\":[\"LosAngeles\",\"Miami\"],\"rails\":12,\"cards\":{\"re" +
            "d\":4,\"green\":4,\"white\":6},\"acquired\":[[\"Houston\",\"LosAngeles\",\"red\",4],[\"Miami\",\"NewYorkCity\",\"white\",4],[\"LosAngeles\",\"Seattle\",\"green\",5]" +
            "]},\"acquired\":[[[\"Houston\",\"NewYorkCity\",\"white\",5],[\"Miami\",\"NewYorkCity\",\"white\",4]],[[\"Houston\",\"Seattle\",\"white\",4]],[[\"Houston\",\"Miami\",\"g" +
            "reen\",4],[\"Boston\",\"Seattle\",\"blue\",5]]]}[\"Boston\",\"Seattle\",\"green\",4]";
        public static string xlegal_3 = "{\"width\":500,\"height\":500,\"cities\":[[\"Montgomery\",[48,500]],[\"Princeton\",[258,32]],[\"WatchungHills\",[0,66]],[\"Ancho" +
            "rage\",[43,104]]],\"connections\":{\"Montgomery\":{\"Princeton\":{\"blue\":5},\"WatchungHills\":{\"red\":5,\"blue\":5}},\"Princeton\":{\"WatchungHills\":{\"white\":5}" +
            "}}}{\"this\":{\"destination1\":[\"Montgomery\",\"WatchungHills\"],\"destination2\":[\"Montgomery\",\"Prince" +
            "ton\"],\"rails\":0,\"cards\":{\"red\":4,\"green\":4,\"blue\":6,\"white\":0},\"acquired\":[[\"Montgomery\",\"WatchungHills\",\"red\",5],]},\"acquired\":[[[\"Prince" +
            "ton\",\"WatchungHills\",\"white\",5]],[[\"Montgomery\",\"WatchungHills\",\"blue\",5]]]}[\"Montgomery\",\"Princeton\",\"blue\",5]";
        public static string xlegal_3_map = "{\"width\":500,\"height\":500,\"cities\":[[\"Montgomery\",[48,500]],[\"Princeton\",[258,32]],[\"WatchungHills\",[0,66]],[\"Ancho" +
            "rage\",[43,104]]],\"connections\":{\"Montgomery\":{\"Princeton\":{\"blue\":5},\"WatchungHills\":{\"red\":5,\"blue\":5}},\"Princeton\":{\"WatchungHills\":{\"white\":5}" +
            "}}}";
        public static string xlegal_3_other = "{\"this\":{\"destination1\":[\"Montgomery\",\"WatchungHills\"],\"destination2\":[\"Montgomery\",\"Prince" +
            "ton\"],\"rails\":0,\"cards\":{\"red\":4,\"green\":4,\"blue\":6,\"white\":0},\"acquired\":[[\"Montgomery\",\"WatchungHills\",\"red\",5],]},\"acquired\":[[[\"Prince" +
            "ton\",\"WatchungHills\",\"white\",5]],[[\"Montgomery\",\"WatchungHills\",\"blue\",5]]]}[\"Montgomery\",\"Princeton\",\"blue\",5]";

        public static IComparer<Destination> destComp = new LexicoDestinationComparer();
        public static IComparer<Connection> connComp = new LexicoConnectionComparer();
        public static IComparer<City> cityComp = new LexicoCityComparer();

        public static JsonAcquired acquired = new JsonAcquired(bosSea.City1.CityName, bosSea.City2.CityName, bosSea.Color.ToString().ToLower(), (int)bosSea.NumSegments);
        public static JsonAcquired acquired2 = new JsonAcquired(houLa.City1.CityName, houLa.City2.CityName, houLa.Color.ToString().ToLower(), (int)houLa.NumSegments);

        public static JsonPlayerState jsonPlayerState = new JsonPlayerState(pgs_allSmall0);
        public static JsonPlayerState jsonPlayerState2 = new JsonPlayerState(pgs_allOwned);

        public static JsonThisPlayer jsonThisPlayer = new JsonThisPlayer(pgs_allSmall0);
        public static JsonThisPlayer jsonThisPlayer1 = new JsonThisPlayer(pgs_allSmall1);

        public static IPlayer player = new Player(@"../../../../../Strategy/bin/Debug/netcoreapp3.1/Strategy.dll");
        public static IPlayer playerBuyNow = new Player(buyNowStrategy);
        public static IPlayer playerHold10 = new Player(hold10Strategy);
        public static IPlayer playerOnlyDrawCards1 = new Player(onlyDrawCards);
        public static IPlayer playerOnlyDrawCards2 = new Player(onlyDrawCards);
        public static IPlayer playerOnlyDrawCards3 = new Player(onlyDrawCards);
        public static IPlayer playerBuy3 = new Player(buyConnectionLength3);
        public static IPlayer playerBuy4 = new Player(buyConnectionLength4);
        public static IPlayer playerBuy5 = new Player(buyConnectionLength5);
        public static IList<IPlayer> buyPlayers = new List<IPlayer>() { playerBuy3, playerBuy4, playerBuy5 };
        public static IList<IPlayer> drawCardPlayers = new List<IPlayer>() { playerOnlyDrawCards1, playerOnlyDrawCards2, playerOnlyDrawCards3 };
        public static IList<IPlayer> holdNowBuy10Players = new List<IPlayer>() { playerBuyNow, playerHold10 };
        public static IDictionary<IPlayer, int> drawCardsPlayersRankings = new Dictionary<IPlayer, int>()
        {
            {playerOnlyDrawCards1, 0}, {playerOnlyDrawCards2, 0}, {playerOnlyDrawCards3, 0}
        };
        public static IDictionary<IPlayer, int> hold10BuyNowPlayersRankings = new Dictionary<IPlayer, int>()
        {

        };
        public static IDictionary<IPlayer, int> buyPlayersRankings = new Dictionary<IPlayer, int>()
        {
            {playerBuy5,  5 },
            {playerBuy4, -16 },
            {playerBuy3, -17 }
        };

        //Assert.AreEqual();

        public static IList<ColoredCard> MakeDeck(int size)
        {
            IList<ColoredCard> rv = new List<ColoredCard>();
            Random r = new Random();
            for (int ii = 1; ii <= size; ++ii)
            {
                int val = r.Next(1, 5);
                rv.Add(GetRandomCard(val));
            }
            return rv;
        }

        private static ColoredCard GetRandomCard(int val)
        {
            switch (val)
            {
                case 1:
                    return new ColoredCard(GamePieceColor.Red);
                    break;
                case 2:
                    return new ColoredCard(GamePieceColor.Green);
                    break;
                case 3:
                    return new ColoredCard(GamePieceColor.Blue);
                    break;
                default: // case 4
                    return new ColoredCard(GamePieceColor.White);
            }
        }

        public static IList<ColoredCard> deck250 = MakeDeck(250);
        public static IList<ColoredCard> deck250v2 = MakeDeck(250);
        public static IList<ColoredCard> deck251 = MakeDeck(251);

        public static Referee referee1 = new Referee(deck250, 1);
        public static Referee referee2 = new Referee(deck250, 1);
        public static Referee referee3 = new Referee(deck250);

        public static Destination dest_BosHou = new Destination(valid, boston, houston);
        public static Destination dest_BosLa = new Destination(valid, boston, la);
        public static Destination dest_BosMia = new Destination(valid, boston, miami);
        public static Destination dest_BosNyc = new Destination(valid, boston, nyc);
        public static Destination dest_BosSea = new Destination(valid, boston, seattle);
        public static Destination dest_HouLa = new Destination(valid, houston, la);
        public static Destination dest_HouMia = new Destination(valid, houston, miami);
        public static Destination dest_HouNyc = new Destination(valid, houston, nyc);
        public static Destination dest_HouSea = new Destination(valid, houston, seattle);
        public static Destination dest_LaMia = new Destination(valid, la, miami);
        public static Destination dest_LaNyc = new Destination(valid, la, nyc);
        public static Destination dest_LaSea = new Destination(valid, la, seattle);
        public static Destination dest_MiaNyc = new Destination(valid, miami, nyc);
        public static Destination dest_MiaSea = new Destination(valid, miami, seattle);
        public static Destination dest_NycSea = new Destination(valid, nyc, seattle);
        public static ICollection<Destination> valid_allDests = new HashSet<Destination>
        {
            dest_BosHou, dest_BosLa, dest_BosMia, dest_BosNyc, dest_BosSea, dest_HouLa, dest_HouMia, dest_HouNyc, dest_HouSea,
            dest_LaMia, dest_LaNyc, dest_LaSea, dest_MiaNyc, dest_MiaSea, dest_NycSea
        };
    }
}
