using Talaryon.Toolbox.CLI.Resources;

namespace Talaryon.Toolbox.CLI.Commands;

public class EnableCommand : BaseCommand
{
    public EnableCommand() : base("enable", "Enable a resource")
    {
        // Auto-discover and add all ResourceEnableCommand<T> implementations
        UseAutodiscoverCommands(typeof(ResourceEnableCommand<>));
    }
}
