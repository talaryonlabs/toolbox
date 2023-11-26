using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

public class YouTrackRessourcesProvider<T>(HttpClient httpClient) : IYouTrackRessoucesProvider<T>, ITalaryonRunner<int>
    where T: IYouTrackRessource
{
    private int _skip = -1, _limit = -1;

    public async Task<IEnumerable<T>?> RunAsync(CancellationToken cancellationToken = default)
    {
        var endpoint = YouTrackEndpoint<T>.GetEndpoint();
        var fields = YouTrackEndpoint<T>.GetFields();
        var query = new QueryBuilder();

        if (_skip > 0)
            query.Add("$skip", _skip.ToString());

        if (_limit > 0)
            query.Add("$limit", _limit.ToString());

        if (fields.Length > 0)
            query.Add("fields", fields);

        var url = $"{endpoint}{query.ToQueryString()}";
        try
        {
            TalaryonLogger.Debug<YouTrackRessourcesProvider<T>>($"Call {url}");
            return await httpClient.GetFromJsonAsync<List<T>>(url, cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<YouTrackRessourcesProvider<T>>(e.Message);
            return default;
        }
    }
    public IEnumerable<T>? Run() => RunAsync().RunSynchronouslyWithResult();

    public ITalaryonEnumerable<T> Take(int count)
    {
        _limit = count;
        return this;
    }

    public ITalaryonEnumerable<T> Skip(int count)
    {
        _skip = count;
        return this;
    }

    public ITalaryonEnumerable<T> SkipUntil(string cursor)
    {
        throw new NotImplementedException();
    }

    ITalaryonRunner<int> ITalaryonCountable.Count() => this;
    async Task<int> ITalaryonRunner<int>.RunAsync(CancellationToken cancellationToken)
    {
        var endpoint = YouTrackEndpoint<T>.GetEndpoint();
        
        var url = $"{endpoint}";
        try
        {
            TalaryonLogger.Debug<YouTrackRessourcesProvider<T>>($"Call {url}");
            return (await httpClient.GetFromJsonAsync<List<T>>(url, cancellationToken)).Count();
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<YouTrackRessourcesProvider<T>>(e.Message);
            return default;
        }
    }
    int ITalaryonRunner<int>.Run() => (this as ITalaryonRunner<int>).RunAsync().RunSynchronouslyWithResult();
}