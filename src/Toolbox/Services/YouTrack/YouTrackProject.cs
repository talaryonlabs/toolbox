using Newtonsoft.Json;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

[YouTrackApiEndpoint("/api/admin/projects")]
[YouTrackApiEndpoint("/api/admin/projects/.id", YouTrackApiEndpointType.Get)]
public class YouTrackProject : IYouTrackRessource
{
    [JsonProperty("id")]
    public string? Id { get; }
    
    [JsonProperty("name")]
    string? Name { get; set; }
    
    [JsonProperty("description")]
    string? Description { get; set; }

    [JsonProperty("createdBy")]
    YouTrackUser? CreatedBy { get; set; } 
}