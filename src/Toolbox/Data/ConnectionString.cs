using System.Collections.Immutable;

namespace Talaryon.Toolbox.Data;

public class ConnectionString
{
    public string Type { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int? Port { get; set; }
    public string? Endpoint { get; set; }
    public ImmutableDictionary<string, string> Options { get; set; } = ImmutableDictionary<string, string>.Empty;
}
