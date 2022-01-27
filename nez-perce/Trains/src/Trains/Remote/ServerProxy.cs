using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Trains.Common.GameState.Json;
using Trains.Common;
using Trains.Common.Map;
using Trains.Player;
using Trains.Remote.Function;
using System.Net;
using Trains.Admin.Exceptions;
using System.Threading;
using System.Collections.Immutable;
using Trains.Remote.Function.Json;
using Trains.Player.Json;

namespace Trains.Remote
{
    public class ServerProxy
    {
        public TrainsMap Map { get; set; }

        public ServerProxy()
        {
        }

        /// <summary>
        /// Receives the remote function from the server.
        /// </summary>
        /// <returns>The Remote Function sent from the server as a JSON.</returns>
        /// <exception cref="JsonException">Thrown if reading from the network stream fails.</exception>
        public RemoteFunction ReceiveRemoteFunction(NetworkStream networkStream)
        {
            while (true) // loop indefinitely because we do not know how fast the server will respond
            {            // example is the server saying start tournament -> ref saying start game could be instant or take 10s
                if (networkStream.CanRead) // stream reading based on .NET 6 docs located at https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream.dataavailable?view=net-6.0
                {
                    byte[] data = new byte[1024]; // arbitrary byte array size
                    StringBuilder response = new();
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = networkStream.ReadAsync(data, 0, data.Length, CancellationToken.None).Result;
                        response.Append(Encoding.ASCII.GetString(data, 0, bytesRead));
                    } while (networkStream.DataAvailable);
                    RemoteFunctionJsonConverter.ContextMap = Map;
                    PlayerResponseJsonConverter.ContextMap = Map;
                    return JsonConvert.DeserializeObject<RemoteFunction>(response.ToString());
                }
            }
            throw new ClientReadException("No data to read");
        }

        /// <summary>
        /// Sends the appropriate response to the server.
        /// </summary>
        /// <param name="functionName">The function name as an enum.</param>
        /// <param name="result">The objects to encode as JSON.</param>
        public void SendResponse(NetworkStream networkStream, RemoteFunctionName functionName, object result)
        {
            string json = "\"void\"";
            switch (functionName)
            {
                case RemoteFunctionName.Start:
                    json = JsonConvert.SerializeObject((TrainsMap)result);
                    break;
                case RemoteFunctionName.Pick:
                    json = JsonConvert.SerializeObject((ImmutableHashSet<Destination>)result);
                    break;
                case RemoteFunctionName.Play:
                    PlayerResponse playerResponse = (PlayerResponse)result;
                    if (playerResponse.ResponseType == ResponseType.DrawCards)
                    {
                        json = "\"more cards\"";
                    }
                    if (playerResponse.ResponseType == ResponseType.ClaimConnection)
                    {
                        AcquiredJson jsonAcquired = AcquiredJson.ConvertFromConnection(((PlayerResponse)result).RequestedConnectionClaim!);
                        json = JsonConvert.SerializeObject(jsonAcquired);
                    }
                    break;
            }
            SendMsg(networkStream, json);
        }

        /// <summary>
        /// Sends the given json string to the server.
        /// </summary>
        /// <param name="json">The string to send to the server</param>
        public void SendMsg(NetworkStream networkStream, string json)
        {
            if (networkStream.CanWrite)
            {
                byte[] buff = Encoding.ASCII.GetBytes(json);
                networkStream.Write(buff);
            }
        }
    }
}
