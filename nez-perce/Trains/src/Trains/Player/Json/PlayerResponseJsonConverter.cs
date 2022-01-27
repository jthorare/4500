using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Trains.Common.GameState.Json;
using Trains.Common.Map;

namespace Trains.Player.Json;

public class PlayerResponseJsonConverter : JsonConverter<PlayerResponse>
{
    /// <summary>
    /// A Map that is used as context for deserializing a <see cref="PlayerResponse"/>.
    /// </summary>
    /// <remarks>
    /// This variable should be set before deserializing a <see cref="PlayerResponse"/>
    /// </remarks>
    public static TrainsMap? ContextMap { get; set; }

    public override void WriteJson(JsonWriter writer, PlayerResponse? value, JsonSerializer serializer)
    {
        switch (value!.ResponseType)
        {
            case ResponseType.DrawCards:
                writer.WriteValue("more cards");
                break;
            case ResponseType.ClaimConnection:
                serializer.Serialize(writer, AcquiredJson.ConvertFromConnection(value.RequestedConnectionClaim));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override PlayerResponse? ReadJson(JsonReader reader, Type objectType, PlayerResponse? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (ContextMap == null)
        {
            throw new JsonException(
                "Please set PlayerResponseJsonConverter.ContextMap before deserializing a PlayerResponse.");
        }

        if (reader.TokenType == JsonToken.String && serializer.Deserialize<string>(reader) == "more cards")
        {
            return PlayerResponse.DrawCard();
        }
        var acquiredJson = serializer.Deserialize<AcquiredJson>(reader);

        try
        {
            return PlayerResponse.ClaimConnection(ContextMap.Connections.First(con =>
                con.Color == acquiredJson.Color &&
                con.Segments == acquiredJson.Length &&
                con.Locations.Select(loc => loc.Name).ToHashSet()
                    .SetEquals(new HashSet<string> { acquiredJson.City1, acquiredJson.City2 })));
        }
        catch (InvalidOperationException ioe)
        {
            return PlayerResponse.ClaimConnection(new Connection(new Location(acquiredJson.City1, .1f, .1f), new Location(acquiredJson.City2, .2f, .2f), acquiredJson.Length, acquiredJson.Color));
        }
    }
}