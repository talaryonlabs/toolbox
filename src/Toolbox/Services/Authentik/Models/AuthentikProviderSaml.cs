using System.Text.Json.Serialization;
using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/providers/saml/")]
[ApiEndpoint("/providers/saml/.id/", ApiEndpointType.Single)]
public class AuthentikProviderSaml : IApiResource
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
    [JsonPropertyName("acs_url")] public Uri? AcsUrl { get; set; }
    [JsonPropertyName("audience")] public string? Audience { get; set; }
    [JsonPropertyName("issuer")] public string? Issuer { get; set; }
    [JsonPropertyName("assertion_valid_not_before")] public string? AssertionValidNotBefore { get; set; }
    [JsonPropertyName("assertion_valid_not_on_or_after")] public string? AssertionValidNotOnOrAfter { get; set; }
    [JsonPropertyName("session_valid_not_on_or_after")] public string? SessionValidNotOnOrAfter { get; set; }
    [JsonPropertyName("name_id_mapping")] public string? NameIdMapping { get; set; }
    [JsonPropertyName("authn_context_class_ref_mapping")] public string? AuthnContextClassRefMapping { get; set; }
    [JsonPropertyName("digest_algorithm")] public string? DigestAlgorithm { get; set; }
    [JsonPropertyName("signature_algorithm")] public string? SignatureAlgorithm { get; set; }
    [JsonPropertyName("signing_kp")] public string? SigningKp { get; set; }
    [JsonPropertyName("verification_kp")] public string? VerificationKp { get; set; }
    [JsonPropertyName("encryption_kp")] public string? EncryptionKp { get; set; }
    [JsonPropertyName("sign_assertion")] public bool SignAssertion { get; set; }
    [JsonPropertyName("sign_response")] public bool SignResponse { get; set; }
    [JsonPropertyName("sp_binding")] public string? SpBinding { get; set; }
    [JsonPropertyName("default_relay_state")] public string? DefaultRelayState { get; set; }
    [JsonPropertyName("default_name_id_policy")] public string? DefaultNameIdPolicy { get; set; }
    [JsonPropertyName("url_download_metadata")] public string? UrlDownloadMetadata { get; set; }
    [JsonPropertyName("url_sso_post")] public string? UrlSsoPost { get; set; }
    [JsonPropertyName("url_sso_redirect")] public string? UrlSsoRedirect { get; set; }
    [JsonPropertyName("url_sso_init")] public string? UrlSsoInit { get; set; }
    [JsonPropertyName("url_slo_post")] public string? UrlSloPost { get; set; }
    [JsonPropertyName("url_slo_redirect")] public string? UrlSloRedirect { get; set; }
}