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

public sealed class AuthentikRequestSingle<T>(HttpClient httpClient, string baseUri) : IApiResourceProviderSingle<T>, ITalaryonParams<T, ApiRequestParams>
    where T : IApiResource
{
    private readonly ApiRequestParams _params = new();
    
    public T? Run() => RunAsync().RunSynchronouslyWithResult();

    public async Task<T?> RunAsync(CancellationToken cancellationToken = default)
    {
        var uri = ApiEndpoint.GetEndpoint<T>(ApiEndpointType.Single) ?? throw new ApiEndpointException<T>();
        var requestUri = new Uri(baseUri)
            .Append(uri)
            .Append(_params.ToQueryString().ToString());

        try
        {
            TalaryonLogger.Debug<AuthentikRequestSingle<T>>($"{requestUri}");
            return await httpClient.GetFromJsonAsync<T>(requestUri, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<Authentik>(e.Message);
            return default;
        }
    }

    public IApiResourceUpdateFactory<T, TParams> GetFactory<TParams>() where TParams : ApiRequestParams
    {
        throw new NotImplementedException();
    }

    public ITalaryonRunner<bool> Exists()
    {
        throw new NotImplementedException();
    }

    public ITalaryonRunner<T> With(Action<ApiRequestParams> withParams)
    {
        withParams(_params);
        return this;
    }
}

/*public sealed class AuthentikRequestMany<T>(HttpClient httpClient, string baseUri) : IAuthentikRequestMany<T> where T : IAuthentikRessource
{
    private readonly Dictionary<string, string?> _params = new();
    
    public AuthentikList<T>? Run() => RunAsync().RunSynchronouslyWithResult();

    public async Task<AuthentikList<T>?> RunAsync(CancellationToken cancellationToken = default)
    {
        var uri = AuthentikApiEndpoint.GetEndpoint<T>() ?? throw new AuthentikApiEndpointException<T>();
        var queryBuilder = new QueryBuilder(_params.AsReadOnly());
        var requestUri = new Uri(baseUri)
            .Append(uri)
            .Append(queryBuilder.ToString());

        try
        {
            TalaryonLogger.Debug<AuthentikRequestMany<T>>($"{requestUri}");
            return await httpClient.GetFromJsonAsync<AuthentikList<T>>(requestUri, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<Authentik>(e.Message);
            return null;
        }
    }

    public IAuthentikRequestMany<T> WithParam(string key, object value)
    {
        _params.Add(key, value.ToString());
        return this;
    }
}*/