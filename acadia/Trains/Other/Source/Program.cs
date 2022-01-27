using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using Trains.Models.GamePieces;
namespace Trains
{
    class Program
    {
        public static void Main()
        {
            City montgomery = new City(48, 500, "Montgomery");
            City princeton = new City(258, 32, "Princeton");
            City watchung = new City(0, 66, "Watchung Hills");
            City anchorage = new City(43, 104, "Anchorage");
            IList<City> cities_discon = new List<City>() { anchorage, montgomery, princeton, watchung };
            Connection montPrin = new Connection( montgomery, princeton , GamePieceColor.White, Connection.Length.Five);
            Connection prinWatch = new Connection(princeton, watchung , GamePieceColor.Blue, Connection.Length.Five);
            Connection watchMontRed = new Connection(watchung, montgomery , GamePieceColor.Red, Connection.Length.Five);
            Connection watchMontBlue = new Connection(watchung, montgomery , GamePieceColor.Green, Connection.Length.Five);
            IList<Connection> connections_discon = new List<Connection>() { montPrin, prinWatch, watchMontRed, watchMontBlue };
            Map valid_discon = new Map(cities_discon, connections_discon, 500, 500);
            MapEditor mapEditor = new MapEditor(valid_discon, 10);
            mapEditor.Visualize();
        }
    }
}
