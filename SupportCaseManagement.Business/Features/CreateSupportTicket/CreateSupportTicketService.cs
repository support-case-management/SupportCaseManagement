using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;


namespace SupportCaseManagement.Application.Features.CreateSupportTicket;

public class CreateSupportTicketService : ICreateSupportTicketService
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateSupportTicketService(ISupportTicketRepository supportTicketRepository, ICustomerRepository customerRepository)
    {
        _supportTicketRepository = supportTicketRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Guid> CreateSupportTicketAsync(string title, string description, Guid customerId, TicketPriority priority)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw new InvalidOperationException("Denna kund finns inte i systemet.");
        }

        var ticket = new SupportTicket(title, description, customerId, priority, customer);

        // lägger till ärendets id till en lista med ärende-id:n och uppdaterar kunden i den sparade filen
        customer.AddSupportTicket(ticket);
        await _customerRepository.UpdateAsync(customer);

        var tickets = await _supportTicketRepository.GetAllAsync();
        tickets.Add(ticket);
        await _supportTicketRepository.SaveAllAsync(tickets);

        return ticket.Id;
    }
}

