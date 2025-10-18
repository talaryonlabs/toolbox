namespace Talaryon.Toolbox.API.Client;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ApiEndpointAttribute(string url, ApiEndpointType type = ApiEndpointType.Many, ApiEndpointMethod method = ApiEndpointMethod.Get) : Attribute
{
    public string Url { get; } = url;
    public ApiEndpointType Type { get; } = type;
    public ApiEndpointMethod Method { get; } = method;
}

[Flags]
public enum ApiEndpointType
{
    Many = 1,
    Single = 2,
    Create = 4,
    Update = 8,
    Delete = 16
}

public enum ApiEndpointMethod
{
    Get,
    Post,
    Put,
    Patch,
    Delete
}