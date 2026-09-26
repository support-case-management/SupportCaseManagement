using SupportCaseManagement.Domain.Interfaces;

namespace SupportCaseManagement.Infrastructure.Repositories;

internal class JsonFileSupportTicketRepository : ISupportTicketRepository
{
    public Task<List<SupportTicket>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveAllAsync(IEnumerable<SupportTicket> supportTicket)
    {
        throw new NotImplementedException();
    }
}
