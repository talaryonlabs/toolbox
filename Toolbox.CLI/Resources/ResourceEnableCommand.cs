using System.CommandLine;
using Talaryon.Toolbox.CLI.Commands;

namespace Talaryon.Toolbox.CLI.Resources;

public abstract class ResourceEnableCommand<TArg> : BaseCommand
    where TArg : Argument<string>, new()
{
    protected ResourceEnableCommand(string name, string description)
        : base(name, description)
    {
        Add(new TArg());
    }
    
    protected virtual void EnableResourceInstance()
    {
        throw new NotImplementedException($"Either {nameof(EnableResourceInstance)} or {nameof(EnableResourceInstanceAsync)} must be overridden.");
    }

    protected virtual Task EnableResourceInstanceAsync()
    {
        EnableResourceInstance();
        return Task.CompletedTask;
    }

    protected abstract void OnResourceEnabled();

    protected override async Task ExecuteAsync()
    {
        await EnableResourceInstanceAsync().ConfigureAwait(false);
        OnResourceEnabled();
    }
}
