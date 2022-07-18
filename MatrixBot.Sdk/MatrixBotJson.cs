using System.Text.Json.Serialization;

namespace MatrixBot.Sdk;

public class MatrixBotJsonSend
{
    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }
}

public class MatrixBotJsonJoin
{
    [JsonPropertyName("room_id")]
    public string? RoomId { get; set; }
}

public class MatrixBotJsonLogin
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    [JsonPropertyName("home_server")]
    public string? HomeServer { get; set; }
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}

public class MatrixBotJsonProfile
{
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
    [JsonPropertyName("displayname")]
    public string? DisplayName { get; set; }

}

public class MatrixBotJsonRooms
{
    [JsonPropertyName("joined_rooms")]
    public string[]? JoinedRooms { get; set; }
}

public class MatrixBotJsonSync
{
    [JsonPropertyName("rooms")]
    public MatrixBotJsonSyncRooms? Rooms { get; set; }
    [JsonPropertyName("next_batch")]
    public string? NextBatch { get; set; }
}

public class MatrixBotJsonSyncRooms
{
    [JsonPropertyName("join")]
    public Dictionary<string, MatrixBotJsonSyncTimeLine>? Join { get; set; }

}
public class MatrixBotJsonSyncTimeLine
{
    [JsonPropertyName("timeline")]
    public MatrixBotJsonSyncEvents? TimeLine { get; set; }
}

public class MatrixBotJsonSyncEvents
{
    [JsonPropertyName("events")]
    public MatrixBotJsonSyncEvent[]? Events { get; set; }
}

public class MatrixBotJsonSyncEvent
{
    [JsonPropertyName("content")]
    public MatrixBotJsonSyncEventContent? Content { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }
    [JsonPropertyName("sender")]
    public string? Sender { get; set; }
}

public class MatrixBotJsonSyncEventContent
{
    [JsonPropertyName("body")]
    public string? Body { get; set; }
    [JsonPropertyName("msgtype")]
    public string? MsgType { get; set; }
    [JsonPropertyName("format")]
    public string? Format { get; set; }
    [JsonPropertyName("formatted_body")]
    public string? FormattedBody { get; set; }
    [JsonPropertyName("membership")]
    public string? Membership { get; set; }
    [JsonPropertyName("displayname")]
    public string? DisplayName { get; set; }
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
}

public class MatrixBotJsonStateContent
{
    [JsonPropertyName("aliases")] //m.room.aliases
    public string[]? Aliases { get; set; }
    [JsonPropertyName("creator")] //m.room.create
    public string? Creator { get; set; }
    [JsonPropertyName("join_rule")] // m.room.join_rules
    public string? JoinRule { get; set; }
    [JsonPropertyName("name")] // m.room.name
    public string? Name { get; set; }
}
