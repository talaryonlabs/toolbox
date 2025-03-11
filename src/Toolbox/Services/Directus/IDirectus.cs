using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Services.Directus;

/// <summary>
/// Represents the interface for making requests to the Directus API.
/// </summary>
public interface IDirectus
{
    /// <summary>
    /// Sends a test request to the Directus API to check if it is online.
    /// </summary>
    /// <returns>Returns true if the API responds with "pong", otherwise false.</returns>
    Task<bool> Test();

    /// <summary>
    /// Returns a request object for retrieving a single item from the Directus API.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="name">The name of the item.</param>
    /// <returns>An object representing the request for retrieving a single item from the Directus API.</returns>
    IDirectusRequestSingle<T> Single<T>(string name);

    /// <summary>
    /// Returns a request object for retrieving multiple items from the Directus API.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="name">The name of the items.</param>
    /// <returns>An object representing the request for retrieving multiple items from the Directus API.</returns>
    IDirectusRequestSingle<T> Select<T>(string name, string? id);

    /// <summary>
    /// Sends a request to the Directus API to retrieve multiple items of type T.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="name">The name of the table to retrieve items from.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response from the API containing an array of items of type T, or null if no items were found.</returns>
    IDirectusRequestMany<T> Many<T>(string name);

    /// <summary>
    /// Gets the URL of an asset in the Directus API.
    /// </summary>
    /// <param name="assetId">The ID of the asset.</param>
    /// <returns>The URL of the asset.</returns>
    string? GetAssetUrl(string? assetId);

    /// <summary>
    /// Returns the URL of the asset with the specified ID.
    /// </summary>
    /// <param name="assetId">The ID of the asset.</param>
    /// <param name="queryString">The query string to be appended to the URL. It can be used to include additional parameters.</param>
    /// <returns>The URL of the asset.</returns>
    string GetAssetUrl(string? assetId, QueryString queryString);

    ITalaryonRunner<IDirectusSchema> Snapshot();
    ITalaryonRunner<IDirectusSchema> Diff(IDirectusSchema current);
    ITalaryonRunner<bool> ApplySchema(IDirectusSchema diff);
}

public interface IDirectusSchema
{
    public string? Data { get; set; }
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