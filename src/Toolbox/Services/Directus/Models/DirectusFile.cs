using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Directus;

[DirectusTable("directus_files")]
public class DirectusFile
{
    [DirectusField("id")] public string? Id { get; set; }
    [DirectusField("title")] public string? Title { get; set; }
    
    [DefaultValue(0)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DirectusField("width")] 
    public int? Width { get; set; }
    
    [DefaultValue(0)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DirectusField("height")] 
    public int? Height { get; set; }
}