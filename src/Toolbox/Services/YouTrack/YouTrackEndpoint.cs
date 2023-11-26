using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

public static class YouTrackEndpoint<T>
{
    public static string? GetEndpoint(YouTrackApiEndpointType type = YouTrackApiEndpointType.List) => typeof(T)
        .GetCustomAttributes<YouTrackApiEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type == type)
        ?.Url;

    public static string GetFields() => string.Join(",", typeof(T)
        .GetProperties()
        .Where(p => p.HasCustomAttribute<JsonPropertyAttribute>())
        .Select(p => p.GetCustomAttribute<JsonPropertyAttribute>(true)?.PropertyName)
        .ToList());
}