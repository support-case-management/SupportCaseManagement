namespace SupportCaseManagement.Domain.SupportTickets.Dtos;

public record GetAllResponse
(
    bool Success,
    string? ErrorMessage,
    IEnumerable<SupportTicket> SupportTickets
);
