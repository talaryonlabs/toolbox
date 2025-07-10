using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Talaryon.Toolbox.Extensions;
using Talaryon.Toolbox.Services.Hub;

namespace Talaryon.Toolbox.Services.ArgoCD;

public class ArgoProvider<T>(HttpClient httpClient, string? id = null) : IArgoResourceProviderSingle<T>
{
    private QueryString? _queryString;
    
    public IArgoResourceUpdateFactory<T> GetFactory() => new ArgoFactory<T>(httpClient, id);

    public ITalaryonRunner<bool> Exists() => new RessourceExists(httpClient, id ?? throw new ArgumentNullException());

    /**
     * IYouTrackResourceProviderSingle
     */
    async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
    {
        var endpoint = (ArgoEndpoint.GetEndpoint<T>(ArgoEndpointType.Get) ?? throw new ArgoEndpointException())
            .Replace(".id", id)
            .TrimStart('/');
        var fields = ArgoEndpoint.GetFields<T>()
            .Concat(ArgoEndpoint.GetAdditionalFields<T>())
            .ToList();
        var query = new QueryBuilder();

        if (fields.Count > 0)
            query.Add("fields", string.Join(",", fields));

        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<ArgoProvider<T>>($"{url}");
            return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<ArgoProvider<T>>(e.Message);
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
            var endpoint = (ArgoEndpoint.GetEndpoint<T>(ArgoEndpointType.Get) ?? throw new ArgoEndpointException())
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
}

public class ArgoProviderMany<T>(HttpClient httpClient) : IArgoResourceProviderMany<T>
{
    private QueryString? _queryString;
    
    public IArgoResourceProviderMany<T> Query(QueryString queryString)
    {
        _queryString = queryString;
        return this;
    }
    
    /**
     * IYouTrackResourceProviderMany
     */
    async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
    {
        var endpoint = (ArgoEndpoint.GetEndpoint<T>() ?? throw new ArgoEndpointException())
            .TrimStart('/');
        var fields = ArgoEndpoint.GetFields<T>()
            .Concat(ArgoEndpoint.GetAdditionalFields<T>())
            .ToList();
        var query = new QueryBuilder();

        // if (_fields is { Count: > 0 })
        // {
        //     fields = fields.Where(v => _fields.Contains(v)).ToList();
        // }
        // if (fields.Count > 0)
        //     query.Add("fields", string.Join(",", fields));
        //
        // if(_queryString is { Length: > 0})
        //     query.Add("query", _queryString);

        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            var responseType = ArgoEndpoint.GetResponseType<T>();
            
            TalaryonLogger.Debug<ArgoProviderMany<T>>($"{url}");
            return (T)await httpClient.GetFromJsonAsync(url, responseType, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<ArgoProviderMany<T>>(e.Message);
            return default;
        }
    }
    T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T?>).RunAsync().RunSynchronouslyWithResult();
}