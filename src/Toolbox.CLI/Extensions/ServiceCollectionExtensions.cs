using Microsoft.Extensions.DependencyInjection;
using Talaryon.Toolbox.CLI.Services;

namespace Talaryon.Toolbox.CLI.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCliServices()
        {
            // Register singleton services
            services.AddSingleton<IErrorService, ErrorService>();
            
            return services;
        }
    }
}