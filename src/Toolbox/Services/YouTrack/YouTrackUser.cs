using System.Text.Json.Serialization;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

[YouTrackEndpoint("/api/users")]
[YouTrackEndpoint("/api/users/.id", YouTrackEndpointType.Get)]
public class YouTrackUser : IYouTrackResource
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("$type")] public string? Type { get; set; }
    [JsonPropertyName("login")] public string? Login { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("fullName")] public string? FullName { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("guest")] public bool IsGuest { get; set; }
    [JsonPropertyName("online")] public bool IsOnline { get; set; }
    [JsonPropertyName("banned")] public bool IsBanned { get; set; }
    [JsonPropertyName("avatarUrl")] public string? AvatarUrl { get; set; }
}