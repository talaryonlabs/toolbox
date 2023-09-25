using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TalaryonLabs.Toolbox.API;

[JsonObject]
public class ApiList<T>
{
    [JsonProperty("items")] public IEnumerable<T> Items { get; set; } = new List<T>();
    [JsonProperty("next_cursor")] public string? NextCursor { get; set; }
    [JsonProperty("total_count")] public int TotalCount { get; set; }
}

[DataContract]
public class ApiListArgs
{
    [QueryMember("cursor")] public string Cursor { get; set; } = string.Empty;
    [QueryMember("skip")] public int Skip { get; set; }
    [QueryMember("limit")] public int Limit { get; set; }
}