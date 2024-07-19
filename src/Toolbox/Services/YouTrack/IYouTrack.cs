using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.YouTrack;

public interface IYouTrack
{
    IYouTrackResourceProviderSingle<T> Single<T>(string id) where T : IYouTrackResource;
    IYouTrackResourceProviderMany<T> Many<T>() where T : IYouTrackResource;
    IYouTrackResourceCreateFactory<T, TParams> Factory<T, TParams>() where T : IYouTrackResource where TParams : YouTrackParams;
}

public interface IYouTrackResource
{
    string? Id { get; set; }
    string? Type { get; set; }
}

public interface IYouTrackResourceCreateFactory<T, out TParams> : ITalaryonCreatable<T, TParams>, ITalaryonParams<T, TParams>
    where T : IYouTrackResource
    where TParams : YouTrackParams
{
    
}

public interface IYouTrackResourceUpdateFactory<T, out TParams> : ITalaryonUpdatable<T, TParams>, ITalaryonDeletable<bool>, ITalaryonParams<T, TParams>
    where T : IYouTrackResource
    where TParams : YouTrackParams
{
    
}

public interface IYouTrackResourceProviderSingle<T> : ITalaryonRunner<T>, ITalaryonExistable
    where T : IYouTrackResource
{
    IYouTrackResourceProviderSingle<T> Fields(params string[] fields);
    IYouTrackResourceUpdateFactory<T, TParams> GetFactory<TParams>() where TParams : YouTrackParams;
}

public interface IYouTrackResourceProviderMany<T> : ITalaryonEnumerable<T>, ITalaryonCountable
    where T : IYouTrackResource
{
    IYouTrackResourceProviderMany<T> Fields(params string[] fields);
    IYouTrackResourceProviderMany<T> Query(string queryString);
}

public interface IYouTrackCustomField
{
    string Id { get; }
    string Name { get; set; }
}

public interface IYouTrackCustomFieldable<T>
    where T : IYouTrackCustomField
{
    List<T> CustomFields { get; }
}