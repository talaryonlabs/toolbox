using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Authentik.Models;

public class AuthentikGroup : IAuthentikRessource
{
    [JsonPropertyName("pk")] public required string Uuid { get; set; }
    [JsonPropertyName("num_pk")] public required string NumericUuid { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("is_superuser")] public bool IsSuperUser { get; set; }
    [JsonPropertyName("attributes")] public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
}