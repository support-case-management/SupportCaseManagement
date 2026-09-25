using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.Pages;
using SupportCaseManagement.Presentation.Views;
using System;

namespace SupportCaseManagement.Presentation.Navigation;

public class NavigationService : INavigationService

{
    private Frame? _frame;
    public bool CanGoBack => _frame?.CanGoBack == true;

    public void GoBack()
    {
        if (_frame is { CanGoBack: true })
        {
            _frame.GoBack();
        }
    }

    public void Initalize(Frame frame)
    {
        _frame = frame;
    }

    public void Initalize()
    {
        throw new NotImplementedException();
    }

    public void Navigate(AppPage page, string? parameter = null)
    {
        var frame = _frame ?? throw new InvalidOperationException("Navigation has not been initialized.");

        var pageType = page switch
        {
            // Här lägger du till nya sidor
            AppPage.Overview => typeof(OverviewView),
            AppPage.Customers => typeof(CustomersView),
            AppPage.Cases => typeof(CasesView),
            AppPage.CaseManager => typeof(CaseManagerView),
            _ => throw new ArgumentOutOfRangeException($"Unable to navigate to {page}.")
        };

        if (frame.CurrentSourcePageType == pageType && parameter is null)
        {
            return;
        }

        if (!frame.Navigate(pageType, parameter))
        {
            throw new InvalidOperationException($"Navigation to {page} failed.");
        }
    }
}

