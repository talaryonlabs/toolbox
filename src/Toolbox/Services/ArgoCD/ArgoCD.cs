using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Talaryon.Toolbox.Extensions;
using Talaryon.Toolbox.Services.ArgoCD.Models;

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

    public ITalaryonRunner<V1alpha1AppProjectList> GetProjects(string? name = default)
    {
        var queryBuilder = new QueryBuilder();
        if (name is not null) queryBuilder.Add("name", name);
        return new DefaultRequest<V1alpha1AppProjectList>(_httpClient, "api/v1/projects", queryBuilder.ToQueryString());
    }

    private class DefaultRequest<T>(HttpClient httpClient, string url, QueryString queryString = default)
        : ITalaryonRunner<T>
    {
        public T? Run() =>
            RunAsync(CancellationToken.None)
                .RunSynchronouslyWithResult();

        public async Task<T?> RunAsync(CancellationToken cancellationToken = default)
        {
            var request = $"{url}?{queryString}";
            try
            {
                TalaryonLogger.Debug<ArgoCD>($"Call {request}");
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