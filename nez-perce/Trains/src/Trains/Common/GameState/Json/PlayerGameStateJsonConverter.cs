using System;
using Newtonsoft.Json;
using Trains.Common.Map;

namespace Trains.Common.GameState.Json;

public class PlayerGameStateJsonConverter : JsonConverter<PlayerGameState>
{
    public static TrainsMap? ContextMap { get; set; }

    public override void WriteJson(JsonWriter writer, PlayerGameState? value, JsonSerializer serializer)
    {
        var playerStateJson = PlayerStateJson.ConvertFromPlayerGameState(value!);
        serializer.Serialize(writer, playerStateJson);
    }

    public override PlayerGameState? ReadJson(JsonReader reader, Type objectType, PlayerGameState? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (ContextMap == null)
        {
            throw new JsonException(
                "Please set PlayerGameStateJsonConverter.ContextMap before deserializing");
        }
        var playerStateJson = serializer.Deserialize<PlayerStateJson>(reader)!;
        var playerGameState = playerStateJson.ConvertToPlayerGameState(ContextMap);
        ContextMap = null;

        return playerGameState;
    }
    
}