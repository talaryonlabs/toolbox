using System.Text.Json.Serialization;
using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/core/applications/")]
[ApiEndpoint("/core/applications/.id/", ApiEndpointType.Single | ApiEndpointType.Update | ApiEndpointType.Delete)]
public class AuthentikApplication : IApiResource
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