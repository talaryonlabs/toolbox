using System.Text.Json.Serialization;
using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/core/groups/")]
[ApiEndpoint("/core/groups/.id/", ApiEndpointType.Single)]
public class AuthentikGroup : IApiResource
{
    [JsonPropertyName("pk")] public string? Uuid { get; set; }
    [JsonPropertyName("num_pk")] public int? NumericUuid { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("is_superuser")] public bool IsSuperUser { get; set; }
    [JsonPropertyName("parent")] public string? Parent { get; set; }
    [JsonPropertyName("parent_name")] public string? ParentName { get; set; }
    [JsonPropertyName("users")] public int[] Users { get; set; } = [];
    [JsonPropertyName("roles")] public string[]? Roles { get; set; }
    [JsonPropertyName("children")] public string[] Children { get; set; } = [];
    [JsonPropertyName("children_objects")] public AuthentikGroup[] ChildrenObjects { get; set; } = [];
    [JsonPropertyName("attributes")] public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
}