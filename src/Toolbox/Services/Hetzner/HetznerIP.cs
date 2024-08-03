using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Hetzner;

// ReSharper disable InconsistentNaming
public class HetznerIP : IHetznerObject
{
    public string Name => "ip";
    
    [JsonPropertyName("ip")]
    public HetznerIPObject? IP { get; set; }   
}

public class HetznerIPObject
{
    [JsonPropertyName("locked")] public bool IsLocked { get; set; }
    [JsonPropertyName("traffic_warnings")] public bool IsTrafficWarningEnabled { get; set; }
    
    [JsonPropertyName("ip")] public string? IPAddress { get; set; }
    [JsonPropertyName("gateway")] public string? Gateway { get; set; }
    [JsonPropertyName("mask")] public int? Netmask { get; set; }
    [JsonPropertyName("broadcast")] public string? Broadcast { get; set; }
    [JsonPropertyName("separate_mac")] public string? MACAddress { get; set; }
    
    [JsonPropertyName("server_number")] public int? ServerId { get; set; }
    [JsonPropertyName("server_ip")] public string? ServerAddress { get; set; }
    
    [JsonPropertyName("traffic_hourly")] public int? HourlyTrafficLimit { get; set; }
    [JsonPropertyName("traffic_daily")] public int? DailyTrafficLimit { get; set; }
    [JsonPropertyName("traffic_monthly")] public int? MonthlyTrafficLimit { get; set; }
}