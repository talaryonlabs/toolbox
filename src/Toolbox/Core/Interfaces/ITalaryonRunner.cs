using System.Threading;
using System.Threading.Tasks;

namespace TalaryonLabs.Toolbox;

public interface ITalaryonRunner
{
    void Run();
    Task RunAsync(CancellationToken cancellationToken = default);
}

public interface ITalaryonRunner<TResult>
{
    TResult? Run();
    Task<TResult?> RunAsync(CancellationToken cancellationToken = default);
}