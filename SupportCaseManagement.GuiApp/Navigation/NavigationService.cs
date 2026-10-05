using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.Pages;
using SupportCaseManagement.Presentation.Views;
using System;
using System.Collections.Generic;

namespace SupportCaseManagement.Presentation.Navigation;

public partial class NavigationService(IServiceProvider services) : ObservableObject, INavigationService
{
    private readonly Stack<AppPage> _history = new();
    private AppPage? _curuntPageType;

    [ObservableProperty]
    public partial Page? CurrentPage { get; private set; }

    [ObservableProperty]
    public partial bool CanGoBack { get; private set; }

    public void Navigate(AppPage page)
    {
        if (_curuntPageType == page)
            return;

        if (_curuntPageType.HasValue)
        {
            _history.Push(_curuntPageType.Value);
        }
        SetPage(page);
    }

    public void GoBack()
    {
        if (_history.Count > 0)
        {
            var previousPage = _history.Pop();
            SetPage(previousPage);
        }
    }

    public void SetPage(AppPage page)
    {
        _curuntPageType = page;
        CurrentPage = page switch
        {
            AppPage.Overview => services.GetRequiredService<OverviewView>(),
            AppPage.Customers => services.GetRequiredService<CustomersView>(),
            AppPage.Cases => services.GetRequiredService<CasesView>(),
            AppPage.CaseManager => services.GetRequiredService<CaseManagerView>(),
            _ => throw new ArgumentOutOfRangeException(nameof(page))
        };

        CanGoBack = _history.Count > 0;
    }
}

