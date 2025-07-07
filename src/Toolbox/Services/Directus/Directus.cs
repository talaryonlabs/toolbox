using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        _httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {optionsAccessor.Value.AccessToken ?? throw new ArgumentNullException(optionsAccessor.Value.AccessToken)}");
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

    public IDirectusRequestSingle<T> Single<T>()
    {
        var attr = typeof(T).GetCustomAttribute<DirectusTableAttribute>() ??
                   throw new Exception($"Missing attribute {nameof(DirectusTableAttribute)}.");

        var fields = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(v => v.GetCustomAttribute<DirectusFieldAttribute>()?.Name)
            .Concat(attr.AdditionalFields ?? [])
            .ToList();

        return new RequestSingle<T>(_httpClient, name: attr.Name, fields: fields.ToArray());
    }

    public IDirectusRequestMany<T> Many<T>()
    {
        var attr = typeof(T).GetCustomAttribute<DirectusTableAttribute>() ??
                   throw new Exception($"Missing attribute {nameof(DirectusTableAttribute)}.");

        var fields = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(v => v.GetCustomAttribute<DirectusFieldAttribute>()?.Name)
            .Concat(attr.AdditionalFields ?? [])
            .ToList();

        return new RequestMany<T>(_httpClient, name: attr.Name, fields: fields.ToArray());
    }

    public string? GetAssetUrl(string? assetId) => $"{_base}assets/{assetId}";

    public string GetAssetUrl(string? assetId, QueryString queryString) =>
        $"{_base}assets/{assetId}{queryString.ToString()}";

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

    private class RequestSingle<T>(HttpClient httpClient, string name, string?[] fields) : IDirectusRequestSingle<T>
    {
        private readonly List<string> _query = new();

        async Task<DirectusResponse<T>?> ITalaryonRunner<DirectusResponse<T>?>.RunAsync(
            CancellationToken cancellationToken)
        {
            _query.Add($"fields={string.Join(",", fields)}");

            var url = $"items/{name}?{string.Join("&", _query)}";
            TalaryonLogger.Debug<Directus>($"Call {url}");
            try
            {
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
    }


    private class RequestMany<T>(HttpClient httpClient, string name, string?[] fields) : IDirectusRequestMany<T>
    {
        private readonly Dictionary<string, string> _query = new();

        public IDirectusRequestMany<T> Filter(string field, string type, string value) =>
            !_query.TryAdd($"filter[{field}][{type}]", value)
                ? throw new Exception($"Filter {field},{type} already exists.")
                : this;

        public IDirectusRequestMany<T> Search(string pattern)
        {
            _query.Add("search", pattern);
            return this;
        }

        public IDirectusRequestMany<T> Sort(params string[] sortingFields)
        {
            _query.Add("sort", string.Join(",", sortingFields));
            return this;
        }

        public IDirectusRequestMany<T> Limit(int limit)
        {
            _query.Add("limit", $"{limit}");
            return this;
        }

        public IDirectusRequestMany<T> Offset(int offset)
        {
            _query.Add("offset", $"{offset}");
            return this;
        }

        public IDirectusRequestMany<T> IncludeMetadata()
        {
            _query.Add("meta", "*");
            return this;
        }

        async Task<DirectusResponse<T[]>?> ITalaryonRunner<DirectusResponse<T[]>?>.RunAsync(
            CancellationToken cancellationToken)
        {
            _query.Add("fields", string.Join(",", fields));
            var url = $"items/{name}?{string.Join("&", _query.Select(v => v.Key + "=" + v.Value).ToArray())}";
            TalaryonLogger.Debug<Directus>($"Call {url}");
            try
            {
                return await httpClient.GetFromJsonAsync<DirectusResponse<T[]>>(url, cancellationToken);
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<Directus>(e.Message);
                return null;
            }
        }

        DirectusResponse<T[]>? ITalaryonRunner<DirectusResponse<T[]>?>.Run() =>
            (this as IDirectusRequestMany<T>)
            .RunAsync(CancellationToken.None)
            .RunSynchronouslyWithResult();
    }
}