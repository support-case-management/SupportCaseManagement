using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.Navigation;

namespace SupportCaseManagement.Presentation;

public sealed partial class MainWindow : Window
{
    private readonly INavigationService _navigationService;
    public MainWindow(INavigationService navigationSerivce)
    {
        InitializeComponent();

        _navigationService = navigationSerivce;
        _navigationService.Initalize(ContentFrame);
        _navigationService.Navigate(AppPage.Overview);

    }

    private void ContentFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        MainNavigation.IsBackEnabled = true;
    }

    private void MainNavigation_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is not NavigationViewItem item)
            return;

        AppPage? page = item.Tag?.ToString() switch
        {
            "overview" => AppPage.Overview,
            "customers" => AppPage.Customers,
            "cases" => AppPage.Cases,
            _ => null
        };

        if (page is AppPage destination)
            _navigationService.Navigate(destination);
    }

    private void MainNavigation_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
    {
        _navigationService.GoBack();
    }
}

