using System.Text.Json;
using System.Text.Json.Serialization;
using Talaryon.Toolbox.Services.Authentik.Models;
using Talaryon.Toolbox.Services.Directus;

namespace Talaryon.Toolbox.Services.Authentik;

public class AuthentikList
{
    [JsonPropertyName("results")] public object[] Results { get; set; } = [];
    [JsonPropertyName("pagination")] public required AuthentikPagination Pagination { get; set; }

    public T[]? GetItems<T>()
    {
        return (from JsonElement element in Results select element.Deserialize<T>()).ToArray();
    }
}