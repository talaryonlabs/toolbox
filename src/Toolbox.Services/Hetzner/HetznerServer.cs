using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Hetzner;

// ReSharper disable InconsistentNaming
public class HetznerServer : IHetznerObject
{
    [JsonPropertyName("server_number")] public int ServerId { get; set; }
    [JsonPropertyName("server_name")] public string? ServerName { get; set; }
    
    [JsonPropertyName("product")] public string? ProductName { get; set; }
    [JsonPropertyName("dc")] public string? DataCenter { get; set; }
    [JsonPropertyName("traffic")] public string? Traffic { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("paid_until")] public string? PaidUntil { get; set; }
    
    [JsonPropertyName("cancelled")] public bool? IsCancelled { get; set; }
    [JsonPropertyName("hot_swap")] public bool? IsHotSwapable { get; set; }
    
    [JsonPropertyName("reset")] public bool? CanReset { get; set; }
    [JsonPropertyName("wol")] public bool? CanWOL { get; set; }
    [JsonPropertyName("rescue")] public bool? CanRescue { get; set; }
    [JsonPropertyName("vnc")] public bool? CanVNC { get; set; }
    [JsonPropertyName("windows")] public bool? CanWindows { get; set; }
    [JsonPropertyName("plesk")] public bool? CanPlesk { get; set; }
    [JsonPropertyName("cpanel")] public bool? CanCPanel { get; set; }
    
    /*
     * Networking
     */
    [JsonPropertyName("server_ip")] public string? IPAddress { get; set; }
    [JsonPropertyName("ip")] public string[]? IPAddresses { get; set; }
    [JsonPropertyName("server_ipv6_net")] public string? Subnet { get; set; }
    [JsonPropertyName("subnet")] public HetznerServerSubnet[]? Subnets { get; set; }
    
    [JsonPropertyName("linked_storagebox")] public int? StorageBoxId { get; set; }
    
    public string Name => "server";
}

public class HetznerServerSubnet
{
    [JsonPropertyName("ip")] public string? IPAddress { get; set; }
    [JsonPropertyName("mask")] public string? Netmask { get; set; }
}