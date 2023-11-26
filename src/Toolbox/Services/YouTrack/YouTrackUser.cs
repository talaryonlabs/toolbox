using Newtonsoft.Json;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

[YouTrackApiEndpoint("/api/users")]
[YouTrackApiEndpoint("/api/users/.id", YouTrackApiEndpointType.Get)]
public class YouTrackUser : IYouTrackRessource
{
    [JsonProperty("id")]
    public string? Id { get; }
    
    [JsonProperty("login")]
    string? Login { get; }
    
    [JsonProperty("name")]
    public string? Name { get; }
    
    [JsonProperty("fullName")]
    string? FullName { get; }
    
    [JsonProperty("email")]
    string? Email { get; }
    
    [JsonProperty("guest")]
    bool IsGuest { get; }
    
    [JsonProperty("online")]
    bool IsOnline { get; }
    
    [JsonProperty("banned")]
    bool IsBanned { get; }
    
    [JsonProperty("avatarUrl")]
    string? AvatarUrl { get; }
}