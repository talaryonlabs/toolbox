using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.YouTrack;

public class YouTrackFactory<T, TParams>(HttpClient httpClient, string? id = null) :
        IYouTrackResourceCreateFactory<T, TParams>,
        IYouTrackResourceUpdateFactory<T, TParams>,
        ITalaryonRunner<T>
    where T : IYouTrackResource
    where TParams : YouTrackParams
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
                 ? YouTrackEndpoint.GetEndpoint<T>(YouTrackEndpointType.Create)
                 : (_update
                     ? YouTrackEndpoint.GetEndpoint<T>(YouTrackEndpointType.Update)
                     : null)) ??
             throw new YouTrackEndpointException())
            .TrimStart('/')
            .Replace(".id", id);
        
        var fields = YouTrackEndpoint.GetFields<T>()
            .Concat(YouTrackEndpoint.GetAdditionalFields<T>())
            .ToList();

        var query = new QueryBuilder { { "fields", string.Join(",", fields) } };
        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<IYouTrackResourceProviderMany<T>>($"[{(_create ? "CREATE" : "UPDATE")}] {url}");
            return await (await httpClient.PostAsJsonAsync(url, _params.ToDictionary(), cancellationToken)).Content
                .ReadFromJsonAsync<T>(cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<IYouTrackResourceProviderMany<T>>(e.Message);
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
            var endpoint = (YouTrackEndpoint.GetEndpoint<T>(YouTrackEndpointType.Delete) ?? throw new YouTrackEndpointException())
                .TrimStart('/')
                .Replace(".id", id);
            
            try
            {
                TalaryonLogger.Debug<YouTrackFactory<T, TParams>>($"[DELETE] {endpoint}");
                return (await httpClient.DeleteAsync(endpoint, cancellationToken)).StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<YouTrackFactory<T, TParams>>(e.Message);
                return default;
            }
        }
        public bool Run() => RunAsync().RunSynchronouslyWithResult();
    }
}