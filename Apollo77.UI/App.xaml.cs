using Apollo77.UI.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using System;

namespace Apollo77.UI;

public partial class App : Application, IDisposable
{
    public Window? m_window;
    private readonly IHost _host;

    public App()
    {
        this.InitializeComponent();

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.ConfigureStartup();
            })
            .Build();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = _host.Services.GetRequiredService<MainWindow>();
        m_window.Activate();
    }

    public void Dispose()
    {
        _host.Dispose();
        Log.CloseAndFlush();
    }
}