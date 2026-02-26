using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Authentik.Models;

public class AuthentikPagination
{
    [JsonPropertyName("next")] public int Next { get; set; }
    [JsonPropertyName("previous")] public int Previous { get; set; }
    [JsonPropertyName("count")] public int Count { get; set; }
    [JsonPropertyName("current")] public int Current { get; set; }
    [JsonPropertyName("total_pages")] public int TotalPages { get; set; }
    [JsonPropertyName("start_index")] public int StartIndex { get; set; }
    [JsonPropertyName("end_index")] public int EndIndex { get; set; }
}