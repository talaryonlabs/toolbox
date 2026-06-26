using Talaryon.Toolbox.CLI.Resources;

namespace Talaryon.Toolbox.CLI.Commands;

public class UpdateCommand : BaseCommand
{
    public UpdateCommand() : base("update", "Update a resource")
    {
        // Auto-discover and add all ResourceUpdateCommand<T> implementations
        UseAutodiscoverCommands(typeof(ResourceUpdateCommand<,>));
    }
}
