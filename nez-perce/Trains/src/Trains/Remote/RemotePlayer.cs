using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Trains.Common.GameState;
using Trains.Common.GameState.Json;
using Trains.Common.Map;
using Trains.Player;
using Trains.Player.Json;
using Trains.Remote.Function;
using Trains.Remote.Function.Json;

namespace Trains.Remote;

/// <summary>
/// Represents a remote player that can play a game of Trains over the network.
/// </summary>.
public partial class RemotePlayer
{
    /// <summary>
    /// The map the player is currently using. Used to help deserialize player game state. Set on first 
    /// </summary>
    private TrainsMap? _contextMap = null;

    /// <summary>
    /// The <see cref="NetworkStream"/> that the server will use to communicate with the player.
    /// </summary>
    private NetworkStream _clientStream;
    public TcpClient _client;
    /// <summary>
    /// Construct a Remote player with a name and the <see cref="NetworkStream"/> that will communicate with them.
    /// </summary>
    /// <param name="name">The name of the player.</param>
    /// <param name="clientStream">The network stream that the server will use to communicate with the player</param>
    public RemotePlayer(string name, TcpClient client)
    {
        Name = name;
        _client = client;
        _clientStream = _client.GetStream();
    }
}

// This partial section implements IPlayer.
public partial class RemotePlayer : IPlayer
{
    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public void Setup(TrainsMap map, uint rails, Cards cards)
    {
        // Update the map to help with future PlayerGameState deserializations.
        _contextMap = map;

        var argCards = cards.ToListOfColor();

        // Construct the function
        var setupFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Setup,
            Arguments = new List<object> { map, rails, argCards }
        };

        // Send the function command
        RemoteFunctionJsonConverter.ContextMap = _contextMap;
        setupFunction.SendAsMessage(_clientStream);

        // Receive response ("void")
        setupFunction.ReceiveResponse(_clientStream);
    }

    /// <inheritdoc/>
    public void MoreCards(Cards cards)
    {
        // Convert internal representation to serializable version
        var argCards = cards.ToListOfColor();

        // Construct the function
        var moreFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.More,
            Arguments = new List<object> { argCards }
        };

        // Send the function command
        moreFunction.SendAsMessage(_clientStream);

        // Receive response ("void")
        moreFunction.ReceiveResponse(_clientStream);
    }

    /// <inheritdoc/>
    public IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations)
    {
        // Construct the function
        var pickFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Pick,
            Arguments = new List<object> { destinations }
        };

        // Send the function command
        pickFunction.SendAsMessage(_clientStream);

        // Receive response
        var response = JsonConvert.DeserializeObject<HashSet<Destination>>(pickFunction.ReceiveResponse(_clientStream));
        return response!.ToImmutableHashSet();
    }

    /// <inheritdoc/>
    public PlayerResponse PlayTurn(PlayerGameState gameState)
    {
        // Construct the function
        var playFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Play,
            Arguments = new List<object> { gameState }
        };

        // Send the function command
        RemoteFunctionJsonConverter.ContextMap = _contextMap;
        playFunction.SendAsMessage(_clientStream);

        PlayerResponseJsonConverter.ContextMap = _contextMap;

        // Receive response
        return JsonConvert.DeserializeObject<PlayerResponse>(playFunction.ReceiveResponse(_clientStream))!;
    }

    /// <inheritdoc/>
    public void IsWinner(bool isWinner)
    {
        // Construct the function
        var winFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Win,
            Arguments = new List<object> { isWinner }
        };

        // Send the function command
        winFunction.SendAsMessage(_clientStream);

        // Receive response ("void")
        winFunction.ReceiveResponse(_clientStream);
    }

    /// <inheritdoc/>
    public TrainsMap Start(bool started)
    {
        var startFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.Start,
            Arguments = new List<object> { started }
        };

        startFunction.SendAsMessage(_clientStream);

        return JsonConvert.DeserializeObject<TrainsMap>(startFunction.ReceiveResponse(_clientStream))!;
    }

    /// <inheritdoc/>
    public void End(bool isWinner)
    {
        // Construct the function
        var endFunction = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.End,
            Arguments = new List<object> { isWinner }
        };

        // Send the function command
        endFunction.SendAsMessage(_clientStream);

        // Receive response ("void")
        endFunction.ReceiveResponse(_clientStream);
    }
}