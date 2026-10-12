using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Application.Features.SupportTicketList;

public interface ISupportTicketListService
{
    bool IsFilterOn { get; }
    Task<IEnumerable<SupportTicket>> GetAllTicketsSortedAsync();
    Task<IEnumerable<SupportTicket>> GetTicketsContainingAsync(string searchText);
    Task<IEnumerable<SupportTicket>> GetTicketsWithAsync(TicketStatus status);
}
