using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Trains.Common;
using Trains.Remote.Function.Json;

namespace Trains.Remote.Function;

/// <summary>
/// Enum representing the different types of Function names for a remote function transmission.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum RemoteFunctionName
{
    [EnumMember(Value = "start")] Start,
    [EnumMember(Value = "setup")] Setup,
    [EnumMember(Value = "pick")] Pick,
    [EnumMember(Value = "play")] Play,
    [EnumMember(Value = "more")] More,
    [EnumMember(Value = "win")] Win,
    [EnumMember(Value = "end")] End
}

/// <summary>
/// Class representing the Remote function transmission from the server to the client.
/// </summary>
[JsonConverter(typeof(RemoteFunctionJsonConverter))]
public class RemoteFunction
{
    /// <summary>
    /// The type of function this <see cref="RemoteFunction"/> represents.
    /// </summary>
    public RemoteFunctionName FunctionName { get; init; }

    /// <summary>
    /// The arguments to supply to the function call.
    /// </summary>
    public List<object> Arguments { get; init; } = new();

    /// <summary>
    /// Writes this RemoteFunction as a JSON to the given stream.
    /// </summary>
    /// <param name="networkStream">The stream to write to.</param>
    /// <exception cref="Exception">Throws an exception if the stream cannot be written to.</exception>
    public void SendAsMessage(NetworkStream networkStream)
    {
        if (networkStream.CanWrite)
        {
            var json = JsonConvert.SerializeObject(this);
            networkStream.Write(Encoding.ASCII.GetBytes(json));
            return;
        }
        throw new Exception("Cannot write to the network stream.");
    }

    /// <summary>
    /// Receives the entire message as a JSON string from the network stream.
    /// </summary>
    /// <param name="networkStream">The network stream to receive from.</param>
    /// <returns>The string received from the network stream</returns>
    /// <exception cref="Exception">Throws an exception if the stream cannot be read from.</exception>
    public string ReceiveResponse(NetworkStream networkStream)
    {
        networkStream.ReadTimeout = Constants.maxResponseWait;
        if (networkStream.CanRead) // stream reading based on .NET 6 docs located at https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream.dataavailable?view=net-6.0
        {
            byte[] data = new byte[1024]; // arbitrary byte array size
            StringBuilder response = new();
            int bytesRead = 0;
            do
            {
                bytesRead = networkStream.Read(data, 0, data.Length);
                response.Append(Encoding.ASCII.GetString(data, 0, bytesRead));
            } while (networkStream.DataAvailable);

            return response.ToString();
        }
        throw new Exception("Failed to receive valid json from the stream.");
    }

    public override bool Equals(object? obj)
    {
        if (obj is not RemoteFunction other)
        {
            return false;
        }
        return FunctionName.Equals(other.FunctionName) && Arguments.SequenceEqual(other.Arguments);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FunctionName, Arguments);
    }
}