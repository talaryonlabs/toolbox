using System.CommandLine;
using Talaryon.Toolbox.CLI.Commands;

namespace Talaryon.Toolbox.CLI.Resources;

public abstract class ResourceInstallCommand<TResource, TArg> : BaseCommand
    where TArg : Argument<string>, new()
{
    protected ResourceInstallCommand(string name, string description)
        : base(name, description)
    {
        Add(new TArg());
    }
    
    protected virtual TResource InstallResourceInstance()
    {
        throw new NotImplementedException($"Either {nameof(InstallResourceInstance)} or {nameof(InstallResourceInstanceAsync)} must be overridden.");
    }

    protected virtual Task<TResource> InstallResourceInstanceAsync()
    {
        return Task.FromResult(InstallResourceInstance());
    }
    
    protected abstract void OnResourceInstalled(TResource resource);

    protected override async Task ExecuteAsync()
    {
        var resource = await InstallResourceInstanceAsync().ConfigureAwait(false);
        OnResourceInstalled(resource);
    }
}
