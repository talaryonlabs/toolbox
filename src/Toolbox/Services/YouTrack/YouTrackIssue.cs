using System.Text.Json.Serialization;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

[YouTrackEndpoint("/api/issues", YouTrackEndpointType.List | YouTrackEndpointType.Create)]
[YouTrackEndpoint("/api/issues/.id", YouTrackEndpointType.Get | YouTrackEndpointType.Update | YouTrackEndpointType.Delete)]
[YouTrackAdditionalFields("reporter(id,name,$type)", "updater(id,name,$type)", "project(id,name,$type)")]
public class YouTrackIssue : IYouTrackResource
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("$type")] public string? Type { get; set; }
    [JsonPropertyName("summary")] public string? Summary { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("created")] public long? CreatedAt { get; set; }
    [JsonPropertyName("updated")] public long? UpdatedAt { get; set; }
    [JsonPropertyName("resolved")] public long? ResolvedAt { get; set; }
    [JsonPropertyName("reporter")] public YouTrackUser? CreatedBy { get; set; }
    [JsonPropertyName("updater")] public YouTrackUser? UpdatedBy { get; set; }
    [JsonPropertyName("project")] public YouTrackProject? Project { get; set; }
}

public class YouTrackIssueParams : YouTrackParams
{
    public YouTrackIssueParams Summary(string summary) => (YouTrackIssueParams)Set("summary", summary);
    public YouTrackIssueParams Description(string description) => (YouTrackIssueParams)Set("description", description);
    public YouTrackIssueParams Project(dynamic project) => (YouTrackIssueParams)Set("project", project);
    public YouTrackIssueParams Reporter(dynamic reporter) => (YouTrackIssueParams)Set("reporter", reporter);
}