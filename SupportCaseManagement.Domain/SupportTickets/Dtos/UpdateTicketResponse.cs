namespace SupportCaseManagement.Domain.SupportTickets.Dtos;

public record UpdateTicketResponse
(
    bool Success,
    string? ErrorMessage,
    SupportTicket? UpdatedTicket
);
