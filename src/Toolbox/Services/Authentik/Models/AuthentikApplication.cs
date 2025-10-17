using Newtonsoft.Json;

namespace Talaryon.Toolbox.Services.Authentik.Models;

public class AuthentikApplication
{
    [JsonProperty("pk")] public required string Uuid { get; set; }
    [JsonProperty("name")] public required string Name { get; set; }
    [JsonProperty("slug")] public required string Slug { get; set; }
    
    [JsonProperty("launch_url")] public string? LaunchUrl { get; set; }
    [JsonProperty("group")] public string? Group { get; set; }
    
    [JsonProperty("meta_icon")] public string? MetaIcon { get; set; }
    [JsonProperty("meta_description")] public string? MetaDescription { get; set; }
    [JsonProperty("meta_publisher")] public string? MetaPublisher { get; set; }
}