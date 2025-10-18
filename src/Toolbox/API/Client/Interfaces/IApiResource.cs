namespace Talaryon.Toolbox.API.Client;

public interface IApiResource
{
}

public interface IApiResourceCreateFactory<TResource, out TParams> : ITalaryonCreatable<TResource, TParams>, ITalaryonParams<TResource, TParams>
    where TResource : IApiResource
    where TParams : ApiRequestParams
{
    
}

public interface IApiResourceUpdateFactory<TResource, out TParams> : ITalaryonUpdatable<TResource, TParams>, ITalaryonDeletable<bool>, ITalaryonParams<TResource, TParams>
    where TResource : IApiResource
    where TParams : ApiRequestParams
{
    
}

public interface IApiResourceProviderSingle<TResource> : ITalaryonRunner<TResource>, ITalaryonExistable
    where TResource : IApiResource
{
    IApiResourceUpdateFactory<TResource, TParams> GetFactory<TParams>() where TParams : ApiRequestParams;
}

public interface IApiResourceProviderMany<TResource> : ITalaryonEnumerable<TResource>, ITalaryonCountable
    where TResource : IApiResource
{
}

public interface IApiResourceProviderMany<TResource, TList> : ITalaryonRunner<TList>, ITalaryonParams<TList, ApiRequestParams>, ITalaryonCountable
    where TResource : IApiResource
{
}