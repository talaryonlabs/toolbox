using System.Net.Http.Json;
using Talaryon.Toolbox.API.Client;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Authentik;

/*public sealed class AuthentikRequestSingleId<T>(HttpClient httpClient, string baseUri, string id) : IAuthentikRequestSingle<T> where T : IAuthentikRessource
{
    public T? Run() => RunAsync().RunSynchronouslyWithResult();

    public async Task<T?> RunAsync(CancellationToken cancellationToken = default)
    {
        var uri = AuthentikApiEndpoint.GetEndpoint<T>(AuthentikApiEndpointType.Get) ?? throw new AuthentikApiEndpointException<T>();
        var requestUri = new Uri(baseUri)
            .Append(uri.Replace(".id", id));

        try
        {
            TalaryonLogger.Debug<AuthentikRequestSingleId<T>>($"{requestUri}");
            return await httpClient.GetFromJsonAsync<T>(requestUri, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<Authentik>(e.Message);
            return default;
        }
    }
}*/

public sealed class AuthentikRequestSingle<T>(HttpClient httpClient, string baseUri, string? id = null) : IApiResourceProviderSingle<T>, ITalaryonParams<T, ApiRequestParams>
    where T : IApiResource
{
    private readonly ApiRequestParams _params = new();
    private readonly ApiRequest<T> _request = new(httpClient, baseUri);

    public Task<T?> RunAsync(CancellationToken cancellationToken = default)
    {
        _request.WithType(ApiEndpointType.Single);

        if (id is not null)
        {
            _request.WithParam(".id", id);
        }
        foreach (var p in _params.ToDictionary())
        {
            _request.WithQueryParam(p.Key, p.Value);
        }
        return _request.RunAsync(cancellationToken);
    }

    public IApiResourceUpdateFactory<T, TParams> GetFactory<TParams>() where TParams : ApiRequestParams
    {
        throw new NotImplementedException();
    }

    public ITalaryonRunner<T> With(Action<ApiRequestParams> withParams)
    {
        withParams(_params);
        return this;
    }

    public ITalaryonRunner<bool> Exists() => new ResourceExists(this);

    private class ResourceExists(ITalaryonRunner<T> runner) : ITalaryonRunner<bool>
    {
        public async Task<bool> RunAsync(CancellationToken cancellationToken = default)
        {
            var response = await runner.RunAsync(cancellationToken);
            return response is not null;
        }
    }
}

public sealed class AuthentikRequestMany<T>(HttpClient httpClient, string baseUri) : IApiResourceProviderMany<T, AuthentikList>
    where T : IApiResource
{
    private readonly ApiRequestParams _params = new();
    private readonly ApiRequest<T, AuthentikList> _request = new(httpClient, baseUri);
    
    public Task<AuthentikList?> RunAsync(CancellationToken cancellationToken = default)
    {
        _request.WithType(ApiEndpointType.Many);
        
        foreach (var p in _params.ToDictionary())
        {
            _request.WithQueryParam(p.Key, p.Value);
        }
        return _request.RunAsync(cancellationToken);
    }
    
    public ITalaryonRunner<AuthentikList> With(Action<ApiRequestParams> withParams)
    {
        withParams(_params);
        return this;
    }

    public ITalaryonRunner<int> Count() => new ResourceCount(this);

    private class ResourceCount(ITalaryonRunner<AuthentikList> runner) : ITalaryonRunner<int>
    {
        public async Task<int> RunAsync(CancellationToken cancellationToken = default)
        {
            var list = await runner.RunAsync(cancellationToken);
            return list is {Pagination.Count: > 0 } ? list.Pagination.Count : 0;
        }
    }
    
}