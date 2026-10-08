using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportCaseManagement.Application.Features.CreateSupportTicket;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CreateSupportTicketViewModel : ObservableObject
{
    private readonly ICreateSupportTicketService _createSupportTicketService;
    private readonly ICustomerRepository _customerRepository;

    public TicketPriority[] PriorityOptions { get; } = Enum.GetValues<TicketPriority>();

    public ObservableCollection<Customer> Customers { get; } = new();

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private Customer? _selectedCustomer;

    [ObservableProperty]
    private TicketPriority _selectedPriority = TicketPriority.Normal;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private string _successMessage = string.Empty;

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
        Customers.Clear();
        var customers = await _customerRepository.GetAllAsync();
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

