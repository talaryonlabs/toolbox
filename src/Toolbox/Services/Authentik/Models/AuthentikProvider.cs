using System.Text.Json.Serialization;
using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/providers/all/")]
[ApiEndpoint("/providers/all/.id/", ApiEndpointType.Single)]
public class AuthentikProvider : IApiResource
{
    [JsonPropertyName("pk")] public int Uuid { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("authentication_flow")] public string? AuthenticationFlow { get; set; }
    [JsonPropertyName("authorization_flow")] public string? AuthorizationFlow { get; set; }
    [JsonPropertyName("invalidation_flow")] public string? InvalidationFlow { get; set; }
    [JsonPropertyName("property_mappings")] public string[] PropertyMappings { get; set; } = [];
    [JsonPropertyName("component")] public string? Component { get; set; }
    [JsonPropertyName("assigned_application_slug")] public string? AssignedApplicationSlug { get; set; }
    [JsonPropertyName("assigned_application_name")] public string? AssignedApplicationName { get; set; }
    [JsonPropertyName("assigned_backchannel_application_slug")] public string? AssignedBackchannelApplicationSlug { get; set; }
    [JsonPropertyName("assigned_backchannel_application_name")] public string? AssignedBackchannelApplicationName { get; set; }
    [JsonPropertyName("verbose_name")] public string? VerboseName { get; set; }
    [JsonPropertyName("verbose_name_plural")] public string? VerboseNamePlural { get; set; }
    [JsonPropertyName("meta_model_name")] public string? MetaModelName { get; set; }
}

public class AuthentikProviderRedirectUri
{
    [JsonPropertyName("matching_mode")] public string MatchingMode { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
}