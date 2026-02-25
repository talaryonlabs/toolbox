using Microsoft.Net.Http.Headers;

namespace Talaryon.Toolbox.Hosting.Api;

public class ApiMediaType : MediaTypeHeaderValue
{
    public ApiMediaType() : base("application/json") { }
}