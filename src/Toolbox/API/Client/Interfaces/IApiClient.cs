namespace Talaryon.Toolbox.API.Client;

public interface IApiClient
{
    ITalaryonRunner<bool> TestConnection();
}

public interface IApiClientFactory
{
    IApiResourceCreateFactory<TResource, TParams> Factory<TResource, TParams>() where TResource : IApiResource where TParams : ApiRequestParams;
}

public interface IApiClientProvider
{
    IApiResourceProviderSingle<TResource> Single<TResource>(string id) where TResource : IApiResource;
    IApiResourceProviderMany<TResource> Many<TResource>() where TResource : IApiResource;
}

public interface IApiClientProvider<TList>
{
    IApiResourceProviderSingle<TResource> Single<TResource>(string id) where TResource : IApiResource;
    IApiResourceProviderMany<TResource, TList> Many<TResource>() where TResource : IApiResource;
}

public interface IApiClient<out TSingleProvider, out TManyProvider, out TFactory>
{
    TSingleProvider Single<T>(string id) where T : IApiResource;
    TManyProvider Many<T>() where T : IApiResource;
    TFactory Factory<T, TParams>() where T : IApiResource where TParams : ApiRequestParams;
}