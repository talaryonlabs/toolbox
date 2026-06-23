namespace Talaryon.Toolbox.Hosting.Api;

public class ApiHostingOptions
{
    public int RateLimit { get; set; } = 100;
    public int QueueLimit { get; set; } = 2;
    public long MaxRequestBodySize { get; set; } = 1024 * 1024;
    public ApiMediaType MediaType { get; set; } = new();
    public List<string> AccessTokens { get; set; } = new();
    public bool IsTokenAuthenticationEnabled { get; set; } = true;   
}