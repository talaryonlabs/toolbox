using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox;

public interface ITalaryonRunner
{
    void Run();
    Task RunAsync(CancellationToken cancellationToken = default);
}

public interface ITalaryonRunner<TResult>
{
    TResult? Run() => RunAsync().RunSynchronouslyWithResult();
    Task<TResult?> RunAsync(CancellationToken cancellationToken = default);
}