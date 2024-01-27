using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace TalaryonLabs.Toolbox.Services.Hub;

public sealed class HubOptions : TalaryonOptions<HubOptions>
{
    public string? BaseUrl { get; set; }
    public string? AccessToken { get; set; }
}

public class Hub : IHub
{
    private readonly HttpClient _httpClient;

    public Hub(HttpClient httpClient, IOptions<HubOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(optionsAccessor.Value.BaseUrl ?? throw new ArgumentNullException(nameof(optionsAccessor.Value.BaseUrl)));
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {optionsAccessor.Value.AccessToken ?? throw new ArgumentNullException(optionsAccessor.Value.AccessToken)}");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public IHubResourceProviderSingle<T> Single<T>(string id) where T : IHubResource => new HubProvider<T>(_httpClient, id);
    public IHubResourceProviderMany<T> Many<T>() where T : IHubResource => new HubProvider<T>(_httpClient);
    public IHubResourceCreateFactory<T, TParams> Factory<T, TParams>() where T : IHubResource where TParams : HubParams => new HubFactory<T, TParams>(_httpClient);
}