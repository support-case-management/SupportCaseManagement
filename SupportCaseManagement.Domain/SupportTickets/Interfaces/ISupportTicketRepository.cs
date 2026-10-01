using SupportCaseManagement.Domain.SupportTickets.Dtos;

namespace SupportCaseManagement.Domain.SupportTickets.Interfaces;

public interface ISupportTicketRepository
{
    Task SaveAllAsync(IEnumerable<SupportTicket> supportTickets);
    GetAllResponse GetAllAsync();
    Task<GetTicketResponse> GetById(GetTicketRequest request);
    Task<UpdateTicketResponse> Update(UpdateTicketRequest request);
}
