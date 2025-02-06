using Apollo77.Core.CoreServiceProvider;

using Microsoft.Extensions.DependencyInjection;

namespace Apollo77.Core;

public static class CoreServiceCollection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<ProcessState>();
        services.AddTransient<IProcessProvider, ProcessProvider>();

        return services;
    }
}