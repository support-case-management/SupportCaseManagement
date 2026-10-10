using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Application.Dtos.Responses;

public record CreateCustomerResponse
(
    bool Success,
    Customer? Customer,
    string? ErrorMessage
);
