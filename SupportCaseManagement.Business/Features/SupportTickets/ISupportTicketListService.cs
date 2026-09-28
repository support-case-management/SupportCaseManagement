namespace SupportCaseManagement.Application.Features.SupportTickets;

public interface ISupportTicketListService
{
    IEnumerable<SupportTicket> GetAllTicketsSorted();
    IEnumerable<SupportTicket> GetTicketsContaining(string searchText);
    IEnumerable<SupportTicket> GetFilteredListOfTickets(Statuses status);
}
