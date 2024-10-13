using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Talaryon.Toolbox.Extensions;
using Talaryon.Toolbox.Services.ArgoCD.Models;
using Talaryon.Toolbox.Services.Hub;

namespace Talaryon.Toolbox.Services.ArgoCD;

public sealed class ArgoCDOptions : TalaryonOptions<ArgoCDOptions>
{
    public string? BaseUrl { get; set; }
    public string? AccessToken { get; set; }
}

public class ArgoCD : IArgoCD
{
    private readonly HttpClient _httpClient;
    private readonly string _base;

    public ArgoCD(HttpClient httpClient, IOptions<ArgoCDOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        _base = optionsAccessor.Value.BaseUrl ?? throw new ArgumentNullException(nameof(optionsAccessor.Value.BaseUrl));
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_base);
        _httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {optionsAccessor.Value.AccessToken ?? throw new ArgumentNullException(optionsAccessor.Value.AccessToken)}");
    }

    public ITalaryonRunner<ArgoCDVersion> GetVersion() => new DefaultRequest<ArgoCDVersion>(_httpClient, "api/version");

    public IArgoResourceProviderSingle<T> Single<T>(string id) => new ArgoProvider<T>(_httpClient, id);

    public IArgoResourceProviderMany<T> Many<T>() => new ArgoProviderMany<T>(_httpClient);
    public IArgoResourceCreateFactory<T> Factory<T>() => new ArgoFactory<T>(_httpClient);

    private class DefaultRequest<T>(HttpClient httpClient, string url, QueryString queryString = default)
        : ITalaryonRunner<T>
    {
        public T? Run() =>
            RunAsync(CancellationToken.None)
                .RunSynchronouslyWithResult();

        public async Task<T?> RunAsync(CancellationToken cancellationToken = default)
        {
            var request = $"{url}{queryString}";
            try
            {
                TalaryonLogger.Debug<ArgoCD>($"GET {request}");
                return await httpClient.GetFromJsonAsync<T>(request, cancellationToken);
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<ArgoCD>(e.Message);
                return default;
            }
        }
    }
}