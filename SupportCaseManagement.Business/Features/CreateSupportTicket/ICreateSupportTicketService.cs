using SupportCaseManagement.Domain.Enums;

namespace SupportCaseManagement.Application.Features.CreateSupportTicket;

public interface ICreateSupportTicketService
{
    Task<Guid> CreateSupportTicketAsync(string title, string description, Guid customerId, TicketPriority priority);
}
