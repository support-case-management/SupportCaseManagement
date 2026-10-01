namespace SupportCaseManagement.Domain.SupportTickets.Dtos;

public record GetTicketResponse
(
    bool Success,
    string? ErrorMessage,
    SupportTicket? SupportTicket
);
