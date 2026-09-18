using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace SupportCaseManagement.GuiApp;

public partial class App : Application
{
    public static IServiceProvider Provider { get; private set; } = null!;
    private Window? _window;

    public App()
    {
        var services = new ServiceCollection();

        services.AddTransient<MainWindow>();

        Provider = services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = Provider.GetRequiredService<MainWindow>();
        _window.Activate();
    }
}
