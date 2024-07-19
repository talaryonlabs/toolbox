using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Talaryon.Toolbox.Services.Directus;

public interface IDirectus
{
    Task<bool> Test();
    IDirectusRequestSingle<T> Single<T>(string name);
    IDirectusRequestSingle<T> Select<T>(string name, string? id);
    IDirectusRequestMany<T> Many<T>(string name);

    string GetAssetUrl(string? assetId);
    string GetAssetUrl(string? assetId, QueryString queryString);
}

public interface IDirectusRequestSingle<T> : ITalaryonRunner<DirectusResponse<T>?>
{
    IDirectusRequestSingle<T> Fields(params string[]? fields);
}

public interface IDirectusRequestMany<T> : ITalaryonRunner<DirectusResponse<T[]>?>
{
    IDirectusRequestMany<T> Fields(params string[]? fields);

    IDirectusRequestMany<T> Filter(string name, string type, string value);
    // IDirectusRequest<T> Search(string pattern);

    IDirectusRequestMany<T> Sort(params string[] fields);
    IDirectusRequestMany<T> Limit(int limit);

    IDirectusRequestMany<T> Offset(int offset);
    // IDirectusRequest<T> Page(int page);

    IDirectusRequestMany<T> IncludeMetadata();
}