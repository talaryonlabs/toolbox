using System.Net.Http.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Authentik;

public sealed class AuthentikRequestSingleId<T>(HttpClient httpClient, string baseUri, string id) : IAuthentikRequestSingle<T> where T : IAuthentikRessource
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
}

public sealed class AuthentikRequestSingle<T>(HttpClient httpClient, string baseUri) : IAuthentikRequestSingle<T> where T : IAuthentikRessource
{
    private readonly Dictionary<string, string?> _params = new();
    
    public T? Run() => RunAsync().RunSynchronouslyWithResult();

    public async Task<T?> RunAsync(CancellationToken cancellationToken = default)
    {
        var uri = AuthentikApiEndpoint.GetEndpoint<T>(AuthentikApiEndpointType.Get) ?? throw new AuthentikApiEndpointException<T>();
        var queryBuilder = new QueryBuilder(_params.AsReadOnly());
        var requestUri = new Uri(baseUri)
            .Append(uri)
            .Append(queryBuilder.ToString());

        try
        {
            TalaryonLogger.Debug<AuthentikRequestMany<T>>($"{requestUri}");
            return await httpClient.GetFromJsonAsync<T>(requestUri, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<Authentik>(e.Message);
            return default;
        }
    }

    public IAuthentikRequestSingle<T> WithParam(string key, object value)
    {
        _params.Add(key, value.ToString());
        return this;
    }
}

public sealed class AuthentikRequestMany<T>(HttpClient httpClient, string baseUri) : IAuthentikRequestMany<T> where T : IAuthentikRessource
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
}