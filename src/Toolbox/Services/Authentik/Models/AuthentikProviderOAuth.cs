using System.Text.Json.Serialization;
using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/providers/oauth2/")]
[ApiEndpoint("/providers/oauth2/.id/", ApiEndpointType.Single)]
public class AuthentikProviderOAuth : IApiResource
{
    [JsonPropertyName("pk")] public int Uuid { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("authentication_flow")]
    public string? AuthenticationFlow { get; set; }

    [JsonPropertyName("authorization_flow")]
    public string? AuthorizationFlow { get; set; }

    [JsonPropertyName("invalidation_flow")]
    public string? InvalidationFlow { get; set; }

    [JsonPropertyName("property_mappings")]
    public string[] PropertyMappings { get; set; } = [];

    [JsonPropertyName("component")] public string? Component { get; set; }

    [JsonPropertyName("assigned_application_slug")]
    public string? AssignedApplicationSlug { get; set; }

    [JsonPropertyName("assigned_application_name")]
    public string? AssignedApplicationName { get; set; }

    [JsonPropertyName("assigned_backchannel_application_slug")]
    public string? AssignedBackchannelApplicationSlug { get; set; }

    [JsonPropertyName("assigned_backchannel_application_name")]
    public string? AssignedBackchannelApplicationName { get; set; }

    [JsonPropertyName("verbose_name")] public string? VerboseName { get; set; }

    [JsonPropertyName("verbose_name_plural")]
    public string? VerboseNamePlural { get; set; }

    [JsonPropertyName("meta_model_name")] public string? MetaModelName { get; set; }
    [JsonPropertyName("client_type")] public string? ClientType { get; set; }
    [JsonPropertyName("client_id")] public string? ClientId { get; set; }
    [JsonPropertyName("client_secret")] public string? ClientSecret { get; set; }

    [JsonPropertyName("access_code_validity")]
    public string? AccessCodeValidity { get; set; }

    [JsonPropertyName("access_token_validity")]
    public string? AccessTokenValidity { get; set; }

    [JsonPropertyName("refresh_token_validity")]
    public string? RefreshTokenValidity { get; set; }

    [JsonPropertyName("include_claims_in_id_token")]
    public bool IncludeClaimsInIdToken { get; set; }

    [JsonPropertyName("signing_key")] public string? SigningKey { get; set; }
    [JsonPropertyName("encryption_key")] public string? EncryptionKey { get; set; }
    [JsonPropertyName("redirect_uris")] public AuthentikProviderRedirectUri[] RedirectUris { get; set; } = [];

    [JsonPropertyName("backchannel_logout_uri")]
    public Uri? BackchannelLogoutUri { get; set; }

    [JsonPropertyName("sub_mode")] public string? SubMode { get; set; }
    [JsonPropertyName("issuer_mode")] public string? IssuerMode { get; set; }

    [JsonPropertyName("jwt_federation_sources")]
    public string[] JwtFederationSources { get; set; } = [];

    [JsonPropertyName("jwt_federation_providers")]
    public int[] JwtFederationProviders { get; set; } = [];
}