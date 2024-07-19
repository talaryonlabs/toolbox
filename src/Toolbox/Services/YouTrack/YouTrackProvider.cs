using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.YouTrack;

public class YouTrackProvider<T>(HttpClient httpClient, string? id = null) : IYouTrackResourceProviderSingle<T>, IYouTrackResourceProviderMany<T> where T : IYouTrackResource
{
    private int _skip = -1, _limit = -1;
    private List<string>? _fields;
    private string? _queryString;
    
    public IYouTrackResourceUpdateFactory<T, TParams> GetFactory<TParams>() where TParams : YouTrackParams => new YouTrackFactory<T, TParams>(httpClient, id);
    public ITalaryonRunner<bool> Exists() => new RessourceExists(httpClient, id ?? throw new ArgumentNullException());
    public ITalaryonRunner<int> Count() => new RessourceCount(httpClient);
    
    IYouTrackResourceProviderSingle<T> IYouTrackResourceProviderSingle<T>.Fields(params string[] fields)
    {
        _fields = new List<string>(fields);
        return this;
    }

    public IYouTrackResourceProviderMany<T> Query(string queryString)
    {
        _queryString = queryString;
        return this;
    }

    IYouTrackResourceProviderMany<T> IYouTrackResourceProviderMany<T>.Fields(params string[] fields)
    {
        _fields = new List<string>(fields);
        return this;
    }
    
    public ITalaryonEnumerable<T> Take(int count)
    {
        _limit = count;
        return this;
    }

    public ITalaryonEnumerable<T> Skip(int count)
    {
        _skip = count;
        return this;
    }

    public ITalaryonEnumerable<T> SkipUntil(string cursor)
    {
        throw new NotImplementedException();
    }
    
    /**
     * IYouTrackResourceProviderMany
     */
    async Task<IEnumerable<T>?> ITalaryonRunner<IEnumerable<T>>.RunAsync(CancellationToken cancellationToken)
    {
        var endpoint = (YouTrackEndpoint.GetEndpoint<T>() ?? throw new YouTrackEndpointException())
            .TrimStart('/');
        var fields = YouTrackEndpoint.GetFields<T>()
            .Concat(YouTrackEndpoint.GetAdditionalFields<T>())
            .ToList();
        var query = new QueryBuilder();
            
        if (_skip > 0)
            query.Add("$skip", _skip.ToString());

        if (_limit > 0)
            query.Add("$limit", _limit.ToString());

        if (_fields is { Count: > 0 })
        {
            fields = fields.Where(v => _fields.Contains(v)).ToList();
        }
        if (fields.Count > 0)
            query.Add("fields", string.Join(",", fields));
        
        if(_queryString is { Length: > 0})
            query.Add("query", _queryString);

        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<YouTrackProvider<T>>($"{url}");
            return await httpClient.GetFromJsonAsync<List<T>>(url, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<YouTrackProvider<T>>(e.Message);
            return default;
        }
    }
    IEnumerable<T>? ITalaryonRunner<IEnumerable<T>>.Run() => (this as ITalaryonRunner<IEnumerable<T>?>).RunAsync().RunSynchronouslyWithResult();

    /**
     * IYouTrackResourceProviderSingle
     */
    async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
    {
        var endpoint = (YouTrackEndpoint.GetEndpoint<T>(YouTrackEndpointType.Get) ?? throw new YouTrackEndpointException())
            .Replace(".id", id)
            .TrimStart('/');
        var fields = YouTrackEndpoint.GetFields<T>()
            .Concat(YouTrackEndpoint.GetAdditionalFields<T>())
            .ToList();
        var query = new QueryBuilder();

        if (fields.Count > 0)
            query.Add("fields", string.Join(",", fields));

        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<YouTrackProvider<T>>($"{url}");
            return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<YouTrackProvider<T>>(e.Message);
            return default;
        }
    }
    T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T>).RunAsync().RunSynchronouslyWithResult();
  
    /***
     * Internal class to check if a resource exists
     */
    class RessourceExists(HttpClient httpClient, string id) : ITalaryonRunner<bool>
    {
        async Task<bool> ITalaryonRunner<bool>.RunAsync(CancellationToken cancellationToken)
        {
            var endpoint = (YouTrackEndpoint.GetEndpoint<T>(YouTrackEndpointType.Get) ?? throw new YouTrackEndpointException())
                .Replace(".id", id);
            var url = $"{endpoint.TrimStart('/')}";
            try
            {
                TalaryonLogger.Debug<RessourceExists>($"{url}");
                return (await httpClient.GetFromJsonAsync<T>(url, cancellationToken)) != null;
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<RessourceExists>(e.Message);
                return default;
            }
        }

        bool ITalaryonRunner<bool>.Run() => (this as ITalaryonRunner<bool>).RunAsync().RunSynchronouslyWithResult();
    }
    
    /***
     * Internal class to count total resources
     */
    class RessourceCount(HttpClient httpClient) : ITalaryonRunner<int>
    {
        async Task<int> ITalaryonRunner<int>.RunAsync(CancellationToken cancellationToken)
        {
            var endpoint = (YouTrackEndpoint.GetEndpoint<T>() ?? throw new YouTrackEndpointException())
                .TrimStart('/');
            try
            {
                TalaryonLogger.Debug<RessourceCount>($"{endpoint}");
                return (await httpClient.GetFromJsonAsync<List<T>>(endpoint, cancellationToken) ?? new List<T>()).Count;
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<RessourceCount>(e.Message);
                return default;
            }
        }

        int ITalaryonRunner<int>.Run() => (this as ITalaryonRunner<int>).RunAsync().RunSynchronouslyWithResult();
    }
}