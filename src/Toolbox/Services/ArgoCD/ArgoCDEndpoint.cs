using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.ArgoCD;

public static class ArgoEndpoint
{
    public static string? GetEndpoint<T>(ArgoEndpointType type = ArgoEndpointType.List) => typeof(T)
        .GetCustomAttributes<ArgoEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type.HasFlag(type))
        ?.Url;

    public static Type? GetResponseType<T>(ArgoEndpointType type = ArgoEndpointType.List) => typeof(T)
        .GetCustomAttributes<ArgoEndpointAttribute>(true)
        .FirstOrDefault(p => p.Type.HasFlag(type))
        ?.ResponseType;
    
    public static List<string> GetFields<T>() => typeof(T)
        .GetProperties()
        .Where(p => p.HasCustomAttribute<JsonPropertyNameAttribute>())
        .Select(p => p.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name)
        .ToList()!;
    
    public static List<string> GetAdditionalFields<T>() => (typeof(T)
        .GetCustomAttributes<ArgoAdditionalFieldsAttribute>(true)
        .FirstOrDefault()
        ?.Fields ?? Array.Empty<string>())
        .ToList();
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ArgoEndpointAttribute(string? url, ArgoEndpointType type = ArgoEndpointType.List, Type? responseType = default) : Attribute
{
    public string? Url { get; set; } = url;
    public ArgoEndpointType Type { get; set; } = type;
    public Type? ResponseType { get; set; } = responseType;
    public string? Name { get; set; }
}

[Flags]
public enum ArgoEndpointType
{
    List = 1,
    Get = 2,
    Create = 4,
    Update = 8,
    Delete = 16
}

public class ArgoEndpointException : Exception
{
    public ArgoEndpointException()
        : base("ArgoEndpoint not found.")
    {
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ArgoAdditionalFieldsAttribute(params string[]? fields) : Attribute
{
    public string[]? Fields { get; } = fields;
}