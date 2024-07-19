using Microsoft.Net.Http.Headers;

namespace Talaryon.Toolbox.API;

public class ApiMediaType : MediaTypeHeaderValue
{
    public ApiMediaType() : base("application/json") { }
}