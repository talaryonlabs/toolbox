using Microsoft.Extensions.Options;
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

    public IAuthentikRequestSingle<T> Single<T>() where T : IAuthentikRessource
    {
        return new AuthentikRequestSingle<T>(_httpClient, _base);
    }

    public IAuthentikRequestSingle<T> Single<T>(string id) where T : IAuthentikRessource
    {
        return new AuthentikRequestSingleId<T>(_httpClient, _base, id);
    }

    public IAuthentikRequestMany<T> Many<T>() where T : IAuthentikRessource
    {
        return new AuthentikRequestMany<T>(_httpClient, _base);
    }
}