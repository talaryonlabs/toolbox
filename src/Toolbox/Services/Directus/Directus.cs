using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
    private static string GetTableName<T>() =>
        (typeof(T).GetCustomAttribute<DirectusTableAttribute>() ??
         throw new Exception($"Missing attribute {nameof(DirectusTableAttribute)}.")).Name;

    private static string?[] GetFields<T>() => DirectusFieldAttribute.GetFields(typeof(T));

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
        return new RequestSingle<T>(_httpClient, name: GetTableName<T>(), fields: GetFields<T>());
    }

    public IDirectusRequestMany<T> Many<T>()
    {
        return new RequestMany<T>(_httpClient, name: GetTableName<T>(), fields: GetFields<T>());
    }

    public string GetAssetUrl(string? assetId) => new Uri(_base).Append($"assets/{assetId}").AbsoluteUri;

    public string GetAssetUrl(string? assetId, QueryString queryString) =>
        new Uri(_base).Append($"assets/{assetId}{queryString.ToString()}").AbsoluteUri;

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