using System.Collections.Generic;

namespace TalaryonLabs.Toolbox.Services;

public interface ICacheService
{
    ICacheServiceEntry<T> Key<T>(string key);
    ICacheServiceEntry<object> Key(string key);
    ITalaryonRunner RemoveMany(IEnumerable<string> keys);
}
    
public interface ICacheServiceEntry<T> :
    ITalaryonRunner<T>,
    ITalaryonExistable,
    ITalaryonDeletable
{
    ITalaryonRunner Set(T? value);
    ITalaryonRunner Refresh(T value);
}