using SupportCaseManagement.Domain.Interfaces;

namespace SupportCaseManagement.Application.Features.SupportTicketList;

internal class SupportTicketListService(ISupportTicketRepository supportTicketRepository) : ISupportTicketListService
{
    private readonly ISupportTicketRepository _supportTicketRepository = supportTicketRepository;
    // en lista med alla ärenden som fitrerats fram för att kunna använda filter och söka efter ett eller flera ärenden samtidigt
    private List<SupportTicket> _filteredTicketList = [];
    // en kontroll för om användaren har ett  filter på eller inte
    private bool _filterOn = false;

    /// <summary>
    /// Hämtar alla sparade ärenden genom repositoryn och returnerar dessa sorterade efter datum
    /// </summary>
    /// <returns>en IEnumerable-lista med alla ärenden sorterade efter datum</returns>
    public async IEnumerable<SupportTicket> GetAllTicketsSorted()
    {
        List<SupportTicket> supportTickets = await _supportTicketRepository.GetAllAsync();

        _filteredTicketList.Clear();
        _filterOn = false;

        return supportTickets.OrderBy(ticket => ticket.Date);
    }

    /// <summary>
    /// Hämtar alla ärenden och filtrerar fram de som har samma status som givet
    /// </summary>
    /// <param name="status">statusen på ärendena som ska filtreras fram</param>
    /// <returns>en IEnumerable-lista med de ärenden som har den givna statusen</returns>
    public async IEnumerable<SupportTicket> GetTicketsWith(Statuses status)
    {
        List<SupportTicket> supportTickets = await _supportTicketRepository.GetAllAsync();

        // tar fram de ärenden som har den givna statusen genom linq, som fungerar så här:
        // from - som i en foreach-loop, ger ett namn på elementen som ska gås igenom ett för ett och anger vilken lista som ska gås igenom, men behöver inte skriva datatypen
        // where - kravet som måste uppfyllas för att ärendet ska tas med, i detta fall att ärendets status är detsamma som den givna statusen
        // orderby - säger hur ärendena som väljs ut ska sorteras, i detta fall efter datum
        // select - vad som ska läggas in i den nya listan, i detta fall hela ärende-objektet som uppfyller kravet
        IEnumerable<SupportTicket> filteredList = from ticket in supportTickets
                                                  where ticket.Status == status
                                                  orderby ticket.Date
                                                  select ticket;

        _filteredTicketList = filteredList;
        _filterOn = true;

        return filteredList;
    }

    /// <summary>
    /// Söker igenom alla ärenden om inget filter är på, eller de ärenden som filtrerats fram, 
    /// och returnerar de ärenden som innehåller den givna sök-texten
    /// </summary>
    /// <param name="searchText">det ord eller del av ord som ska finnas i ärendets rubrik eller kundens namn</param>
    /// <returns>en IEnumerable-lista med de ärenden som innehåller sök-texten</returns>
    public async IEnumerable<SupportTicket> GetTicketsContaining(string searchText)
    {
        List<SupportTicket> supportTickets;

        if (_filterOn)
        {
            // om ett filter är på används den filtrerade listan
            supportTickets = _filteredTicketList;
        }
        else
        {
            // annars kontrolleras alla sparade ärenden
            supportTickets = await _supportTicketRepository.GetAllAsync();
        }

        string normalizedSearchText = searchText.Trim().ToLower();

        // tar alla ärenden där den sökta texten finns i ärendets rubrik eller i kundens namn, oberoende på stora eller små bokstäver
        IEnumerable<SupportTicket> searchedTickets = from ticket in supportTickets
                                                     where ticket.Title.ToLower().Contains(normalizedSearchText) ||
                                                           ticket.Customer.CustomerName.Value.ToLower().Contains(normalizedSearchText)
                                                     orderby ticket.Date
                                                     select ticket;

        if (searchedTickets is null)
        {
            searchedTickets = [];
        }

        return searchedTickets;
    }
}