using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace Talaryon.Toolbox.Data;

public sealed class SqliteOptions : TalaryonOptions<SqliteOptions>
{
    public string? DataSource { get; set; }
}

public sealed class SqliteAdapter : SqlAdapterBase, IDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteAdapter(IOptions<SqliteOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        if (optionsAccessor.Value.DataSource != null)
            _connection = new SqliteConnection($"Data Source={optionsAccessor.Value.DataSource}");

        UseConnection(_connection ?? throw new InvalidOperationException());
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}