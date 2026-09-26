namespace SupportCaseManagement.Domain.Interfaces;

public interface ISupportTicketRepository
{
    Task<List<SupportTicket>> GetAllAsync();
    Task SaveAllAsync(IEnumerable<SupportTicket> supportTicket);
}
