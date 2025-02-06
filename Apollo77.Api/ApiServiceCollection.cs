using Apollo77.Core;

using Apollo77.Api.ApiService;

using Microsoft.Extensions.DependencyInjection;

namespace Apollo77.Api;

public static class ApiServiceCollection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCoreServices();
        services.AddTransient<IProcessApi, ProcessApi>();

        return services;
    }
}