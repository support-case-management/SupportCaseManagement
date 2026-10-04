using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Presentation.Pages;
using SupportCaseManagement.Presentation.Views;
using System; 

namespace SupportCaseManagement.Presentation.Navigation;

public partial class NavigationService(IServiceProvider services) : ObservableObject, INavigationService
{
    [ObservableProperty]
    public partial Page? CurrentPage { get; private set; }

    public void Navigate(AppPage page, SupportTicket? ticket = null)
    {
        if (page == AppPage.CaseManager)
        {
            var view = services.GetRequiredService<CaseManagerView>();
            view.ViewModel.SelectedTicket = ticket;
            CurrentPage = view;
            return;

        }


        CurrentPage = page switch
        {
            AppPage.Overview => services.GetRequiredService<OverviewView>(),
            AppPage.Customers => services.GetRequiredService<CustomersView>(),
            AppPage.Cases => services.GetRequiredService<CasesView>(),
            _ => throw new ArgumentOutOfRangeException(nameof(page))
        };
    }
}

