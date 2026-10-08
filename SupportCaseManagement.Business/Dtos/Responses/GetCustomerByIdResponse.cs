using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Application.Dtos.Responses;

public record GetCustomerByIdResponse
(
    bool Success,
    Customer? Customer,
    string? ErrorMessage
);
