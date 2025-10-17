using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[AuthentikApiEndpoint("/core/users/")]
[AuthentikApiEndpoint("/core/users/.id/", AuthentikApiEndpointType.Get)]
public class AuthentikUser : IAuthentikRessource
{
    [JsonPropertyName("pk")] public required int Uuid { get; set; }
    [JsonPropertyName("username")] public required string Username { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }
    
    [JsonPropertyName("is_superuser")] public bool IsSuperUser { get; set; }
    [JsonPropertyName("is_active")] public bool IsActive { get; set; }
    
    [JsonPropertyName("groups")] public string[] Groups { get; set; } = [];
    [JsonPropertyName("avatar")] public string? Avatar { get; set; }
    [JsonPropertyName("attributes")] public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
    [JsonPropertyName("uid")] public required string Uid { get; set; }
    [JsonPropertyName("path")] public string? Path { get; set; }

    [JsonPropertyName("date_joined")] public DateTime? DateJoined { get; set; }
    [JsonPropertyName("last_login")] public DateTime? LastLogin { get; set; }
    [JsonPropertyName("last_updated")] public DateTime? LastUpdated { get; set; }
}