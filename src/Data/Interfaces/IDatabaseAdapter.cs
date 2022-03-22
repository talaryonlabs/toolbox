using System;
using System.Collections.Generic;

namespace Talaryon.Data
{
    public interface IDatabaseAdapter
    {
        IDatabaseCount<TItem> Count<TItem>() where TItem : class;

        ITalaryonRunner<TItem> Get<TItem>(string id) where TItem : class;
        IDatabaseQuery<TItem> First<TItem>() where TItem : class;
        IDatabaseQueryList<TItem> Many<TItem>() where TItem : class;

        ITalaryonRunner<TItem> Insert<TItem>(TItem item) where TItem : class;
        ITalaryonRunner<TItem> Update<TItem>(TItem item) where TItem : class;
        ITalaryonRunner<TItem> Delete<TItem>(TItem item) where TItem : class;
        
        
        
        ITalaryonRunner<IEnumerable<TItem>> DeleteMany<TItem>(IEnumerable<TItem> items) where TItem : class;
        
        IDatabaseQuery<TItem> Delete<TItem>() where TItem : class;
        IDatabaseQuery<IEnumerable<TItem>> DeleteMany<TItem>() where TItem : class;
    }

    public interface IDatabaseCount<TItem> :
        ITalaryonRunner<int>
    {
        IDatabaseCount<TItem> Join<TJoinItem>(string column, string joinedColumn);
        IDatabaseCount<TItem> Where(Action<IDatabaseFilter<TItem>> filter);
    }

    public interface IDatabaseQuery<TItem> :
        ITalaryonRunner<TItem>
    {
        IDatabaseQuery<TItem> With(Action<IDatabaseSelector<TItem>> select);
        IDatabaseQuery<TItem> Join<TJoinItem>(string column, string joinedColumn);
        IDatabaseQuery<TItem> Where(Action<IDatabaseFilter<TItem>> filter);
    }

    public interface IDatabaseQueryList<TItem> :
        ITalaryonRunner<IEnumerable<TItem>>
    {
        IDatabaseQueryList<TItem> Distinct();
        IDatabaseQueryList<TItem> With(Action<IDatabaseSelector<TItem>> select);
        IDatabaseQueryList<TItem> Join<TJoinItem>(string column, string joinedColumn);
        IDatabaseQueryList<TItem> Where(Action<IDatabaseFilter<TItem>> filter);
        IDatabaseQueryList<TItem> OrderBy(Action<IDatabaseOrder<TItem>> order);
        IDatabaseQueryList<TItem> Limit(int count);
        IDatabaseQueryList<TItem> Offset(int count);
    }

    public interface IDatabaseSelector<TItem>
    {
        IDatabaseSelector<TItem> Column(string column, string? alias = null) => Column<TItem>(column, alias);
        IDatabaseSelector<TItem> Column<TJoinItem>(string column, string? alias = null);
    }

    public interface IDatabaseOrder<TItem>
    {
        IDatabaseOrder<TItem> Asc(string column) => Asc<TItem>(column);
        IDatabaseOrder<TItem> Asc<TJoinItem>(string column);

        IDatabaseOrder<TItem> Desc(string column) => Desc<TItem>(column);
        IDatabaseOrder<TItem> Desc<TJoinItem>(string column);
    }
    
    public interface IDatabaseFilter<TItem>
    {
        IDatabaseFilterColumn<TItem> Is(string column) => Is<TItem>(column);
        IDatabaseFilterColumn<TItem> Is<TJoinItem>(string column);

        IDatabaseFilterColumn<TItem> IsNot(string column) => IsNot<TItem>(column);
        IDatabaseFilterColumn<TItem> IsNot<TJoinItem>(string column);
        
        IDatabaseFilterOperator<TItem> Clamp(Action<IDatabaseFilter<TItem>> filter);
    }

    public interface IDatabaseFilterColumn<TItem>
    {
        IDatabaseFilterOperator<TItem> EqualTo(object value);
        IDatabaseFilterOperator<TItem> Like(object value, DatabasePatternType patternType = DatabasePatternType.Any);
        IDatabaseFilterOperator<TItem> In(IEnumerable<object> values);
        IDatabaseFilterOperator<TItem> Between(object value1, object value2);
    }
    
    public interface IDatabaseFilterOperator<TItem>
    {
        IDatabaseFilter<TItem> Or();
        IDatabaseFilter<TItem> And();
    }

    public enum DatabasePatternType
    {
        Any,
        StartsWith,
        EndsWith
    }
}