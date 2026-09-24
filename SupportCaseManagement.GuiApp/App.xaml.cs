using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SupportCaseManagement.Infrastructure;
using System;

namespace SupportCaseManagement.Presentation;

public partial class App : Application
{
    // Gör så att vi kan komma åt propertyn Provider från andra klasser i applikationen
    private readonly IServiceProvider _provider;
    private Window? _window;

    public App()
    {
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure();
        services.AddTransient<MainWindow>();

        _provider = services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = _provider.GetRequiredService<MainWindow>();
        _window.Activate();
    }
}
