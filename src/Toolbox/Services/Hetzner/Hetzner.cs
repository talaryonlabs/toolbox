using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
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
                return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<Hetzner>(e.Message);
                return default;
            }
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
                return await httpClient.GetFromJsonAsync<T[]>(name, cancellationToken);
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<Hetzner>(e.Message);
                return default;
            }
        }
    }

    public ITalaryonRunner<T[]?> Many<T>() where T : IHetznerObject =>
        new Request<T>(_httpClient, Activator.CreateInstance<T>().Name, null);

    public ITalaryonRunner<T?> Single<T>(string id) where T : IHetznerObject =>
        new Request<T>(_httpClient, Activator.CreateInstance<T>().Name, id);

    public T Set<T>()
    {
        throw new NotImplementedException();
    }
}