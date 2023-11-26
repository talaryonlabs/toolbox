using Newtonsoft.Json;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

[YouTrackApiEndpoint("/api/issues")]
[YouTrackApiEndpoint("/api/issues/.id", YouTrackApiEndpointType.Get)]
public class YouTrackIssue : IYouTrackRessource
{
    [JsonProperty("id")]
    public string? Id { get; }

    [JsonProperty("summary")]
    public string? Summary { get; set; }
    
    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("created")]
    public long CreatedAt { get; }
    
    [JsonProperty("updated")]
    public long UpdatedAt { get; }
    
    [JsonProperty("resolved")]
    public long ResolvedAt { get; }

    [JsonProperty("reporter")]
    public YouTrackUser? CreatedBy { get; }
    
    [JsonProperty("updater")]
    public YouTrackUser? UpdatedBy { get; }
    
//    YouTrackProject Project { get; }
}