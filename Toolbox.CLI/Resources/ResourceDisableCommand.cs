using System.CommandLine;
using Talaryon.Toolbox.CLI.Commands;

namespace Talaryon.Toolbox.CLI.Resources;

public abstract class ResourceDisableCommand<TArg> : BaseCommand
    where TArg : Argument<string>, new()
{
    protected ResourceDisableCommand(string name, string description)
        : base(name, description)
    {
        Add(new TArg());
    }
    
    protected virtual void DisableResourceInstance()
    {
        throw new NotImplementedException($"Either {nameof(DisableResourceInstance)} or {nameof(DisableResourceInstanceAsync)} must be overridden.");
    }

    protected virtual Task DisableResourceInstanceAsync()
    {
        DisableResourceInstance();
        return Task.CompletedTask;
    }

    protected abstract void OnResourceDisabled();

    protected override async Task ExecuteAsync()
    {
        await DisableResourceInstanceAsync().ConfigureAwait(false);
        OnResourceDisabled();
    }
}
