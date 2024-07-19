using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Hub;

public static class HubEndpoint
{
    public static string? GetEndpoint<T>(HubEndpointType type = HubEndpointType.List) => typeof(T)
        .GetCustomAttributes<HubEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type.HasFlag(type))
        ?.Url;

    public static Type? GetResponseType<T>(HubEndpointType type = HubEndpointType.List) => typeof(T)
        .GetCustomAttributes<HubEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type.HasFlag(type))
        ?.ResponseType;
    
    public static List<string> GetFields<T>() => typeof(T)
        .GetProperties()
        .Where(p => p.HasCustomAttribute<JsonPropertyNameAttribute>())
        .Select(p => p.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name)
        .ToList()!;
    
    public static List<string> GetAdditionalFields<T>() => (typeof(T)
        .GetCustomAttributes<HubAdditionalFieldsAttribute>(true)
        .FirstOrDefault()
        ?.Fields ?? Array.Empty<string>())
        .ToList();
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HubEndpointAttribute(string? url, HubEndpointType type = HubEndpointType.List, Type? responseType = default) : Attribute
{
    public string? Url { get; set; } = url;
    public HubEndpointType Type { get; set; } = type;
    public Type? ResponseType { get; set; } = responseType;
}

[Flags]
public enum HubEndpointType
{
    List = 1,
    Get = 2,
    Create = 4,
    Update = 8,
    Delete = 16
}

public class HubEndpointException : Exception
{
    public HubEndpointException()
        : base("HubEndpoint not found.")
    {
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HubAdditionalFieldsAttribute(params string[]? fields) : Attribute
{
    public string[]? Fields { get; } = fields;
}