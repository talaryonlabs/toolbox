using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Hub;

public class HubProvider<T>(HttpClient httpClient, string? id = null) : IHubResourceProviderSingle<T>, IHubResourceProviderMany<T> where T : IHubResource
{
    private int _skip = -1, _limit = -1;
    private List<string>? _fields;
    private string? _queryString;
    
    public IHubResourceUpdateFactory<T, TParams> GetFactory<TParams>() where TParams : HubParams => new HubFactory<T, TParams>(httpClient, id);
    public ITalaryonRunner<bool> Exists() => new RessourceExists(httpClient, id ?? throw new ArgumentNullException());
    public ITalaryonRunner<int> Count() => new RessourceCount(httpClient);
    
    IHubResourceProviderSingle<T> IHubResourceProviderSingle<T>.Fields(params string[] fields)
    {
        _fields = new List<string>(fields);
        return this;
    }

    public IHubResourceProviderMany<T> Query(string queryString)
    {
        _queryString = queryString;
        return this;
    }

    IHubResourceProviderMany<T> IHubResourceProviderMany<T>.Fields(params string[] fields)
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
        var endpoint = (HubEndpoint.GetEndpoint<T>() ?? throw new HubEndpointException())
            .TrimStart('/');
        var fields = HubEndpoint.GetFields<T>()
            .Concat(HubEndpoint.GetAdditionalFields<T>())
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
            var responseType = HubEndpoint.GetResponseType<T>();
            
            TalaryonLogger.Debug<HubProvider<T>>($"{url}");
            var obj = (IHubResponseType)await httpClient.GetFromJsonAsync(url, responseType, cancellationToken);


            return (IEnumerable<T>?)obj?.List;
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<HubProvider<T>>(e.Message);
            return default;
        }
    }
    IEnumerable<T>? ITalaryonRunner<IEnumerable<T>>.Run() => (this as ITalaryonRunner<IEnumerable<T>?>).RunAsync().RunSynchronouslyWithResult();

    /**
     * IYouTrackResourceProviderSingle
     */
    async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
    {
        var endpoint = (HubEndpoint.GetEndpoint<T>(HubEndpointType.Get) ?? throw new HubEndpointException())
            .Replace(".id", id)
            .TrimStart('/');
        var fields = HubEndpoint.GetFields<T>()
            .Concat(HubEndpoint.GetAdditionalFields<T>())
            .ToList();
        var query = new QueryBuilder();

        if (fields.Count > 0)
            query.Add("fields", string.Join(",", fields));

        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<HubProvider<T>>($"{url}");
            return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<HubProvider<T>>(e.Message);
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
            var endpoint = (HubEndpoint.GetEndpoint<T>(HubEndpointType.Get) ?? throw new HubEndpointException())
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
            var endpoint = (HubEndpoint.GetEndpoint<T>() ?? throw new HubEndpointException())
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