using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SupportCaseManagement.Presentation.Navigation;
using SupportCaseManagement.Presentation.ViewModels;
using System.ComponentModel;

namespace SupportCaseManagement.Presentation;

public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }
    public MainWindow(MainViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        MainNavigation.IsBackEnabled = true;
    }

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
        }
    }

    private void MainNavigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        // _navigationService.GoBack();
    }
}

