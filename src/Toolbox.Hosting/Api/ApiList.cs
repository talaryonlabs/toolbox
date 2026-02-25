using System.Text.Json.Serialization;
using Talaryon.Toolbox.Hosting.Api.Attributes;

namespace Talaryon.Toolbox.Hosting.Api;

public class ApiList<T>
{
    [JsonPropertyName("items")] public IEnumerable<T> Items { get; set; } = new List<T>();
    [JsonPropertyName("next_cursor")] public string? NextCursor { get; set; }
    [JsonPropertyName("total_count")] public int TotalCount { get; set; }
}

public class ApiListArgs
{
    [QueryMember("cursor")] public string Cursor { get; set; } = string.Empty;
    [QueryMember("skip")] public int Skip { get; set; }
    [QueryMember("limit")] public int Limit { get; set; }
}