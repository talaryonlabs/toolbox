using System.Collections.Generic;
using Talaryon.Toolbox.Services.YouTrack;

namespace Talaryon.Toolbox.Services.Hub;

public interface IHub
{
    IHubResourceProviderSingle<T> Single<T>(string id) where T : IHubResource;
    IHubResourceProviderMany<T> Many<T>() where T : IHubResource;
    IHubResourceCreateFactory<T, TParams> Factory<T, TParams>() where T : IHubResource where TParams : HubParams;
}

public interface IHubResource
{
    string? Id { get; set; }
    string? Type { get; set; }
}

public interface IHubResourceCreateFactory<T, out TParams> : ITalaryonCreatable<T, TParams>, ITalaryonParams<T, TParams>
    where T : IHubResource
    where TParams : HubParams
{
    
}

public interface IHubResourceUpdateFactory<T, out TParams> : ITalaryonUpdatable<T, TParams>, ITalaryonDeletable<bool>, ITalaryonParams<T, TParams>
    where T : IHubResource
    where TParams : HubParams
{
    
}

public interface IHubResourceProviderSingle<T> : ITalaryonRunner<T>, ITalaryonExistable
    where T : IHubResource
{
    IHubResourceProviderSingle<T> Fields(params string[] fields);
    IHubResourceUpdateFactory<T, TParams> GetFactory<TParams>() where TParams : HubParams;
}

public interface IHubResourceProviderMany<T> : ITalaryonEnumerable<T>, ITalaryonCountable
    where T : IHubResource
{
    IHubResourceProviderMany<T> Fields(params string[] fields);
    IHubResourceProviderMany<T> Query(string queryString);
}

public interface IHubCustomField
{
    string Id { get; }
    string Name { get; set; }
}

public interface IHubCustomFieldable<T>
    where T : IHubCustomField
{
    List<T> CustomFields { get; }
}

public interface IHubResponseType
{
    int Skip { get; set; }
    int Top { get; set; }
    int Total { get; set; }
    List<HubUser>? List { get; set; }
}