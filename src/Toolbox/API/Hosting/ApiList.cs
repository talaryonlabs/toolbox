using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.API.Hosting;

[DataContract]
public class ApiList<T>
{
    [JsonPropertyName("items")] public IEnumerable<T> Items { get; set; } = new List<T>();
    [JsonPropertyName("next_cursor")] public string? NextCursor { get; set; }
    [JsonPropertyName("total_count")] public int TotalCount { get; set; }
}

[DataContract]
public class ApiListArgs
{
    [QueryMember("cursor")] public string Cursor { get; set; } = string.Empty;
    [QueryMember("skip")] public int Skip { get; set; }
    [QueryMember("limit")] public int Limit { get; set; }
}