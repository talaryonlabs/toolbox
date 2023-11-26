using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

public class YouTrackRessourceProvider<T>(string id, HttpClient httpClient) : IYouTrackRessouceProvider<T>
    where T : IYouTrackRessource
{
    async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
    {
        var endpoint = YouTrackEndpoint<T>.GetEndpoint(YouTrackApiEndpointType.Get)?.Replace(".id", id);
        var fields = YouTrackEndpoint<T>.GetFields();
        var query = new QueryBuilder();

        if (fields.Length > 0)
            query.Add("fields", fields);

        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<YouTrackRessourcesProvider<T>>($"Call {url}");
            return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<YouTrackRessourcesProvider<T>>(e.Message);
            return default;
        }
    }
    T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T>).RunAsync().RunSynchronouslyWithResult();

    ITalaryonRunner<bool> ITalaryonExistable.Exists() => new Exists(httpClient, id);

    class Exists(HttpClient httpClient, string id) : ITalaryonRunner<bool>
    {
        async Task<bool> ITalaryonRunner<bool>.RunAsync(CancellationToken cancellationToken)
        {
            var endpoint = YouTrackEndpoint<T>.GetEndpoint(YouTrackApiEndpointType.Get)?.Replace(".id", id);
            var url = $"{endpoint}";
            try
            {
                TalaryonLogger.Debug<YouTrackRessourcesProvider<T>>($"Call {url}");
                return (await httpClient.GetFromJsonAsync<T>(url, cancellationToken)) != null;
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<YouTrackRessourcesProvider<T>>(e.Message);
                return default;
            }
        }
        bool ITalaryonRunner<bool>.Run() => (this as ITalaryonRunner<bool>).RunAsync().RunSynchronouslyWithResult();
    }
}