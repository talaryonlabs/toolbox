﻿using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Data;

public partial class SqlAdapterBase
{
    private class Delete<T> :  
        ITalaryonRunner<T> where T : class?
    {
        private readonly IDbConnection _connection;
        private readonly T? _entity;

        public Delete(IDbConnection connection, T? entity)
        {
            _connection = connection;
            _entity = entity;
        }

        T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T?>)
            .RunAsync()
            .RunSynchronouslyWithResult();

        async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
        {
            return await _connection.DeleteAsync(_entity) ? _entity : null;
        }
    }
}