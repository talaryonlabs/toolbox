using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[AuthentikApiEndpoint("/core/users/me/", AuthentikApiEndpointType.Get)]
public class AuthentikSelf : IAuthentikRessource
{
    [JsonPropertyName("user")] public AuthentikSelfUser? User { get; set; }
    [JsonPropertyName("original")] public AuthentikSelfUser? Original { get; set; }
} 

public class AuthentikSelfUser
{
    [JsonPropertyName("pk")] public int Pk { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("is_active")] public bool IsActive { get; set; }
    [JsonPropertyName("is_superuser")] public bool IsSuperuser { get; set; }
    [JsonPropertyName("groups")] public AuthentikSelfUserGroup[] Groups { get; set; } = [];
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("avatar")] public string Avatar { get; set; } = string.Empty;
    [JsonPropertyName("uid")] public string Uid { get; set; } = string.Empty;
    [JsonPropertyName("settings")] public Dictionary<string, object> Settings { get; set; } = new();
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("system_permissions")] public string[] SystemPermissions { get; set; } = [];
}

public class AuthentikSelfUserGroup
{
    [JsonPropertyName("pk")] public string? Uuid { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}