using Newtonsoft.Json;

namespace Talaryon.Toolbox.Services.Authentik.Models;

public class AuthentikGroup
{
    [JsonProperty("pk")] public required string GroupUuid { get; set; }
    [JsonProperty("num_pk")] public required string NumericGroupUuid { get; set; }
    [JsonProperty("name")] public required string Name { get; set; }
    [JsonProperty("is_superuser")] public bool IsSuperUser { get; set; }
    [JsonProperty("attributes")] public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
}