using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SupportCaseManagement.Infrastructure;
using SupportCaseManagement.Presentation.Navigation;
using SupportCaseManagement.Presentation.ViewModels;
using SupportCaseManagement.Presentation.Views;
using System;

namespace SupportCaseManagement.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    // Gör så att vi kan komma åt propertyn Provider från andra klasser i applikationen
    public static IServiceProvider Provider { get; private set; } = null;
    private Window? _window;

    public App()
    {
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure();

        services.AddTransient<MainWindow>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddTransient<OverviewViewModel>();

        services.AddTransient<CasesViewModel>();
        services.AddTransient<CasesView>();

        Provider = services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = Provider.GetRequiredService<MainWindow>();
        _window.Activate();
    }
}
