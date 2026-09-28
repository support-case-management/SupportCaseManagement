using SupportCaseManagement.Domain.Interfaces;

namespace SupportCaseManagement.Application.Features.SupportTicketList;

internal class SupportTicketListService(ISupportTicketRepository supportTicketRepository) : ISupportTicketListService
{
    private readonly ISupportTicketRepository _supportTicketRepository = supportTicketRepository;
    private List<SupportTicket> _filteredTicketList = [];
    private bool _filterOn = false;

    public async IEnumerable<SupportTicket> GetAllTicketsSorted()
    {
        List<SupportTicket> supportTickets = await _supportTicketRepository.GetAllAsync();

        _filterOn = false;

        return supportTickets.OrderBy(ticket => ticket.Date);
    }

    public async IEnumerable<SupportTicket> GetTicketsWith(Statuses status)
    {
        List<SupportTicket> supportTickets = await _supportTicketRepository.GetAllAsync();

        IEnumerable<SupportTicket> filteredList = from ticket in supportTickets
                                                  where ticket.Status == status
                                                  orderby ticket.Date
                                                  select ticket;

        _filteredTicketList = filteredList;
        _filterOn = true;

        return _filteredTicketList;
    }

    public IEnumerable<SupportTicket> GetTicketsContaining(string searchText)
    {
        throw new NotImplementedException();
    }
}