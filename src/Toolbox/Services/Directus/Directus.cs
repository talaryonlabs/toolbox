using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Directus;

public sealed class DirectusOptions : TalaryonOptions<DirectusOptions>
{
    public string? BaseUrl { get; set; }
    public string? AccessToken { get; set; }
}

public class DirectusMetadata
{
    [JsonPropertyName("total_count")] public int TotalCount { get; set; }
    [JsonPropertyName("filter_count")] public int FilterCount { get; set; }
}

public class DirectusResponse<T>
{
    public T? Data { get; set; }
    public DirectusMetadata? Meta { get; set; }
}

public class Directus : IDirectus
{
    private readonly HttpClient _httpClient;
    private readonly string _base;

    public Directus(HttpClient httpClient, IOptions<DirectusOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        _base = optionsAccessor.Value.BaseUrl ?? throw new ArgumentNullException(nameof(optionsAccessor.Value.BaseUrl));
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_base);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {optionsAccessor.Value.AccessToken ?? throw new ArgumentNullException(optionsAccessor.Value.AccessToken)}");
    }

    public async Task<bool> Test()
    {
        try
        {
            return (await _httpClient.GetStringAsync("server/ping")) == "pong";
        }
        catch (Exception e)
        {
            TalaryonLogger.Debug<Directus>($"API offline. {e.Message}"); // Debug, cause it's no error in this case
            return false;
        }
    }

    public IDirectusRequestSingle<T> Single<T>(string name)
    {
        return new Request<T>(name, _httpClient, null);
    }

    public IDirectusRequestSingle<T> Select<T>(string name, string? id)
    {
        return new Request<T>(name, _httpClient, id);
    }

    public IDirectusRequestMany<T> Many<T>(string name) => new Request<T>(name, _httpClient, null);
    public string GetAssetUrl(string? assetId) => $"{_base}assets/{assetId}";
    public string GetAssetUrl(string? assetId, QueryString queryString) => $"{_base}assets/{assetId}{queryString.ToString()}";
    public ITalaryonRunner<IDirectusSchema> Snapshot()
    {
        throw new NotImplementedException();
    }

    public ITalaryonRunner<IDirectusSchema> Diff(IDirectusSchema current)
    {
        throw new NotImplementedException();
    }

    public ITalaryonRunner<bool> ApplySchema(IDirectusSchema diff)
    {
        throw new NotImplementedException();
    }

    private class Request<T>(string name, HttpClient httpClient, string? id) : IDirectusRequestMany<T>,
        IDirectusRequestSingle<T>
    {
        private readonly List<string> _query = new();

        public IDirectusRequestMany<T> Fields(params string[]? fields)
        {
            _query.Add($"fields={string.Join(",", fields)}");
            return this;
        }

        public IDirectusRequestMany<T> Filter(string name, string type, string value)
        {
            _query.Add($"filter[{name}][{type}]={value}");
            return this;
        }

        public IDirectusRequestMany<T> Sort(params string[] fields)
        {
            _query.Add($"sort={string.Join(",", fields)}");
            return this;
        }

        public IDirectusRequestMany<T> Limit(int limit)
        {
            _query.Add($"limit={limit}");
            return this;
        }

        public IDirectusRequestMany<T> Offset(int offset)
        {
            _query.Add($"offset={offset}");
            return this;
        }

        public IDirectusRequestMany<T> IncludeMetadata()
        {
            _query.Add($"meta=*");
            return this;
        }

        async Task<DirectusResponse<T>?> ITalaryonRunner<DirectusResponse<T>?>.RunAsync(CancellationToken cancellationToken)
        {
            var url = $"items/{name}/{id}?{string.Join("&", _query)}";
            try
            {
                TalaryonLogger.Debug<Directus>($"Call {url}");
                return await httpClient.GetFromJsonAsync<DirectusResponse<T>>(url, cancellationToken);
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<Directus>(e.Message);
                return null;
            }
        }

        DirectusResponse<T>? ITalaryonRunner<DirectusResponse<T>?>.Run() =>
            (this as IDirectusRequestSingle<T>)
            .RunAsync(CancellationToken.None)
            .RunSynchronouslyWithResult();

        async Task<DirectusResponse<T[]>?> ITalaryonRunner<DirectusResponse<T[]>?>.RunAsync(CancellationToken cancellationToken)
        {
            var url = $"items/{name}?{string.Join("&", _query)}";
            try
            {
                TalaryonLogger.Debug<Directus>($"Call {url}");
                return await httpClient.GetFromJsonAsync<DirectusResponse<T[]>>(url, cancellationToken);
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<Directus>(e.Message);
                return null;
            };
        }

        DirectusResponse<T[]>? ITalaryonRunner<DirectusResponse<T[]>?>.Run() =>
            (this as IDirectusRequestMany<T>)
            .RunAsync(CancellationToken.None)
            .RunSynchronouslyWithResult();

        IDirectusRequestSingle<T> IDirectusRequestSingle<T>.Fields(params string[]? fields) =>
            (Fields(fields) as IDirectusRequestSingle<T>)!;

        
    }
}