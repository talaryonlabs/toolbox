using System.CommandLine;
using Talaryon.Toolbox.CLI.Commands;

namespace Talaryon.Toolbox.CLI.Resources;

public abstract class ResourceUpdateCommand<TResource, TArg> : BaseCommand
    where TArg : Argument<string>, new()
{
    protected ResourceUpdateCommand(string name, string description)
        : base(name, description)
    {
        Add(new TArg());
    }
    
    protected virtual TResource LoadResource()
    {
        throw new NotImplementedException($"Either {nameof(LoadResource)} or {nameof(LoadResourceAsync)} must be overridden.");
    }

    protected virtual Task<TResource> LoadResourceAsync()
    {
        return Task.FromResult(LoadResource());
    }

    protected virtual void UpdateResourceInstance(TResource resource)
    {
        throw new NotImplementedException($"Either {nameof(UpdateResourceInstance)} or {nameof(UpdateResourceInstanceAsync)} must be overridden.");
    }

    protected virtual Task UpdateResourceInstanceAsync(TResource resource)
    {
        UpdateResourceInstance(resource);
        return Task.CompletedTask;
    }

    protected abstract void OnResourceUpdated(TResource resource);

    protected override async Task ExecuteAsync()
    {
        var resource = await LoadResourceAsync().ConfigureAwait(false);
        await UpdateResourceInstanceAsync(resource).ConfigureAwait(false);
        OnResourceUpdated(resource);
    }
}
