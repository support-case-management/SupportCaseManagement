using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SupportCaseManagement.Infrastructure;
using SupportCaseManagement.Presentation.Navigation;
using SupportCaseManagement.Presentation.Pages;
using SupportCaseManagement.Presentation.ViewModels;
using SupportCaseManagement.Presentation.Views;
using System;

namespace SupportCaseManagement.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    // Gör så att vi kan komma åt propertyn Provider från andra klasser i applikationen
    private readonly IServiceProvider _services;
    private Window? _window;

    public App()
    {
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure();

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddTransient<OverviewViewModel>();
        services.AddTransient<OverviewView>();

        services.AddTransient<CasesViewModel>();
        services.AddTransient<CasesView>();

        services.AddTransient<CustomersViewModel>();
        services.AddTransient<CustomersView>();

        services.AddTransient<CaseManagerViewModel>();
        services.AddTransient<CaseManagerView>();

        _services = services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = _services.GetRequiredService<MainWindow>();

        var navigation = _services.GetRequiredService<INavigationService>();
        navigation.Navigate(AppPage.Overview);

        _window.Activate();
    }
}
