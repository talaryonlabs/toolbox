using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace Talaryon.Toolbox.Services.YouTrack;

public sealed class YouTrackOptions : TalaryonOptions<YouTrackOptions>
{
    public string? BaseUrl { get; set; }
    public string? AccessToken { get; set; }
}

public class YouTrack : IYouTrack
{
    private readonly HttpClient _httpClient;

    public YouTrack(HttpClient httpClient, IOptions<YouTrackOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(optionsAccessor.Value.BaseUrl ?? throw new ArgumentNullException(nameof(optionsAccessor.Value.BaseUrl)));
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {optionsAccessor.Value.AccessToken ?? throw new ArgumentNullException(optionsAccessor.Value.AccessToken)}");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public IYouTrackResourceProviderSingle<T> Single<T>(string id) where T : IYouTrackResource => new YouTrackProvider<T>(_httpClient, id);
    public IYouTrackResourceProviderMany<T> Many<T>() where T : IYouTrackResource => new YouTrackProvider<T>(_httpClient);
    public IYouTrackResourceCreateFactory<T, TParams> Factory<T, TParams>() where T : IYouTrackResource where TParams : YouTrackParams => new YouTrackFactory<T, TParams>(_httpClient);
}