using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Hetzner;

public class HetznerOptions : TalaryonOptions<HetznerOptions>
{
    public string? BaseUrl { get; set; }
    public string? WebUser { get; set; }
    public string? WebPassword { get; set; }
}

public class Hetzner : IHetzner
{
    private class HetznerContainer<T> : Dictionary<string, T>;
    private class HetznerList<T> : List<HetznerContainer<T>>;
    
    private readonly HttpClient _httpClient;

    public Hetzner(HttpClient httpClient, IOptions<HetznerOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        var @base = optionsAccessor.Value.BaseUrl ??
                    throw new ArgumentNullException(nameof(optionsAccessor.Value.BaseUrl));
        var user = optionsAccessor.Value.WebUser ?? throw new ArgumentException(nameof(optionsAccessor.Value.WebUser));
        var password = optionsAccessor.Value.WebPassword ??
                       throw new ArgumentException(nameof(optionsAccessor.Value.WebPassword));

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(@base);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {$"{user}:{password}".ToBase64String()}");
    }
    
    private class Request<T>(HttpClient httpClient, string name, string? id)
        : ITalaryonRunner<T[]?>, ITalaryonRunner<T?>
    {
        T? ITalaryonRunner<T?>.Run() =>
            (this as ITalaryonRunner<T?>)
            .RunAsync(CancellationToken.None)
            .RunSynchronouslyWithResult();

        async Task<T?> ITalaryonRunner<T?>.RunAsync(CancellationToken cancellationToken)
        {
            var url = $"{name}/{id}";
            try
            {
                TalaryonLogger.Debug<Hetzner>($"Call {url}");
                var response = await httpClient.GetFromJsonAsync<HetznerContainer<T>>(url, cancellationToken);
                if (response is not null)
                {
                    return response.First().Value;
                }
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<Hetzner>(e.Message);
            }
            return default;
        }

        T[]? ITalaryonRunner<T[]?>.Run() =>
            (this as ITalaryonRunner<T[]?>)
            .RunAsync(CancellationToken.None)
            .RunSynchronouslyWithResult();

        async Task<T[]?> ITalaryonRunner<T[]?>.RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                TalaryonLogger.Debug<Hetzner>($"Call {name}");
                var response = await httpClient.GetFromJsonAsync<HetznerList<T>>(name, cancellationToken);
                if (response is not null)
                {
                    return response.Select(v => v.First().Value).ToArray();
                }
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<Hetzner>(e.Message);
            }
            return default;
        }
    }

    public ITalaryonRunner<T[]?> Many<T>() where T : IHetznerObject =>
        new Request<T>(_httpClient, Activator.CreateInstance<T>().Name, null);

    public ITalaryonRunner<T?> Single<T>(string id) where T : IHetznerObject =>
        new Request<T>(_httpClient, Activator.CreateInstance<T>().Name, id);
}