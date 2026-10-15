using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Presentation.Pages;
using SupportCaseManagement.Presentation.Views;
using System;
using System.Collections.Generic;

namespace SupportCaseManagement.Presentation.Navigation;



public partial class NavigationService(IServiceProvider services) : ObservableObject, INavigationService
{
    private readonly Stack<(AppPage Page, SupportTicket? Ticket)> _history = new();
    private AppPage? _curuntPageType;
    private SupportTicket? _currentTicket;

    [ObservableProperty]
    public partial Page? CurrentPage { get; private set; }
    [ObservableProperty]
    public partial bool CanGoBack { get; private set; }

    public void Navigate(AppPage page, SupportTicket? ticket = null, Customer? customer = null)
    {
        if (_curuntPageType == page && _currentTicket == ticket)
            return;

        if (_curuntPageType.HasValue)
        {
            _history.Push((_curuntPageType.Value, _currentTicket));
        }
        SetPage(page, ticket, customer);
    }

    public void GoBack()
    {
        if (_history.Count > 0)
        {
            var (previousPage, previousTicket) = _history.Pop();

            _curuntPageType = null;
            _currentTicket = null;

            SetPage(previousPage, previousTicket);
        }
    }


    public void SetPage(AppPage page, SupportTicket? ticket = null, Customer? customer = null)
    {
        Page nextPage;

        if (page == AppPage.CaseManager)
        {
            var view = services.GetRequiredService<CaseManagerView>();
            view.ViewModel.SelectedTicket = ticket;
            nextPage = view;

        }
        else if (page == AppPage.CreateCustomer)
        {
            var view = services.GetRequiredService<CreateCustomerView>();
            view.ViewModel.SelectedCustomer = customer;
            nextPage = view;
        }
        else
        {
            nextPage = page switch
            {
                AppPage.Overview => services.GetRequiredService<OverviewView>(),
                AppPage.Customers => services.GetRequiredService<CustomersView>(),
                AppPage.Cases => services.GetRequiredService<CasesView>(),
                AppPage.Setting => services.GetRequiredService<SettingsView>(),

                _ => throw new ArgumentOutOfRangeException(nameof(page))
            };
        }
        _curuntPageType = page;
        _currentTicket = ticket;
        CurrentPage = nextPage;
        CanGoBack = _history.Count > 0;


    }




}


