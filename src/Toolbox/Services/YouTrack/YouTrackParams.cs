using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.YouTrack;

public class YouTrackParams
{
    private readonly Dictionary<string, object> _params = new();

    public YouTrackParams Set(string name, object value)
    {
        if(!_params.TryAdd(name, value))
            _params[name] = value;

        return this;
    }

    public Dictionary<string, object> ToDictionary() => _params;
}