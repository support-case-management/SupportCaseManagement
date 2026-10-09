using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportCaseManagement.Application.Dtos.Requests;
using SupportCaseManagement.Application.Dtos.Responses;
using SupportCaseManagement.Application.Features.Customers;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Presentation.Navigation;
using System.Threading.Tasks;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CreateCustomerViewModel(ICustomerService customerService, INavigationService navigationService) : ObservableObject
{
    private const string _ErrorMessage = "Någonting gick fel, försök igen";
    private readonly ICustomerService _customerService = customerService;
    private readonly INavigationService _navigationService = navigationService;


    [ObservableProperty]
    public partial string CustomerName { get; private set; }

    [ObservableProperty]
    public partial string EmailAddress { get; private set; }

    [ObservableProperty]
    public partial bool IsCompany { get; set; }

    [ObservableProperty]
    public partial string ErrorMessage { get; private set; } = string.Empty;

    [ObservableProperty]
    public partial Customer? SelectedCustomer { get; set; }

    public string NameLabel => IsCompany ? "Företagsnamn" : "Kundnamn";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; private set; }

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task SaveCustomerAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        bool success;

        try
        {
            CreateCustomerRequest request = new(CustomerName, EmailAddress, IsCompany);

            CreateCustomerResponse response = await _customerService.CreateCustomerAsync(request);
            success = response.Success;

            if (!success)
            {
                ErrorMessage = response.ErrorMessage ?? _ErrorMessage;
            }
        }
        catch
        {
            success = false;
            ErrorMessage = _ErrorMessage;
        }
        finally
        {
            IsBusy = false;
        }

        if (success)
        {
            _navigationService.Navigate(AppPage.Customers);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.Navigate(AppPage.Customers);
    }
}
