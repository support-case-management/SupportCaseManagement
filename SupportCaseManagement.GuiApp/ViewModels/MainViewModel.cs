using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportCaseManagement.Presentation.Navigation;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class MainViewModel(INavigationService navigationService) : ObservableObject
{
    public INavigationService Navigation { get; } = navigationService;

    [RelayCommand]
    private void ShowOverView()
    {
        Navigation.Navigate(AppPage.Overview);
    }

    [RelayCommand]
    private void ShowCustomersView()
    {
        Navigation.Navigate(AppPage.Customers);
    }

    [RelayCommand]
    private void ShowCasesView()
    {
        Navigation.Navigate(AppPage.Cases);
    }

    [RelayCommand]
    private void ShowCaseManagerView()
    {
        Navigation.Navigate(AppPage.CaseManager);
    }
}
