using Talaryon.Toolbox.CLI.Resources;

namespace Talaryon.Toolbox.CLI.Commands;

public class DeleteCommand : BaseCommand
{
    public DeleteCommand() : base("delete", "Delete a resource")
    {
        // Auto-discover and add all ResourceDeleteCommand<T> implementations
        UseAutodiscoverCommands(typeof(ResourceDeleteCommand<,>));
    }
}
