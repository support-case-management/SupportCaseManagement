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

        _filteredTicketList.Clear();
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

        return filteredList;
    }

    public async IEnumerable<SupportTicket> GetTicketsContaining(string searchText)
    {
        List<SupportTicket> supportTickets;

        if (_filterOn)
        {
            supportTickets = _filteredTicketList;
        }
        else
        {
            supportTickets = await _supportTicketRepository.GetAllAsync();
        }

        string normalizedSearchText = searchText.Trim().ToLower();

        IEnumerable<SupportTicket> searchedTickets = from ticket in supportTickets
                                                     where ticket.Title.ToLower().Contains(normalizedSearchText) || 
                                                           ticket.Customer.CustomerName.Value.ToLower().Contains(normalizedSearchText)
                                                     orderby ticket.Date
                                                     select ticket;

        return searchedTickets;
    }
}