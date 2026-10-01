namespace SupportCaseManagement.Application.Features.SupportTicketList;

public interface ISupportTicketListService
{
    IEnumerable<SupportTicket> GetAllTicketsSorted();
    IEnumerable<SupportTicket> GetTicketsContaining(string searchText);
    IEnumerable<SupportTicket> GetTicketsWith(Statuses status);
}
