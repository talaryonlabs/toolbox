using System.Reflection;

namespace Talaryon.Toolbox.API.Client;

public static class ApiEndpoint
{
    public static string? GetEndpoint<T>(ApiEndpointType type = ApiEndpointType.Many) => typeof(T)
        .GetCustomAttributes<ApiEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type.HasFlag(type))
        ?.Url;
}