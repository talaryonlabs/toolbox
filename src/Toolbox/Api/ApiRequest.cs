using System.Net.Http.Json;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Api;

public class ApiRequest<TResource>(HttpClient httpClient, string baseUri)
    : ApiRequest<TResource, TResource>(httpClient, baseUri);

public class ApiRequest<TResource, TOut>(HttpClient httpClient, string baseUri) : IApiRequest<TResource, TOut>
{
    private readonly Dictionary<string,string> _uriParams = new();
    private readonly ApiRequestParams _queryParams = new();
    private ApiEndpointType _type = ApiEndpointType.Many;
    private ApiEndpointMethod _method = ApiEndpointMethod.Get;
    private TResource? _content;

    public TOut? Run() => RunAsync().RunSynchronouslyWithResult();

    public async Task<TOut?> RunAsync(CancellationToken cancellationToken = default)
    {
        var uri = ApiEndpoint.GetEndpoint<TResource>(_type) ?? throw new ApiEndpointException<TResource>();
        var requestUri = new Uri(baseUri)
            .Append(_uriParams is {Count: >0 } ? uri.ReplaceMany(_uriParams) : uri)
            .Append(_queryParams.ToQueryString().ToString());
        
        // TalaryonLogger.Debug<ApiRequest<TResource, TOut>>($"{_method.ToString().ToUpper()} ({_type.ToString()}) {requestUri}");

        var response = await (_method switch
        {
            ApiEndpointMethod.Post => httpClient.PostAsJsonAsync(requestUri, _content, cancellationToken),
            ApiEndpointMethod.Put => httpClient.PutAsJsonAsync(requestUri, _content, cancellationToken),
            ApiEndpointMethod.Patch => httpClient.PatchAsJsonAsync(requestUri, _content, cancellationToken),
            ApiEndpointMethod.Delete => httpClient.DeleteAsync(requestUri, cancellationToken),
            ApiEndpointMethod.Get => httpClient.GetAsync(requestUri, cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        });
        if (response.IsSuccessStatusCode) 
            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken);
        
        var error = await response.Content.ReadFromJsonAsync<ApiError>(cancellationToken);
        if (error is not null)
        {
            throw error;
        }
        throw new InvalidOperationException($"Request failed with status code {response.StatusCode}");

    }

    public IApiRequest<TResource, TOut> WithType(ApiEndpointType type)
    {
        _method = (_type = type) switch
        {
            ApiEndpointType.Single or ApiEndpointType.Many => ApiEndpointMethod.Get,
            ApiEndpointType.Create => ApiEndpointMethod.Post,
            ApiEndpointType.Update => ApiEndpointMethod.Put,
            ApiEndpointType.Delete => ApiEndpointMethod.Delete,
            _ => throw new ArgumentOutOfRangeException()
        };
        return this;
    }

    public IApiRequest<TResource, TOut> WithParam(string name, string value)
    {
        _uriParams.Add(name, value);
        return this;
    }

    public IApiRequest<TResource, TOut> WithQueryParam(string key, object value)
    {
        _queryParams.Set(key, value);
        return this;
    }

    public IApiRequest<TResource, TOut> WithContent(TResource resource)
    {
        _content = resource;
        return this;
    }
}