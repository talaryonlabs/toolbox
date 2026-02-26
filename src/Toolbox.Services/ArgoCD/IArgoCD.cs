using Microsoft.AspNetCore.Http;
using Talaryon.Toolbox.Services.ArgoCD.Models;
using Talaryon.Toolbox.Services.Hub;

namespace Talaryon.Toolbox.Services.ArgoCD;

public interface IArgoCD
{
    public ITalaryonRunner<ArgoCDVersion> GetVersion();
    IArgoResourceProviderSingle<T> Single<T>(string id);
    IArgoResourceProviderMany<T> Many<T>();
    IArgoResourceCreateFactory<T> Factory<T>();
}

public interface IArgoResourceCreateFactory<T> : ITalaryonCreatable<T>, ITalaryonParams<T>
{
    
}

public interface IArgoResourceUpdateFactory<T> : ITalaryonUpdatable<T>, ITalaryonDeletable<bool>, ITalaryonParams<T>
{
    
}

public interface IArgoResourceProviderSingle<T> : ITalaryonRunner<T>, ITalaryonExistable
{
    IArgoResourceUpdateFactory<T> GetFactory();
}

public interface IArgoResourceProviderMany<T> : ITalaryonRunner<T>
{
    IArgoResourceProviderMany<T> Query(QueryString queryString);
}

