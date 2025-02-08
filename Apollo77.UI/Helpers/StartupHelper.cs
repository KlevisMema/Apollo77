using Serilog;

using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;

using Apollo77.Api;
using Apollo77.UI.ViewModels;
using Apollo77.UI.Services;

namespace Apollo77.UI.Helpers;

internal static class StartupHelper
{
    internal static void ConfigureStartup(this IServiceCollection services)
    {
        string? libraryPath = Path.GetDirectoryName(typeof(App).Assembly.Location);

        if (String.IsNullOrEmpty(libraryPath))
        {
            throw new ArgumentNullException(libraryPath);
        }

        string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        Directory.CreateDirectory(projectPath);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                path: Path.Combine(projectPath, "log-.txt"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
            )
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog();
        });

        services.AddSingleton<AppState>();
        services.AddTransient<DialogService>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ProcessesViewControlViewModel>();
        services.AddApiServices();
    }
}