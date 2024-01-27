using System.Collections.Generic;

namespace TalaryonLabs.Toolbox.Services.Hub;

public class HubParams
{
    private readonly Dictionary<string, object> _params = new();

    public HubParams Set(string name, object value)
    {
        if(!_params.TryAdd(name, value))
            _params[name] = value;

        return this;
    }

    public Dictionary<string, object> ToDictionary() => _params;
}