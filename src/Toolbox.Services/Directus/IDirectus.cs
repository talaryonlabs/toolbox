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
    /// <returns>An object representing the request for retrieving a single item from the Directus API.</returns>
    IDirectusRequestSingle<T> Single<T>();

    /// <summary>
    /// Sends a request to the Directus API to retrieve multiple items of type T.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response from the API containing an array of items of type T, or null if no items were found.</returns>
    IDirectusRequestMany<T> Many<T>();

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
}

/// <summary>
/// Represents an interface for sending requests to the Directus API to retrieve multiple items of a specified type.
/// </summary>
/// <typeparam name="T">The type of items to be retrieved.</typeparam>
public interface IDirectusRequestMany<T> : ITalaryonRunner<DirectusResponse<T[]>?>
{
    /// <summary>
    /// Adds a filter to the request based on the specified field, filter type, and value.
    /// </summary>
    /// <param name="field">The field to filter on.</param>
    /// <param name="type">The filter type (e.g., "eq", "lt", "gt").</param>
    /// <param name="value">The value to filter the field by.</param>
    /// <returns>Returns the updated request instance with the added filter applied.</returns>
    IDirectusRequestMany<T> Filter(string field, string type, string value);
    IDirectusRequestMany<T> Filter(Action<IDirectusFilter> filterBuilder);

    /// <summary>
    /// Searches for items based on a specified search pattern in the Directus API.
    /// </summary>
    /// <param name="pattern">The search pattern to filter items.</param>
    /// <returns>Returns the current instance of <see cref="IDirectusRequestMany{T}"/> with the applied search filter.</returns>
    IDirectusRequestMany<T> Search(string pattern);

    /// <summary>
    /// Specifies the sorting fields for the requested items.
    /// </summary>
    /// <param name="fields">An array of field names to sort by, where each field can be prefixed with a '-' for descending order.</param>
    /// <returns>Returns the updated instance of <see cref="IDirectusRequestMany{T}"/> for further query modifications.</returns>
    IDirectusRequestMany<T> Sort(params string[] fields);

    /// <summary>
    /// Sets the maximum number of items to be returned in the request.
    /// </summary>
    /// <param name="limit">The maximum number of items to retrieve.</param>
    /// <returns>Returns the current instance of <see cref="IDirectusRequestMany{T}"/> for further modification.</returns>
    IDirectusRequestMany<T> Limit(int limit);

    /// <summary>
    /// Sets the offset for the results returned by the Directus API request.
    /// This determines the starting point for the requested results.
    /// </summary>
    /// <param name="offset">The number of items to skip before starting to fetch the results.</param>
    /// <returns>Returns the current instance of <c>IDirectusRequestMany</c> for method chaining.</returns>
    IDirectusRequestMany<T> Offset(int offset);

    /// <summary>
    /// Includes metadata in the resulting data from a Directus API request.
    /// </summary>
    /// <returns>Returns the updated request object with metadata inclusion configured.</returns>
    IDirectusRequestMany<T> IncludeMetadata();
}