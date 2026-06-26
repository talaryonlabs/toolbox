using Talaryon.Toolbox.CLI.Resources;

namespace Talaryon.Toolbox.CLI.Commands;

public class DisableCommand : BaseCommand
{
    public DisableCommand() : base("disable", "Disable a resource")
    {
        // Auto-discover and add all ResourceDisableCommand<T> implementations
        UseAutodiscoverCommands(typeof(ResourceDisableCommand<>));
    }
}
