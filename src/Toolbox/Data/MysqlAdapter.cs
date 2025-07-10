using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Talaryon.Toolbox.Data;

public sealed class MysqlOptions : TalaryonOptions<MysqlOptions>
{
    public string? Server { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public string? Database { get; set; }
}
    
public sealed class MysqlAdapter : SqlAdapterBase, IDisposable
{
    private readonly MySqlConnection _connection;

    public MysqlAdapter(IOptions<MysqlOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        var options = optionsAccessor.Value;

        _connection = new MySqlConnection(
            $"server={options.Server};" +
            $"uid={options.User};" +
            $"pwd={options.Password};" +
            $"database={options.Database}"
        );

        UseConnection(_connection);
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}