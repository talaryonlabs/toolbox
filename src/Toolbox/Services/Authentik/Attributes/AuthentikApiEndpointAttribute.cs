using System.Reflection;

public static class AuthentikApiEndpoint
{
    public static string? GetEndpoint<T>(AuthentikApiEndpointType type = AuthentikApiEndpointType.List) => typeof(T)
        .GetCustomAttributes<AuthentikApiEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type.HasFlag(type))
        ?.Url;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AuthentikApiEndpointAttribute(string? url, AuthentikApiEndpointType type = AuthentikApiEndpointType.List) : Attribute
{
    public string? Url { get; set; } = url;
    public AuthentikApiEndpointType Type { get; set; } = type;
}

[Flags]
public enum AuthentikApiEndpointType
{
    List = 1,
    Get = 2,
    Create = 4,
    Update = 8,
    Delete = 16
}

public class AuthentikApiEndpointException<T>() : Exception($"AuthentikApiEndpoint for {typeof(T).Name} not found.");