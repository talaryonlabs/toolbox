using System.Collections.Generic;

namespace Talaryon.Toolbox;

public class TalaryonParams
{
    private readonly Dictionary<string, object> _params = new();
    
    protected Dictionary<string,object> BaseDictionary => _params;

    protected TalaryonParams Set(string name, object value)
    {
        if (!_params.TryAdd(name, value))
            _params[name] = value;

        return this;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return _params;
    }
}