using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.Pages;
using SupportCaseManagement.Presentation.Views;
using System;

namespace SupportCaseManagement.Presentation.Navigation;

public partial class NavigationService(IServiceProvider services) : ObservableObject, INavigationService
{
    [ObservableProperty]
    public partial Page? CurrentPage { get; private set; }

    public void Navigate(AppPage page)
    {
        CurrentPage = page switch
        {
            AppPage.Overview => services.GetRequiredService<OverviewView>(),
            AppPage.Customers => services.GetRequiredService<CustomersView>(),
            AppPage.Cases => services.GetRequiredService<CasesView>(),
            AppPage.CaseManager => services.GetRequiredService<CaseManagerView>(),
            _ => throw new ArgumentOutOfRangeException(nameof(page))
        };
    }
}

