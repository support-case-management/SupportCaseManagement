using SupportCaseManagement.Domain.SupportTickets.Models;

namespace SupportCaseManagement.Domain.SupportTickets.Interfaces;

public interface ISupportTicketRepository
{
    Task SaveAllAsync(IEnumerable<SupportTicket> supportTickets);
    Task<List<SupportTicket>> GetAllAsync();
    Task<SupportTicket> GetByIdAsync(Guid id);
    Task<SupportTicket> UpdateAsync(SupportTicket updatedTicket);
}
