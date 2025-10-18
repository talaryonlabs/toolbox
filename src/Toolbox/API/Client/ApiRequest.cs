using System.Net.Http.Json;
using Talaryon.Toolbox.Extensions;
using Talaryon.Toolbox.Services.Authentik;

namespace Talaryon.Toolbox.API.Client;

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

        try
        {
            TalaryonLogger.Debug<ApiRequest<TResource, TOut>>($"{_method.ToString().ToUpper()} ({_type.ToString()}) {requestUri}");
            HttpResponseMessage response;
            
            switch (_method)
            {
                case ApiEndpointMethod.Post:
                    response = await httpClient.PostAsJsonAsync(requestUri, _content, cancellationToken);
                    break;
                case ApiEndpointMethod.Put:
                    response = await httpClient.PutAsJsonAsync(requestUri, _content, cancellationToken);
                    break;
                case ApiEndpointMethod.Patch:
                    response = await httpClient.PatchAsJsonAsync(requestUri, _content, cancellationToken);
                    break;
                case ApiEndpointMethod.Delete:
                    response = await httpClient.DeleteAsync(requestUri, cancellationToken);
                    break;
                case ApiEndpointMethod.Get:
                    return await httpClient.GetFromJsonAsync<TOut>(requestUri, cancellationToken);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<Authentik>(e.Message);
        }
        return default;
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