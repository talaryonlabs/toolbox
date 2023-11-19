using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace TalaryonLabs.Toolbox.Data;

public partial class SqlAdapterBase
{
    private class Insert<T> :  
        ITalaryonRunner<T> where T : class?
    {
        private readonly IDbConnection _connection;
        private readonly T? _entity;

        public Insert(IDbConnection connection, T? entity)
        {
            _connection = connection;
            _entity = entity;
        }

        T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T?>)
            .RunAsync()
            .RunSynchronouslyWithResult();

        async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
        {
            await _connection.InsertAsync(_entity);
            return _entity;
        }
    }
}