namespace Talaryon.Toolbox.API.Client;

public interface IApiRequest<in TResource, TOut> : ITalaryonRunner<TOut>
{
    IApiRequest<TResource, TOut> WithType(ApiEndpointType type);
    IApiRequest<TResource, TOut> WithParam(string name, string value);
    IApiRequest<TResource, TOut> WithQueryParam(string key, object value);
    IApiRequest<TResource, TOut> WithContent(TResource resource);
}