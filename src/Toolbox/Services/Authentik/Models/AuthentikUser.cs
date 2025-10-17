using Newtonsoft.Json;

namespace Talaryon.Toolbox.Services.Authentik.Models;

public class AuthentikUser
{
    [JsonProperty("pk")] public required string Uuid { get; set; }
    [JsonProperty("username")] public required string Username { get; set; }
    [JsonProperty("email")] public string? Email { get; set; }
    [JsonProperty("name")] public required string Name { get; set; }
    
    [JsonProperty("first_name")] public string? FirstName { get; set; }
    [JsonProperty("last_name")] public string? LastName { get; set; }
    
    [JsonProperty("is_superuser")] public bool IsSuperUser { get; set; }
    [JsonProperty("is_active")] public bool IsActive { get; set; }
    
    [JsonProperty("groups")] public string[] Groups { get; set; } = [];
    [JsonProperty("avatar")] public string? Avatar { get; set; }
    [JsonProperty("attributes")] public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
    [JsonProperty("uid")] public required string Uid { get; set; }
    [JsonProperty("path")] public string? Path { get; set; }

    [JsonProperty("date_joined")] public DateTime DateJoined { get; set; }
    [JsonProperty("last_login")] public DateTime LastLogin { get; set; }
    [JsonProperty("last_updated")] public DateTime LastUpdated { get; set; }
}