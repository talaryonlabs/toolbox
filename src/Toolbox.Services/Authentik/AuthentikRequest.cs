using Talaryon.Toolbox.Api;

namespace Talaryon.Toolbox.Services.Authentik;

public sealed class AuthentikRequestSingle<T>(HttpClient httpClient, string baseUri, string? id = null) : IApiResourceProviderSingle<T>, ITalaryonParams<ApiResponse<T>, ApiRequestParams>
    where T : IApiResource
{
    private readonly ApiRequestParams _params = new();
    private readonly ApiRequest<T> _request = new(httpClient, baseUri);

    public Task<ApiResponse<T>> RunAsync(CancellationToken cancellationToken = default)
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

    public ITalaryonRunner<bool> Exists() => new ResourceExists(this);

    private class ResourceExists(ITalaryonRunner<ApiResponse<T>> runner) : ITalaryonRunner<bool>
    {
        public async Task<bool> RunAsync(CancellationToken cancellationToken = default)
        {
            var response = await runner.RunAsync(cancellationToken);
            return response.Data is not null;
        }
    }

    public ITalaryonRunner<ApiResponse<T>> With(Action<ApiRequestParams> withParams)
    {
        withParams(_params);
        return this;
    }
}

public sealed class AuthentikRequestMany<T>(HttpClient httpClient, string baseUri) : IApiResourceProviderMany<T, AuthentikList>
    where T : IApiResource
{
    private readonly ApiRequestParams _params = new();
    private readonly ApiRequest<T, AuthentikList> _request = new(httpClient, baseUri);


    public ITalaryonRunner<int> Count() => new ResourceCount(this);

    private class ResourceCount(ITalaryonRunner<ApiResponse<AuthentikList>> runner) : ITalaryonRunner<int>
    {
        public async Task<int> RunAsync(CancellationToken cancellationToken = default)
        {
            var list = await runner.RunAsync(cancellationToken);
            return list is { Data: null } ? 0 : list.Data.Pagination.Count;
        }
    }

    public ITalaryonRunner<ApiResponse<AuthentikList>> With(Action<ApiRequestParams> withParams)
    {
        withParams(_params);
        return this;   
    }

    public Task<ApiResponse<AuthentikList>> RunAsync(CancellationToken cancellationToken = default)
    {
        _request.WithType(ApiEndpointType.Many);
        
        foreach (var p in _params.ToDictionary())
        {
            _request.WithQueryParam(p.Key, p.Value);
        }
        return _request.RunAsync(cancellationToken);
    }
}