using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.Navigation;
using SupportCaseManagement.Presentation.Pages;
using SupportCaseManagement.Presentation.ViewModels;
using SupportCaseManagement.Presentation.Views;
using System.ComponentModel;
using System.Linq;

namespace SupportCaseManagement.Presentation;

public sealed partial class MainWindow : Window
{
    private readonly INavigationService _navigationService;
    public MainViewModel ViewModel { get; }
    public MainWindow(MainViewModel viewModel, INavigationService navigationService)
    {
        ViewModel = viewModel;

        _navigationService = navigationService;

        InitializeComponent();

        if (_navigationService is INotifyPropertyChanged notifyService)
        {
            notifyService.PropertyChanged += (send, args) =>
            {
                if (args.PropertyName == nameof(INavigationService.CanGoBack))
                {
                    MainNavigation.IsBackEnabled = _navigationService.CanGoBack;
                }
            };
        }

    }

    //private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    //{
    //    MainNavigation.IsBackEnabled = true;
    //}

    private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        switch (args.InvokedItemContainer?.Tag)
        {
            case "overview":
                ViewModel.ShowOverViewCommand.Execute(null);
                break;

            case "cases":
                ViewModel.ShowCasesViewCommand.Execute(null);
                break;

            case "customers":
                ViewModel.ShowCustomersViewCommand.Execute(null);
                break;

            case "caseManager":
                ViewModel.ShowCaseManagerViewCommand.Execute(null);
                break;

            case "settings":
                ViewModel.ShowSettingsCommand.Execute(null);
                break;

        }
    }

    private void MainNavigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (_navigationService.CanGoBack)
        {
            _navigationService.GoBack();
            MainNavigation.SelectedItem = _navigationService.CurrentPage switch
            {
                OverviewView => GetMenuItem("overview"),
                CustomersView => GetMenuItem("customers"),
                CasesView => GetMenuItem("cases"),
                CaseManagerView => GetMenuItem("caseManager"),
                SettingsView => GetMenuItem("settings"),
                _ => MainNavigation.SelectedItem
            };
        }
    }

    private NavigationViewItem? GetMenuItem(string tag)
    {
        return MainNavigation.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(x => x.Tag?.ToString() == tag);
    }

}

