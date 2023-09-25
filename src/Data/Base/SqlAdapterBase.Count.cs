using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace TalaryonLabs.Toolbox.Data;

public partial class SqlAdapterBase
{
    private class Count<T> :
        IDatabaseCount<T>
    {
        private readonly IDbConnection _connection;
        private readonly QueryBuilder<T> _queryBuilder;

        public Count(IDbConnection connection)
        {
            _connection = connection;
            _queryBuilder = new QueryBuilder<T>();
            _queryBuilder.Count();
        }

        public IDatabaseCount<T> Join<TJoinItem>(string column, string joinedColumn)
        {
            _queryBuilder.Join<TJoinItem>(column, joinedColumn);
            return this;
        }

        IDatabaseCount<T> IDatabaseCount<T>.Where(Action<IDatabaseFilter<T>> filter)
        {
            _queryBuilder.Where(filter);
            return this;
        }

        int ITalaryonRunner<int>.Run() => (this as ITalaryonRunner<int>)
            .RunAsync()
            .RunSynchronouslyWithResult();

        Task<int> ITalaryonRunner<int>.RunAsync(CancellationToken cancellationToken) =>
            _connection.QuerySingleAsync<int>(_queryBuilder.Build());
    }
}