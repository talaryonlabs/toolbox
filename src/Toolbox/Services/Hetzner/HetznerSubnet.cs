using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Hetzner;

// ReSharper disable InconsistentNaming
public class HetznerSubnet : IHetznerObject
{
    [JsonPropertyName("locked")] public bool IsLocked { get; set; }
    [JsonPropertyName("failover")] public bool IsFailover { get; set; }
    [JsonPropertyName("traffic_warnings")] public bool IsTrafficWarningEnabled { get; set; }
    
    [JsonPropertyName("ip")] public string? IPAddress { get; set; }
    [JsonPropertyName("gateway")] public string? Gateway { get; set; }
    [JsonPropertyName("mask")] public int? Netmask { get; set; }
    
    [JsonPropertyName("server_number")] public int? ServerId { get; set; }
    [JsonPropertyName("server_ip")] public string? ServerIPAddress { get; set; }
    
    [JsonPropertyName("traffic_hourly")] public int? HourlyTrafficLimit { get; set; }
    [JsonPropertyName("traffic_daily")] public int? DailyTrafficLimit { get; set; }
    [JsonPropertyName("traffic_monthly")] public int? MonthlyTrafficLimit { get; set; }
    
    public string Name => "subnet";
}