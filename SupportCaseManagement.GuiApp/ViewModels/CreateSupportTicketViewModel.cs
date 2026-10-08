using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportCaseManagement.Application.Features.CreateSupportTicket;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CreateSupportTicketViewModel : ObservableObject
{
    private readonly ICreateSupportTicketService _createSupportTicketService;
    private readonly ICustomerRepository _customerRepository;

    public TicketPriority[] PriorityOptions { get; } = Enum.GetValues<TicketPriority>();

    public ObservableCollection<Customer> Customers { get; } = [];

    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Customer? SelectedCustomer { get; set; }

    [ObservableProperty]
    public partial TicketPriority SelectedPriority { get; set; }

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SuccessMessage { get; set; } = string.Empty;

    public CreateSupportTicketViewModel(
         ICreateSupportTicketService createSupportTicketService,
         ICustomerRepository customerRepository)
    {
        _createSupportTicketService = createSupportTicketService;
        _customerRepository = customerRepository;

        _ = LoadCustomersAsync();
    }

    public async Task LoadCustomersAsync()
    {

        var customers = await _customerRepository.GetAllAsync();
        Customers.Clear();
        foreach (var customer in customers)
        {
            Customers.Add(customer);
        }
    }

    [RelayCommand]
    public async Task SaveTicketAsync()
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        if (SelectedCustomer == null)
        {
            ErrorMessage = "Du måste välja en kund i listan.";
            return;
        }

        try
        {
            Guid ticketId = await _createSupportTicketService.CreateSupportTicketAsync(
                Title,
                Description,
                SelectedCustomer.CustomerId,
                SelectedPriority);

            SuccessMessage = $"Ärendet har skapats framgångsrikt (ID: {ticketId})!";
            ResetForm();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ett fel uppstod vid skapandet av ärendet: {ex.Message}";
        }
    }

    private void ResetForm()
    {
        Title = string.Empty;
        Description = string.Empty;
        SelectedCustomer = null;
        SelectedPriority = TicketPriority.Normal;
    }
}

