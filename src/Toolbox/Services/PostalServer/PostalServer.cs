namespace Talaryon.Toolbox.Services.PostalServer;

public sealed class PostalServerOptions : TalaryonOptions<PostalServerOptions>
{
    public string? BaseUrl { get; set; }
    public string? ApiKey { get; set; }
}

public class PostalServer : IPostalServer
{
    // https://docs.postalserver.io/developer/api
    // waiting for v2 api before implementing
}