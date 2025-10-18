using Microsoft.Net.Http.Headers;

namespace Talaryon.Toolbox.API.Hosting;

public class ApiMediaType : MediaTypeHeaderValue
{
    public ApiMediaType() : base("application/json") { }
}