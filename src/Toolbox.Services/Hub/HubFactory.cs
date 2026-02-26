using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Hub;

public class HubFactory<T, TParams>(HttpClient httpClient, string? id = null) :
        IHubResourceCreateFactory<T, TParams>,
        IHubResourceUpdateFactory<T, TParams>,
        ITalaryonRunner<T>
    where T : IHubResource
    where TParams : HubParams
{
    private TParams _params = null!;
    private bool _update, _create;
    
    public ITalaryonParams<T, TParams> Create()
    {
        _params = Activator.CreateInstance<TParams>();
        _create = true;
        return this;
    }
        
    public ITalaryonParams<T, TParams> Update()
    {
        _params = Activator.CreateInstance<TParams>();
        _update = true;
        return this;
    }
    
    public ITalaryonRunner<bool> Delete(bool force = false) => new DeleteResource(httpClient, id, force);

    public ITalaryonRunner<T> With(Action<TParams> withParams)
    {
        withParams(_params);
        return this;
    }

    public async Task<T?> RunAsync(CancellationToken cancellationToken = default)
    {
        var endpoint =
            ((_create
                 ? HubEndpoint.GetEndpoint<T>(HubEndpointType.Create)
                 : (_update
                     ? HubEndpoint.GetEndpoint<T>(HubEndpointType.Update)
                     : null)) ??
             throw new HubEndpointException())
            .TrimStart('/')
            .Replace(".id", id);
        
        var fields = HubEndpoint.GetFields<T>()
            .Concat(HubEndpoint.GetAdditionalFields<T>())
            .ToList();

        var query = new QueryBuilder { { "fields", string.Join(",", fields) } };
        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<IHubResourceProviderMany<T>>($"[{(_create ? "CREATE" : "UPDATE")}] {url}");
            return await (await httpClient.PostAsJsonAsync(url, _params.ToDictionary(), cancellationToken)).Content
                .ReadFromJsonAsync<T>(cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<IHubResourceProviderMany<T>>(e.Message);
            return default;
        }
    }
    T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T>).RunAsync().RunSynchronouslyWithResult();

    /***
     * Internal class to delete a specific resource
     */
    class DeleteResource(HttpClient httpClient, string? id, bool force) : ITalaryonRunner<bool>
    {
        public bool Force { get; } = force;

        public async Task<bool> RunAsync(CancellationToken cancellationToken = default)
        {
            var endpoint = (HubEndpoint.GetEndpoint<T>(HubEndpointType.Delete) ?? throw new HubEndpointException())
                .TrimStart('/')
                .Replace(".id", id);
            
            try
            {
                TalaryonLogger.Debug<HubFactory<T, TParams>>($"[DELETE] {endpoint}");
                return (await httpClient.DeleteAsync(endpoint, cancellationToken)).StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<HubFactory<T, TParams>>(e.Message);
                return default;
            }
        }
        public bool Run() => RunAsync().RunSynchronouslyWithResult();
    }
}