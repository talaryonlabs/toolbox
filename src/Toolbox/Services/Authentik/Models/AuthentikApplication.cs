using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[AuthentikApiEndpoint("/core/applications/")]
[AuthentikApiEndpoint("/core/applications/.id/", AuthentikApiEndpointType.Get)]
public class AuthentikApplication : IAuthentikRessource
{
    [JsonPropertyName("pk")] public required string Uuid { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("slug")] public required string Slug { get; set; }
    
    [JsonPropertyName("launch_url")] public string? LaunchUrl { get; set; }
    [JsonPropertyName("group")] public string? Group { get; set; }
    
    [JsonPropertyName("meta_icon")] public string? MetaIcon { get; set; }
    [JsonPropertyName("meta_description")] public string? MetaDescription { get; set; }
    [JsonPropertyName("meta_publisher")] public string? MetaPublisher { get; set; }
}