using Microsoft.Net.Http.Headers;

namespace TalaryonLabs.Toolbox.API;

public class ApiMediaType : MediaTypeHeaderValue
{
    public ApiMediaType() : base("application/json") { }
}