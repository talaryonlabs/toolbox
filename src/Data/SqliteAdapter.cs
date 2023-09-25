using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace TalaryonLabs.Toolbox.Data;

public sealed class SqliteOptions : TalaryonOptions<SqliteOptions>
{
    public string? DataSource { get; set; }
}

public sealed class SqliteAdapter : SqlAdapterBase, IDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteAdapter(IOptions<SqliteOptions> optionsAccessor)
    {
        if (optionsAccessor is null)
        {
            throw new ArgumentNullException(nameof(optionsAccessor));
        }

        if (optionsAccessor.Value.DataSource != null)
            _connection = new SqliteConnection($"Data Source={optionsAccessor.Value.DataSource}");

        UseConnection(_connection ?? throw new InvalidOperationException());
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}