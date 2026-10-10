using CommunityToolkit.Mvvm.ComponentModel;
using SupportCaseManagement.Application.Dtos.Responses;
using SupportCaseManagement.Application.Features.Customers;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Presentation.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CustomersViewModel(ICustomerService customerService, INavigationService navigationService) : ObservableObject
{
    private const string _ErrorMessage = "Kunderna kunde inte läsas in från fil";
    private readonly ICustomerService _customerService = customerService;
    private readonly INavigationService _navigationService = navigationService;

    public ObservableCollection<Customer> Customers { get; } = [];

    [ObservableProperty]
    public partial string StatusMessage { get; private set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; private set; }

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    public partial int TotalNumberOfCustomers { get; private set; }

    public async Task LoadAllCustomersAsync()
    {
        IsBusy = true;

        try
        {
            GetAllCustomersResponse response = await _customerService.GetAllCustomersAsync();

            if (response.Success)
            {
                IEnumerable<Customer> customers = response.Customers;

                Customers.Clear();

                foreach (Customer customer in customers)
                {
                    Customers.Add(customer);
                }

                SetStatusMessage("ärende i listan", "ärenden i listan", "Det finns inga sparade ärenden än");

                TotalNumberOfCustomers = Customers.Count;
            }
            else
            {
                StatusMessage = response.ErrorMessage ?? "Någonting gick fel, försök igen";
            }
        }
        catch
        {
            StatusMessage = _ErrorMessage;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void SetStatusMessage(string textOneCustomer, string textMultipleCustomers, string textNoCustomers)
    {
        if (Customers.Count == 1)
        {
            StatusMessage = $"{Customers.Count} {textOneCustomer}";
        }
        else if (Customers.Count > 1)
        {
            StatusMessage = $"{Customers.Count} {textMultipleCustomers}";
        }
        else
        {
            StatusMessage = textNoCustomers;
        }
    }
}