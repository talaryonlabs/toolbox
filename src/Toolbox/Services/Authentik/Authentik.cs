using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Talaryon.Toolbox.Services.Authentik.Models;

namespace Talaryon.Toolbox.Services.Authentik;

public sealed class AuthentikOptions : TalaryonOptions<AuthentikOptions>
{
    public string? BaseUrl { get; set; }
    public string? AccessToken { get; set; }
}

public class Authentik : IAuthentik
{
    private readonly HttpClient _httpClient;
    private readonly string _base;
    
    public Authentik(HttpClient httpClient, IOptions<AuthentikOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        _base = optionsAccessor.Value.BaseUrl ?? throw new ArgumentNullException(nameof(optionsAccessor.Value.BaseUrl));
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_base);
        _httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {optionsAccessor.Value.AccessToken ?? throw new ArgumentNullException(optionsAccessor.Value.AccessToken)}");
    }


    public ITalaryonRunner Test()
    {
        return new AuthentikTester(_httpClient);
    }
}

public class AuthentikResult<T>
{
    [JsonProperty("results")] public T[] Result { get; set; } = [];
    [JsonProperty("pagination")] public object Pagination { get; set; }
}

public class AuthentikTester(HttpClient httpClient) : ITalaryonRunner
{
    public void Run() => RunAsync().RunSynchronously();

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<AuthentikResult<AuthentikApplication>>("/core/applications/", cancellationToken);
    }
}