using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Trains.Admin.Exceptions;
using Trains.Common;
using Trains.Common.GameState;
using Trains.Common.Map;
using Trains.Player;
using Trains.Remote.Function;
using System;
using Trains.Common.Map.Json;

namespace Trains.Remote
{
    /// <summary>
    /// This class is used for testing the Remote proxy implementation of Trains.com
    /// </summary>
    public class Client
    {
        private IPlayer _player;
        private TcpClient _server;
        private ServerProxy _proxy;

        /// <summary>
        /// Constructor for a Client.
        /// </summary>
        /// <param name="name">The name for the player</param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public Client(IPlayer player)
        {
            _server = new TcpClient();
            _player = player;
            _proxy = new ServerProxy();
        }

        /// <summary>
        /// Connects to the server at the given address and port.
        /// </summary>
        /// <param name="address">The IPAddress of the server.</param>
        /// <param name="port">The port to connect to.</param>
        private void ConnectToServer(IPAddress? address, int port)
        {
            if (address == null) { _server.Connect(IPAddress.Loopback, port); }
            if (address != null) { _server.Connect(address, port); }
            _proxy.SendMsg(_server.GetStream(), $"\"{_player.Name}\"");
        }

        /// <summary>
        /// Closes the connection to the server.
        /// </summary>
        public void CloseConnection()
        {
            _server.Close();
        }

        /// <summary>
        /// Connects to the Trains server and plays a tournament.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public void ConnectToTrains(IPAddress? address, int port)
        {
            this.ConnectToServer(address, port);
            NetworkStream networkStream = _server.GetStream();
            bool tournamentOver = false;
            while (!tournamentOver)
            {
                try
                {
                    RemoteFunction function = _proxy.ReceiveRemoteFunction(networkStream);
                    _proxy.SendResponse(networkStream, function.FunctionName, FunctionResult(function));
                    tournamentOver = function.FunctionName == RemoteFunctionName.End; // did we just reply to the end of the tournament
                }
                catch (ClientReadException readException)
                {
                    // not time to read, DO NOTHING TO HANDLE EXCEPTION
                }
            }
        }

        /// <summary>
        /// Delegates to the player of this client.
        /// </summary>
        /// <param name="function">The function the server sent.</param>
        /// <returns>The result of whatever server call.</returns>
        /// <exception cref="ArgumentException">Thrown if the remote function had an invalid name.</exception>
        private object? FunctionResult(RemoteFunction function)
        {
            switch (function.FunctionName)
            {
                case RemoteFunctionName.Start:
                    return _player.Start((bool)function.Arguments[0]);
                case RemoteFunctionName.Setup:
                    TrainsMap gameMap = (TrainsMap)function.Arguments[0];
                    _proxy.Map = gameMap;
                    _player.Setup(gameMap, (uint)function.Arguments[1], Cards.FromListOfColor(function.Arguments[2] as List<Color>));
                    return null;
                case RemoteFunctionName.Pick:
                    return _player.ChooseDestinations(function.Arguments[0] as IImmutableSet<Destination>);
                case RemoteFunctionName.Play:
                    return _player.PlayTurn((PlayerGameState)function.Arguments[0]);
                case RemoteFunctionName.More:
                    _player.MoreCards(Cards.FromListOfColor(function.Arguments[0] as List<Color>));
                    return null;
                case RemoteFunctionName.Win:
                    _player.IsWinner((bool)function.Arguments[0]);
                    return null;
                case RemoteFunctionName.End:
                    _player.End((bool)function.Arguments[0]);
                    return null;
            }
            throw new ArgumentException("The remote function was an invalid RemoteFunctionName.");
        }
    }
}
