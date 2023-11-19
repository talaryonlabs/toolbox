using System;
using System.Collections.Generic;

namespace TalaryonLabs.Toolbox;

public interface ITalaryonEnumerable<TItem> :
    ITalaryonRunner<IEnumerable<TItem>>
{
    ITalaryonEnumerable<TItem> Take(int count);
    ITalaryonEnumerable<TItem> Skip(int count);
    ITalaryonEnumerable<TItem> SkipUntil(string cursor);
}
    
public interface ITalaryonEnumerable<TItem, out TParams> :
    ITalaryonRunner<IEnumerable<TItem>>
{
    ITalaryonEnumerable<TItem, TParams> Take(int count);
    ITalaryonEnumerable<TItem, TParams> Skip(int count);
    ITalaryonEnumerable<TItem, TParams> SkipUntil(string cursor);
    ITalaryonEnumerable<TItem, TParams> Where(Action<TParams> whereParams);
}