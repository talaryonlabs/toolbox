using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TalaryonLabs.Toolbox;

public static class TalaryonExtensions
{ 
    public static IServiceCollection AddConfig<TConfig>(this IServiceCollection services, TConfig config)
        where TConfig : class, IOptions<TConfig>, new()
    {
        return services
            .AddSingleton<IOptions<TConfig>>(config);
    }
        
    public static IServiceCollection AddSingleton<TService, TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
        where TService : class
        where TImplementation : class, TService
        where TOptions : class, IOptions<TOptions>, new()
    {
        return services
            .AddOptions()
            .Configure(configureOptions)
            .AddSingleton<TImplementation>()
            .AddSingleton<TService>(x => x.GetRequiredService<TImplementation>());
    }
        
    public static IServiceCollection AddScoped<TService, TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
        where TService : class
        where TImplementation : class, TService
        where TOptions : class, IOptions<TOptions>, new()
    {
        return services
            .AddOptions()
            .Configure(configureOptions)
            .AddScoped<TImplementation>()
            .AddScoped<TService>(x => x.GetRequiredService<TImplementation>());
    }
        
    public static IServiceCollection AddTransient<TService, TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
        where TService : class
        where TImplementation : class, TService
        where TOptions : class, IOptions<TOptions>, new()
    {
        return services
            .AddOptions()
            .Configure(configureOptions)
            .AddTransient<TImplementation>()
            .AddTransient<TService>(x => x.GetRequiredService<TImplementation>());
    }
        
    public static T RunSynchronouslyWithResult<T>(this Task<T> task)
    {
        task.RunSynchronously();
        return task.Result;
    }
}