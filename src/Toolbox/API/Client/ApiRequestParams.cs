using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Talaryon.Toolbox.API.Client;

public class ApiRequestParams
{
    private readonly Dictionary<string, object> _params = new();

    public ApiRequestParams Set(string name, object value)
    {
        if(!_params.TryAdd(name, value))
            _params[name] = value;

        return this;
    }

    public Dictionary<string, object> ToDictionary() => _params;
    public QueryString ToQueryString() => new QueryBuilder(_params.Select(v => new KeyValuePair<string, string>(v.Key,v.Value.ToString() ?? string.Empty))).ToQueryString();
}