using System.Text.Json.Serialization;
using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/providers/proxy/")]
[ApiEndpoint("/providers/proxy/.id/", ApiEndpointType.Single)]
public class AuthentikProviderProxy : IApiResource
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
    [JsonPropertyName("client_id")] public string? ClientId { get; set; }
    [JsonPropertyName("internal_host")] public Uri? InternalHost { get; set; }
    [JsonPropertyName("external_host")] public Uri? ExternalHost { get; set; }
    [JsonPropertyName("internal_host_ssl_validation")] public bool InternalHostSslValidation { get; set; }
    [JsonPropertyName("certificate")] public string? Certificate { get; set; }
    [JsonPropertyName("skip_path_regex")] public string? SkipPathRegex { get; set; }
    [JsonPropertyName("basic_auth_enabled")] public bool BasicAuthEnabled { get; set; }
    [JsonPropertyName("basic_auth_password_attribute")] public string? BasicAuthPasswordAttribute { get; set; }
    [JsonPropertyName("basic_auth_user_attribute")] public string? BasicAuthUserAttribute { get; set; }
    [JsonPropertyName("mode")] public string? Mode { get; set; }
    [JsonPropertyName("intercept_header_auth")] public bool InterceptHeaderAuth { get; set; }
    [JsonPropertyName("redirect_uris")] public AuthentikProviderRedirectUri[] RedirectUris { get; set; } = [];
    [JsonPropertyName("cookie_domain")] public string? CookieDomain { get; set; }
    [JsonPropertyName("jwt_federation_sources")] public string[] JwtFederationSources { get; set; } = [];
    [JsonPropertyName("jwt_federation_providers")] public int[] JwtFederationProviders { get; set; } = [];
    [JsonPropertyName("access_token_validity")] public string? AccessTokenValidity { get; set; }
    [JsonPropertyName("refresh_token_validity")] public string? RefreshTokenValidity { get; set; }
    [JsonPropertyName("outpost_set")] public string[] OutpostSet { get; set; } = [];
}