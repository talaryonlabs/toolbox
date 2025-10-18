using System.Text.Json.Serialization;
using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/providers/ldap/")]
[ApiEndpoint("/providers/ldap/.id/", ApiEndpointType.Single)]
public class AuthentikProviderLdap : IApiResource
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
    [JsonPropertyName("base_dn")] public string? BaseDn { get; set; }
    [JsonPropertyName("certificate")] public string? Certificate { get; set; }
    [JsonPropertyName("tls_server_name")] public string? TlsServerName { get; set; }
    [JsonPropertyName("uid_start_number")] public int? UidStartNumber { get; set; }
    [JsonPropertyName("gid_start_number")] public int? GidStartNumber { get; set; }
    [JsonPropertyName("outpost_set")] public string[] OutpostSet { get; set; } = [];
    [JsonPropertyName("search_mode")] public string? SearchMode { get; set; }
    [JsonPropertyName("bind_mode")] public string? BindMode { get; set; }
    [JsonPropertyName("mfa_support")] public bool MfaSupport { get; set; }
}