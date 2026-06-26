using Talaryon.Toolbox.CLI.Resources;

namespace Talaryon.Toolbox.CLI.Commands;

public class GetCommand : BaseCommand
{
    public GetCommand() : base("get", "Get a resource")
    {
        // Auto-discover and add all ResourceGetCommand<T> implementations
        UseAutodiscoverCommands(typeof(ResourceGetCommand<>));
    }
}
