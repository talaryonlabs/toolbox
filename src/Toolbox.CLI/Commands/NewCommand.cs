using Talaryon.Toolbox.CLI.Resources;

namespace Talaryon.Toolbox.CLI.Commands;

public class NewCommand : BaseCommand
{
    public NewCommand() : base("new", "Create a new resource")
    {
        // Auto-discover and add all ResourceGetCommand<T> implementations
        UseAutodiscoverCommands(typeof(ResourceCreateCommand<,>));
    }
}
