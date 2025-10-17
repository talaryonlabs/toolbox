using System.Text.Json.Serialization;
using Talaryon.Toolbox.Services.Authentik.Models;

namespace Talaryon.Toolbox.Services.Authentik;

public class AuthentikList<T>
{
    [JsonPropertyName("results")] public T[] Items { get; set; } = [];
    [JsonPropertyName("pagination")] public required AuthentikPagination Pagination { get; set; }
}