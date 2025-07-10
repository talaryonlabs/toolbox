using System.Reflection;
using System.Text.Json.Serialization;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.ArgoCD;

public static class ArgoContainer
{
    public static string? GetName<T>(ArgoEndpointType type = ArgoEndpointType.List) => typeof(T)
        .GetCustomAttributes<ArgoContainerAttribute>(true)
        .FirstOrDefault()
        ?.Name;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ArgoContainerAttribute(string name) : Attribute
{
    public string? Name { get; set; } = name;
}

public class ArgoContainerException : Exception
{
    public ArgoContainerException()
        : base("ArgoContainer not found.")
    {
    }
}