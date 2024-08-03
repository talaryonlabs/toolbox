using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Hetzner;

public class HetznerFailoverAddress : IHetznerObject
{
    public string Name => "failover";
    
    [JsonPropertyName("failover")] 
    public HetznerFailoverAddressObject? FailoverAddress { get; set; }
}

public class HetznerFailoverAddressObject
{
    // ReSharper disable once InconsistentNaming
    [JsonPropertyName("ip")] public string? IPAddress { get; set; }
    [JsonPropertyName("netmask")] public string? Netmask { get; set; }
    
    [JsonPropertyName("server_number")] public int? ServerId { get; set; }
    [JsonPropertyName("server_ip")] public string? ServerAddress { get; set; }
    
    [JsonPropertyName("active_server_ip")] public string? ActiveServerAddress { get; set; }
}