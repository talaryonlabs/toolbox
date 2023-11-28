using System.Text.Json.Serialization;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

[YouTrackEndpoint("/api/admin/projects")]
[YouTrackEndpoint("/api/admin/projects/.id", YouTrackEndpointType.Get)]
[YouTrackAdditionalFields("createdBy(id,name,$type)")]
public class YouTrackProject : IYouTrackResource
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("$type")] public string? Type { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("shortName")] public string? ShortName { get; set;}
    [JsonPropertyName("createdBy")] public YouTrackUser? CreatedBy { get; set; }
}

public class YouTrackProjectParams : YouTrackParams
{
    public YouTrackProjectParams Name(string name) => (YouTrackProjectParams)Set("name", name);
    public YouTrackProjectParams Description(string description) => (YouTrackProjectParams)Set("description", description);
    public YouTrackProjectParams ShortName(string shortName) => (YouTrackProjectParams)Set("shortName", shortName);
}