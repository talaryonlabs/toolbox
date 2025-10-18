using Microsoft.Extensions.Options;
using Talaryon.Toolbox.API.Client;
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
        _httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {optionsAccessor.Value.AccessToken ?? throw new ArgumentNullException(optionsAccessor.Value.AccessToken)}");
    }
    
    public IApiResourceProviderSingle<T> Single<T>() where T : IApiResource
    {
        return new AuthentikRequestSingle<T>(_httpClient, _base);
    }

    public IApiResourceProviderSingle<T> Single<T>(string id) where T : IApiResource
    {
        return new AuthentikRequestSingle<T>(_httpClient, _base, id);
    }

    public IApiResourceProviderMany<TResource, AuthentikList> Many<TResource>() where TResource : IApiResource
    {
        return new AuthentikRequestMany<TResource>(_httpClient, _base);
    }

    public IApiResourceCreateFactory<T, TParams> Factory<T, TParams>() where T : IApiResource where TParams : ApiRequestParams
    {
        throw new NotImplementedException();
    }

    public ITalaryonRunner<bool> TestConnection()
    {
        throw new NotImplementedException();
    }
}