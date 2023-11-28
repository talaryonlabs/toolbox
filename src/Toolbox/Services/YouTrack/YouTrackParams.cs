using System.Collections.Generic;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

public abstract class YouTrackParams
{
    private readonly Dictionary<string, object> _params = new();

    protected YouTrackParams Set(string name, object value)
    {
        if(!_params.TryAdd(name, value))
            _params[name] = value;

        return this;
    }

    public Dictionary<string, object> ToDictionary() => _params;
}