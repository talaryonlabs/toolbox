using System.Reflection;
using System.Text.Json.Serialization;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.YouTrack;

public static class YouTrackEndpoint
{
    public static string? GetEndpoint<T>(YouTrackEndpointType type = YouTrackEndpointType.List) => typeof(T)
        .GetCustomAttributes<YouTrackEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type.HasFlag(type))
        ?.Url;

    public static List<string> GetFields<T>() => typeof(T)
        .GetProperties()
        .Where(p => p.HasCustomAttribute<JsonPropertyNameAttribute>())
        .Select(p => p.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name)
        .ToList()!;
    
    public static List<string> GetAdditionalFields<T>() => (typeof(T)
        .GetCustomAttributes<YouTrackAdditionalFieldsAttribute>(true)
        .FirstOrDefault()
        ?.Fields ?? Array.Empty<string>())
        .ToList();
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class YouTrackEndpointAttribute(string? url, YouTrackEndpointType type = YouTrackEndpointType.List) : Attribute
{
    public string? Url { get; set; } = url;
    public YouTrackEndpointType Type { get; set; } = type;
}

[Flags]
public enum YouTrackEndpointType
{
    List = 1,
    Get = 2,
    Create = 4,
    Update = 8,
    Delete = 16
}

public class YouTrackEndpointException : Exception
{
    public YouTrackEndpointException()
        : base("YouTrackEndpoint not found.")
    {
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class YouTrackAdditionalFieldsAttribute(params string[]? fields) : Attribute
{
    public string[]? Fields { get; } = fields;
}