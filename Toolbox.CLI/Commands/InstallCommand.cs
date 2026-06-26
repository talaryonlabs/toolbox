using Talaryon.Toolbox.CLI.Resources;

namespace Talaryon.Toolbox.CLI.Commands;

public class InstallCommand : BaseCommand
{
    public InstallCommand() : base("install", "Install a resource")
    {
        // Auto-discover and add all ResourceInstallCommand<T> implementations
        UseAutodiscoverCommands(typeof(ResourceInstallCommand<,>));
    }
}
