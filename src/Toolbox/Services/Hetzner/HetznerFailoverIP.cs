using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Hetzner;

public class HetznerFailoverAddress : IHetznerObject
{
    // ReSharper disable once InconsistentNaming
    [JsonPropertyName("ip")] public string? IPAddress { get; set; }
    [JsonPropertyName("netmask")] public string? Netmask { get; set; }
    
    [JsonPropertyName("server_number")] public int? ServerId { get; set; }
    [JsonPropertyName("server_ip")] public string? ServerAddress { get; set; }
    
    [JsonPropertyName("active_server_ip")] public string? ActiveServerAddress { get; set; }
    
    public string Name => "failover";
}