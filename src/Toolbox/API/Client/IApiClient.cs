namespace Talaryon.Toolbox.API.Client;

public interface IApiClient : IApiClientProvider, IApiClientFactory
{
    ITalaryonRunner<bool> TestConnection();
}

public interface IApiClientFactory
{
    IApiResourceCreateFactory<T, TParams> Factory<T, TParams>() where T : IApiResource where TParams : ApiRequestParams;
}

public interface IApiClientProvider
{
    IApiResourceProviderSingle<T> Single<T>(string id) where T : IApiResource;
    IApiResourceProviderMany<T> Many<T>() where T : IApiResource;
}

public interface IApiClient<out TSingleProvider, out TManyProvider, out TFactory>
{
    TSingleProvider Single<T>(string id) where T : IApiResource;
    TManyProvider Many<T>() where T : IApiResource;
    TFactory Factory<T, TParams>() where T : IApiResource where TParams : ApiRequestParams;
}

public interface IApiResource
{
}

public interface IApiResourceCreateFactory<T, out TParams> : ITalaryonCreatable<T, TParams>, ITalaryonParams<T, TParams>
    where T : IApiResource
    where TParams : ApiRequestParams
{
    
}

public interface IApiResourceUpdateFactory<T, out TParams> : ITalaryonUpdatable<T, TParams>, ITalaryonDeletable<bool>, ITalaryonParams<T, TParams>
    where T : IApiResource
    where TParams : ApiRequestParams
{
    
}

public interface IApiResourceProviderSingle<T> : ITalaryonRunner<T>, ITalaryonExistable
    where T : IApiResource
{
    IApiResourceUpdateFactory<T, TParams> GetFactory<TParams>() where TParams : ApiRequestParams;
}

public interface IApiResourceProviderMany<T> : ITalaryonEnumerable<T>, ITalaryonCountable
    where T : IApiResource
{
}