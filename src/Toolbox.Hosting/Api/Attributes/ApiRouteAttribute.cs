using Microsoft.AspNetCore.Mvc;

namespace Talaryon.Toolbox.Hosting.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ApiRouteAttribute : RouteAttribute
{
    public ApiRouteAttribute(string template)
        : base("v{version:apiVersion}" + (template.StartsWith("/") ? "" : "/") + template)
    {
    }
}