using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trains.Admin;
using Trains.Common;
using Trains.Player;

namespace Trains.Remote
{
    /// <summary>
    /// Class representing the Server for Trains.com
    /// </summary>
    public class Server
    {
        private Deck _tournamentDeck;
        private int _portNumber;

        /// <summary>
        /// Constructor for a Server that takes in the Deck to be used when playing a tournament of Trains
        /// </summary>
        /// <param name="deck">The deck to be used throughout the tournament</param>
        public Server(int port, Deck deck)
        {
            _tournamentDeck = deck;
            _portNumber = port;
        }

        /// <summary>
        /// Runs the server, connects players, and plays a tournament.
        /// </summary>
        /// <returns>The results of the tournament where the first item is the list of winners and the second item is the list of kicked players.</returns>
        public Tuple<List<IPlayer>, List<IPlayer>> RunServer()
        {
            List<RemotePlayer> signedUpPlayers = SignUpPlayers();

            if (signedUpPlayers.Count < Constants.trainsGameMinPlayers)
            {
                CloseClients(signedUpPlayers);
                return Tuple.Create(new List<IPlayer>(), new List<IPlayer>());
            }
            var results = RunTournament(signedUpPlayers);
            CloseClients(signedUpPlayers);
            return results;
        }

        /// <summary>
        /// Closes the TcpClients
        /// </summary>
        /// <param name="signedUpPlayers">The remote proxies to close.</param>
        private void CloseClients(List<RemotePlayer> signedUpPlayers)
        {
            foreach (var player in signedUpPlayers)
            {
                player._client.Close(); // close each client from the server
            }
        }

        /// <summary>
        /// Runs the tournament with the given players.
        /// </summary>
        /// <param name="signedUpPlayers">The players who signed up to the server.</param>
        /// <returns>The results of running a tournament with the given players.</returns>
        private Tuple<List<IPlayer>, List<IPlayer>> RunTournament(List<RemotePlayer> signedUpPlayers)
        {
            Manager manager = new(new List<IPlayer>(signedUpPlayers), _tournamentDeck);
            return manager.RunTournament();
        }

        /// <summary>
        /// Signs up players to the server.
        /// </summary>
        /// <returns>The signed up players.</returns>
        private List<RemotePlayer> SignUpPlayers()
        {
            List<RemotePlayer> signedUpPlayers = new();
            TcpListener server = new TcpListener(IPAddress.Any, _portNumber); // use Any to have the local provider delegate the IP address accordingly
                                                                              // first waiting period, run if at least 5 players sign up
            server.Start(); // START LISTENING
            // enter iff <5 players signed up, second waiting period, run if at least 2 clients connect
            signedUpPlayers.AddRange(WaitForPlayerSignUps(server).Result);

            if (signedUpPlayers.Count < Constants.serverPreferredMinPlayers)
            { // second waiting period
                signedUpPlayers.AddRange(WaitForPlayerSignUps(server).Result);
            }

            return signedUpPlayers;
        }

        /// <summary>
        /// Waits for players to sign up to the server.
        /// </summary>
        /// <param name="server">The server players are connecting to.</param>
        /// <returns>The players that signed up for the tournament.</returns>
        private async Task<List<RemotePlayer>> WaitForPlayerSignUps(TcpListener server)
        {
            List<RemotePlayer> signedUpPlayers = new();
            List<Task> signUps = new();
            DateTime endTime = DateTime.Now.AddMilliseconds(Constants.serverSignUpTimeLength);
            while (!(DateTime.Now > endTime))
            {
                if (server.Pending())
                {
                    try
                    {
                        var signUpTask = Task.Run(() => SignUpPlayer(server.AcceptTcpClient(), signedUpPlayers)); // spawns a separate thread that signs up a player
                        signUps.Add(signUpTask);
                        await signUpTask;
                    }
                    catch (PlayerNameException) { } // do nothing to the list
                }
            }
            Task.WaitAll(signUps.ToArray()); // waits for all signups to finish
            return signedUpPlayers;
        }

        /// <summary>
        /// Signs up a single player to the server.
        /// </summary>
        /// <param name="client">The client signing up to play.</param>
        /// <returns>The remote proxy representing the client's connection in.</returns>
        /// <exception cref="PlayerNameException">Thrown if any player name errors are encountered.</exception>
        private void SignUpPlayer(TcpClient client, List<RemotePlayer> signedUpPlayers)
        {
            string clientName = ReceivePlayerName(client); // name must be 1 <= name.Length <= 50
            if (clientName.Length > 0)
            {
                lock (signedUpPlayers)
                {
                    signedUpPlayers.Add(new RemotePlayer(clientName, client));
                }
                return;
            }
            throw new PlayerNameException("PlayerName failed");
        }

        /// <summary>
        /// Receives the PlayerName from the client.
        /// </summary>
        /// <param name="client">The client that is submitting the name.</param>
        /// <returns>The PlayerName from the client. If the PlayerName is invalid or one is not received, the return is an empty string.</returns>
        private string ReceivePlayerName(TcpClient client)
        {
            DateTime endTime = DateTime.Now.AddMilliseconds(Constants.serverPlayerNameTimeout);
            NetworkStream stream = client.GetStream();
            string submittedName = "";
            try
            {
                while (!(DateTime.Now > endTime))
                {
                    if (stream.CanRead) // stream reading based on .NET 6 docs located at https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream.dataavailable?view=net-6.0
                    {
                        byte[] data = new byte[1024]; // arbitrary byte array size
                        StringBuilder playerNameJson = new();
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = stream.Read(data, 0, data.Length);
                            playerNameJson.Append(Encoding.ASCII.GetString(data, 0, bytesRead));
                            submittedName = JsonConvert.DeserializeObject<string>(playerNameJson.ToString())!; // name submitted is a JSON string
                        } while (stream.DataAvailable);
                        if (ValidLengthPlayerName(playerNameJson.ToString()))
                        {
                            return JsonConvert.DeserializeObject<string>(playerNameJson.ToString())!;
                        }
                    }
                }
            }
            catch
            {
                return submittedName;
            }
            return submittedName;
        }

        /// <summary>
        /// Ensures the player submitted a name of valid length
        /// </summary>
        /// <param name="playerName">The name to validate</param>
        /// <returns>Is the name of valid length</returns>
        private static bool ValidLengthPlayerName(string playerName)
        {
            return playerName.Length > Constants.playerNameMinLength && playerName.Length <= Constants.playerNameMaxLength;
        }
    }
}

