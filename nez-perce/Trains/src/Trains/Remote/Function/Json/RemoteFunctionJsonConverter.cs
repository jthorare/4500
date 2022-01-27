using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;
using Trains.Common.GameState;
using Trains.Common.GameState.Json;
using Trains.Common.Map;

namespace Trains.Remote.Function.Json;

/// <summary>
/// A Converter that allows serializing and deserializing a <see cref="RemoteFunction"/>
/// </summary>
public class RemoteFunctionJsonConverter : JsonConverter<RemoteFunction>
{
    /// <summary>
    /// A Map that is used as context for deserializing some remote function arguments.
    /// </summary>
    /// <remarks>
    /// This variable should be set before deserializing a <see cref="RemoteFunction"/> with a <see cref="PlayerGameState"/>
    /// </remarks>
    public static TrainsMap? ContextMap { get; set; }
    
    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, RemoteFunction? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartArray();
        serializer.Serialize(writer, value.FunctionName);
        serializer.Serialize(writer, value.Arguments);
        writer.WriteEndArray();
    }

    /// <inheritdoc/>
    public override RemoteFunction? ReadJson(JsonReader reader, Type objectType, RemoteFunction? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartArray)
            throw new JsonException("Malformed Remote Function Json.");

        reader.Read();
        var functionName = serializer.Deserialize<RemoteFunctionName>(reader);

        if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
            throw new JsonException("Malformed Remote Function Json Arguments.");
        
        var arguments = ReadArguments(functionName, reader, serializer);
        
        if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
            throw new JsonException("Remote Function Json Arguments has too many elements.");

        if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
            throw new JsonException("Remote Function Json has too many elements.");

        return new RemoteFunction()
        {
            FunctionName = functionName,
            Arguments = arguments
        };
    }

    /// <summary>
    /// Reads from a <see cref="JsonReader"/> and returns the <see cref="List{T}"/> of object that represents arguments
    /// for a <see cref="RemoteFunction"/>
    /// </summary>
    /// <param name="functionName">The <see cref="RemoteFunctionName"/>.</param>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="serializer">The <see cref="JsonSerializer"/> to deserialize from.</param>
    /// <returns>The <see cref="List{T}"/> of object arguments</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If functionName is not a valid <see cref="RemoteFunctionName"/>
    /// </exception>
    private static List<object> ReadArguments(RemoteFunctionName functionName, JsonReader reader, JsonSerializer serializer)
    {
        switch (functionName)
        {
            case RemoteFunctionName.Start:
                return ReadBoolArgs(reader, serializer);
            case RemoteFunctionName.Setup:
                return ReadSetupArgs(reader, serializer);
            case RemoteFunctionName.Pick:
                return ReadPickArgs(reader, serializer);
            case RemoteFunctionName.Play:
                return ReadPlayArgs(reader, serializer);
            case RemoteFunctionName.More:
                return ReadMoreArgs(reader, serializer);
            case RemoteFunctionName.Win:
                return ReadBoolArgs(reader, serializer);
            case RemoteFunctionName.End:
                return ReadBoolArgs(reader, serializer);
            default:
                throw new ArgumentOutOfRangeException(nameof(functionName), functionName,
                    "No such function name exists");
        }
    }

    /// <summary>
    /// Read the next value as a <see cref="List{T}"/> of bool.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="serializer">The <see cref="JsonSerializer"/> to deserialize from.</param>
    /// <returns>The <see cref="List{T}"/> of object arguments</returns>
    private static List<object> ReadBoolArgs(JsonReader reader, JsonSerializer serializer)
    {
        reader.Read();
        return new List<object> { serializer.Deserialize<bool>(reader) };
    }

    
    /// <summary>
    /// Read the next value as a <see cref="List{T}"/> of object in the format [ <see cref="TrainsMap"/>, uint, <see cref="List{T}"/> of <see cref="Color"/> ]/>.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="serializer">The <see cref="JsonSerializer"/> to deserialize from.</param>
    /// <returns>The <see cref="List{T}"/> of object arguments</returns>
    private static List<object> ReadSetupArgs(JsonReader reader, JsonSerializer serializer)
    {
        reader.Read();
        var map = serializer.Deserialize<TrainsMap>(reader)!;
        reader.Read();
        var rails = serializer.Deserialize<uint>(reader);
        reader.Read();
        var cards = serializer.Deserialize<List<Color>>(reader)!;

        return new List<object> { map, rails, cards };
    }
    
    /// <summary>
    /// Read the next value as a <see cref="List{T}"/> of object in the format [ <see cref="List{T}"/> of <see cref="Destination"/> ]/>.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="serializer">The <see cref="JsonSerializer"/> to deserialize from.</param>
    /// <returns>The <see cref="List{T}"/> of object arguments</returns>
    private static List<object> ReadPickArgs(JsonReader reader, JsonSerializer serializer)
    {
        reader.Read();
        var destinations = serializer.Deserialize<List<Destination>>(reader)!;
        return new List<object>{ destinations.ToImmutableHashSet() };
    }
    
    /// <summary>
    /// Read the next value as a <see cref="List{T}"/> of object in the format [ <see cref="PlayerGameState"/> ]/>.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="serializer">The <see cref="JsonSerializer"/> to deserialize from.</param>
    /// <returns>The <see cref="List{T}"/> of object arguments</returns>
    private static List<object> ReadPlayArgs(JsonReader reader, JsonSerializer serializer)
    {
        if (ContextMap == null)
        {
            throw new JsonException(
                "Please set RemoteFunctionJsonConverter.ContextMap before deserializing a PlayFunction");
        }

        reader.Read();
        PlayerGameStateJsonConverter.ContextMap = ContextMap;
        var pgs = serializer.Deserialize<PlayerGameState>(reader)!;
        return new List<object> { pgs };
    }
    
    /// <summary>
    /// Read the next value as a <see cref="List{T}"/> of object in the format [ <see cref="List{T}"/> of <see cref="Color"/> ]/>.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="serializer">The <see cref="JsonSerializer"/> to deserialize from.</param>
    /// <returns>The <see cref="List{T}"/> of object arguments</returns>
    private static List<object> ReadMoreArgs(JsonReader reader, JsonSerializer serializer)
    {
        reader.Read();
        var cards = serializer.Deserialize<List<Color>>(reader)!;
        return new List<object> { cards };
    }
}